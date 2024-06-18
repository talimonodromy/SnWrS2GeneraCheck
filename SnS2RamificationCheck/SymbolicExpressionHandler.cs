using SnS2RamificationCheck.Interfaces;
using SnS2RamificationCheck.Objects;
using System;
using System.Collections.Generic;
using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;


namespace SnS2RamificationCheck
{

    public class SymbolicExpressionHandler : ISymbolicExpressionHandler
    {
        private string _x;
        private string _y;
        private string _z;

        private Dictionary<string, ParityAssumption> _assumptions;

        public Dictionary<string, ParityAssumption> Assumptions { get { return _assumptions; } }




        protected Expr x { get { return Expr.Variable(_x); } }
        protected Expr y { get { return Expr.Variable(_y); } }
        protected Expr z { get { return Expr.Variable(_z); } }

        protected int CapOnTries { get; private set; }
        public SymbolicExpressionHandler(string x, string y, string z, Dictionary<string, ParityAssumption> assumptions, int capOnTries= 12)
        {
            _x = x;
            _y = y;
            _z = z;
            _assumptions = assumptions;
            CapOnTries = capOnTries;


        }

      
        //TODO check this if ever used
        public ISymbolicExpression Add(ISymbolicExpression exp1, ISymbolicExpression exp2)
        {
            Expr parsed = Expr.Parse(exp1.Expression +"+"+ exp2.Expression);
            return new SymExpression(parsed.ToString());
        }

        public ISymbolicExpression Divide(ISymbolicExpression exp1, ISymbolicExpression exp2)
        {
            Expr parsed = Expr.Parse("(" + exp1.Expression + ")/(" + exp2.Expression + ")");
            return new SymExpression(parsed.ToString());
        }

        public Parity AssessParity(ISymbolicExpression exp1)
        {
            var xReplacement = Assumptions[_x].EquivTo;

            // (sum up all parities and take mod 2) //TODO instead of replacing the variables, assumptions should be replaced in the entire expression?
            var parsed = Expr.Parse(exp1.Expression).Substitute(x, xReplacement) ;

            if (_y!= null && Assumptions.ContainsKey(_y))
            {
                var yReplacement = Assumptions[_y].EquivTo;
                parsed = parsed.Substitute(y, yReplacement);


            }

            if (_z!=null && Assumptions.ContainsKey(_z))
            {
                var zReplacement = Assumptions[_z].EquivTo;
                parsed = parsed.Substitute(z, zReplacement);
            }
            //do: try and extract variable and substitute values in it until assumption holds.

            var isInt = IsInteger(parsed.RealNumberValue);
            if (!isInt)
            {
                throw new SymbolicExpressionHandlingException("error assessing parity for" + exp1);
            }
            var mod2 = (int)parsed.RealNumberValue % 2;
            mod2 = mod2 < 0 ? mod2 + 2 : mod2;
            return (Parity)Enum.ToObject(typeof(Parity), mod2);
        }

        public bool TryParseInteger(ISymbolicExpression exp, out int number)
        {

            bool success = int.TryParse(exp.Expression, out number);
            return success;
        }

        public bool LeadingCoefficientNonnegative(string expr,string parName)
        {
            var nParam = SymbolicExpression.Parse(parName);
            var leading = Expr.Parse(expr).RationalSimplify(nParam).LeadingCoefficient(nParam);
            return leading.RationalNumberValue.IsPositive || leading.RationalNumberValue.IsZero;
        }

        public ISymbolicExpression SimplifyTo01orOther(ISymbolicExpression symbolicExpression)
        {

            var zero = new SymExpression("0");
            var one = new SymExpression("1");


            var nParameter = SymbolicExpression.Parse("n");
            var parsed = Expr.Parse(symbolicExpression.Expression).RationalSimplify(nParameter);
            if (parsed.Expression.IsNumber)
            {
                if (parsed.RationalNumberValue.IsZero)
                {
                    return zero;
                }


                if (parsed.RationalNumberValue.IsOne)
                {
                    return one;
                }
            }

            return new SymExpression(parsed.ToString()); 
        }



        public int TryFindSubstitute(string var_name, string str,Parity parity)
        {
            int i = 1;

            do
            {
                var parsed = Expr.Parse(str).Substitute(var_name, i);
                var isInteger = IsInteger(parsed.RealNumberValue);
                if (isInteger)
                {

                    var substitutedInt = (int)parsed.RealNumberValue;
                    //check if substitute
                    var parityOfSubstitutedInt = substitutedInt % 2;
                    if (parityOfSubstitutedInt == (int)parity)
                    {
                        return i;
                    }                  
                }
                i++;
            }
            while (i <= CapOnTries);
            throw new SymbolicExpressionHandlingException(String.Format("could not find suitable int for assumption {0},{1},{2}",str, var_name, parity));
        }


        


        public ISymbolicExpression Halve(ISymbolicExpression expr)
        {
            Expr halved = Expr.Parse("("+expr.Expression + ") /" + 2);
            return new SymExpression(halved.ToString());
        }

        public static bool IsInteger(double number)
        {
            return Math.Abs(number - Math.Truncate(number)) < double.Epsilon;
        }

        public bool ExpressionsEqual(ISymbolicExpression expr1, ISymbolicExpression expr2)
        {
            var parsed1 = Expr.Parse(expr1.Expression).Expand();
            var parsed2 = Expr.Parse(expr2.Expression).Expand();

            return parsed1.Expression.Equals(parsed2.Expression);
            //return Expr.Equals(expr1, expr2);

        }

        public ISymbolicExpression Multiply(ISymbolicExpression exp1, ISymbolicExpression exp2)
        {
            Expr parsed = Expr.Parse("("+exp1.Expression + ")*(" + exp2.Expression+")");
            return new SymExpression(parsed.ToString());
        }
    }
}
