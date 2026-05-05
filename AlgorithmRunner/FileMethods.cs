using CONTOPT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using Excel = Microsoft.Office.Interop.Excel;

namespace AlgorithmRunner
{
    internal class FileMethods
    {
        /*
         * Read benchmark function parameters from text file
         * filename = text file name
         * return function name, lower boundary, upper boundary, best known/optimal value
         */
        public static (string, double, double, double) ReadBenchmark(string filename)
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

        /*
         * Read parameters from text file
         * filename = text file name
         * return population size, dimension count, maximum iterations
         */
        public static (int, int, int, double, double) ReadParameters(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            string line = sr.ReadLine();
            int PS = -1; int Iter_N = -1; int alpha = -1; double mu = -1; double epsilon = -1;
            while (line != null)
            {
                // Get rid of comments
                line = line.Split('*')[0].Trim();
                // Split line to parameter name and value
                string[] parameters = line.Split('=');
                switch (parameters[0])
                {
                    case "PS":
                        PS = Int32.Parse(parameters[1]);
                        break;
                    case "Iter_N":
                        Iter_N = Int32.Parse(parameters[1]);
                        break;
                    case "alpha":
                        alpha = Int32.Parse(parameters[1]); 
                        break;
                    case "mu":
                        mu = Double.Parse(parameters[1]);
                        break;
                    case "epsilon":
                        epsilon = Double.Parse(parameters[1]);
                        break;
                    default:
                        break;
                }
                line = sr.ReadLine();
            }
            sr.Close();
            return (PS, Iter_N, alpha, mu, epsilon);
        }

        public static void SaveParameters(string filename, int PS, int Iter_N)
        {
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("PS = " + PS);
            sw.WriteLine("Iter_N = " + Iter_N);
            sw.Close();
        }

        public static List<string> ReadComponent(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            string line = sr.ReadLine();
            List<string> names = new List<string>();
            while (line != null)
            {
                // Get rid of comments
                line = line.Split('*')[0].Trim();
                // Split line to parameter name and value
                string[] parameters = line.Split('=');
                switch (parameters[0])
                {
                    case "Name":
                        string nameLine = parameters[1];
                        foreach (string name in nameLine.Split(';')) { names.Add(name.Trim()); }
                        break;
                    default:
                        break;
                }
                line = sr.ReadLine();
            }
            sr.Close();
            return (names);
        }

        public static List<C> PopulateComboBox<T, C>(string directory, System.Windows.Forms.ComboBox comboBox, List<C> components)
            where C : Form1.ComponentMethod
        {
            try {
                comboBox.Items.Clear();
                List<string> names;
                foreach (string file in Directory.GetFiles(directory, "*.dat"))
                {
                    names = ReadComponent(file);
                    foreach (string name in names)
                    {
                        MethodInfo method = typeof(ContOpt).GetMethod(name);
                        T function = (T)(object)Delegate.CreateDelegate(typeof(T), method);
                        
                        C component = (C)Activator.CreateInstance(typeof(C), new object[] { name, function });
                        components.Add(component);
                    }
                }
                comboBox.Items.AddRange(components.Select(m => m.name).ToArray());
                comboBox.SelectedIndex = 0;
            } catch (Exception ex) {
                MessageBox.Show("Error: " + ex);
            }
            return components;
        }

        public static void SaveDataGridToExcel(DataGridView dataGridView1)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Excel Workbook|*.xls" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Excel.Application xlApp = new Excel.Application();
                    Excel.Workbook xlWorkBook = xlApp.Workbooks.Add(Type.Missing);
                    Excel.Worksheet xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                    // Export headers
                    for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                    {
                        xlWorkSheet.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                    }

                    // Export data
                    for (int i = 0; i < dataGridView1.Rows.Count; i++)
                    {
                        for (int j = 0; j < dataGridView1.Columns.Count; j++)
                        {
                            xlWorkSheet.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value?.ToString();
                        }
                    }

                    xlWorkBook.SaveAs(saveFileDialog.FileName);
                    xlWorkBook.Close();
                    xlApp.Quit();

                    // Clean up COM objects to prevent memory leaks
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkSheet);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkBook);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                }
            }
        }

        public static void SaveDataGridToText(DataGridView dataGridView1, int PS, int M_Iter, int runs, int alpha, double mu, double epsilon, string numGen, string MOAMOP, string solutionInit, string stepCalc)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "txt file|*.txt" })
            {
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, true)) 
                    {
                        sw.WriteLine();
                        // Export parameter list
                        sw.WriteLine(" Population size = {0}, Maximum iterations = {1}, Amount of runs = {2}, α = {3}, μ = {4}, ε = {5}, Number generator = {6}, MOA and MOP = {7}, Solution initialization = {8}, Step calculation = {9}",
                            PS, M_Iter, runs, alpha, mu, epsilon, numGen, MOAMOP, solutionInit, stepCalc);

                        // Export headers
                        sw.WriteLine("| ID  | Benchmark Function | Dimensions | Known Optimum | Reached Optimum | Time Elapsed, ms |");

                        // Export data
                        for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                        {
                            sw.Write("|");
                            sw.Write(" {0,3} |", dataGridView1.Rows[i].Cells[0].Value);
                            sw.Write(" {0,-18} |", dataGridView1.Rows[i].Cells[1].Value);
                            sw.Write(" {0,10} |", dataGridView1.Rows[i].Cells[2].Value);
                            sw.Write(" {0,13:F6} |", dataGridView1.Rows[i].Cells[3].Value);
                            sw.Write(" {0,15:F6} |", dataGridView1.Rows[i].Cells[4].Value);
                            sw.WriteLine(" {0,16} |", dataGridView1.Rows[i].Cells[5].Value);
                        }
                    }
                }
            }
        }

        public static string ArrayToString<T>(T[] X)
        {
            string line = "";
            foreach (T element in X)
            {
                line += "\n"+element.ToString();
            }
            return line;
        }
        public static string StringListToString(List<string> X)
        {
            string line = "";
            foreach (var element in X)
            {
                line += "\n" + element.ToString();
            }
            return line;
        }

        public static string MatrixToString(double[,] X)
        {
            string line = "";
            for (int i = 0; i < X.GetLength(0); i++)
            {
                for (int j = 0; j < X.GetLength(1); j++)
                {
                    line += X[i, j].ToString() + " ";
                }
                line += "\n";
            }
            return line;
        }

        public static void TempMethod(double[,] X, double[] fitness, int bestIdx)
        {
            Console.WriteLine("{");
            for (int j = 0; j < X.GetLength(1); j++)
            {
                Console.WriteLine("  " + X[bestIdx, j] + ",");
            }
            Console.WriteLine("}");
            Console.WriteLine(fitness[bestIdx]);
        }
    }
}
