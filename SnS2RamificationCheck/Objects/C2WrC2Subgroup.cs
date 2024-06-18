using System.Collections.Generic;

namespace SnS2RamificationCheck.Objects
{
    public partial class C2WrC2Subgroup
    {
        public C2WrC2Subgroup(C2WrC2SubgroupName name, IEnumerable<C2WrC2BranchCycle> elements)
        {
            _name = name;
            var elementsWithComparer = new HashSet<C2WrC2BranchCycle>(
                elements, new C2WrC2ElementComparer());

            _elements = elementsWithComparer;

        }
        public C2WrC2Subgroup(C2WrC2SubgroupName name, HashSet<C2WrC2BranchCycle> elements)
        {
            _name = name;
            var elementsWithComparer = new HashSet<C2WrC2BranchCycle>(
                elements, new C2WrC2ElementComparer());
            
            _elements = elementsWithComparer;
            
        }
        private C2WrC2SubgroupName _name;
        public C2WrC2SubgroupName Name { get { return _name; } }

       private HashSet<C2WrC2BranchCycle> _elements;
        public HashSet<C2WrC2BranchCycle> Elements { get { return _elements; } }

        public int Size => Elements.Count;

        public int Index => 8 / Size;
    }
    
}



    