// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% RandomInitialization.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using System;
using System.Runtime.CompilerServices;

namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        private static Random _rnd = new Random();

        /*
         * Generate a matrix of random solutions
         * PS = population size (rows)
         * D = dimension count (columns)
         * ub = upper boundary
         * lb = lower boundary
         * return random solution matrix
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[,] RandomInitialization(int PS, int D, double ub, double lb)
        {
            double[,] X = new double[PS, D];

            for (int i = 0; i < PS; i++)
            {
                for (int j = 0; j < D; j++)
                {
                    double r = _rnd.NextDouble();
                    X[i, j] = r * (ub - lb) + lb;
                }
            }
            Console.WriteLine("Using base!");
            return X;
        }
    }
}
