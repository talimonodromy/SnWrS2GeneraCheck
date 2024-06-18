using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SnS2RamificationCheck.Objects
{
    public class SnRamificationType : IEqualityComparer<SnRamificationType>
    {
        public SnRamificationType(string name, IEnumerable<SnBranchCycle> branches)
        {
            _branchCycles = branches;
            Name = name;
        }
        public string Name { get; set; }

        protected IEnumerable<SnBranchCycle> _branchCycles;
        public IEnumerable<SnBranchCycle> BranchCycles {get {return _branchCycles; }
}

        public string  ToLatexString()
        {
           // var str = BranchCycles.ConcatenateToString();

           var str = String.Join(",", BranchCycles.Select(b => b.Partition.ToString()));

            str = "$" + str + "$";
            return str;
        }
        public override string ToString()
        {
            return Name + BranchCycles.ConcatenateToString();
        }


        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            SnRamificationType other = (SnRamificationType)obj;
            var strippedOtherBranchCycles = other.BranchCycles.Where(b => !b.IsIdentity());
            var strippedThisBranchCycles = BranchCycles.Where(b => !b.IsIdentity());
            return StaticAndBenEll.ListEquals<SnBranchCycle>(strippedThisBranchCycles, strippedOtherBranchCycles);
        }
        public bool Equals([AllowNull] SnRamificationType x, [AllowNull] SnRamificationType y)
        {
            if (x is null || y is null)
            {
                throw new System.Exception("comparing null ramification types");
            }
            return x.Equals(y); //we've overriden Equals so this sould work
        }

        public override int GetHashCode()
        {

            int hc = 0;
            if (BranchCycles != null)
                foreach (var p in BranchCycles)
                    hc ^= p.GetHashCode();
            return hc;
        }

        public int GetHashCode([DisallowNull] SnRamificationType obj)
        {
            return obj.GetHashCode();
        }
    }
}
