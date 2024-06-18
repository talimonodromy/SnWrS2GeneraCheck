using SnS2RamificationCheck.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace SnS2RamificationCheck.Interfaces
{
    public interface ISymbolicExpressionHandler
    {
        public ISymbolicExpression Add(ISymbolicExpression exp1, ISymbolicExpression exp2);

        public ISymbolicExpression Multiply(ISymbolicExpression exp1, ISymbolicExpression exp2);

        public ISymbolicExpression Divide(ISymbolicExpression expr1, ISymbolicExpression expr2);
        public Parity AssessParity(ISymbolicExpression exp1);

        public bool TryParseInteger(ISymbolicExpression exp, out int number);
        public ISymbolicExpression SimplifyTo01orOther(ISymbolicExpression symbolicExpression);

        public ISymbolicExpression Halve(ISymbolicExpression expr);
        public Dictionary<string, ParityAssumption> Assumptions { get; }
 
        public bool ExpressionsEqual(ISymbolicExpression expr1, ISymbolicExpression expr2);


        public bool LeadingCoefficientNonnegative(string expr, string parName);

    }
}
