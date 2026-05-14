// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% StepCalculation.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using Meta.Numerics.Functions;
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
                            X[i, j] = X[bestIdx, j] / ((MOP + epsilon) * ((ub - lb) * mu + lb));
                            //X[i, j] = X[bestIdx, j] / ((MOP + epsilon) * mu + lb);
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

        private static double NextNormalDistribution(double mean, double std, Func<double> map)
        {
            double u1 = map(); double u2 = map();
            double r = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + std * r;
        }

        public static double CalculateLRS(double a, Func<double> map)
        {
            double numerator = AdvancedMath.Gamma(1 + a) * Math.Sin(Math.PI * a / 2);
            double denominator = AdvancedMath.Gamma((1 + a) / 2) * a * Math.Pow(2, (a - 1) / 2);
            double qu = Math.Pow(numerator / denominator, 1 / a);
            double qv = 1;

            double u = NextNormalDistribution(0, Math.Pow(qu, 2), map);
            double v = NextNormalDistribution(0, Math.Pow(qv, 2), map);

            return u / Math.Pow(Math.Abs(v), 1 / a);
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
                    double S = CalculateLRS(1.5, map);

                    if (Math.Abs(S) < 1) 
                    {
                        if (S < 0) S = -1; 
                        else S = 1;
                    }

                    if (r1 > MOA)
                    {
                        // Exploration
                        if (r2 < 0.5)
                        {
                            // Apply Division math operator
                            X[i, j] = X[bestIdx, j] / (S * (MOP + epsilon) * ((ub - lb) * mu + lb));
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
                            Xpos[j] = X[bestIdx, j] / ((MOP + epsilon) * ((ub - lb) * mu + lb));
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

                    if (Xpos[j] < lb) Xpos[j] = lb;
                    else if (Xpos[j] > ub) Xpos[j] = ub;

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
                            Xpos[j] = X[bestIdx, j] / ((MOP + epsilon) * ((ub - lb) * mu + lb));
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

                    if (Xpos[j] < lb) Xpos[j] = lb;
                    else if (Xpos[j] > ub) Xpos[j] = ub;

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
