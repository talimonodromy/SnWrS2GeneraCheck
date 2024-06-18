using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SnS2RamificationCheck.Objects
{
    public class SnWrS2RamificationType 
    {
        public SnWrS2RamificationType(string name, IEnumerable<SnWrS2BranchCycleReducedForm> branches)
        {
            _branchCycles = branches;
            Name = name;
        }
        public string Name { get; set; }
        protected IEnumerable<SnWrS2BranchCycleReducedForm> _branchCycles;

        public IEnumerable<SnWrS2BranchCycleReducedForm> BranchCycles { get { return _branchCycles; } }


        public override string ToString()
        {
            return Name + BranchCycles.ConcatenateToString();
        }




        public bool Equals([AllowNull] SnWrS2RamificationType x, [AllowNull] SnWrS2RamificationType y)
        {
            if (x is null || y is null)
            {
                throw new System.Exception("comparing null ramification types");
            }

            return StaticAndBenEll.ListEquals<SnWrS2BranchCycleReducedForm>(x.BranchCycles, y.BranchCycles);
        }


        public int GetHashCode([DisallowNull] SnWrS2RamificationType obj)
        {

            int hashCode = 17;

            foreach (SnWrS2BranchCycleReducedForm element in obj.BranchCycles)
            {
                hashCode = hashCode * 31 + (element?.GetHashCode() ?? 0);
            }

            return hashCode;

        }



}
}
