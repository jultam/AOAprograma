using CONTOPT;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmRunner
{
    internal class Algorithm
    {
        public static Results.AlgorithmResults AOA(int iterations, int PS, int M_Iter, int D, int alpha, double mu, double epsilon,
            Form1.BenchmarkFunction benchmark, Form1.Components components)
        {
            // Initialize MOA and MOP variables
            double MOA; double MOP;
            double lb = benchmark.arg_range_1; double ub = benchmark.arg_range_2;
            double optimumSum = 0; double timeSum = 0; double errorSum = 0;
            double bestOptimum = 999999; double[] bestPosition = new double[D];

            for (int iter = 0; iter < iterations; iter++)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                int C_Iter = 1;
                int bestIdx = 0; double[] fitness = new double[PS];
                double bestFoundOptimum = 99999.0;

                // Initialize starting solution positions
                double[,] X = components.initMethod(PS, D, ub, lb, components.mapMethod, benchmark.function);
                //fitness = AlgorithmMethods.CalculateFitnessFunctions(X, D, benchmark.function);

                while (C_Iter < M_Iter)
                {
                    fitness = AlgorithmMethods.CalculateFitnessFunctions(X, D, benchmark.function);
                    bestIdx = AlgorithmMethods.FindBestSolution(fitness);
                    if (bestFoundOptimum > fitness[bestIdx])
                    {
                        bestFoundOptimum = fitness[bestIdx];
                        bestPosition = AlgorithmMethods.ExtractSolutionPositions(X, bestIdx);
                    }

                    (MOA, MOP) = components.MOAMOPMethod(C_Iter, M_Iter, alpha);

                    X = components.stepMethod(X, D, epsilon, mu, lb, ub, MOA, MOP, bestIdx, components.mapMethod, benchmark.function);
                    X = AlgorithmMethods.ClampSolutions(X, ub, lb); // Not included in original algorithm

                    /*double[,] newX = components.stepMethod(X, D, epsilon, mu, lb, ub, MOA, MOP, bestIdx, components.mapMethod, benchmark.function);
                    newX = AlgorithmMethods.ClampSolutions(newX, ub, lb); // Not included in original algorithm

                    // Only update positions if an improvement was made (not included in original algorithm)
                    double[] newFitness = AlgorithmMethods.CalculateFitnessFunctions(X, D, benchmark.function);
                    int newBestIdx = AlgorithmMethods.FindBestSolution(newFitness);
                    if (fitness[bestIdx] > newFitness[newBestIdx])
                    {
                        fitness = newFitness;
                        bestIdx = newBestIdx;
                        X = newX;
                    }*/

                    C_Iter++;
                }
                fitness = AlgorithmMethods.CalculateFitnessFunctions(X, D, benchmark.function);
                bestIdx = AlgorithmMethods.FindBestSolution(fitness);
                optimumSum += fitness[bestIdx];
                errorSum += Math.Pow(benchmark.best_known - fitness[bestIdx], 2);

                if (fitness[bestIdx] < bestOptimum)
                {
                    bestOptimum = fitness[bestIdx];
                    bestPosition = AlgorithmMethods.ExtractSolutionPositions(X, bestIdx);
                }

                stopwatch.Stop();
                timeSum += stopwatch.Elapsed.TotalMilliseconds;
            }

            return new Results.AlgorithmResults(optimumSum / iterations, timeSum / iterations, bestPosition, errorSum / iterations);
        }
    }
}
