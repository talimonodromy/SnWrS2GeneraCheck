using System;
using System.Collections.Generic;
using System.Text;

namespace SnS2RamificationCheck
{
    public enum Parity
    {
        Even = 0,
        Odd = 1
    }
    public static class ParityExtensions
    {
        public static bool SignHom(this Parity value)
        {
            // even is true "true" ones
            return value == Parity.Even;
        }
    }
}
