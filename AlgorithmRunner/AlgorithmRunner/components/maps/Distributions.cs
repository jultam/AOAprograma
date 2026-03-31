using Meta.Numerics.Functions;
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
        public static double BetaDistribution()
        {
            double a = 3; double b = 2; double x = _rnd.NextDouble();
            double gammas = AdvancedMath.Gamma(a + b) / (AdvancedMath.Gamma(a) + AdvancedMath.Gamma(b));
            double X = gammas * Math.Pow(x, a - 1) * Math.Pow(1 - x, b - 1);
            //double normX = x
            return lastX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UniformDistribution()
        {
            return _rnd.NextDouble();
        }
    }
}
