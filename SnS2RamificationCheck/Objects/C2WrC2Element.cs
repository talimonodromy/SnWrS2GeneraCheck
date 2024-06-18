using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SnS2RamificationCheck.Objects
{
    public class C2WrC2BranchCycle
    {
        
        public C2WrC2BranchCycle(Parity lhs,Parity rhs, bool swap)
        {
            _rhs = rhs;
            _lhs = lhs;
            _swap = swap;
        }
        private Parity _rhs;
        
        public Parity RightHandSide { get { return _rhs; } }

        private Parity _lhs;

        public Parity LeftHandSide { get { return _lhs;  } }

        private bool _swap;

        public bool Swap { get { return _swap; } }

        public override string ToString()
        {
            return String.Join(((int)RightHandSide).ToString(), ((int)LeftHandSide).ToString(), Swap ? "s" : System.String.Empty);
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                var otherElement = (C2WrC2BranchCycle)obj;
                return C2WrC2ElementComparer.SameC2wrC2Elements(this, otherElement);
            }
        }
    }

    public  class C2WrC2ElementComparer : EqualityComparer<C2WrC2BranchCycle>
    {
        public static bool SameC2wrC2Elements(C2WrC2BranchCycle x,C2WrC2BranchCycle y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            if (x == null || y == null)
            {
                return false;
            }
            return x.RightHandSide == y.RightHandSide && x.LeftHandSide == y.LeftHandSide && x.Swap == y.Swap;
        }

        public override bool Equals([AllowNull] C2WrC2BranchCycle x, [AllowNull] C2WrC2BranchCycle y)
        {
            return SameC2wrC2Elements(x, y);
        }

        public override int GetHashCode([DisallowNull] C2WrC2BranchCycle obj)
        {
            return obj.LeftHandSide.GetHashCode() + obj.LeftHandSide.GetHashCode() + obj.Swap.GetHashCode();
        }
    }
}
