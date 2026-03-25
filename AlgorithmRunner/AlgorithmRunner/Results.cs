using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlgorithmRunner
{
    internal class Results
    {
        public class OverallResults
        {
            public List<BenchmarkResults> benchmarkResults { get; set; }
            public OverallResults(List<BenchmarkResults> benchmarkResults)
            {
                this.benchmarkResults = benchmarkResults;
            }
            public OverallResults() { }
        }

        public class BenchmarkResults
        {
            public Form1.BenchmarkFunction benchmark { get; }
            public List<DimensionResults> dimensionResults { get; }
            public BenchmarkResults(Form1.BenchmarkFunction benchmark, List<DimensionResults> dimensionResults)
            {
                this.benchmark = benchmark;
                this.dimensionResults = dimensionResults;
            }
        }

        public class DimensionResults
        {
            public int D { get; }
            public double optimum { get; }
            public DimensionResults(int D, double optimum)
            {
                this.D = D;
                this.optimum = optimum;
            }
        }

        public static void PopulateDataGridView(OverallResults overallResults, DataGridView dataGridView)
        {
            dataGridView.Rows.Clear();

            foreach (BenchmarkResults benchmarkResults in overallResults.benchmarkResults)
            {
                foreach (DimensionResults dimensionResults in benchmarkResults.dimensionResults)
                {
                    dataGridView.Rows.Add(benchmarkResults.benchmark.name, dimensionResults.D, dimensionResults.optimum);
                }
            }
        }
    }
}
