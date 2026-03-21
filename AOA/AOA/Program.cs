using System;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using CONTOPT;

namespace AOA
{
    class Program
    {
        private static Random rnd = new Random();

        /*
         * Calculate fitness functions for all solutions
         * X = solution position matrix
         * D = dimension count
         * return value matrix
         */
        static double[] CalculateFitnessFunctions(double[,] X, int D)
        {
            double[] solutions = new double[X.GetLength(0)];
            // Cycle through all solutions
            for (int i = 0; i < X.GetLength(0); i++)
            {
                // Convert matrix row to an array
                double[] positions = ExtractSolutionPositions(X, i);
                // Get solution value using the objective function
                solutions[i] = ContOpt.Objective_Function_Value_1(positions, D);
            }
            return solutions;
        } 

        /*
         * Convert matrix row to an array
         * X = matrix
         * i = row index
         * return position array
         */
        static double[] ExtractSolutionPositions(double[,] X, int i)
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
        static int FindBestSolution(double[] fitness)
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
         * Generate a matrix of random solutions
         * PS = population size (rows)
         * D = dimension count (columns)
         * ub = upper boundary
         * lb = lower boundary
         * return random solution matrix
         */
        static double[,] GenerateRandomSolutions(int PS, int D, double ub, double lb)
        {
            double[,] X = new double[PS, D];

            for (int i = 0; i < PS; i++)
            {
                for (int j = 0; j < D; j++)
                {
                    double r = rnd.NextDouble();
                    X[i, j] = r * (ub - lb) + lb;
                }
            }

            return X;
        }

        /*
         * Clamp solution positions to boundaries when they exit the boundaries
         * X = solution position matrix
         * ub = upper boundary
         * lb = lower boundary
         * return clamped solution position matrix
         */
        static double[,] ClampSolutions(double[,] X, double ub, double lb)
        {
            for (int i = 0; i < X.GetLength(0); i++)
            {
                for (int j = 0; j < X.GetLength(1); j++)
                {
                    if (X[i,j] > ub) X[i,j] = ub;
                    else if (X[i,j] < lb) X[i,j] = lb;
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
        static double CalculateMOA(int C_Iter, int M_Iter, double Min, double Max) 
        { 
            return Min + C_Iter * (Max - Min) / M_Iter;
        }

        /*
         * C_Iter is current iteration
         * M_Iter is maximum number of iterations
         * return MOP
         */
        static double CalculateMOP(int C_Iter, int M_Iter, int alpha)
        {
            return 1 - Math.Pow(C_Iter, 1.0 / alpha) / Math.Pow(M_Iter, 1.0 / alpha);
        }

        static void Main(string[] args)
        {
            // Initialize PS = solution count and D = dimension count
            int PS; int D;
            // Initialize iteration counter
            int C_Iter = 1; int M_Iter;
            // Read parameters from parameters text file
            (PS, D, M_Iter) = ReadParameters("../../../param.txt");

            // Initialize benchmark function name and best known/optimal value
            string benchName; double optimum;
            // Initialize upper and lower boundaries
            double ub; double lb;
            (benchName, lb, ub, optimum) = ReadBenchmark("../../../benchmark/Griewank.dat");
            Console.WriteLine(benchName + " " + lb + " " + ub + " " + optimum);
            
            // Initialize Arithmetic Optimization Algorithm parameters
            int alpha = 5; double mu = 0.6; int epsilon = 1;
            // Initialize random solution positions
            double[,] X = GenerateRandomSolutions(PS, D, ub, lb);
            // Initialize MOA and MOP variables
            double MOA; double MOP;

            int bestIdx = 0; double[] fitness = new double[PS];

            while (C_Iter < M_Iter)
            {
                fitness = CalculateFitnessFunctions(X, D);
                bestIdx = FindBestSolution(fitness);
                MOA = CalculateMOA(C_Iter, M_Iter, 0.2, 0.9); // verify min and max
                MOP = CalculateMOP(C_Iter, M_Iter, alpha);

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
                X = ClampSolutions(X, ub, lb); // Not included in original algorithm
                C_Iter++;
            }

            fitness = CalculateFitnessFunctions(X, D);
            bestIdx = FindBestSolution(fitness);
            Console.WriteLine("{");
            for (int j = 0; j < X.GetLength(1); j++)
            {
                Console.WriteLine("  "+X[bestIdx, j]+",");
            }
            Console.WriteLine("}");
            Console.WriteLine(fitness[bestIdx]);

        }

        /*
         * Read parameters from text file
         * filename = text file name
         * return population size, dimension count, maximum iterations
         */
        static (int, int, int) ReadParameters(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            string line = sr.ReadLine();
            int PS = -1; int D = -1; int Iter_N = -1;
            while (line != null)
            {
                // Get rid of comments
                line = line.Split('*')[0].Trim();
                // Split line to parameter name and value
                string[] parameters = line.Split("=");
                switch (parameters[0])
                {
                    case "PS":
                        PS = Int32.Parse(parameters[1]);
                        break;
                    case "D":
                        D = Int32.Parse(parameters[1]);
                        break;
                    case "Iter_N":
                        Iter_N = Int32.Parse(parameters[1]);
                        break;
                    default:
                        break;
                }
                line = sr.ReadLine();
            }
            sr.Close();
            return (PS, D, Iter_N);
        }

        /*
         * Read benchmark function parameters from text file
         * filename = text file name
         * return function name, lower boundary, upper boundary, best known/optimal value
         */
        static (string, double, double, double) ReadBenchmark(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            string line = sr.ReadLine();
            string name = "blank"; double lb = 0; double ub = 0; double optimum = 0;
            while (line != null)
            {
                // Get rid of comments
                line = line.Split('*')[0].Trim();
                // Split line to parameter name and value
                string[] parameters = line.Split("=");
                switch (parameters[0])
                {
                    case "Name":
                        name = parameters[1];
                        break;
                    case "Arg_Range_1":
                        lb = Double.Parse(parameters[1]);
                        break;
                    case "Arg_Range_2":
                        ub = Double.Parse(parameters[1]);
                        break;
                    case "Best_known/optimal_value":
                        optimum = Double.Parse(parameters[1]);
                        break;
                    default:
                        break;
                }
                line = sr.ReadLine();
            }
            sr.Close();
            return (name, lb, ub, optimum);
        }

        static void PrintArray(double[] X)
        {
            foreach (double element in X)
            {
                Console.WriteLine(element);
            }
        }

        static void PrintMatrix(double[,] X)
        {
            for (int i = 0; i < X.GetLength(0); i++)
            {
                for(int j = 0; j < X.GetLength(1); j++)
                {
                    Console.Write(X[i, j]+" ");
                }
                Console.WriteLine();
            }
        }
    }
}