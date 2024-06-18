using System;
using System.Collections.Generic;
using System.Text;

namespace SnS2RamificationCheck.Objects
{
    public class ParityAssumption
    {
       
        private int _modulo;
        private int _equivTo;

        public int Modulo { get { return _modulo; }  }
        public int EquivTo { get { return _equivTo; } }


        public ParityAssumption(Parity parity)
        {
            _modulo = 2;
            _equivTo = (int)parity;
        }

        public ParityAssumption(int modulo, int equivTo)
        {
            _modulo = modulo;
            _equivTo = equivTo;

        }

        public static bool AssumptionsEqual(ParityAssumption assumption1, ParityAssumption assumption2)
        {
            return assumption1.Modulo == assumption2.Modulo && assumption1.EquivTo == assumption2.EquivTo;
        }
        public override string ToString()
        {
            return "=" + EquivTo + "mod" + Modulo;
        }

        public override int GetHashCode()
        {
            return (Modulo + EquivTo).GetHashCode();
        }
    }
}
