// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Griewank.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using System;
using System.Runtime.CompilerServices;

namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Griewank(double[] x, int d)
        // calculation of the value of the objective function (1) (Griewank function)
        // input: x - the current argument (solution),
        //        d - the number of dimensions;
        // output: value of the objective function
        {
            int i = 1;
            double value_1 = 0.0, value_2 = 1.0;

            loop_1:
                value_1 += x[i-1] * x[i-1];
                if (i < d) { ++i; goto loop_1; }
            i = 1;
            loop_2:
                value_2 *= Math.Cos(x[i-1] / Math.Sqrt((double)i));
                if (i < d) { ++i; goto loop_2; }
            return value_1 / 4000.0 - value_2 + 1.0;
        }
    }
}