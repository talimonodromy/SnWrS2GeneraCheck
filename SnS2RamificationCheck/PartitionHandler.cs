using SnS2RamificationCheck.Interfaces;
using System.Collections.Generic;

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
