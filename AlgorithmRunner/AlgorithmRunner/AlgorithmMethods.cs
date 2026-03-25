using CONTOPT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        /*
         * C_Iter is current iteration
         * M_Iter is maximum number of iterations
         * Min and Max are minimum and maximum values of the accelerated function
         * return function value at the tth iteration
         */
        public static double CalculateMOA(int C_Iter, int M_Iter, double Min, double Max)
        {
            return Min + C_Iter * (Max - Min) / M_Iter;
        }

        /*
         * C_Iter is current iteration
         * M_Iter is maximum number of iterations
         * return MOP
         */
        public static double CalculateMOP(int C_Iter, int M_Iter, int alpha)
        {
            return 1 - Math.Pow(C_Iter, 1.0 / alpha) / Math.Pow(M_Iter, 1.0 / alpha);
        }
    }
}
