// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% StepCalculation.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using AlgorithmRunner;
using System;
using System.Runtime.CompilerServices;

namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[,] BaseStepCalc(double[,] X, int D, double epsilon, double mu, double lb, double ub, double MOA, double MOP, 
            int bestIdx, Func<double> map, Func<double[], int, double> objectiveFunction)
        {
            for (int i = 0; i < X.GetLength(0); i++)
            {
                for (int j = 0; j < X.GetLength(1); j++)
                {
                    double r1 = map();
                    double r2 = map();
                    double r3 = map();
                    if (r1 > MOA)
                    {
                        // Exploration
                        if (r2 < 0.5)
                        {
                            // Apply Division math operator
                            X[i, j] = X[bestIdx, j] % (MOP + epsilon) * ((ub - lb) * mu + lb);
                        }
                        else
                        {
                            // Apply Multiplication math operator
                            X[i, j] = X[bestIdx, j] * MOP * ((ub - lb) * mu + lb);
                        }
                    }
                    else
                    {
                        // Exploitation
                        if (r3 < 0.5)
                        {
                            // Apply Subtraction math operator
                            X[i, j] = X[bestIdx, j] - MOP * ((ub - lb) * mu + lb);
                        }
                        else
                        {
                            // Apply Addition math operator
                            X[i, j] = X[bestIdx, j] + MOP * ((ub - lb) * mu + lb);
                        }
                    }
                }
            }
            return X;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[,] LevyRandomStepCalc(double[,] X, int D, double epsilon, double mu, double lb, double ub, double MOA, double MOP,
            int bestIdx, Func<double> map, Func<double[], int, double> objectiveFunction)
        {
            for (int i = 0; i < X.GetLength(0); i++)
            {
                for (int j = 0; j < X.GetLength(1); j++)
                {
                    double r1 = map();
                    double r2 = map();
                    double r3 = map();
                    double S = AlgorithmMethods.CalculateLRS(1.5, map);
                    if (r1 > MOA)
                    {
                        // Exploration
                        if (r2 < 0.5)
                        {
                            // Apply Division math operator
                            X[i, j] = X[bestIdx, j] % S * (MOP + epsilon) * ((ub - lb) * mu + lb);
                        }
                        else
                        {
                            // Apply Multiplication math operator
                            X[i, j] = X[bestIdx, j] * S * MOP * ((ub - lb) * mu + lb);
                        }
                    }
                    else
                    {
                        // Exploitation
                        if (r3 < 0.5)
                        {
                            // Apply Subtraction math operator
                            X[i, j] = X[bestIdx, j] - S * MOP * ((ub - lb) * mu + lb);
                        }
                        else
                        {
                            // Apply Addition math operator
                            X[i, j] = X[bestIdx, j] + S * MOP * ((ub - lb) * mu + lb);
                        }
                    }
                }
            }
            return X;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double[,] OppoStepCalc(double[,] X, int D, double epsilon, double mu, double lb, double ub, double MOA, double MOP, 
            int bestIdx, Func<double> map, Func<double[], int, double> objectiveFunction)
        {
            for (int i = 0; i < X.GetLength(0); i++)
            {
                double[] Xpos = new double[X.GetLength(1)];
                double[] OppXpos = new double[X.GetLength(1)];

                for (int j = 0; j < X.GetLength(1); j++)
                {
                    double r1 = map();
                    double r2 = map();
                    double r3 = map();
                    if (r1 > MOA)
                    {
                        // Exploration
                        if (r2 < 0.5)
                        {
                            // Apply Division math operator
                            Xpos[j] = X[bestIdx, j] % (MOP + epsilon) * ((ub - lb) * mu + lb);
                        }
                        else
                        {
                            // Apply Multiplication math operator
                            Xpos[j] = X[bestIdx, j] * MOP * ((ub - lb) * mu + lb);
                        }
                    }
                    else
                    {
                        // Exploitation
                        if (r3 < 0.5)
                        {
                            // Apply Subtraction math operator
                            Xpos[j] = X[bestIdx, j] - MOP * ((ub - lb) * mu + lb);
                        }
                        else
                        {
                            // Apply Addition math operator
                            Xpos[j] = X[bestIdx, j] + MOP * ((ub - lb) * mu + lb);
                        }
                    }
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
        public static double[,] RandomOppoStepCalc(double[,] X, int D, double epsilon, double mu, double lb, double ub, double MOA, double MOP,
            int bestIdx, Func<double> map, Func<double[], int, double> objectiveFunction)
        {
            for (int i = 0; i < X.GetLength(0); i++)
            {
                double[] Xpos = new double[X.GetLength(1)];
                double[] OppXpos = new double[X.GetLength(1)];

                for (int j = 0; j < X.GetLength(1); j++)
                {
                    double r1 = map();
                    double r2 = map();
                    double r3 = map();
                    if (r1 > MOA)
                    {
                        // Exploration
                        if (r2 < 0.5)
                        {
                            // Apply Division math operator
                            Xpos[j] = X[bestIdx, j] % (MOP + epsilon) * ((ub - lb) * mu + lb);
                        }
                        else
                        {
                            // Apply Multiplication math operator
                            Xpos[j] = X[bestIdx, j] * MOP * ((ub - lb) * mu + lb);
                        }
                    }
                    else
                    {
                        // Exploitation
                        if (r3 < 0.5)
                        {
                            // Apply Subtraction math operator
                            Xpos[j] = X[bestIdx, j] - MOP * ((ub - lb) * mu + lb);
                        }
                        else
                        {
                            // Apply Addition math operator
                            Xpos[j] = X[bestIdx, j] + MOP * ((ub - lb) * mu + lb);
                        }
                    }

                    double r = map();
                    OppXpos[j] = ub + lb - r * Xpos[j];
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
    }
}
