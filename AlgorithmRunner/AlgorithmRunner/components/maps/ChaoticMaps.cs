// %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% ChaoticMaps.cs %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%
using System;
using System.Runtime.CompilerServices;

namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        private static Random _rnd = new Random();
        private static double lastX = 0.6;
        /*
         * Generate a chaotic map
         * return chaotic map
         */
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ChebyshevMap()
        {
            int k = 1;
            lastX = Math.Cos(k / Math.Cos(lastX));
            lastX = (lastX + 1) / 2;
            return lastX;
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
            lastX = 1 / (lastX % 1);
            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double IterativeMap()
        {
            double a = 0.5; // (0, 1)
            lastX = Math.Sin(a * Math.PI / lastX);
            return lastX;
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
            double p = 0.25; // (0, 1)
            if (lastX >= 0 && lastX < p) lastX = lastX / p;
            else if (lastX >= p && lastX < 1 / 2) lastX = (lastX - p) / (0.5 - p);
            else if (lastX >= 1/2 && lastX < 1 - p) lastX = (1 - p - lastX) / (0.5 - p);
            else return (1 - lastX) / p;
            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SineMap()
        {
            int a = 2; // (0, 4]
            lastX = (a / 4) * Math.Sin(Math.PI * lastX);
            return lastX;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SingerMap()
        {
            double mu = 0.98; // (0.9, 1.08)
            lastX = mu * (7.86 * lastX - 23/31 * lastX * lastX + 28.75 * lastX * lastX * lastX - 13.302875 * lastX * lastX * lastX * lastX);
            return lastX;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SinusoidalMap()
        {
            double a = 2.3;
            lastX = a * lastX * lastX * Math.Sin(Math.PI * lastX);
            return lastX; // stuck at 0
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double TentMap()
        {
            double a = 0.55; // (0, 1)
            if (lastX < a) lastX = lastX / a;
            else lastX = (1 - lastX) / (1 - a);
            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double RandomMap()
        {
            return _rnd.NextDouble();
        }
    }
}
