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
using TorchSharp;
using TorchSharp.Modules;

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

        int[] testingDimensions = { 1, 30, 100 };

        public Form1()
        {
            InitializeComponent();
            richTextBox = richTextBox1;
            ParameterAIMethods.CreateModel();
            //ParameterAIMethods.LoadModel("../../parameter_ai_model.pt");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string directory;

            // ----------- Reads default parameters file -----------------------------\
            try {
                int PS; int M_Iter;
                (PS, M_Iter, alpha, mu, epsilon) = FileMethods.ReadParameters("../../param.txt");
                PSUpDown.Value = PS; MIterUpDown.Value = M_Iter; alphaUpDown.Value = alpha; muUpDown.Value = (decimal)mu; epsilonUpDown.Value = (decimal)epsilon;
            }
            catch (Exception ex) {
                MessageBox.Show("Error: " + ex);
            }
            // -----------------------------------------------------------------------/

            // ----------- Reads all benchmark function parameters -------------------\
            try {
                directory = "../../benchmarks";
                string benchName; double optimum; double ub; double lb;
                foreach (string file in Directory.GetFiles(directory, "*.dat"))
                {
                    (benchName, lb, ub, optimum) = FileMethods.ReadBenchmark(file);
                    BenchmarkFunction benchmark = new BenchmarkFunction(benchName, lb, ub, optimum);
                    benchmarkFunctions.Add(benchmark);
                }
            } catch (DirectoryNotFoundException ex) {
                MessageBox.Show("Error loading benchmark directory: "+ex);
            }
            // -----------------------------------------------------------------------/

            // ----------- Creates delegates of function methods ---------------------\
            try {
                foreach (BenchmarkFunction benchmark in benchmarkFunctions)
                {
                    MethodInfo method = typeof(ContOpt).GetMethod(benchmark.name);
                    Func<double[], int, double> function = (Func<double[], int, double>)Delegate.CreateDelegate(typeof(Func<double[], int, double>), method);
                    benchmarkFunctions.Find(x => x.name == benchmark.name).function = function;
                }
            } catch (Exception ex){
                MessageBox.Show("Error: " + ex);
            }
            // -----------------------------------------------------------------------/

            // ----------- Reads all methods --------------------------\

            directory = "../../components/maps";
            mapComponents = FileMethods.PopulateComboBox<Func<double>, MapComponentMethod>(directory, comboBox1, mapComponents);

            directory = "../../components/MOAandMOP";
            MOAMOPComponents = FileMethods.PopulateComboBox<Func<int, int, int, (double, double)>, MOAMOPComponentMethod>(directory, comboBox2, MOAMOPComponents);

            directory = "../../components/initialization";
            initializationComponents = FileMethods.PopulateComboBox<Func<int, int, double, double, Func<double>, Func<double[], int, double>, double[,]>, InitializationComponentMethod>(directory, comboBox3, initializationComponents);

            directory = "../../components/steps";
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
                (PS, M_Iter, alpha, mu, epsilon) = FileMethods.ReadParameters(openFileDialog.FileName);
                PSUpDown.Value = PS; MIterUpDown.Value = M_Iter; alphaUpDown.Value = alpha; muUpDown.Value = (decimal)mu; epsilonUpDown.Value = (decimal)epsilon;
            }
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
                    Results.AlgorithmResults results = await Task.Run(() => Algorithm.AOA(runs, PS, M_Iter, D, alpha, mu, epsilon, 
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
            // Diagnostic check
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
            double mean; double std;

            int k = 0;

            foreach (BenchmarkFunction benchmark in benchmarkFunctions)
            {
                foreach (int D in testingDimensions)
                {
                    (mean, std) = ParameterAIMethods.RandomSamples(benchmark, D, 10 * D);

                    dataMatrix[k, 0] = benchmark.arg_range_1;
                    dataMatrix[k, 1] = benchmark.arg_range_2;
                    dataMatrix[k, 2] = D;
                    dataMatrix[k, 3] = mean;
                    dataMatrix[k, 4] = std;

                    targetMatrix[k, 0] = benchmark.best_known;

                    k++;
                }
            }

            // Convert 2D arrays to 1D then reshape
            var inputsFlat = new float[k * 5];
            var targetsFlat = new float[k * 1];

            for (int i = 0; i < k; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    inputsFlat[i * 5 + j] = (float)dataMatrix[i, j];
                }
                targetsFlat[i] = (float)targetMatrix[i, 0];
            }

            torch.Tensor inputs = torch.tensor(dataMatrix, dtype: torch.float32);
            torch.Tensor targets = torch.tensor(targetMatrix, dtype: torch.float32);

            AppendTextSafe($"Input tensor shape: [{string.Join(", ", inputs.shape)}]\n");
            AppendTextSafe($"Target tensor shape: [{string.Join(", ", targets.shape)}]\n");

            // Create manual batches
            int batchSize = 4;
            var batches = new List<IList<torch.Tensor>>();

            for (int i = 0; i < k; i += batchSize)
            {
                int endIdx = Math.Min(i + batchSize, k);
                
                var batchInputs = inputs.slice(0, i, endIdx, 1);
                var batchTargets = targets.slice(0, i, endIdx, 1);
                
                batches.Add(new List<torch.Tensor> { batchInputs, batchTargets });
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

            await Task.Run(() => ParameterAIMethods.TrainModel(20, batches, benchmarkFunctions[0], components));
            ParameterAIMethods.SaveModel("../../parameter_ai_model.pt");
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
    }
}
