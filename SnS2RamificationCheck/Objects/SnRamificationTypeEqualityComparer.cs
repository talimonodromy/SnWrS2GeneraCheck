using System.Collections.Generic;
using System.Linq;

namespace SnS2RamificationCheck.Objects
{
    public class SnRamificationTypeEqualityComparer : IEqualityComparer<SnRamificationType>
    {
        public bool Equals(SnRamificationType x, SnRamificationType y)
        {
            if (ReferenceEquals(x, y))
                return true;

            if (x == null || y == null)
                return false;

            //before comparing, strip both of identities
            var xBranches = x.BranchCycles.Where(b => !b.IsIdentity());
            var yBranches = y.BranchCycles.Where(b => !b.IsIdentity());
            return StaticAndBenEll.ListEquals<SnBranchCycle>(xBranches, yBranches);

        }

        public int GetHashCode(SnRamificationType obj)
        {
            return obj.GetHashCode();
        }
    }
}
