// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% ChaoticMaps.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
        private static Random chaosMapRandom = new Random();

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
            lastX = Math.Cos(4 * Math.Acos(lastX));
            return (lastX + 1.0) / 2.0;
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
            double p = 0.3; // (0, 1)

            if (lastX <= 0) lastX = 0.1;
            else if (lastX > 0 && lastX < p) lastX = lastX / p;
            else if (lastX >= p && lastX < 0.5) lastX = (lastX - p) / (0.5 - p);
            else if (lastX >= 0.5 && lastX < 1 - p) lastX = (1 - p - lastX) / (0.5 - p);
            else if (lastX >= 1-p && lastX < 1) lastX = (1 - lastX) / p;
            else lastX = 0.9;

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

            if (lastX <= 0) lastX = 0.01;
            else if (lastX >= 1) lastX = 0.99;

            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double MCDOLMap()
        {
            double theta = chaosMapRandom.NextDouble();

            if (theta < 0.2) lastX = CircleMap();
            else if (theta < 0.4) lastX = LogisticMap();
            else if (theta < 0.6) lastX = PiecewiseMap();
            else if (theta < 0.8) lastX = SineMap();
            else lastX = TentMap();

            return lastX;
        }
    }
}
