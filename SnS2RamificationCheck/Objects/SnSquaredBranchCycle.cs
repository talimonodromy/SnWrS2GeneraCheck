using SnS2RamificationCheck.Interfaces;

namespace SnS2RamificationCheck.Objects
{
    public class SnSquaredBranchCycle
    {
        public SnSquaredBranchCycle(Interfaces.Partition lhs, Interfaces.Partition rhs)
        {
            _rhs = rhs;
            _lhs = lhs;
       }

        public SnSquaredBranchCycle(SnBranchCycle lhs, SnBranchCycle rhs)
        {
            _rhs = rhs.Partition;
            _lhs = lhs.Partition;
        }
        //ISymbolicExpressionHandler should be private

        protected Interfaces.Partition _lhs;
        public Interfaces.Partition LeftHandPartition { get { return _lhs;  } }

        protected Interfaces.Partition _rhs;
        public Interfaces.Partition RightHandPartition { get { return _rhs; }  }


        public override string ToString()
        {
            return " [ " + LeftHandPartition + "," + RightHandPartition + " ]";
        }

    }
}
