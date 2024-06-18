using System;
using System.Collections.Generic;
using System.Linq;

namespace SnS2RamificationCheck.Objects
{
    public class C2WrC2Type
    {
        public C2WrC2Type(IEnumerable<C2WrC2BranchCycle> cycles)
        {
            BranchCycles = cycles;
        }
        public String Name { get; private set; }
        public IEnumerable<C2WrC2BranchCycle> BranchCycles { get; private set; }

        public override string ToString()
        {
            return string.Concat(BranchCycles, ",");
        }

        public HashSet<C2WrC2BranchCycle> ToHashset()
        {
            var res = new HashSet<C2WrC2BranchCycle>(BranchCycles);
            var comparer = new C2WrC2ElementComparer();
            foreach (var cycle in BranchCycles)
            {
                if (!res.Contains(cycle, comparer))
                {
                    res.Add(cycle);
                }
            }
            return res;
        }

    }

}

    