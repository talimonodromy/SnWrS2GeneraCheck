using System;
using SnS2RamificationCheck.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SnS2RamificationCheck
{
    public class PartitionHandler
    {
        private ISymbolicExpressionHandler _symbolicExpressionHandler;
        public PartitionHandler(ISymbolicExpressionHandler symbolicExpressionHandler)
        {
            _symbolicExpressionHandler = symbolicExpressionHandler;
        }

        public ISymbolicExpressionHandler SymbolicExpressionHandler{get {return _symbolicExpressionHandler;} }
        public Parity GetPartParity(PartitionPart part)
        {
            var partParity = Mod((int)SymbolicExpressionHandler.AssessParity(part.Part) +1,2);
            var timesParity = SymbolicExpressionHandler.AssessParity(part.Times);
            return (Parity)(Mod(partParity * (int)timesParity,2)); //TODO verify this is desired logic
        }
        public Parity GetParity(Partition partition)
        {
            var res = 0;
            foreach (var part in partition.Parts)
            {
                res = Mod(res + (int)GetPartParity(part),2); //todo verify this is desired logic
            }
            return (Parity)res;
        }  

        public int Mod(int x, int modulus)
        {
            int r = x % modulus;
            return r < 0 ? r + modulus : r;
        }

        public SymExpression CalculateGcd(SymExpression expr1, SymExpression expr2)
        {
            //both are integers
            var handler = SymbolicExpressionHandler;
            int number1;
            int number2;
            if (handler.TryParseInteger(expr1, out number1) && handler.TryParseInteger(expr2, out number2))
            {
                var intGcd = RamificationTypeCalculator.CalculateIntegerGCD(number1, number2);
                return new SymExpression(intGcd.ToString());
            }

            throw new Exception("unable to calculate GCD");
            return null;

        }

        public Partition GetSecondPower(Partition partition)
        {
            //for each part, if odd, stays the same. If even, add halves.
            var resParts = new List<PartitionPart>();
            foreach (var part in partition.Parts)
            {   //for 2nd power we care about the *integer* parity of the parts
                var part_int_parity = SymbolicExpressionHandler.AssessParity(part.Part);
                switch (part_int_parity) {
                    case Parity.Odd:
                        resParts.Add(part); //TODO hope passing instance is OK 
                        break;
                    case Parity.Even:
                        var halvedPart = SymbolicExpressionHandler.Halve(part.Part);
                        resParts.Add(new PartitionPart(new SymExpression(halvedPart), new SymExpression(SymbolicExpressionHandler.Add(part.Times, part.Times))));
                        break;
                }                            
            }
            return new Partition(resParts);
        }


        //TODO should something similar to this be the underlying object for partition? or possibly we need two partitions, one which is the "latex" partition given as inpur
        // and one the "code" partition which includes some information on handling? at any rate doesn't matter for this scale.
        public Dictionary<string, ISymbolicExpression> PartitionToPartsDictionary(Partition partition)
        {
           
                var res = new Dictionary<string,ISymbolicExpression>();
                foreach (var partitionPart in partition.Parts)
                {
                    var partExpression = partitionPart.Part.Expression;
                    if (res.ContainsKey(partExpression)) {
                        var existingTimesExpression = res[partExpression];
                        res[partExpression] = SymbolicExpressionHandler.Add(existingTimesExpression, partitionPart.Times); // there's probably some interface you can implement which gives you nice syntax here
                    }
                    else
                    {
                        res.Add(partExpression, partitionPart.Times);
                    }
                }
                //go over all keys and simplify values? 
            return res;                
        }


        public Partition Power(Partition partition, ISymbolicExpression power)
        {
            // for every part, you need to take its power
            var parts = new List<PartitionPart>();
            foreach (var part in partition.Parts)
            {
                parts.AddRange(GetIntegerPartPower(part,power));
            }
            return new Partition(parts);
        }

        public bool PartIsInteger(PartitionPart part)
        {
            int outputInt;
          var res = SymbolicExpressionHandler.TryParseInteger(part.Part, out outputInt);
          return res;
        }

        public bool HasOnlyIntegerParts(Partition partition) //TODO need to move this to partition handler
        {
            int outputInt;
            return partition.Parts.Select(PartIsInteger).All(success => success);
        }
        public IEnumerable<PartitionPart> GetIntegerPartPower(PartitionPart part, ISymbolicExpression power)
        {
            //we assume part.part is an integer and power is an integer (times can be an expression)
            //if both integers: a'th power of a cycle of length c is c/gcd(c,a), gcd(c,a) times.
            int intPower;
            int partLength;
            if (SymbolicExpressionHandler.TryParseInteger(part.Part, out partLength) &&
                SymbolicExpressionHandler.TryParseInteger(power, out intPower))
            {
                var gcd = RamificationTypeCalculator.CalculateIntegerGCD(partLength, intPower);
                var newLength = partLength / gcd;
                var cycleTimes = new SymExpression(gcd);
                var newTimes = (SymExpression)SymbolicExpressionHandler.Multiply(part.Times, cycleTimes);
                var newPart = new PartitionPart(new SymExpression(newLength), newTimes);
                return new List<PartitionPart>() { newPart }; // theoretically we could have obtained here a longer list (though not really) so signature returns a list.

            }
            //TODO if we got here throw some exception
            throw new SymbolicExpressionHandlingException(
                "Expected things to be an integers when calculating integer part power");
            return null; 
        }

        //TODO we're still assuming the "part" strings are unique...but OK 
        //TODO there's a severe error here, part comparison needs to be performed by symbolic expression handler or at least strip the strings..
        public bool PartitionEquals(Partition partition1, Partition partition2)
        {
            var res = false;
            var partition1Parts = PartitionToPartsDictionary(partition1);
            var partition2Parts = PartitionToPartsDictionary(partition2);
            //check if have same parts
            var sameKeys = StaticAndBenEll.ListEquals<string>(partition1Parts.Keys, partition2Parts.Keys);
            if (sameKeys)
            {
                foreach(var key in partition1Parts.Keys)
                {
                    var sameTimes = SymbolicExpressionHandler.ExpressionsEqual(partition1Parts[key], partition2Parts[key]); //check if times exlressions are equal.
                    if (!sameTimes)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        
    }
}
