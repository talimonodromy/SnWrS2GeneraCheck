using System;
using SnS2RamificationCheck.Interfaces;

namespace SnS2RamificationCheck.Objects
{
    public class SnSquaredBranchCycle
    {
        public static  SymExpression times1 = new SymExpression("1");
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

        public SnSquaredBranchCycle(SnBranchCycle lhs, SnBranchCycle rhs, ISymbolicExpression times)
        {
            _rhs = rhs.Partition;
            _lhs = lhs.Partition;
            _times = times;
        }

        public SnSquaredBranchCycle(Interfaces.Partition lhs, Interfaces.Partition rhs, ISymbolicExpression times)
        {
            _rhs = rhs;
            _lhs = lhs;
            _times = times;
        }
        //ISymbolicExpressionHandler should be private

        protected Interfaces.Partition _lhs;
        public Interfaces.Partition LeftHandPartition { get { return _lhs;  } }

        protected Interfaces.Partition _rhs;
        public Interfaces.Partition RightHandPartition { get { return _rhs; }  }

        protected ISymbolicExpression _times = times1;

        public ISymbolicExpression Times
        {
            get { return _times; }
        }

        public override string ToString()
        {
            return " [ " + LeftHandPartition + "," + RightHandPartition + " ] x"+Times.ToString();
        }

    }
}
