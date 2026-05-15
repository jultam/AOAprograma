using Meta.Numerics.Functions;
using System;
using System.Runtime.CompilerServices;

namespace CONTOPT
{
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    sealed partial class ContOpt
    // <><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><><>
    {
        private static Random _rnd = new Random();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double UniformDist()
        {
            double value = _rnd.NextDouble();
            return value;
        }
    }
}
