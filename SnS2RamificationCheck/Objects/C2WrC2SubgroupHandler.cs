using System;
using System.Collections.Generic;
using System.Linq;
using static SnS2RamificationCheck.Objects.C2WrC2Subgroup;

namespace SnS2RamificationCheck.Objects
{
    public class C2WrC2SubgroupHandler
    {

        public static C2WrC2Subgroup GetMonodromyGroupGeneratedByType(C2WrC2Type c2wrc2type)
        {
            var candidateSubgroups = C2WrC2Subgroups.Where(c => c2wrc2type.BranchCycles.All(b => c.Elements.Contains(b)));
            //now we want the subgroup with the minimal number of elements
            var minimalCandidate = candidateSubgroups.OrderBy(c => c.Elements.Count).FirstOrDefault();

            var match = minimalCandidate;
            return match;
        }
       
        public static HashSet<C2WrC2BranchCycle> GetElementsInKernel(HashSet<C2WrC2BranchCycle> inputSet)
        {
            var kernelElements = inputSet.Where(e => !e.Swap);
            return kernelElements.ToHashSet();
        }

        public static C2WrC2BranchCycle E00s = new C2WrC2BranchCycle(Parity.Even, Parity.Even, true);
        public static C2WrC2BranchCycle E00 = new C2WrC2BranchCycle(Parity.Even, Parity.Even, false);
        public static C2WrC2BranchCycle E10s = new C2WrC2BranchCycle(Parity.Odd, Parity.Even, true);
        public static C2WrC2BranchCycle E10 = new C2WrC2BranchCycle(Parity.Odd, Parity.Even, false);
        public static C2WrC2BranchCycle E01s = new C2WrC2BranchCycle(Parity.Even, Parity.Odd, true);
        public static C2WrC2BranchCycle E01 = new C2WrC2BranchCycle(Parity.Even, Parity.Odd, false);
        public static C2WrC2BranchCycle E11s = new C2WrC2BranchCycle(Parity.Odd, Parity.Odd, true);
        public static C2WrC2BranchCycle E11 = new C2WrC2BranchCycle(Parity.Odd, Parity.Odd, false);

        public static C2WrC2Subgroup MatchSubgroup(HashSet<C2WrC2BranchCycle> elements)
        {
            var match = C2WrC2Subgroups.SingleOrDefault(t => t.Elements.SetEquals(elements));
            return match;
        }

        public static C2WrC2BranchCycle Take2ndPower(C2WrC2BranchCycle element)
        {
            var lhs = (int)element.LeftHandSide;
            var rhs = (int)element.RightHandSide;
            
            if (element.Swap)
            {
                return new C2WrC2BranchCycle(lhs: (Parity)((lhs + rhs) % 2), (Parity)((rhs + lhs) % 2), false);
            }
            else
            {
                return new C2WrC2BranchCycle((Parity)((lhs + lhs) % 2), (Parity)((rhs+rhs)%2),false);
            }
        }
        public static List<C2WrC2Subgroup> C2WrC2Subgroups = new List<C2WrC2Subgroup>
        {

            new C2WrC2Subgroup(C2WrC2SubgroupName.SnWrS2, new HashSet<C2WrC2BranchCycle> {E00s,E00,E10s,E10,E01s,E01,E11s,E11}),
             new C2WrC2Subgroup(C2WrC2SubgroupName.SnSquared, new HashSet<C2WrC2BranchCycle> {E00,E10,E01,E11}),
                  new C2WrC2Subgroup(C2WrC2SubgroupName.AnC4, new HashSet<C2WrC2BranchCycle> {E00,E10s,E01s,E11}),
                     new C2WrC2Subgroup(C2WrC2SubgroupName.FiberWrS2, new HashSet<C2WrC2BranchCycle> {E00s,E00,E11s,E11}),
                         new C2WrC2Subgroup(C2WrC2SubgroupName.AnxSn, new HashSet<C2WrC2BranchCycle> {E00,E01}),
                            new C2WrC2Subgroup(C2WrC2SubgroupName.SnxAn, new HashSet<C2WrC2BranchCycle> {E00,E10}),
                            new C2WrC2Subgroup(C2WrC2SubgroupName.Fiber, new HashSet<C2WrC2BranchCycle> {E00,E11}),
                             new C2WrC2Subgroup(C2WrC2SubgroupName.AnxAn, new HashSet<C2WrC2BranchCycle> {E00}),
            new C2WrC2Subgroup(C2WrC2SubgroupName.AnWrS2, new HashSet<C2WrC2BranchCycle> {E00s,E00}),
                 new C2WrC2Subgroup(C2WrC2SubgroupName.AnWrS2Cong, new HashSet<C2WrC2BranchCycle> {E11s,E00})
        };

        public static C2WrC2Subgroup GetSubgroup(C2WrC2SubgroupName name)
        {
            return C2WrC2Subgroups.SingleOrDefault(s =>String.Equals(s.Name,name));
        }

        public static bool C2WrC2SubgroupContains(C2WrC2Subgroup group, C2WrC2BranchCycle element)
        {
            return group.Elements.Any(e => e.Equals(element));
        }
        public static bool ElementContainedIn(C2WrC2BranchCycle element, C2WrC2SubgroupName groupName)
        {
            var subgroup = GetSubgroup(groupName);
            return C2WrC2SubgroupContains(subgroup, element);
        }


    }
}



    