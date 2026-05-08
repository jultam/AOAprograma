// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% SolutionInitialization.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[,] BaseRandomInit(int PS, int D, double ub, double lb, Func<double> map, Func<double[], int, double> objectiveFunction)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[,] OppositionInit(int PS, int D, double ub, double lb, Func<double> map, Func<double[], int, double> objectiveFunction)
        {
            double[,] X = new double[PS, D];

            for (int i = 0; i < PS; i++)
            {
                double[] Xpos = new double[X.GetLength(1)];
                double[] OppXpos = new double[X.GetLength(1)];

                for (int j = 0; j < D; j++)
                {
                    double r = map();
                    Xpos[j] = r * (ub - lb) + lb;
                    OppXpos[j] = ub + lb - Xpos[j];
                }

                double[] best;
                if (objectiveFunction(Xpos, D) < objectiveFunction(OppXpos, D)) best = Xpos;
                else best = OppXpos;

                for (int j = 0; j < D; j++)
                {
                    X[i, j] = best[j];
                }
            }
            return X;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[,] RandomOppositionInit(int PS, int D, double ub, double lb, Func<double> map, Func<double[], int, double> objectiveFunction)
        {
            double[,] X = new double[PS, D];

            for (int i = 0; i < PS; i++)
            {
                double[] Xpos = new double[X.GetLength(1)];
                double[] OppXpos = new double[X.GetLength(1)];

                for (int j = 0; j < D; j++)
                {
                    double r1 = map(); double r2 = map();
                    Xpos[j] = r1 * (ub - lb) + lb;
                    OppXpos[j] = ub + lb - r2 * Xpos[j];
                }

                double[] best;
                if (objectiveFunction(Xpos, D) < objectiveFunction(OppXpos, D)) best = Xpos;
                else best = OppXpos;

                for (int j = 0; j < D; j++)
                {
                    X[i, j] = best[j];
                }
            }
            return X;
        }

        class Solution
        {
            public double value;
            public double[] position;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[,] MergedOppoInit(int PS, int D, double ub, double lb, Func<double> map, Func<double[], int, double> objectiveFunction)
        {
            double[,] X = new double[PS, D];
            List<Solution> mergedSolutions = new List<Solution> ();

            for (int i = 0; i < PS; i++)
            {
                double[] Xpos = new double[X.GetLength(1)];
                double[] OppXpos = new double[X.GetLength(1)];

                for (int j = 0; j < D; j++)
                {
                    double r1 = map(); double r2 = map();
                    Xpos[j] = r1 * (ub - lb) + lb;
                    OppXpos[j] = ub + lb - r2 * Xpos[j];
                }

                mergedSolutions.Add(new Solution { value = objectiveFunction(Xpos, D), position = Xpos });
                mergedSolutions.Add(new Solution { value = objectiveFunction(OppXpos, D), position = OppXpos });
            }

            double[][] finalSolutions = mergedSolutions.OrderBy(sol => sol.value).Select(sol => sol.position).Take(PS).ToArray();
            for (int i = 0; i < PS; i++)
            {
                for (int j = 0; j < D; j++)
                {
                    X[i, j] = finalSolutions[i][j];
                }
            }

            return X;
        }
    }
}