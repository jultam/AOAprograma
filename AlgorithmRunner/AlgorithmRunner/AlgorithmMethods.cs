using CONTOPT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics;
using Meta.Numerics;
using Meta.Numerics.Functions;

namespace AlgorithmRunner
{
    internal class AlgorithmMethods
    {
        public static double[,] SolutionInitialization(int PS, int D, double ub, double lb, Func<double> map)
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

        /*
         * Calculate fitness functions for all solutions
         * X = solution position matrix
         * D = dimension count
         * return value matrix
         */
        public static double[] CalculateFitnessFunctions(double[,] X, int D, Func<double[], int, double> objectiveFunction)
        {
            double[] solutions = new double[X.GetLength(0)];
            // Cycle through all solutions
            for (int i = 0; i < X.GetLength(0); i++)
            {
                // Convert matrix row to an array
                double[] positions = ExtractSolutionPositions(X, i);
                // Get solution value using the objective function
                solutions[i] = objectiveFunction(positions, D);
            }
            return solutions;
        }

        /*
         * Convert matrix row to an array
         * X = matrix
         * i = row index
         * return position array
         */
        public static double[] ExtractSolutionPositions(double[,] X, int i)
        {
            double[] positions = new double[X.GetLength(1)];
            for (int j = 0; j < X.GetLength(1); j++)
            {
                positions[j] = X[i, j];
            }
            return positions;
        }

        /*
         * Find the solution with the lowest value
         * fitness = fitness value of every solution
         * return best value index
         */
        public static int FindBestSolution(double[] fitness)
        {
            int bestIdx = 0;
            double bestFitness = fitness[0];
            for (int i = 1; i < fitness.Length; i++)
            {
                if (fitness[i] < bestFitness)
                {
                    bestIdx = i;
                    bestFitness = fitness[i];
                }
            }
            return bestIdx;
        }

        /*
         * Clamp solution positions to boundaries when they exit the boundaries
         * X = solution position matrix
         * ub = upper boundary
         * lb = lower boundary
         * return clamped solution position matrix
         */
        public static double[,] ClampSolutions(double[,] X, double ub, double lb)
        {
            for (int i = 0; i < X.GetLength(0); i++)
            {
                for (int j = 0; j < X.GetLength(1); j++)
                {
                    if (X[i, j] > ub) X[i, j] = ub;
                    else if (X[i, j] < lb) X[i, j] = lb;
                }
            }
            return X;
        }

        // 0 < a <= 2
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

        // Gaussian
        private static double NextNormalDistribution(double mean, double std, Func<double> map)
        {
            double u1 = map(); double u2 = map();
            double r = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + std * r;
        }
    }
}
