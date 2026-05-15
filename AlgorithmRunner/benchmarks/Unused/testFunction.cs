// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Schwefel.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using System;
using System.Runtime.CompilerServices;

namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double testFunction (double[] x, int d)
        // calculation of the value of the objective function (4) (Schwefel function)
        // input: x - the current argument (solution),
        //        d - the number of dimensions;
        // output: value of the objective function
        {
            int i = 1;
            double value = 0.0;

        loop:
            value += x[i - 1] * x[i - 1];
                if (i < d) { ++i; goto loop; }
            return value;
        }
    }
}