using SnS2RamificationCheck.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SnS2RamificationCheck.Interfaces
{
    public class Partition
    {

        private IEnumerable<PartitionPart> _parts;

        public Partition(String str)
        {
            throw new NotImplementedException();
        }

        public Partition(IEnumerable<PartitionPart> parts)
        {
            _parts = parts;
        }

        public IEnumerable<PartitionPart> Parts => _parts;

   
        public override int GetHashCode()
        {
            int hc = 0;
            if (Parts != null)
                foreach (var p in Parts)
                    hc ^= p.GetHashCode();
            return hc;
        }

        public bool IsIdentity()
        {
            //if all parts equal 1, we are the identity.
            //i.e., if there exists a part not equal to 1, we are not the identity.
            //TODO without a handler, we cannot check for legality
            return Parts.All(p =>  String.Equals(p.Part.Expression, "1"));
            
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Partition other = (Partition)obj;
            return StaticAndBenEll.ListEquals<PartitionPart>(this.Parts, other.Parts);
        }
        public override string ToString()
        {
            return "["+string.Join(", ", _parts)+"]";
        }

    }
}
