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
        public class AlgorithmResults
        {
            public double optimum { get; }
            public double MSE { get; }
            public double time { get; }
            public double[] position { get; }
            public AlgorithmResults(double optimum, double time, double[] position, double MSE)
            {
                this.optimum = optimum;
                this.time = time;
                this.position = position;
                this.MSE = MSE;
            }
        }
    }
}
