// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% MOAMOP.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using System;
using System.Runtime.CompilerServices;

namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        private static double lastMOA = 0.2;
        private static double lastMOP = 0.9;

        /*
         * Calculate MOA and MOP
         * return MOA and MOP tuple
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double, double) BaseMOAandMOP(int C_Iter, int M_Iter, int alpha)
        {
            double Min = 0.2; double Max = 0.9;

            double MOA = Min + C_Iter * (Max - Min) / M_Iter;
            double MOP = 1 - Math.Pow(C_Iter, 1.0 / alpha) / Math.Pow(M_Iter, 1.0 / alpha);

            return (MOA, MOP);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double, double) ChaoticMOAandMOP(int C_Iter, int M_Iter, int alpha)
        {
            alpha = 4;
            lastMOA = alpha * lastMOA * (1 - lastMOA);
            double value1 = Math.Pow((double)C_Iter / (double)M_Iter, 1.0 / 6.0);
            double MOA = lastMOA * value1;
            lastMOP = alpha * lastMOP * (1 - lastMOP);
            double value2 = 1 - Math.Pow((double)C_Iter / (double)M_Iter, 1.0 / 2.0);
            double MOP = lastMOP * value2;
            return (MOA, MOP);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double, double) BaseMOAChaoticMOP(int C_Iter, int M_Iter, int alpha)
        {
            double Min = 0.2; double Max = 0.9;
            double MOA = Min + C_Iter * (Max - Min) / M_Iter;
            //alpha = 4;
            lastMOP = alpha * lastMOP * (1 - lastMOP);
            double value2 = 1 - Math.Pow((double)C_Iter / (double)M_Iter, 1.0 / 2.0);
            double MOP = lastMOP * value2;
            return (MOA, MOP);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (double, double) ChaoticMOABaseMOP(int C_Iter, int M_Iter, int alpha)
        {
            //alpha = 4;
            lastMOA = alpha * lastMOA * (1 - lastMOA);
            double value1 = Math.Pow((double)C_Iter / (double)M_Iter, 1.0 / 6.0);
            double MOA = lastMOA * value1;
            double MOP = 1 - Math.Pow(C_Iter, 1.0 / alpha) / Math.Pow(M_Iter, 1.0 / alpha);
            return (MOA, MOP);
        }
    }
}