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
            public Func<int, int, double, double, Func<double>> method { get; set; }

            public InitializationComponentMethod(string name, Func<int, int, double, double, Func<double>> method)
            {
                this.name = name;
                this.method = method;
            }
        }

        private List<InitializationComponentMethod> initializationComponents = new List<InitializationComponentMethod>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string directory;

            // ----------- Reads default parameters file -----------------------------\
            try {
                int PS; int M_Iter;
                (PS, M_Iter) = FileMethods.ReadParameters("../../param.txt");
                PSUpDown.Value = PS; MIterUpDown.Value = M_Iter;
            } catch (Exception ex) {
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
                    richTextBox1.AppendText("\n"+benchmark.ToString()); // debug
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

            directory = "../../components/SolutionInitialization";
            initializationComponents = FileMethods.PopulateComboBox<Func<int, int, double, double, Func<double>>, InitializationComponentMethod>(directory, comboBox3, initializationComponents);
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
                int PS; int M_Iter;
                (PS, M_Iter) = FileMethods.ReadParameters(openFileDialog.FileName);
                PSUpDown.Value = PS; MIterUpDown.Value = M_Iter;
            }
        }

        private async void start_Click(object sender, EventArgs e)
        {
            start.Enabled = false;
            dataGridView1.Rows.Clear();

            int PS = (int)PSUpDown.Value; int M_Iter = (int)MIterUpDown.Value;
            int alpha = (int)alphaUpDown.Value; double mu = (double)muUpDown.Value; int epsilon = (int)epsilonUpDown.Value;
            int iterations = (int)testUpDown.Value;

            string initMethodName = comboBox1.Text;
            Func<double> initMethod = mapComponents.Find(m => m.name == initMethodName).method;

            string MOAMOPMethodName = comboBox2.Text;
            Func<int, int, int, (double, double)> MOAMOPMethod = MOAMOPComponents.Find(m => m.name == MOAMOPMethodName).method;

            int[] testingDimensions = { 1, 30, 100 };

            foreach (BenchmarkFunction benchmark in benchmarkFunctions)
            {
                foreach (int D in testingDimensions)
                {
                    Results.AlgorithmResults results = await Task.Run(() => Algorithm.AOA(iterations, PS, M_Iter, D, alpha, mu, epsilon, benchmark, initMethod, MOAMOPMethod));
                    dataGridView1.Rows.Add(benchmark.name, D, results.optimum, Math.Sqrt(results.MSE), results.time);
                }
            }

            ContOpt.ResetMap();

            download.Enabled = true;
            start.Enabled = true;
        }

        private void download_Click(object sender, EventArgs e)
        {
            FileMethods.SaveDataGridToExcel(dataGridView1);
        }
    }
}
