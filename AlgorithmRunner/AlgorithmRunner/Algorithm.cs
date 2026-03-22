using CONTOPT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmRunner
{
    internal class Algorithm
    {
        public static Random rnd = new Random();

        public static Results.OverallResults AOA(int PS, int M_Iter, int alpha, double mu, int epsilon, List<Form1.BenchmarkFunction> benchmarks, Func<int, int, double, double, double[,]> initMethod)
        {
            Results.OverallResults overallResults = new Results.OverallResults();

            int[] testingDimensions = { 1, 30, 100 };

            // Initialize MOA and MOP variables
            double MOA; double MOP;

            List<Results.BenchmarkResults> benchmarkResults = new List<Results.BenchmarkResults>();

            foreach (Form1.BenchmarkFunction benchmark in benchmarks) 
            {
                double lb = benchmark.arg_range_1; double ub = benchmark.arg_range_2;
                List<Results.DimensionResults> dimensionResults = new List<Results.DimensionResults>();
                foreach (int D in testingDimensions)
                {
                    int C_Iter = 1;

                    int bestIdx = 0; double[] fitness = new double[PS];

                    // Initialize random solution positions
                    double[,] X = initMethod(PS, D, ub, lb);
                    //double[,] X = ContOpt.RandomInitialization(PS, D, ub, lb);

                    while (C_Iter < M_Iter)
                    {
                        fitness = AlgorithmMethods.CalculateFitnessFunctions(X, D, benchmark.function);
                        bestIdx = AlgorithmMethods.FindBestSolution(fitness);
                        MOA = AlgorithmMethods.CalculateMOA(C_Iter, M_Iter, 0.2, 0.9); // verify min and max
                        MOP = AlgorithmMethods.CalculateMOP(C_Iter, M_Iter, alpha);

                        for (int i = 0; i < X.GetLength(0); i++)
                        {
                            for (int j = 0; j < X.GetLength(1); j++)
                            {
                                double r1 = rnd.NextDouble();
                                double r2 = rnd.NextDouble();
                                double r3 = rnd.NextDouble();
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
                        X = AlgorithmMethods.ClampSolutions(X, ub, lb); // Not included in original algorithm
                        C_Iter++;
                    }
                    fitness = AlgorithmMethods.CalculateFitnessFunctions(X, D, benchmark.function);
                    bestIdx = AlgorithmMethods.FindBestSolution(fitness);

                    dimensionResults.Add(new Results.DimensionResults(D, fitness[bestIdx]));
                    //FileMethods.TempMethod(X, fitness, bestIdx);
                }
                benchmarkResults.Add(new Results.BenchmarkResults(benchmark, dimensionResults));
            }
            overallResults.benchmarkResults = benchmarkResults;
            return overallResults;
        }
    }
}
