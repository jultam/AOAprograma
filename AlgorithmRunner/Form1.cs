using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CONTOPT;
using Microsoft.CodeAnalysis.CSharp;
using TorchSharp;
using TorchSharp.Modules;
using Microsoft.CodeAnalysis;
using System.Net.Http;
using MathNet.Numerics.Optimization;

namespace AlgorithmRunner
{
    public partial class Form1 : Form
    {
        public class BenchmarkFunction
        {
            public string name { get; set; }
            public double arg_range_1 { get; set; }
            public double arg_range_2 { get; set; }
            public double best_known { get; set; }
            public Func<double[], int, double> function { get; set; }
            public BenchmarkFunction(string name, double lb, double ub, double optimum)
            {
                this.name = name;
                this.arg_range_1 = lb;
                this.arg_range_2 = ub;
                this.best_known = optimum;
            }

            public override string ToString()
            {
                return String.Format(name+" "+arg_range_1+" "+arg_range_2+" "+best_known);
            }
        }

        private List<BenchmarkFunction> benchmarkFunctions = new List<BenchmarkFunction>();
        public class ComponentMethod
        {
            public string name { get; set; }
            public ComponentMethod() { }
        }
        public class MapComponentMethod : ComponentMethod
        {
            public Func<double> method { get; set; }

            public MapComponentMethod(string name, Func<double> method)
            {
                this.name = name;
                this.method = method;
            }
        }

        private List<MapComponentMethod> mapComponents = new List<MapComponentMethod>();

        public class MOAMOPComponentMethod : ComponentMethod
        {
            public Func<int,int,int,(double,double)> method { get; set; }

            public MOAMOPComponentMethod(string name, Func<int, int, int, (double, double)> method)
            {
                this.name = name;
                this.method = method;
            }
        }

        private List<MOAMOPComponentMethod> MOAMOPComponents = new List<MOAMOPComponentMethod>();

        public class InitializationComponentMethod : ComponentMethod
        {
            public Func<int, int, double, double, Func<double>, Func<double[], int, double>, double[,]> method { get; set; }

            public InitializationComponentMethod(string name, Func<int, int, double, double, Func<double>, Func<double[], int, double>, double[,]> method)
            {
                this.name = name;
                this.method = method;
            }
        }

        private List<InitializationComponentMethod> initializationComponents = new List<InitializationComponentMethod>();

        public class StepComponentMethod : ComponentMethod
        {
            public Func<double[,], int, double, double, double, double, double, double, int, 
                Func<double>, Func<double[], int, double>, double[,]> method { get; set; }
        
            public StepComponentMethod(string name, Func<double[,], int, double, double, double, double, double, double, int, 
                Func<double>, Func<double[], int, double>, double[,]> method)
            {
                this.name = name;
                this.method = method;
            }
        }

        private List<StepComponentMethod> stepsComponents = new List<StepComponentMethod>();

        public class Components
        {
            public Func<double> mapMethod { get; set; }

            public Func<int, int, int, (double, double)> MOAMOPMethod { get; set; }

            public Func<int, int, double, double, Func<double>, Func<double[], int, double>, double[,]> initMethod { get; set; }

            public Func<double[,], int, double, double, double, double, double, double, int, 
                Func<double>, Func<double[], int, double>, double[,]> stepMethod { get; set; }

            public Components(Func<double> mapMethod, Func<int, int, int, (double, double)> MOAMOPMethod,
                Func<int, int, double, double, Func<double>, Func<double[], int, double>, double[,]> initMethod,
                Func<double[,], int, double, double, double, double, double, double, int,
                Func<double>, Func<double[], int, double>, double[,]> stepMethod)
            {
                this.mapMethod = mapMethod;
                this.MOAMOPMethod = MOAMOPMethod;
                this.initMethod = initMethod;
                this.stepMethod = stepMethod;
            }
        }

        public static RichTextBox richTextBox = new RichTextBox();

        int PS = 100; int M_Iter = 100; int runs = 1;
        int alpha = 5; double mu = 0.4; double epsilon = 0.05;
        
        string generatorMethodName = "";
        string MOAMOPMethodName = "";
        string initializationMethodName = "";
        string stepMethodName = "";
        string pathToFiles = "../../../";

        int[] testingDimensions = { 1, 30, 100 };
        
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        static extern bool AllocConsole();

        public Form1()
        {
            InitializeComponent();
            AllocConsole();
            richTextBox = richTextBox1;
            ParameterAIMethods.CreateModel();
            ParameterAIMethods.LoadModel(pathToFiles+"parameter_ai_model.pt");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string directory;

            // ----------- Reads default parameters file -----------------------------\
            try {
                int PS; int M_Iter;
                (PS, M_Iter, alpha, mu, epsilon, testingDimensions) = FileMethods.ReadParameters(pathToFiles + "param.txt");
                PSUpDown.Value = PS; MIterUpDown.Value = M_Iter; alphaUpDown.Value = alpha; muUpDown.Value = (decimal)mu; epsilonUpDown.Value = (decimal)epsilon;
            }
            catch (Exception ex) {
                string message = "";
                if (!File.Exists(pathToFiles + "param.txt")) message += "Default parameter file does not exist! Make sure it is called \"param.txt\"!\n";
                message += ex.Message;
                MessageBox.Show(message);
            }
            // -----------------------------------------------------------------------/

            // ----------- Reads all benchmark function parameters -------------------\
            directory = pathToFiles + "benchmarks";
            try {
                string benchName; double optimum; double ub; double lb;
                foreach (string file in Directory.GetFiles(directory, "*.dat"))
                {
                    (benchName, lb, ub, optimum) = FileMethods.ReadBenchmark(file);
                    BenchmarkFunction benchmark = new BenchmarkFunction(benchName, lb, ub, optimum);
                    benchmarkFunctions.Add(benchmark);
                }
            } catch (Exception ex)
            {
                string message = "";
                if (Directory.Exists(directory) && Directory.GetFiles(directory, "*.dat").Length <= 0) message += "Benchmarks directory has no readable files! Make sure the files have a .dat extension!\n";
                message += ex.Message;
                MessageBox.Show(message);
            }
            // -----------------------------------------------------------------------/

            // ----------- Creates delegates of function methods ---------------------\
            try {
                foreach (BenchmarkFunction benchmark in benchmarkFunctions)
                {
                    string code = File.ReadAllText(pathToFiles + "benchmarks/" + benchmark.name + ".cs");

                    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

                    var references = new List<MetadataReference>
                    {
                        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),    // mscorlib.dll
                        MetadataReference.CreateFromFile(typeof(Math).Assembly.Location),      // System.dll
                        MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location) // System.Core.dll
                    };

                    CSharpCompilation compilation = CSharpCompilation.Create(
                        Path.GetRandomFileName(),
                        new[] { syntaxTree },
                        references,
                        new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                    using (var ms = new MemoryStream())
                    {
                        // 4. Build the DLL in memory
                        var result = compilation.Emit(ms);

                        if (result.Success)
                        {
                            ms.Seek(0, SeekOrigin.Begin);

                            // 5. Load assembly using the legacy method for .NET 4.7.2
                            Assembly assembly = Assembly.Load(ms.ToArray());

                            // 6. Access your class and method (adjust namespace if different)
                            Type type = assembly.GetType("CONTOPT.ContOpt");
                            MethodInfo method = type.GetMethod(benchmark.name);

                            if (method != null)
                            {
                                benchmark.function = (Func<double[], int, double>)Delegate.CreateDelegate(
                                    typeof(Func<double[], int, double>), method);
                            }
                            else
                            {
                                MessageBox.Show($"Could not find method '{benchmark.name}' in class 'ContOpt'");
                            }
                        }
                        else
                        {
                            // Display specific errors from the .cs file
                            var errors = string.Join("\n", result.Diagnostics
                                .Where(d => d.Severity == DiagnosticSeverity.Error)
                                .Select(d => d.GetMessage()));
                            MessageBox.Show($"Compilation failed for {benchmark.name}:\n{errors}");
                        }
                    }

                    //MethodInfo method = typeof(ContOpt).GetMethod(benchmark.name);
                    //Func<double[], int, double> function = (Func<double[], int, double>)Delegate.CreateDelegate(typeof(Func<double[], int, double>), method);
                    //benchmarkFunctions.Find(x => x.name == benchmark.name).function = function;
                }
            } catch (Exception ex)
            {
                string message = "";
                message += "Benchmark file could not be found! Make sure it is named exactly as in the corresponding .dat file and that it has an .cs extension!\n";
                message += ex.Message;
                MessageBox.Show(message);
            }
            // -----------------------------------------------------------------------/

            // ----------- Reads all methods --------------------------\

            directory = pathToFiles + "components/maps";
            mapComponents = FileMethods.PopulateComboBox<Func<double>, MapComponentMethod>(directory, comboBox1, mapComponents);

            directory = pathToFiles + "components/MOAandMOP";
            MOAMOPComponents = FileMethods.PopulateComboBox<Func<int, int, int, (double, double)>, MOAMOPComponentMethod>(directory, comboBox2, MOAMOPComponents);

            directory = pathToFiles + "components/initialization";
            initializationComponents = FileMethods.PopulateComboBox<Func<int, int, double, double, Func<double>, Func<double[], int, double>, double[,]>, InitializationComponentMethod>(directory, comboBox3, initializationComponents);

            directory = pathToFiles + "components/steps";
            stepsComponents = FileMethods.PopulateComboBox<Func<double[,], int, double, double, double, double, double, double, int, Func<double>, Func<double[], int, double>, double[,]>, StepComponentMethod>(directory, comboBox4, stepsComponents);

            // -----------------------------------------------------------------------/
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Filter = "txt files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                int PS; int M_Iter; int alpha; double mu; double epsilon;
                (PS, M_Iter, alpha, mu, epsilon, testingDimensions) = FileMethods.ReadParameters(openFileDialog.FileName);
                PSUpDown.Value = PS; MIterUpDown.Value = M_Iter; alphaUpDown.Value = alpha; muUpDown.Value = (decimal)mu; epsilonUpDown.Value = (decimal)epsilon;
            }
        }
        private void saveParamButton_Click(object sender, EventArgs e)
        {
            FileMethods.SaveParameters((int)PSUpDown.Value, (int)MIterUpDown.Value, (int)alphaUpDown.Value, (double)muUpDown.Value, (double)epsilonUpDown.Value, testingDimensions);
        }

        private async void start_Click(object sender, EventArgs e)
        {
            start.Enabled = false;
            dataGridView1.Rows.Clear();

            PS = (int)PSUpDown.Value; int M_Iter = (int)MIterUpDown.Value; runs = (int)testUpDown.Value;
            alpha = (int)alphaUpDown.Value; mu = (double)muUpDown.Value; epsilon = (double)epsilonUpDown.Value;
            

            generatorMethodName = comboBox1.Text;
            Func<double> generatorMethod = mapComponents.Find(m => m.name == generatorMethodName).method;

            MOAMOPMethodName = comboBox2.Text;
            Func<int, int, int, (double, double)> MOAMOPMethod = MOAMOPComponents.Find(m => m.name == MOAMOPMethodName).method;

            initializationMethodName = comboBox3.Text;
            Func<int, int, double, double, Func<double>, Func<double[], int, double>, double[,]> initializationMethod = initializationComponents.Find(m => m.name == initializationMethodName).method;

            stepMethodName = comboBox4.Text;
            Func<double[,], int, double, double, double, double, double, double, int, Func<double>, Func<double[], int, double>, double[,]> stepMethod = stepsComponents.Find(m => m.name == stepMethodName).method;

            int rowID = 1;

            Components components = new Components(generatorMethod, MOAMOPMethod, initializationMethod, stepMethod);

            foreach (BenchmarkFunction benchmark in benchmarkFunctions)
            {
                foreach (int D in testingDimensions)
                {
                    Results.AlgorithmResults results = await Task.Run(() => Algorithm.AOA(runs, PS*D, M_Iter, D, alpha, mu, epsilon, 
                        benchmark, components));
                    dataGridView1.Rows.Add(rowID, benchmark.name, D, benchmark.best_known, results.optimum,  results.time);
                    rowID++;
                }
            }

            ContOpt.ResetMap();

            download.Enabled = true;
            start.Enabled = true;
        }

        private void download_Click(object sender, EventArgs e)
        {
            FileMethods.SaveDataGridToText(dataGridView1, PS, M_Iter, runs, alpha, mu, epsilon, generatorMethodName, MOAMOPMethodName, initializationMethodName, stepMethodName);
        }

        private async void train_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();

            string binPath = AppDomain.CurrentDomain.BaseDirectory;
            bool hasTorchCpu = File.Exists(Path.Combine(binPath, "torch_cpu.dll"));
            bool hasLibTorchSharp = File.Exists(Path.Combine(binPath, "LibTorchSharp.dll"));

            if (!hasTorchCpu || !hasLibTorchSharp)
            {
                MessageBox.Show("Native libraries still missing!\n" +
                               $"torch_cpu.dll: {hasTorchCpu}\n" +
                               $"LibTorchSharp.dll: {hasLibTorchSharp}");
                return;
            }

            double[,] dataMatrix = new double[benchmarkFunctions.Count() * testingDimensions.Count(), 5];
            double[,] targetMatrix = new double[benchmarkFunctions.Count() * testingDimensions.Count(), 1];

            List<BenchmarkFunction> activeBenchmarks = new List<BenchmarkFunction>();
            double mean; double std;
            int k = 0;

            foreach (BenchmarkFunction benchmark in benchmarkFunctions)
            {
                foreach (int D in testingDimensions)
                {
                    (mean, std) = ParameterAIMethods.RandomSamples(benchmark, D, 50 * D);

                    dataMatrix[k, 0] = benchmark.arg_range_1;
                    dataMatrix[k, 1] = benchmark.arg_range_2;
                    dataMatrix[k, 2] = D;
                    dataMatrix[k, 3] = mean;
                    dataMatrix[k, 4] = std;

                    targetMatrix[k, 0] = benchmark.best_known;

                    activeBenchmarks.Add(benchmark);

                    k++;
                }
            }

            /*var inputsFlat = new float[k * 5];
            var targetsFlat = new float[k * 1];

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    inputsFlat[i * 5 + j] = (float)dataMatrix[i, j];
                }
                targetsFlat[i] = (float)targetMatrix[i, 0];
            }*/

            torch.Tensor inputs = torch.tensor(dataMatrix, dtype: torch.float32);
            torch.Tensor targets = torch.tensor(targetMatrix, dtype: torch.float32);

            AppendTextSafe($"Input tensor shape: [{string.Join(", ", inputs.shape)}]\n");
            AppendTextSafe($"Target tensor shape: [{string.Join(", ", targets.shape)}]\n");

            // Manual batches
            int batchSize = 4;
            List<(IList<torch.Tensor> tensors, List<BenchmarkFunction> benchmarks)> batches =
                new List<(IList<torch.Tensor> tensors, List<BenchmarkFunction> benchmarks)>();

            torch.Tensor perm = torch.randperm(k);
            long[] indices = perm.data<long>().ToArray();
            inputs = inputs.index_select(0, perm);
            targets = targets.index_select(0, perm);
            activeBenchmarks = indices.Select(i => activeBenchmarks[(int)i]).ToList();

            for (int i = 0; i < k; i += batchSize)
            {
                int endIdx = Math.Min(i + batchSize, k);

                torch.Tensor batchInputs = inputs.slice(0, i, endIdx, 1);
                torch.Tensor batchTargets = targets.slice(0, i, endIdx, 1);
                List<BenchmarkFunction> batchBenchmarks = activeBenchmarks.GetRange(i, endIdx - i);

                batches.Add((new List<torch.Tensor> { batchInputs, batchTargets }, batchBenchmarks));
            }
            AppendTextSafe($"Created {batches.Count} batches\n");

            generatorMethodName = comboBox1.Text;
            Func<double> generatorMethod = mapComponents.Find(m => m.name == generatorMethodName).method;

            MOAMOPMethodName = comboBox2.Text;
            Func<int, int, int, (double, double)> MOAMOPMethod = MOAMOPComponents.Find(m => m.name == MOAMOPMethodName).method;

            initializationMethodName = comboBox3.Text;
            Func<int, int, double, double, Func<double>, Func<double[], int, double>, double[,]> initializationMethod = initializationComponents.Find(m => m.name == initializationMethodName).method;

            stepMethodName = comboBox4.Text;
            Func<double[,], int, double, double, double, double, double, double, int, Func<double>, Func<double[], int, double>, double[,]> stepMethod = stepsComponents.Find(m => m.name == stepMethodName).method;

            Components components = new Components(generatorMethod, MOAMOPMethod, initializationMethod, stepMethod);

            await Task.Run(() => ParameterAIMethods.TrainModel(7, batches, components));
            ParameterAIMethods.SaveModel(pathToFiles+"parameter_ai_model.pt");
        }

        public static void AppendTextSafe(string text)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new Action<string>(AppendTextSafe), text);
            }
            else
            {
                richTextBox.AppendText(text);
            }
        }

        private async void generateParamsButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();

            string binPath = AppDomain.CurrentDomain.BaseDirectory;
            bool hasTorchCpu = File.Exists(Path.Combine(binPath, "torch_cpu.dll"));
            bool hasLibTorchSharp = File.Exists(Path.Combine(binPath, "LibTorchSharp.dll"));

            if (!hasTorchCpu || !hasLibTorchSharp)
            {
                MessageBox.Show("Native libraries still missing!\n" +
                               $"torch_cpu.dll: {hasTorchCpu}\n" +
                               $"LibTorchSharp.dll: {hasLibTorchSharp}");
                return;
            }

            double[,] dataMatrix = new double[benchmarkFunctions.Count() * testingDimensions.Count(), 5];
            double[,] targetMatrix = new double[benchmarkFunctions.Count() * testingDimensions.Count(), 1];

            List<BenchmarkFunction> activeBenchmarks = new List<BenchmarkFunction>();
            double mean; double std;
            int k = 0;

            foreach (BenchmarkFunction benchmark in benchmarkFunctions)
            {
                foreach (int D in testingDimensions)
                {
                    (mean, std) = ParameterAIMethods.RandomSamples(benchmark, D, 50 * D);

                    dataMatrix[k, 0] = benchmark.arg_range_1;
                    dataMatrix[k, 1] = benchmark.arg_range_2;
                    dataMatrix[k, 2] = D;
                    dataMatrix[k, 3] = mean;
                    dataMatrix[k, 4] = std;

                    targetMatrix[k, 0] = benchmark.best_known;

                    activeBenchmarks.Add(benchmark);

                    k++;
                }
            }

            torch.Tensor inputs = torch.tensor(dataMatrix, dtype: torch.float32);
            torch.Tensor targets = torch.tensor(targetMatrix, dtype: torch.float32);

            AppendTextSafe($"Input tensor shape: [{string.Join(", ", inputs.shape)}]\n");
            AppendTextSafe($"Target tensor shape: [{string.Join(", ", targets.shape)}]\n");

            // Manual batches



            int batchSize = 4;
            List<(IList<torch.Tensor> tensors, List<BenchmarkFunction> benchmarks)> batches = 
                new List<(IList<torch.Tensor> tensors, List<BenchmarkFunction> benchmarks)>();

            for (int i = 0; i < k; i += batchSize)
            {
                int endIdx = Math.Min(i + batchSize, k);

                torch.Tensor batchInputs = inputs.slice(0, i, endIdx, 1);
                torch.Tensor batchTargets = targets.slice(0, i, endIdx, 1);
                List<BenchmarkFunction> batchBenchmarks = activeBenchmarks.GetRange(i, endIdx - i);

                batches.Add((new List<torch.Tensor> { batchInputs, batchTargets }, batchBenchmarks));
            }

            AppendTextSafe($"Created {batches.Count} batches\n");

            generatorMethodName = comboBox1.Text;
            Func<double> generatorMethod = mapComponents.Find(m => m.name == generatorMethodName).method;

            MOAMOPMethodName = comboBox2.Text;
            Func<int, int, int, (double, double)> MOAMOPMethod = MOAMOPComponents.Find(m => m.name == MOAMOPMethodName).method;

            initializationMethodName = comboBox3.Text;
            Func<int, int, double, double, Func<double>, Func<double[], int, double>, double[,]> initializationMethod = initializationComponents.Find(m => m.name == initializationMethodName).method;

            stepMethodName = comboBox4.Text;
            Func<double[,], int, double, double, double, double, double, double, int, Func<double>, Func<double[], int, double>, double[,]> stepMethod = stepsComponents.Find(m => m.name == stepMethodName).method;

            Components components = new Components(generatorMethod, MOAMOPMethod, initializationMethod, stepMethod);

            int taskCount = benchmarkFunctions.Count() * testingDimensions.Count();
            torch.Tensor generatedParameters = torch.zeros(taskCount, 5);
            await Task.Run(() => generatedParameters = ParameterAIMethods.GenerateParameters(3, batches, components, taskCount));
            double[] overallParameters = new double[5];
        
            for (int i = 0; i < 5; i++)
            {
                double sum = 0;
                for (int j = 0; j < taskCount; j++)
                {
                    sum += (double)generatedParameters[j][i];
                }
                overallParameters[i] = sum / taskCount;
            }

            PSUpDown.Value = (decimal)overallParameters[0];
            MIterUpDown.Value = (decimal)overallParameters[1];
            alphaUpDown.Value = (decimal)overallParameters[2];
            muUpDown.Value = (decimal)overallParameters[3];
            epsilonUpDown.Value = (decimal)overallParameters[4];
        }

        private void saveLogsButton_Click(object sender, EventArgs e)
        {
            FileMethods.SaveText(richTextBox1.Text);

        }

        private void quitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
