using System;
using SnS2RamificationCheck.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SnS2RamificationCheck.Objects
{
    //Represent Sn wr S2 (perm,perm,optional swap) and Sn x Sn (perm perm) conjugacy classes are immutable objects.

    public class SnBranchCycle
    {
        public SnBranchCycle(Interfaces.Partition partition)
        {
            _partition = partition;
        }

        public SnBranchCycle(string str)
        {
            _partition = new Partition(str);
        }

        public SnBranchCycle(Interfaces.Partition partition, ISymbolicExpression times)
        {
            _partition = partition;
            _times = times;
        }
        protected Interfaces.Partition _partition;
        protected  ISymbolicExpression _times = new SymExpression("1");
        public Partition Partition {get {return _partition;} }

        public ISymbolicExpression Times
        {
            get { return _times; }
        }


        public override int GetHashCode()
        {
            return Partition.GetHashCode();
        }

        public bool IsIdentity()
        {
            return Partition.IsIdentity();
        }


    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        SnBranchCycle other = (SnBranchCycle)obj;
            return Partition.Equals(other.Partition);
    }
    public override string ToString()
        {
            return _partition.ToString();
        }
    }
}
