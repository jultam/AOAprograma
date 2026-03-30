// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% SolutionInitialization.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using System;
using System.Runtime.CompilerServices;

namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[,] BaseInitialization(int PS, int D, double ub, double lb, Func<double> map)
        {
            double[,] X = new double[PS, D];

            for (int i = 0; i < PS; i++)
            {
                for (int j = 0; j < D; j++)
                {
                    double r = map();
                    X[i, j] = r * (ub - lb) + lb;
                }
            }
            return X;
        }



    }
}