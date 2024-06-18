using System.Collections.Generic;

namespace SnS2RamificationCheck.Interfaces
{
    public class SymExpression : ISymbolicExpression
    {
        public SymExpression(ISymbolicExpression symexpr)
        {
            _expression = symexpr.Expression;
        }
        public SymExpression(string expr)
        {
            _expression = expr;
        }

        private string _expression;
        public string Expression { get { return _expression; } }


        public override int GetHashCode()
        {
            return Expression.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            SymExpression other = (SymExpression)obj;
            return this.Expression.Equals(other.Expression);
        }
        public override string ToString()
        {
            return Expression;
        }

    }
}
