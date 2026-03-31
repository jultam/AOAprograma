// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% ChaoticMaps.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using Meta.Numerics.Functions;
using System;
using System.Runtime.CompilerServices;

namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        private static double lastX = 0.6;
        private static bool firstIter = true;
        private static int currentIter = 1;

        public static void ResetMap()
        {
            lastX = 0.6;
            firstIter = true;
            currentIter = 1;
        }

        /*
         * Generate a chaotic map
         * return chaotic map
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ChebyshevMap()
        {
            lastX = Math.Cos(currentIter / Math.Cos(lastX));
            currentIter++;
            return (lastX + 1) / 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double CircleMap()
        {
            double a = 0.5; double b = 0.2;
            lastX = (lastX + b - (a / (2 * Math.PI)) * Math.Sin(2 * Math.PI * lastX)) % 1;
            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GaussMap()
        {
            if (firstIter) { lastX = 0.8495672; firstIter = false; }
            if (lastX == 0) lastX = 0;
            else lastX = 1 / lastX - Math.Floor(1 / lastX);
            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double IterativeMap()
        {
            double a = 0.786; // (0, 1)
            lastX = Math.Sin(a * Math.PI / lastX);
            return (lastX + 1) / 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double LogisticMap()
        {
            int a = 4;
            lastX = a * lastX * (1 - lastX);
            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double PiecewiseMap()
        {
            double p = 0.7; // (0, 1)
            if (lastX >= 0 && lastX < p) lastX = lastX / p;
            else if (lastX >= p && lastX < 1 / 2) lastX = (lastX - p) / (0.5 - p);
            else if (lastX >= 1/2 && lastX < 1 - p) lastX = (1 - p - lastX) / (0.5 - p);
            else lastX = (1 - lastX) / p;
            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SineMap()
        {
            double a = 3.9; // (0, 4]
            lastX = (a / 4) * Math.Sin(Math.PI * lastX);
            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SingerMap()
        {
            double mu = 1.07; // (0.9, 1.08)
            lastX = mu * (7.86 * lastX - 23.31 * lastX * lastX + 28.75 * lastX * lastX * lastX - 13.302875 * lastX * lastX * lastX * lastX);
            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SinusoidalMap()
        {
            double a = 2.3;
            lastX = a * lastX * lastX * Math.Sin(Math.PI * lastX);
            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double TentMap()
        {
            double a = 0.55; // (0, 1)
            if (lastX < a) lastX = lastX / a;
            else lastX = (1 - lastX) / (1 - a);
            return lastX;
        }
    }
}
