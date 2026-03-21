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
        class BenchmarkFunction
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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ----------- Reads all benchmark function parameters -------------------\
            try {
                string directory = "../../benchmarks";
                string benchName; double optimum; double ub; double lb;
                foreach (string file in Directory.GetFiles(directory, "*.dat"))
                {
                    (benchName, lb, ub, optimum) = ReadBenchmark(file);
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
                    var function = (Func<double[], int, double>)Delegate.CreateDelegate(typeof(Func<double[], int, double>), method);
                    benchmarkFunctions.Find(x => x.name == benchmark.name).function = function;
                }
            } catch (Exception ex){
                MessageBox.Show("Error: " + ex);
            }
            // -----------------------------------------------------------------------/
        }

        /*
         * Read benchmark function parameters from text file
         * filename = text file name
         * return function name, lower boundary, upper boundary, best known/optimal value
         */
        static (string, double, double, double) ReadBenchmark(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            string line = sr.ReadLine();
            string name = "blank"; double lb = 0; double ub = 0; double optimum = 0;
            while (line != null)
            {
                // Get rid of comments
                line = line.Split('*')[0].Trim();
                // Split line to parameter name and value
                string[] parameters = line.Split('=');
                switch (parameters[0])
                {
                    case "Name":
                        name = parameters[1];
                        break;
                    case "Arg_Range_1":
                        lb = Double.Parse(parameters[1]);
                        break;
                    case "Arg_Range_2":
                        ub = Double.Parse(parameters[1]);
                        break;
                    case "Best_known/optimal_value":
                        optimum = Double.Parse(parameters[1]);
                        break;
                    default:
                        break;
                }
                line = sr.ReadLine();
            }
            sr.Close();
            return (name, lb, ub, optimum);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Filter = "txt files (*.txt)|*.txt";

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                label1.Text = openFileDialog.FileName;
            }
        }

    }
}
