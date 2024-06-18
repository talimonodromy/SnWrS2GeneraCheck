using System;

namespace SnS2RamificationCheck.Interfaces
{
    public class PartitionPart 
    {
        private SymExpression _part;
        private SymExpression _times;


        public PartitionPart(SymExpression part, SymExpression times)
        {
            _part = part;
            _times = times;
        }

        public PartitionPart(String part, String times)
        {
            _part = new SymExpression(part);
            _times = new SymExpression(times);
        }
        public ISymbolicExpression Times => _times;

        public ISymbolicExpression Part => _part;


        public override int GetHashCode()
        {
            return new Tuple<ISymbolicExpression,ISymbolicExpression>(Part, Times).GetHashCode() ;
        }


        public override bool Equals(object obj) 
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            PartitionPart other = (PartitionPart)obj;
            return Part.Equals(other.Part) && Times.Equals(other.Times);
        }
        public override string ToString()
        {
            return "{"+Part.ToString()+"}" + "^{" + Times.ToString() + "}";
        }

    }
}
