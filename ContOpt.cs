// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% ContOpt.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using System;

namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        static void Main(string[] args)
        // main program: 
        // function minimizer (optimizer) based on the arithmetic optimization algorithm (AOA)
        // (the goal is to search for a variable (argument) Final_X from [X_Range1, X_Range2] such that 
        // the given objective function is minimized        
        // program input: the given benchmark test function,
        //                the user-defined parameters,
        //                D - the number of dimensions (args[1]),
        //                Run_N - the number of runs of the algorithm (args[2]);
        // program output: Final_X - final/overall best found solution (argument),
        //                 Final_F - final/overall best found objective function value
        {
            if ((args[0].Length == 0) || (args[1].Length == 0) || (args[2].Length == 0))
            {
                Console.WriteLine("Program usage: CONTOPT.exe data_file_name #_of_dimensions " + 
                                  "run_n");
                Console.ReadKey();
                return;
            }

            D = (int)Convert.ToInt16(args[1]); // number of dimensions
                                               // (1 <= D <= MAX_D)
            if ((D < 1) || (D > MAX_D))
            {
                Console.WriteLine("Incorrect number of dimensions. Program aborted");
                Console.ReadKey();
                return;
            }
                        
            Run_N = (short)Convert.ToInt16(args[2]); // number of runs of the algorithm
                                                     // (1 <= Run_N <= MAX_RUN_N)
            if ((Run_N < 1) || (Run_N > MAX_RUN_N))
            {
                Console.WriteLine("Incorrect number of runs of the algorithm. Program aborted");
                Console.ReadKey();
                return;
            }

            // ----------------------------------------------------------------------------------------------
            // Getting data
            // ----------------------------------------------------------------------------------------------
            string data_file_name = Data_Directory_Name + (File_Name_1 = args[0]); // name of the data file
            bool rc = OK; // return code
            Get_Data(data_file_name, ref rc);
            if (rc != OK) return; // finish (abort) program

            // ----------------------------------------------------------------------------------------------
            // Variables initialization, initialization of global (common) arrays (memory allocation) (1)
            // ----------------------------------------------------------------------------------------------
            Variables_Initialization_1();
            Arrays_Initialization_1();
            Assign_Values(); // assigning initial array values
            
            // ----------------------------------------------------------------------------------------------
            // Getting parameters, initialization of parameter values
            // ----------------------------------------------------------------------------------------------
            string param_file_name = Work_Directory_Name + File_Name_2; // name of the parameters file
            Get_Param(param_file_name, ref rc);
            if (rc != OK) return; // finish (abort) program
            Parameters_Initialization();
            
            // ----------------------------------------------------------------------------------------------
            // Variables initialization, initialization of global (common) arrays (memory allocation) (2)
            // ----------------------------------------------------------------------------------------------
            Variables_Initialization_2();
            Arrays_Initialization_2();
            
            // ----------------------------------------------------------------------------------------------
            // Printing information to console
            // ----------------------------------------------------------------------------------------------
            //Console.Write(TEXT_SEPARATOR_HORIZONTAL_LINE);
            Console.BackgroundColor = LINE_BACKGROUND_COLOR;
            Console.ForegroundColor = LINE_FOREGROUND_COLOR;
            Console.WriteLine();
            Console.Write("Arithmetic optimization algorithm for function optimization");
            Console.SetCursorPosition(Cursor_Location_1, Cursor_Location_2);
            Console.ResetColor();
            Console.WriteLine();
            //Console.Write(TEXT_SEPARATOR_HORIZONTAL_LINE);
            Console.WriteLine("Data file name: {0} ", File_Name_1);
            Console.WriteLine("Number of dimensions: {0}", D);
            Console.WriteLine("Number of runs: {0}", Run_N);
            //Console.Write(TEXT_SEPARATOR_DASHED_LINE);
            
            // ----------------------------------------------------------------------------------------------
            // Execution of the arithmetic optimization algorithm
            // ----------------------------------------------------------------------------------------------
            Initialize_Random_Number_Generator(Rand_Seed); // initializing random number generator
                                                           // with the current seed value
            short run_index = 1; // current run index
            Start_Time = DateTime.Now; // get algorithm start time
            do // perform multiple runs of the algorithm
            {
                Best_F = LARGE_NUMBER;
                Console.Write(".");
                switch (Benchmark_Function)
                {
                    case GRI: // Griewank  function
                         Arithmetic_Optimization_Algorithm_1(ref D, ref PS, ref Iter_N);
                         break;
                    case RAS: // Rastrigin function
                         Arithmetic_Optimization_Algorithm_2(ref D, ref PS, ref Iter_N);
                         break;
                    case ROS: // Rosenbrock function
                         Arithmetic_Optimization_Algorithm_3(ref D, ref PS, ref Iter_N);
                         break;
                    case SCH: // Schwefel function
                         Arithmetic_Optimization_Algorithm_4(ref D, ref PS, ref Iter_N);
                         break;
                }
                F_Sum += Best_F;
                if (Best_F < Final_F)
                {
                    Copy(Best_X, Final_X); // memorize argument corresponding to the final 
                                           // (best found) objective function value
                    Final_F = Best_F; // memorize final (best found) objective function value
                    Console.WriteLine("\nFinal_F: {0:0.00000000}", Final_F);
                    Console.WriteLine("Final_X:");
                    for (int i = 1; i <= D; ++i)
                        Console.Write(" {0:0.00000000}", Final_X[i]);
                    Console.WriteLine();
                }
                if (Math.Abs(Best_F - Best_Known_Value) < LIMITING_VALUE) ++Best_Known_Solution_N;
            } while (++run_index <= Run_N);
            End_Time = DateTime.Now;
            Elapsed_Run_Time = Elapsed_Time(); // calculation of the elapsed run time of the algorithm
            
            // ----------------------------------------------------------------------------------------------
            // Printing results to file
            // ----------------------------------------------------------------------------------------------
            string result_file_name = Work_Directory_Name + File_Name_3; // name of the results file
            Print_Results(result_file_name);
            
            // ----------------------------------------------------------------------------------------------
            // Printing results to console
            // ----------------------------------------------------------------------------------------------
            Console.WriteLine("\nFinal/best found objective function value over {0} run(s): " + 
                              "{1:0.00000000} ({2:0.00000000})",
                              Run_N, Final_F, Objective_Function_Value_1(ref Final_X, ref D));
            Console.WriteLine("Final/best found argument (solutions) value:");
            for (int i = 1; i <= D; ++i)
                Console.Write(" {0:0.00000000}", Final_X[i]);
            Console.WriteLine();
            Console.WriteLine("Elapsed time (sec): {0,1}.{1:000}",
                              Elapsed_Run_Time / (long)1000, Elapsed_Run_Time % (long)1000);
            Console.ReadKey();
        }
    }
}