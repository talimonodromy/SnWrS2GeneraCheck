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

        protected Interfaces.Partition _partition;

        public Partition Partition {get {return _partition;} }

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
