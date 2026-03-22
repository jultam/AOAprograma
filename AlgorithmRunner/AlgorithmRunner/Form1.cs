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

        public class InitComponentMethod
        {
            public string name { get; set; }
            public Func<int, int, double, double, double[,]> method { get; set; }

            public InitComponentMethod(string name, Func<int, int, double, double, double[,]> method)
            {
                this.name = name;
                this.method = method;
            }
        }

        private List<InitComponentMethod> initComponents = new List<InitComponentMethod>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                string directory = "../../benchmarks";
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

            // ----------- Reads all initialization methods --------------------------\
            try
            {
                comboBox1.Items.Clear();
                string directory = "../../components/initialization";
                string initName;
                foreach (string file in Directory.GetFiles(directory, "*.dat"))
                {
                    (initName) = FileMethods.ReadComponent(file);
                    MethodInfo method = typeof(ContOpt).GetMethod(initName);
                    Func<int, int, double, double, double[,]> function = (Func<int, int, double, double, double[,]>)Delegate.CreateDelegate(typeof(Func<int, int, double, double, double[,]>), method);

                    initComponents.Add(new InitComponentMethod(initName, function));
                }
                richTextBox1.AppendText("\n" + FileMethods.ArrayToString(initComponents.Select(m => m.name).ToArray())); // debug
                comboBox1.Items.AddRange(initComponents.Select(m => m.name).ToArray());
                comboBox1.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
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

        private void start_Click(object sender, EventArgs e)
        {
            int PS = (int)PSUpDown.Value; int M_Iter = (int)MIterUpDown.Value;
            int alpha = (int)alphaUpDown.Value; double mu = (double)muUpDown.Value; int epsilon = (int)epsilonUpDown.Value;

            string initMethodName = comboBox1.Text;
            Func<int, int, double, double, double[,]> initMethod = initComponents.Find(m => m.name == initMethodName).method;

            Results.OverallResults results = Algorithm.AOA(PS, M_Iter, alpha, mu, epsilon, benchmarkFunctions, initMethod);

            Results.PopulateDataGridView(results, dataGridView1);

            download.Enabled = true;
        }

        private void download_Click(object sender, EventArgs e)
        {
            FileMethods.SaveDataGridToExcel(dataGridView1);
        }
    }
}
