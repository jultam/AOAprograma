using Meta.Numerics.Functions;
using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        private static Random _rnd = new Random();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double BetaDist()
        {
            double a = 3; double b = 2; double x = _rnd.NextDouble();
            //double gammas = AdvancedMath.Gamma(a + b) / (AdvancedMath.Gamma(a) + AdvancedMath.Gamma(b));
            //double X = gammas * Math.Pow(x, a - 1) * Math.Pow(1 - x, b - 1);
            lastX = Beta.Sample(a, b);
            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double RayleighDist()
        {
            lastX = Rayleigh.Sample(0.4);
            return Rayleigh.Sample(0.4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double LatinHypercubeSample()
        {
            return Rayleigh.Sample(0.4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double SobolLowDiscrepancySeq()
        {
            return Rayleigh.Sample(0.4);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UniformDist()
        {
            return _rnd.NextDouble();
        }
    }
}
