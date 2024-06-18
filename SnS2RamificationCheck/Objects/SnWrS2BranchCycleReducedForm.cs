using SnS2RamificationCheck.Interfaces;
using System.Text;

namespace SnS2RamificationCheck.Objects
{

    public class SnWrS2BranchCycleReducedForm : SnSquaredBranchCycle
    {

        public SnWrS2BranchCycleReducedForm(SnBranchCycle lhs, SnBranchCycle rhs, bool swap) : base(lhs, rhs)
        {
            _swap = swap;
        }
        public SnWrS2BranchCycleReducedForm(Interfaces.Partition lhs, Interfaces.Partition rhs, bool swap) : base(lhs, rhs)
        {
            _swap = swap;
        }
        protected bool _swap = false;
        public bool Swap { get { return _swap; } }

        //TODO add constructor here to handle inserting just lhs?
        //some sugar :-)
        public bool HasSwap()
        {
            return Swap;
        }
        public bool IsIdentity()
        {
            return LeftHandPartition.IsIdentity() && RightHandPartition.IsIdentity() && !Swap;
        }
        public override string ToString()
        {
            return  " [ " + LeftHandPartition + "," + RightHandPartition + " ]" + (Swap ? "s" : "");
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            SnWrS2BranchCycleReducedForm other = (SnWrS2BranchCycleReducedForm)obj;
            if (other.HasSwap()!=this.HasSwap())
            {
                return false;
            }

            return SnBranchCycle.Equals(other.LeftHandPartition, this.LeftHandPartition) && SnBranchCycle.Equals(other.RightHandPartition, this.RightHandPartition);
        }

        public override int GetHashCode()
        {
            return LeftHandPartition.GetHashCode();
        }
    }
}
