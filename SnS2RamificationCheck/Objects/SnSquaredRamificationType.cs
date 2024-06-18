using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SnS2RamificationCheck.Objects
{
    public class SnSquaredRamificationType
    {
        public SnSquaredRamificationType(string name, IEnumerable<SnSquaredBranchCycle> branches)
        {
            _branchCycles = branches;
            Name = name;
        }
        public string Name { get; set; }
        protected IEnumerable<SnSquaredBranchCycle> _branchCycles;

        public IEnumerable<SnSquaredBranchCycle> BranchCycles { get { return _branchCycles; } }


        public override string ToString()
        {
            return Name + BranchCycles.ConcatenateToString();
        }




        public bool Equals([AllowNull] SnSquaredRamificationType x, [AllowNull] SnSquaredRamificationType y)
        {
            if (x is null || y is null)
            {
                throw new System.Exception("comparing null ramification types");
            }

            return StaticAndBenEll.ListEquals<SnSquaredBranchCycle>(x.BranchCycles, y.BranchCycles);
        }


        public int GetHashCode([DisallowNull] SnSquaredRamificationType obj)
        {

            int hashCode = 17;

            foreach (SnSquaredBranchCycle element in obj.BranchCycles)
            {
                hashCode = hashCode * 31 + (element?.GetHashCode() ?? 0);
            }

            return hashCode;

        }

    }
}
