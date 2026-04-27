// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Rosenbrock.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using System;
using System.Runtime.CompilerServices;

namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Rosenbrock(double[] x, int d)
        // calculation of the value of the objective function (3) (Rosenbrock function)
        // input: x - the current argument (solution),
        //        d - the number of dimensions;
        // output: value of the objective function
        {
            int i = 1, d_minus_1 = d - 1;
            double value = 0.0;

            loop: 
                if (d_minus_1 < i) { return 100.0 * (x[i - 1] - x[i - 1] * x[i - 1]) * (x[i - 1] - x[i - 1] * x[i - 1]) +
                         (x[i - 1] - 1.0) * (x[i - 1] - 1.0); }
                value += 100.0 * (x[i-1 + 1] - x[i-1] * x[i-1]) * (x[i-1 + 1] - x[i-1] * x[i-1]) + 
                         (x[i-1] - 1.0) * (x[i-1] - 1.0);
                if (i < d_minus_1) { ++i; goto loop; }
            return value;
        }
    }
}