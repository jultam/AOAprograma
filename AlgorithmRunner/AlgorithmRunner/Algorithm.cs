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
        public static Results.OverallResults AOA(int iterations, int PS, int M_Iter, int alpha, double mu, int epsilon,
            List<Form1.BenchmarkFunction> benchmarks, Func<double> MapMethod, Func<int, int, int, (double, double)> MOAMOPMethod)
        {
            // main algorithm:
            // function minimizer (optimizer) based on the arithmetic optimization algorithm (AOA)
            // (the goal is to search for a variable (argument) Final_X from [X_Range1, X_Range2] such that 
            // the given objective function is minimized        
            // method input:  iterations - the number of runs for each test,
            //                PS - population size
            //                M_Iter - maximum iteration count for the search process
            //                alpha, mu, epsilon
            //                MapMethod - chaotic map
            // program output: Final_X - final/overall best found solution (argument),
            //                 Final_F - final/overall best found objective function value

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
                    double optimumSum = 0;
                    for (int iter = 0; iter < iterations; iter++)
                    {
                        int C_Iter = 1;
                        int bestIdx = 0; double[] fitness = new double[PS];

                        // Initialize random solution positions
                        double[,] X = AlgorithmMethods.SolutionInitialization(PS, D, ub, lb, MapMethod);

                        while (C_Iter < M_Iter)
                        {
                            fitness = AlgorithmMethods.CalculateFitnessFunctions(X, D, benchmark.function);
                            bestIdx = AlgorithmMethods.FindBestSolution(fitness);
                            (MOA, MOP) = MOAMOPMethod(C_Iter, M_Iter, alpha);
                            //MOA = AlgorithmMethods.CalculateMOA(C_Iter, M_Iter, 0.2, 0.9); // verify min and max
                            //MOP = AlgorithmMethods.CalculateMOP(C_Iter, M_Iter, alpha);

                            for (int i = 0; i < X.GetLength(0); i++)
                            {
                                for (int j = 0; j < X.GetLength(1); j++)
                                {
                                    double r1 = MapMethod();
                                    double r2 = MapMethod();
                                    double r3 = MapMethod();
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
                        optimumSum += fitness[bestIdx];
                    }

                    dimensionResults.Add(new Results.DimensionResults(D, optimumSum/iterations));
                }
                benchmarkResults.Add(new Results.BenchmarkResults(benchmark, dimensionResults));
            }
            overallResults.benchmarkResults = benchmarkResults;
            return overallResults;
        }
    }
}
