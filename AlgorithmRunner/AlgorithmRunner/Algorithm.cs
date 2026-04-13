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
            Form1.BenchmarkFunction benchmark, Func<double> NumGeneratorMethod, Func<int, int, int, (double, double)> MOAMOPMethod,
            Func<int, int, double, double, Func<double>, Func<double[], int, double>, double[,]> InitializationMethod,
            Func<double[,], int, double, double, double, double, double, double, int, Func<double>, Func<double[], int, double>, double[,]> StepMethod)
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

                // Initialize starting solution positions
                double[,] X = InitializationMethod(PS, D, ub, lb, NumGeneratorMethod, benchmark.function);

                while (C_Iter < M_Iter)
                {
                    fitness = AlgorithmMethods.CalculateFitnessFunctions(X, D, benchmark.function);
                    bestIdx = AlgorithmMethods.FindBestSolution(fitness);
                    (MOA, MOP) = MOAMOPMethod(C_Iter, M_Iter, alpha);

                    X = StepMethod(X, D, epsilon, mu, lb, ub, MOA, MOP, bestIdx, NumGeneratorMethod, benchmark.function);
                    X = AlgorithmMethods.ClampSolutions(X, ub, lb); // Not included in original algorithm
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
