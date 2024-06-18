using SnS2RamificationCheck.Interfaces;
using SnS2RamificationCheck.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using MathNet.Symbolics;

namespace SnS2RamificationCheck
{
    public class RamificationTypeCalculator
    {

        private PartitionHandler _partitionHandler;

        public PartitionHandler PartitionHandler { get { return _partitionHandler; } }
        public ISymbolicExpressionHandler SymHandler => PartitionHandler.SymbolicExpressionHandler;

        public RamificationTypeCalculator(PartitionHandler partitionHandler)
        {
            _partitionHandler = partitionHandler;
        }


        public ISymbolicExpression GetMiddleGenusByAccola(ISymbolicExpression top, ISymbolicExpression mid1,
            ISymbolicExpression mid2, ISymbolicExpression bottom)
        {
            //top-2bottom = sum of middle
            var topPlusTwiceBottom =
                PartitionHandler.SymbolicExpressionHandler.Add(top,
                    SymHandler.Multiply(new SymExpression("2"), bottom));
            var midSum = SymHandler.Add(mid1, mid2);
            return SymHandler.Add(topPlusTwiceBottom, SymHandler.Multiply(new SymExpression("-1"), midSum));
        }

        public ISymbolicExpression GetTwoPointStabilizer(SnRamificationType sntype, ISymbolicExpression twoSetGenus)
        {
            //RH contribution is number of even orbits
            var rhContribution = new SymExpression("0");
            foreach (var branch in sntype.BranchCycles)
            {
                foreach (var part in branch.Partition.Parts)
                {
                    if (SymHandler.AssessParity(part.Part) == Parity.Even)
                    {
                        rhContribution = (SymExpression)SymHandler.Add(part.Times, rhContribution);
                    }
                }
            }

            return CalculateTopGenus(new SymExpression("2"), rhContribution, twoSetGenus);
        }

        public SnRamificationType Get2SetRamificationType(SnRamificationType snType)
        {
            var handler = PartitionHandler.SymbolicExpressionHandler;

            var cycles = new List<SnBranchCycle>();
            foreach (var branchCycle in snType.BranchCycles)
            {
                var twoSetOrbits = GetOrbitsInTwoSetAction(branchCycle);
                cycles.Add(new SnBranchCycle(twoSetOrbits));
            }

            var res = new SnRamificationType(snType.Name, cycles);
            return res;
        }

        public ISymbolicExpression GetTwoSetStabilizerGenus(SnRamificationType sntype, ISymbolicExpression degree, ISymbolicExpression bottomGenus)
        {
            ISymbolicExpression contribution = new SymExpression("0");
            var handler = PartitionHandler.SymbolicExpressionHandler;
            foreach (var branchCycle in sntype.BranchCycles)
            {
                var twoSetOrbits = GetOrbitsInTwoSetAction(branchCycle);
                var partContribution = new SymExpression("0");
                foreach (var part in twoSetOrbits.Parts)
                {
                    var partMinus1 = handler.Add(part.Part, new SymExpression("-1"));

                    partContribution = (SymExpression)handler.Multiply(partMinus1, part.Times);
                    contribution = handler.Add(contribution, partContribution);
                }

            }

            return CalculateTopGenus(degree: degree, rhContribution: contribution,bottomGenus: bottomGenus);
        }

        public ISymbolicExpression CalculateTopGenus(ISymbolicExpression degree, ISymbolicExpression rhContribution,
            ISymbolicExpression bottomGenus)
        {
            return CalculateTopGenus(PartitionHandler.SymbolicExpressionHandler, degree,
                rhContribution, bottomGenus);
        }
        public ISymbolicExpression CalculateTopGenus(ISymbolicExpressionHandler handler, ISymbolicExpression degree, ISymbolicExpression rhContribution, ISymbolicExpression bottomGenus)
        {
            var bottomEulerChar = handler.Add(handler.Multiply(new SymExpression("2"), bottomGenus),new SymExpression("-2"));
            var degreeTimesChar = handler.Multiply(degree, bottomEulerChar);
            var rhs = handler.Add(degreeTimesChar, new SymExpression("2+" + rhContribution));
           var res = handler.Multiply(new SymExpression("1/2"), rhs);
           return SimplifyGenusExpression(res);



        }

        //TODO get handler from partition handler, you have one!
        public ISymbolicExpression CalculateGenusOfSymmetricActionPointStab(ISymbolicExpression degree, SnRamificationType ramTypeSnAction, ISymbolicExpression bottomGenus)
        {
            var handler = PartitionHandler.SymbolicExpressionHandler;
            ISymbolicExpression rhContribution = new SymExpression("");
            foreach(var cycle in ramTypeSnAction.BranchCycles)
            {
                foreach(var part in cycle.Partition.Parts) {
                    var times = part.Times;
                    var contribution = handler.Add(part.Part, new SymExpression("-1"));
                    var cycleContribution = handler.Multiply(contribution, times);
                    rhContribution = handler.Add(rhContribution, cycleContribution);
                }
            }
            var genus = CalculateTopGenus(handler, degree, rhContribution, bottomGenus);
            return genus;
        }


        public Parity GetRamificationTypeParity(SnRamificationType sntype)
        {
            var res = 0;
            foreach (var cycle in sntype.BranchCycles)
            {
                res = (res + (int)PartitionHandler.GetParity(cycle.Partition)) % 2; //todo verify this is desired logic
            }
            return (Parity)res;
        }

        public Parity GetSnBranchCycleParity(SnBranchCycle snbranch)
        {
            return PartitionHandler.GetParity(snbranch.Partition);
        }
        public SnRamificationType ProjectToRhsType(SnSquaredRamificationType input_snsquaredtype)
        {
            var cycles = new List<SnBranchCycle>();
            foreach (var input_branchcycle in input_snsquaredtype.BranchCycles)
            {
                cycles.Add(new SnBranchCycle(input_branchcycle.RightHandPartition));
            }
            return new SnRamificationType(input_snsquaredtype.Name, cycles);
        }

        public SnRamificationType ProjectToLhsType(SnSquaredRamificationType input_snsquaredtype)
        {
            var cycles = new List<SnBranchCycle>();
            foreach (var input_branchcycle in input_snsquaredtype.BranchCycles)
            {
                cycles.Add(new SnBranchCycle(input_branchcycle.LeftHandPartition));
            }
            return new SnRamificationType(input_snsquaredtype.Name, cycles);
        }

        public IEnumerable<SnSquaredRamificationType> GetKernelTypes(IEnumerable<SnWrS2RamificationType> input_list)
        {

            var kernel_types = input_list.Select(t => KernelTypeFromSnS2Type(t));
            return kernel_types;
        }

        public IEnumerable<SnSquaredRamificationType> GetSnAnTypes(IEnumerable<SnSquaredRamificationType> input_list)
        {
            var snan_types = input_list.Select(t => SnAnTypeFromSnSquaredType(t));
            return snan_types;
        }

        public IEnumerable<SnSquaredRamificationType> GetFiberTypes(IEnumerable<SnSquaredRamificationType> input_list)
        {
            var fiber_types = input_list.Select(t => FiberRamificationTypeFromSnSquaredType(t));
            return fiber_types;
        }
        public SnSquaredBranchCycle GetSecondPower(SnSquaredBranchCycle c)
        {
            return new SnSquaredBranchCycle(PartitionHandler.GetSecondPower(c.LeftHandPartition), PartitionHandler.GetSecondPower(c.RightHandPartition));
        }

        public SnSquaredRamificationType FiberRamificationTypeFromSnSquaredType(SnSquaredRamificationType snsnType)
        {
            var res_cycles = new List<SnSquaredBranchCycle>();
            foreach(var branch in snsnType.BranchCycles)
            {
                //compare rhs and lhs signs. If same, duplicate; else, raise both sides to 2nd power.
                var rhsSgn = PartitionHandler.GetParity(branch.RightHandPartition);
                var lhsSgn = PartitionHandler.GetParity(branch.LeftHandPartition);
                if (rhsSgn==lhsSgn)
                {
                    res_cycles.Add(branch);
                    res_cycles.Add(branch);
                }
                else
                {
                    res_cycles.Add(GetSecondPower(branch));
                }
            }
            return new SnSquaredRamificationType(snsnType.Name, res_cycles);
        }


        public int GetNumberOfRamPointsFiberOverKernel(SnSquaredRamificationType snsnType)
        {
            var numRam = 0;
            foreach (var branch in snsnType.BranchCycles)
            {
                //compare rhs and lhs signs. If same, duplicate; else, raise both sides to 2nd power.
                var rhsSgn = PartitionHandler.GetParity(branch.RightHandPartition);
                var lhsSgn = PartitionHandler.GetParity(branch.LeftHandPartition);
                if (rhsSgn == lhsSgn)
                {
                    //do nothing
                }
                else
                {
                    numRam++;
                }
            }
            return numRam;
        }
        public bool RamifiedInAnC4(SnWrS2BranchCycleReducedForm cycle)
        {
            var rhsParity = PartitionHandler.GetParity(cycle.RightHandPartition);
            var lhsParity = PartitionHandler.GetParity(cycle.LeftHandPartition);
            return !((rhsParity == lhsParity && !cycle.Swap) || (rhsParity != lhsParity && cycle.Swap));
        }
        public SnWrS2RamificationType CalculateAnC4TypeFromWreathType(SnWrS2RamificationType wreathtype)
        {
            var resCycles = new List<SnWrS2BranchCycleReducedForm>();
            foreach (var cycle in wreathtype.BranchCycles)
            {
                if (RamifiedInAnC4(cycle))
                {
                    var secondPower = GetSecondPower(cycle);
                    resCycles.Add(secondPower);
                }
                else
                {
                    resCycles.Add(cycle);
                    resCycles.Add(cycle);
                }
            }
            return new SnWrS2RamificationType(wreathtype.Name, resCycles);
        }

        internal IEnumerable<SnSquaredRamificationType> GetAnAnTypesFromSnAnTypes(IEnumerable<SnSquaredRamificationType> snanTypes)
        {
            return snanTypes.Select(t => SnAnTypeFromSnSquaredType(t));
        }

        public SnSquaredRamificationType GetAnAnTypeFromFiberType(SnSquaredRamificationType fiberType)
        {
            // for each branch cycle, check sign of RHS, if it's odd -> halve, else -> duplicate
            var res_cycles = new List<SnSquaredBranchCycle>();
            foreach (var branch in fiberType.BranchCycles)
            {
                var rhsSgn = PartitionHandler.GetParity(branch.RightHandPartition);
                var lhsSgn = PartitionHandler.GetParity(branch.RightHandPartition);
                if (rhsSgn != lhsSgn)
                {
                    throw new Exception("something wrong with fiber type");
                }
                switch (rhsSgn)
                {
                    case Parity.Odd:
                        res_cycles.Add(GetSecondPower(branch));
                        break;
                    case Parity.Even:
                        res_cycles.Add(branch);
                        res_cycles.Add(branch); //TODO is it OK to add two different instances here or will it cause a mess?
                        break;
                }
            }
            return new SnSquaredRamificationType(fiberType.Name, res_cycles);
        }

        public SnSquaredRamificationType GetAnAnTypeFromSnAnType(SnSquaredRamificationType snanType)
        {
            return AnAnTypeFromSnSquaredTypeFromSnAnTypes(snanType);
        }
        public SnSquaredRamificationType AnAnTypeFromSnSquaredTypeFromSnAnTypes(SnSquaredRamificationType snanType)
        {
            // for each branch cycle, check sign of RHS, if it's odd -> halve, else -> duplicate
            var res_cycles = new List<SnSquaredBranchCycle>();
            foreach (var branch in snanType.BranchCycles)
            {
                var lhsSgn = PartitionHandler.GetParity(branch.LeftHandPartition);
                switch (lhsSgn)
                {
                    case Parity.Odd:
                        res_cycles.Add(GetSecondPower(branch));
                        break;
                    case Parity.Even:
                        res_cycles.Add(branch);
                        res_cycles.Add(branch); //TODO is it OK to add two different instances here or will it cause a mess?
                        break;
                }
            }
            return new SnSquaredRamificationType(snanType.Name, res_cycles);
        }


        public int GetNumberOfRamifiedPointsAnAnOverFiber(SnSquaredRamificationType fiberType)
        {
            var numRam = 0;

            foreach (var branch in fiberType.BranchCycles)
            {
                var lhsSgn = PartitionHandler.GetParity(branch.LeftHandPartition);
                switch (lhsSgn)
                {
                    case Parity.Odd:
                        numRam++;
                        break;
                    case Parity.Even:
                        break;
                }
            }

            return numRam;
        }
        public int GetNumberOfRamPointsAnAnOverSnAn(SnSquaredRamificationType snAnType)
        {
            var numRam = 0;
            foreach (var branch in snAnType.BranchCycles)
            {
                var lhsSgn = PartitionHandler.GetParity(branch.LeftHandPartition);
                switch (lhsSgn)
                {
                    case Parity.Odd:
                        numRam++;
                        break;
                    case Parity.Even:
                        break;
                }
            }
            return numRam;
        }

        public int GetNumberOfRamPointsSnAnOverSnSn(SnSquaredRamificationType snsnType)
        {
            // for each branch cycle, check sign of RHS, if it's odd -> halve, else -> duplicate
            int numRam = 0;
            foreach (var branch in snsnType.BranchCycles)
            {
                var rhsSgn = PartitionHandler.GetParity(branch.RightHandPartition);
                switch (rhsSgn)
                {
                    case Parity.Odd:
                        numRam++;
                        break;
                    case Parity.Even:
                        break;
                }
            }
            return numRam;
        }

        public bool ContainedInFiber(SnSquaredRamificationType snSquaredType)
        {
            var sameSign = snSquaredType.BranchCycles.Any(b => PartitionHandler.GetParity(b.RightHandPartition) != PartitionHandler.GetParity(b.LeftHandPartition));
            return !sameSign;
        }

        public bool ContainedInAnSquared(SnSquaredRamificationType snSquaredType)
        {
            var allEven = snSquaredType.BranchCycles.All(b => PartitionHandler.GetParity(b.LeftHandPartition) == Parity.Even && PartitionHandler.GetParity(b.RightHandPartition) == Parity.Even);
            return allEven;
        }

        public SnSquaredRamificationType SnAnTypeFromSnSquaredType(SnSquaredRamificationType snsnType)
        {
            // for each branch cycle, check sign of RHS, if it's odd -> halve, else -> duplicate
            var res_cycles = new List<SnSquaredBranchCycle>();
            foreach (var branch in snsnType.BranchCycles)
            {
                var rhsSgn = PartitionHandler.GetParity(branch.RightHandPartition);
                switch (rhsSgn)
                {
                    case Parity.Odd:
                        res_cycles.Add(GetSecondPower(branch));
                        break;
                    case Parity.Even:
                        res_cycles.Add(branch);
                        res_cycles.Add(branch); //TODO is it OK to add two different instances here or will it cause a mess?
                        break;
                }
            }
            return new SnSquaredRamificationType(snsnType.Name, res_cycles);
        }

        public SnSquaredRamificationType AnSnTypeFromSnSquaredType(SnSquaredRamificationType snsnType)
        {
            // for each branch cycle, check sign of LHS, if it's odd -> halve, else -> duplicate
            var res_cycles = new List<SnSquaredBranchCycle>();
            foreach (var branch in snsnType.BranchCycles)
            {
                var lhsSgn = PartitionHandler.GetParity(branch.LeftHandPartition);
                switch (lhsSgn)
                {
                    case Parity.Odd:
                        res_cycles.Add(GetSecondPower(branch));
                        break;
                    case Parity.Even:
                        res_cycles.Add(branch);
                        res_cycles.Add(branch); //TODO is it OK to add two different instances here or will it cause a mess?
                        break;
                }
            }
            return new SnSquaredRamificationType(snsnType.Name, res_cycles);
        }

        public int GetNumberOfRamifiedPointsForKernelTypeFromSnS2Type(SnWrS2RamificationType input_type)
        {
            return input_type.BranchCycles.Count(b => b.HasSwap());

        }
        public SnSquaredRamificationType KernelTypeFromSnS2Type(SnWrS2RamificationType input_type)
        {
            var cycles = new List<SnSquaredBranchCycle>();
            foreach (var branch in input_type.BranchCycles)
            {
                if (branch.HasSwap())
                {
                    cycles.Add(new SnSquaredBranchCycle(branch.LeftHandPartition, branch.LeftHandPartition));
                }
                else
                {
                    cycles.Add(new SnSquaredBranchCycle(branch.LeftHandPartition, branch.RightHandPartition));
                    cycles.Add(new SnSquaredBranchCycle(branch.RightHandPartition, branch.LeftHandPartition));
                }
            }
            return new SnSquaredRamificationType(input_type.Name, cycles);
        }


        public ISymbolicExpression GetAnPtStabGenusFromSnRamificationType(ISymbolicExpressionHandler handler, SnRamificationType snType, ISymbolicExpression degree, ISymbolicExpression bottomGenus)
        {
            var ptStabGenus = CalculateGenusOfSymmetricActionPointStab(degree, snType, bottomGenus);

            var rhContribution = GetNumberOfOddCyclesInOddBranchCycles(handler,snType);

            var genus = CalculateTopGenus(handler, new SymExpression("2"), rhContribution, ptStabGenus);
            return genus;
        }

        public SnWrS2RamificationType CalculateFiberS2TypeFromWreathType(SnWrS2RamificationType wreathType)
        {
            var resCycles = new List<SnWrS2BranchCycleReducedForm>();
            foreach(var cycle in wreathType.BranchCycles)
            {
                if (RamifiedInFiberWrS2(cycle))
                {
                    var secondPower = GetSecondPower(cycle);
                    resCycles.Add(secondPower);
                }
                else
                {
                    resCycles.Add(cycle);
                    resCycles.Add(cycle);
                }
            }
            return new SnWrS2RamificationType(wreathType.Name, resCycles);
        }

        public int GetNumberOfRamifiedPointsInFiberS2(SnWrS2RamificationType wreathType)
        {
            var numRam = wreathType.BranchCycles.Count(b => RamifiedInFiberWrS2(b));
            return numRam;
        }

        public SnWrS2BranchCycleReducedForm GetSecondPower(SnWrS2BranchCycleReducedForm inputBranch)
        {
            if (inputBranch.HasSwap())
            {
                return new SnWrS2BranchCycleReducedForm(inputBranch.LeftHandPartition, inputBranch.LeftHandPartition, false);
            }
            else
            {
                var lhs2ndPower = PartitionHandler.GetSecondPower(inputBranch.LeftHandPartition);
                var rhs2ndPower = PartitionHandler.GetSecondPower(inputBranch.RightHandPartition);

                return new SnWrS2BranchCycleReducedForm(lhs2ndPower, rhs2ndPower, false);
            }
        }
        public bool RamifiedInFiberWrS2(SnWrS2BranchCycleReducedForm branchCycle)
        {
            return PartitionHandler.GetParity(branchCycle.LeftHandPartition) != PartitionHandler.GetParity(branchCycle.RightHandPartition);
        }
       
        public C2WrC2Subgroup GetC2WrC2SubgroupGeneratedBy(SnWrS2RamificationType wreathType)
        {
            var projection = GetC2WrC2Type(wreathType);
            var generatedSubgroup = C2WrC2SubgroupHandler.GetMonodromyGroupGeneratedByType(projection);
         
            return generatedSubgroup;
        }
        public C2WrC2Type GetC2WrC2Type(SnWrS2RamificationType wreathType)
        {
            var cycles = wreathType.BranchCycles.Select(c => GetC2C2Projection(c));
            return new C2WrC2Type(cycles);
        }

        public C2WrC2BranchCycle GetC2C2Projection(SnWrS2BranchCycleReducedForm wreathbranch)
        {
            var lhsSign = PartitionHandler.GetParity(wreathbranch.LeftHandPartition);
            var rhsSign = PartitionHandler.GetParity(wreathbranch.RightHandPartition);
            return new C2WrC2BranchCycle(lhsSign, rhsSign, wreathbranch.Swap);
        }
        
        public bool SnwrS2BranchCyclesEqual(SnWrS2RamificationType ram1,SnWrS2RamificationType ram2)
        {
            var branches1 = ram1.BranchCycles.Where(b => !b.IsIdentity());
            var branches2 = ram2.BranchCycles.Where(b => !b.IsIdentity());


            var list1Empty = !branches1.Any();
            var list2Empty = !branches2.Any();
            if (list1Empty && list2Empty)
            {
                return true;
            }

            if (branches1.Count() != branches2.Count())
            {
                return false;
            }
            //copy list2
            var tempList = branches2.ToList();
            foreach (var item in branches1)
            {

                var matchingBranch = tempList.FirstOrDefault(t => SnwrS2BranchCyclesEqual(t,item));
                //lookup in list2 and remove
                if (matchingBranch != null)
                {
                    tempList.Remove(matchingBranch);
                }
                else
                {
                    return false;
                }
            }

            //if we are still here after the loop then list1 is contained in list2 (=temp list)
            //check if anything remains in temp list
            return tempList.Count() == 0 ? true : false;
        }

        public bool SnwrS2BranchCyclesEqual(SnWrS2BranchCycleReducedForm cycle1, SnWrS2BranchCycleReducedForm cycle2)
        {
            if (cycle1.HasSwap() != cycle2.HasSwap())
            {
                return false;
            }
            //if swap status is the same, we need to consider the reduced form representatives.
            return PartitionHandler.PartitionEquals(cycle1.LeftHandPartition,cycle2.LeftHandPartition) && PartitionHandler.PartitionEquals(cycle1.RightHandPartition,cycle2.RightHandPartition);
        }



        public ISymbolicExpression GetNumberOfOddCyclesInOddBranchCycles(ISymbolicExpressionHandler handler,
            SnRamificationType snType)
        {
            var oddBranches = snType.BranchCycles.Where(b => GetSnBranchCycleParity(b) == Parity.Odd);
            var oddCycles = oddBranches.SelectMany(b =>
                b.Partition.Parts.Where(p => handler.AssessParity(p.Part) == Parity.Odd));
            var times = oddCycles.Select(c => c.Times);
            var res = (ISymbolicExpression)new SymExpression("0");
            foreach (var t in times)
            {
                res = handler.Add(res, t);
            }

            return res;
        }
        public bool BranchCyclesEqual(SnRamificationType ram1, SnRamificationType ram2)
        {

            var branches1 = ram1.BranchCycles.Where(b => !b.IsIdentity());
            var branches2 = ram2.BranchCycles.Where(b => !b.IsIdentity());


                var list1Empty = !branches1.Any();
                var list2Empty = !branches2.Any();
                if (list1Empty && list2Empty)
                {
                    return true;
                }

                if (branches1.Count() != branches2.Count()) 
                {
                    return false;
                }
                //copy list2
                var tempList = branches2.ToList();
                foreach (var item in branches1)
                {

                var matchingBranch = tempList.FirstOrDefault(t => PartitionHandler.PartitionEquals(t.Partition, item.Partition));
                    //lookup in list2 and remove
                    if (matchingBranch != null) 
                    {
                        tempList.Remove(matchingBranch);
                    }
                    else
                    {
                        return false;
                    }
                }

                //if we are still here after the loop then list1 is contained in list2 (=temp list)
                //check if anything remains in temp list
                return tempList.Count() == 0 ? true : false;
            

        }

        public ISymbolicExpression GetC2C2SubgroupGenus(C2WrC2Type c2c2Type, C2WrC2SubgroupName subgroup)
        {
            var subgroupObject = C2WrC2SubgroupHandler.GetSubgroup((subgroup));
            var bottomGenus = new SymExpression("0");
            var degree = new SymExpression(subgroupObject.Index.ToString());
            var rh = new SymExpression(GetRiemannHurwitzContribution(c2c2Type, subgroup).ToString());
            
            var genus = CalculateTopGenus(degree, rh, bottomGenus);
            return genus;
        }

        public ISymbolicExpression SimplifyGenusExpression(ISymbolicExpression genusExpression)
        {
            return PartitionHandler.SymbolicExpressionHandler.SimplifyTo01orOther(genusExpression);
        }
        public int GetRiemannHurwitzContribution(C2WrC2Type c2c2type, C2WrC2SubgroupName subgroup)
        {
            if (subgroup == C2WrC2SubgroupName.SnWrS2)
            {
                return 0;
            }
            var contribution = 0;
            var subgroupObject = C2WrC2SubgroupHandler.GetSubgroup((subgroup));
            var subgroupIndex = subgroupObject.Index;
            foreach (var cycle in c2c2type.BranchCycles)
            {
                var cycleContribution = 0;
                if (C2WrC2ElementComparer.SameC2wrC2Elements(C2WrC2SubgroupHandler.E00, cycle))
                {
                    cycleContribution = 0; //do nothing - contribution is 0
                }
                else if (C2WrC2ElementComparer.SameC2wrC2Elements(C2WrC2SubgroupHandler.E11,cycle))
                {
                   if (subgroup == C2WrC2SubgroupName.Fiber || subgroup == C2WrC2SubgroupName.FiberWrS2 ||
                       subgroup == C2WrC2SubgroupName.AnC4 || subgroup == C2WrC2SubgroupName.SnSquared)
                   {
                       cycleContribution = 0;
                   }
                   else if (subgroup == C2WrC2SubgroupName.AnxAn)
                   {
                       cycleContribution = 4;
                   }
                   else if (subgroup == C2WrC2SubgroupName.AnxSn || subgroup == C2WrC2SubgroupName.SnxAn || subgroup == C2WrC2SubgroupName.AnWrS2 || subgroup == C2WrC2SubgroupName.AnWrS2Cong)
                   {
                       cycleContribution = 2;
                   }
                }
                else if (C2WrC2ElementComparer.SameC2wrC2Elements(C2WrC2SubgroupHandler.E10s, cycle) || C2WrC2ElementComparer.SameC2wrC2Elements(C2WrC2SubgroupHandler.E01s, cycle))
                {
                   if (subgroup == C2WrC2SubgroupName.AnC4)
                   {
                       cycleContribution = 0;
                   }
                   else if (subgroup == C2WrC2SubgroupName.Fiber)
                   {
                       cycleContribution = 2;
                   }
                   else
                   {
                       if (subgroupIndex == 2)
                       {
                           cycleContribution = 1;
                       }
                       else if (subgroupIndex == 4)
                       {
                           cycleContribution = 3;
                       }
                       else //subgroup index should be 8 here
                       {
                           cycleContribution = 6;
                       }
                   }
                }
                else if (C2WrC2ElementComparer.SameC2wrC2Elements(C2WrC2SubgroupHandler.E00s, cycle) ||
                         C2WrC2ElementComparer.SameC2wrC2Elements(C2WrC2SubgroupHandler.E11s, cycle))
                {
                    if (C2WrC2SubgroupHandler.C2WrC2SubgroupContains(subgroupObject, cycle))
                    {
                        if (subgroupIndex == 2)
                        {
                            cycleContribution = 0;
                        }

                        if (subgroupIndex == 4)
                        {
                            cycleContribution = 1;
                        }
                    }
                    else
                    {
                        switch ( subgroup)
                        {
                            case C2WrC2SubgroupName.SnWrS2:
                                cycleContribution = 0;
                                break;
                            case C2WrC2SubgroupName.AnxAn:
                                cycleContribution = 4;
                                break;
                            case C2WrC2SubgroupName.AnC4:
                            case C2WrC2SubgroupName.SnSquared:
                                cycleContribution = 1;
                                break;
                            case C2WrC2SubgroupName.SnxAn:
                            case C2WrC2SubgroupName.AnxSn:
                            case C2WrC2SubgroupName.Fiber:
                                cycleContribution = 2;
                                break;
                            default:
                                cycleContribution = 1; //if element is 00s then we reach default for conjugate subgroup of AnwrS2, for 11s we reach here for AnwrS2
                                break;
                        }
                    }
                }
                else if (C2WrC2ElementComparer.SameC2wrC2Elements(C2WrC2SubgroupHandler.E01, cycle) ||
                         C2WrC2ElementComparer.SameC2wrC2Elements(C2WrC2SubgroupHandler.E10, cycle))
                {
                    if (C2WrC2SubgroupHandler.C2WrC2SubgroupContains(subgroupObject, cycle))
                    {
                        
                        if (subgroupIndex == 2)
                        {
                            cycleContribution = 0;
                        }

                        if (subgroupIndex == 4)
                        {
                            cycleContribution = 1;
                        }
                    }
                    else
                    {
                        switch (subgroup)
                        {
                            case C2WrC2SubgroupName.SnWrS2:
                                cycleContribution = 0;
                                break;
                            case C2WrC2SubgroupName.AnxAn:
                                cycleContribution = 4;
                                break;
                            case C2WrC2SubgroupName.AnC4:
                            case C2WrC2SubgroupName.FiberWrS2:
                                cycleContribution = 1;
                                break;
                            case C2WrC2SubgroupName.AnWrS2:
                            case C2WrC2SubgroupName.AnWrS2Cong:
                            case C2WrC2SubgroupName.Fiber:
                                cycleContribution = 2;
                                break;
                            default:
                                cycleContribution = 1; //if element is 01 then we reach default for SnAn, for 10 we reach here for AnSn
                                break;
                        }
                    }
                }

                contribution += cycleContribution;
            }

            return contribution;
        }

        public bool RamifiedInAnWrC2(SnWrS2BranchCycleReducedForm fiberCycle)
        {
            return PartitionHandler.GetParity(fiberCycle.LeftHandPartition) == Parity.Odd;
        }

        public SnWrS2RamificationType CalculateAnwrS2TypeFromFiberType(SnWrS2RamificationType wreathType)
        {
            var resCycles = new List<SnWrS2BranchCycleReducedForm>();
            foreach (var cycle in wreathType.BranchCycles)
            {
                if (RamifiedInAnWrC2(cycle))
                {
                    var secondPower = GetSecondPower(cycle);
                    resCycles.Add(secondPower);
                }
                else
                {
                    resCycles.Add(cycle);
                    resCycles.Add(cycle);
                }
            }
            return new SnWrS2RamificationType(wreathType.Name, resCycles);
        }

        public int GetNumberOfRamifiedPointsAnwrSrOverFiberSw(SnWrS2RamificationType wreathType)
        {
           return wreathType.BranchCycles.Count(RamifiedInAnWrC2);
        }

        public SymExpression CalculateGcd(SymExpression expr1, SymExpression expr2)
        {
            //both are integers
            var handler = PartitionHandler.SymbolicExpressionHandler;
            int number1;
            int number2;
            if (handler.TryParseInteger(expr1,out number1) && handler.TryParseInteger(expr2,out number2))
            {
                var intGcd =  CalculateIntegerGCD(number1, number2);
                return new SymExpression(intGcd.ToString());
            }

            throw new Exception("unable to calculate GCD");
            return null;

        }

        public SymExpression CalculateLcm(SymExpression expr1, SymExpression expr2)
        {
            var product = PartitionHandler.SymbolicExpressionHandler.Multiply(expr1, expr2);
            var gcd = CalculateGcd(expr1, expr2);
            return (SymExpression)PartitionHandler.SymbolicExpressionHandler.Divide(product, gcd);
        }

        public static int CalculateIntegerLcm(int a, int b)
        {
            var product = a * b;
            return product / CalculateIntegerGCD(a, b);
        }
       public static int CalculateIntegerGCD(int a, int b)
        {
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }


        private static List<Tuple<Partition, ParityAssumption, Partition>> NParameterHardcoded2Orbits =
            new List<Tuple<Partition, ParityAssumption, Partition>>()
            {
                new Tuple<Partition, ParityAssumption, Partition>(
                    new Partition(new List<PartitionPart>() { new PartitionPart("n", "1") }),
                    new ParityAssumption(Parity.Even),

                    new Partition(new List<PartitionPart>()
                    {
                        new PartitionPart("n/2", "1"),
                        new PartitionPart("n", "n/2-1")
                    })


                ),
                new Tuple<Partition, ParityAssumption, Partition>(
                    new Partition(new List<PartitionPart>() { new PartitionPart("n", "1") }),
                           new ParityAssumption(Parity.Odd),
                    new Partition(new List<PartitionPart>()
                    {
                        new PartitionPart("n", "(n-1)/2"),
                    })
                ),new Tuple<Partition, ParityAssumption, Partition>(new Partition(new List<PartitionPart>(){new PartitionPart("n/2","2")}),
                    new ParityAssumption(equivTo: 0 , modulo:4),
                    new Partition(new List<PartitionPart>(){new PartitionPart("n/4","2"),new PartitionPart("n/2","n/2-2"),new PartitionPart("n/2","n/2")})),
                new Tuple<Partition, ParityAssumption, Partition>(new Partition(new List<PartitionPart>(){new PartitionPart("n/2","2")}),
                    new ParityAssumption(equivTo: 2 , modulo:4),
                    new Partition(new List<PartitionPart>(){new PartitionPart("n/2","n/2-1"),new PartitionPart("n/2","n/2")}))

            };

        //first pariaty is for n the second for a
        private static List<Tuple<Partition, ParityAssumption, ParityAssumption, Partition>> NAParameterHardcoded2Orbits =
            new List<Tuple<Partition, ParityAssumption, ParityAssumption, Partition>>() {
                new Tuple<Partition, ParityAssumption, ParityAssumption, Partition>(new Partition(new List<PartitionPart>() { new PartitionPart("a", "1") , new PartitionPart("n-a","1")}),
               new ParityAssumption(Parity.Even),
            new ParityAssumption(Parity.Odd),
                new Partition(new List<PartitionPart>()
                {

                    new PartitionPart("a","(a-1)/2"),
                    new PartitionPart("n-a","(n-a-1)/2"),
                    new PartitionPart("(n-a)*a","1")
                })),
            new Tuple<Partition, ParityAssumption, ParityAssumption, Partition>(new Partition(new List<PartitionPart>() { new PartitionPart("a", "1") , new PartitionPart("n-a","1")}),
            new ParityAssumption(Parity.Odd),
            new ParityAssumption(Parity.Odd),
            new Partition(new List<PartitionPart>()
                {
                    new PartitionPart("a","(a-1)/2"),
                    new PartitionPart("(n-a)/2","1"),
                    new PartitionPart("n-a","(n-a)/2-1"),
                    new PartitionPart("(n-a)*a","1")
                }
            )),
            new Tuple<Partition, ParityAssumption, ParityAssumption, Partition>(new Partition(new List<PartitionPart>() { new PartitionPart("a", "1") , new PartitionPart("n-a","1")}),
                new ParityAssumption(Parity.Odd),
                new ParityAssumption(Parity.Even),
                new Partition(new List<PartitionPart>()
                    {
                        new PartitionPart("a/2","1"),
                        new PartitionPart("a","a/2-1"),
                        new PartitionPart("n-a","(n-a-1)/2"),
                        new PartitionPart("(n-a)*a","1")
                    }
                )),
            new Tuple<Partition, ParityAssumption, ParityAssumption, Partition>(new Partition(new List<PartitionPart>(){new PartitionPart("a/2","2"), new PartitionPart("n-a","1")}),
                new ParityAssumption(Parity.Odd),
                new ParityAssumption(equivTo:0, modulo:4),
                new Partition(new List<PartitionPart>()
                {
                    new PartitionPart("a/4","2"),
                    new PartitionPart("a/2","a/2-2"),
                    new PartitionPart("a/2","a/2"),
                    new PartitionPart("n-a","(n-a-1)/2"),
                    new PartitionPart("a*(n-a)/2","2")

                })),
            new Tuple<Partition, ParityAssumption, ParityAssumption, Partition>(new Partition(new List<PartitionPart>(){new PartitionPart("a/2","2"), new PartitionPart("n-a","1")}),
                new ParityAssumption(Parity.Odd),
                new ParityAssumption(equivTo:2, modulo:4),
                new Partition(new List<PartitionPart>()
                {
                    new PartitionPart("a/2","a/2-1"),
                    new PartitionPart("a/2","a/2"),
                    new PartitionPart("n-a","(n-a-1)/2"),
                    new PartitionPart("a*(n-a)/2","2")

                })),
            new Tuple<Partition, ParityAssumption, ParityAssumption, Partition>(new Partition(new List<PartitionPart>(){new PartitionPart("a/2","2"), new PartitionPart("n-a","1")}),
                new ParityAssumption(Parity.Even),
                new ParityAssumption(equivTo:2, modulo:4),
                new Partition(new List<PartitionPart>()
                {
                    new PartitionPart("a/2","a/2-1"),
                    new PartitionPart("a/2","a/2"),
                    new PartitionPart("(n-a)/2","1"),
                    new PartitionPart("n-a","(n-a)/2-1"),
                    new PartitionPart("a*(n-a)/2","2")

                })),
            new Tuple<Partition, ParityAssumption, ParityAssumption, Partition>(new Partition(new List<PartitionPart>(){new PartitionPart("a","1"),new PartitionPart("(-a + n)/2", "2")}),
                new ParityAssumption(equivTo:1,modulo:4),
                new ParityAssumption(equivTo:1,modulo:4),
                new Partition(new List<PartitionPart>()
                {
                    new PartitionPart("a","(a-1)/2"),
                    new PartitionPart("(n-a)/4","2"),
                    new PartitionPart("(n-a)/2","(n-a)/2-2"),
                    new PartitionPart("a*(n-a)/2","2"),
                    new PartitionPart("(n-a)/2","(n-a)/2")
                })),

            new Tuple<Partition, ParityAssumption, ParityAssumption, Partition>(new Partition(new List<PartitionPart>(){new PartitionPart("a","1"),new PartitionPart("(-a + n)/2", "2")}),
                new ParityAssumption(equivTo:1,modulo:4),
                new ParityAssumption(equivTo:3, modulo:4),  new Partition(new List<PartitionPart>()
                {
                    new PartitionPart("a","(a-1)/2"),
                    new PartitionPart("(n-a)/2","(n-a)/2-1"),
                    new PartitionPart("a*(n-a)/2","2"),
                    new PartitionPart("(n-a)/2","(n-a)/2")
                })),

            new Tuple<Partition, ParityAssumption, ParityAssumption, Partition>(new Partition(new List<PartitionPart>(){new PartitionPart("a","1"),new PartitionPart("(-a + n)/2","2")}),
                new ParityAssumption(equivTo:3,modulo:4),
                new ParityAssumption(equivTo:1,modulo:4),  
                new Partition(new List<PartitionPart>()
                {
                    new PartitionPart("a","(a-1)/2"),
                    new PartitionPart("(n-a)/2","(n-a)/2-1"),
                    new PartitionPart("a*(n-a)/2","2"),
                    new PartitionPart("(n-a)/2","(n-a)/2")
                })),

            new Tuple<Partition, ParityAssumption, ParityAssumption, Partition>(new Partition(new List<PartitionPart>(){new PartitionPart("a","1"),new PartitionPart("(-a + n)/2", "2")}),
                new ParityAssumption(equivTo:3,modulo:4),
                new ParityAssumption(equivTo:3,modulo:4), new Partition(new List<PartitionPart>()
                {
                    new PartitionPart("a","(a-1)/2"),
                    new PartitionPart("a*(n-a)/2","2"),
                    new PartitionPart("(n-a)/4","2"),
                    new PartitionPart("(n-a)/2","(n-a)/2-1"),
                    new PartitionPart("(n-a)/2","(n-a)/2")
                }))
        };

        public bool AssumptionsEqual(Dictionary<string, ParityAssumption> set1,
            Dictionary<string, ParityAssumption> set2)
        {
            var keys1 = new HashSet<string>(set1.Keys);
            var keys2 = new HashSet<string>(set2.Keys);
           
            if (!keys1.Equals(keys2))
            {
                return false;
            }

            foreach (var key in set1.Keys)
            {
                var assumption1 = set1[key];
                var assumption2 = set2[key];
                if (!ParityAssumptionsEqual(assumption1, assumption2))
                {
                    return false;
                }
            }
            return true;
        }


        public bool ParityAssumptionsEqual(ParityAssumption parity1, ParityAssumption parity2)
        {
            if (parity1 != null && parity2 == null)
            {
                return false;
            }

            if (parity1 == null && parity2 == null)
            {
                return false;
            }
            return parity1.EquivTo == parity2.EquivTo && parity1.Modulo == parity2.Modulo;
        }

        public bool ContainsPartWithParameterA(SnBranchCycle snBranchCycle)
        {
            return snBranchCycle.Partition.Parts.Any(p => p.Part.Expression.Contains("a"));
        }

        public bool ContainsPartWithParameterN(SnBranchCycle snBranchCycle)
        {

            return snBranchCycle.Partition.Parts.Any(p => p.Part.Expression.Contains("n"));
        }

        //this should be defined as compatible to lower modulo.
        public static bool AssumptionsCompatible(ParityAssumption assumption1, ParityAssumption assumption2)
        {
            var mod1 = assumption1.Modulo;
            var mod2 = assumption2.Modulo;

            if (mod1 == mod2)
            {
                return assumption1.EquivTo == assumption2.EquivTo;
            }

           else if (mod2 % mod1 ==0 ) //mod1 | mod2
            {
                return assumption2.EquivTo % mod1 == assumption1.EquivTo;
            }
            else if (mod1 % mod2 ==0) //mod2 |mod1
            {
                return assumption1.EquivTo % mod2 == assumption2.EquivTo;
            }
            else
            {
                throw new Exception("TypesWithPossibleNAssumptionsBasedOnDenominator cannot be compared"); //TODO should we just return false? 
            }

        }
        public bool naAssumptionsMatch(Dictionary<string,ParityAssumption> assumptionsNA1, ParityAssumption nAssumption2, ParityAssumption aAssumption2)
        {
            var nAssumptions1 = assumptionsNA1["n"];
            var aAssumptions1 = assumptionsNA1["a"];
            if (nAssumptions1 == null || aAssumptions1 == null)
            {
                throw new Exception("assumptions issue");
            }

            return AssumptionsCompatible(nAssumptions1, nAssumption2) &&
                   AssumptionsCompatible(aAssumptions1, aAssumption2);
        }
        public Partition GetOrbitsInTwoSetAction(SnBranchCycle snBranchCycle)
        {
            var handler = PartitionHandler.SymbolicExpressionHandler;
            var nAssumption = handler.Assumptions["n"];
            if (ContainsPartWithParameterN(snBranchCycle))
            {
                if (!ContainsPartWithParameterA(snBranchCycle))
                {
                    var hardCodedMatchForN = NParameterHardcoded2Orbits.FirstOrDefault(t =>
                        PartitionHandler.PartitionEquals(t.Item1, snBranchCycle.Partition) && AssumptionsCompatible(nAssumption,t.Item2)); 
                    if (hardCodedMatchForN != null)
                    {
                        return hardCodedMatchForN.Item3;
                    }
                }
                else
                {
                    
                    var hardCodedMatchForNA = NAParameterHardcoded2Orbits.FirstOrDefault(t =>
                        PartitionHandler.PartitionEquals(t.Item1, snBranchCycle.Partition) && naAssumptionsMatch(handler.Assumptions,t.Item2,t.Item3));
                    if (hardCodedMatchForNA != null)
                    {
                        return hardCodedMatchForNA.Item4;
                    }
                }

                throw new Exception("Cannot find two orbits for partition" + snBranchCycle.Partition.ToString());

            }
            //we only get here if parts are integers
            List<PartitionPart> res = new List<PartitionPart>();
            var parts = snBranchCycle.Partition.Parts;
            
            //contribution of each part separately
            foreach (var part in parts)
            {
                var r = part.Part;
                if (PartitionHandler.SymbolicExpressionHandler.AssessParity(part.Part) == Parity.Even)
                {
            
                    //add one orbit of cardinality r/2 and r/2 orbits of cardinality r
                    var halfR = PartitionHandler.SymbolicExpressionHandler.Halve((SymExpression)r);

                    var oneOrbit = new PartitionPart((SymExpression)halfR, (SymExpression)part.Times);
                    res.Add(oneOrbit);

                    var halfRminus1 = PartitionHandler.SymbolicExpressionHandler.Add(halfR, new SymExpression("-1"));
                    if (PartitionHandler.SymbolicExpressionHandler.SimplifyTo01orOther(halfRminus1).Expression != "0")
                    {
                        var rOrbitTimes = PartitionHandler.SymbolicExpressionHandler.Multiply(halfRminus1, part.Times);
                        var rLengthOrbits = new PartitionPart((SymExpression)r, (SymExpression)rOrbitTimes);
                        res.Add(rLengthOrbits);
                    }
                }
                else
                {
                    //add (r-1)/2 orbits of cardinality r

                    var numberOfNewOrbits = String.Format("({0}-1)/2", r);
                    if (PartitionHandler.SymbolicExpressionHandler.SimplifyTo01orOther(
                            new SymExpression(numberOfNewOrbits)).Expression != "0")
                    {
                        var totalContributionToOrbits = String.Format("{0}*{1}", numberOfNewOrbits, part.Times);
                        var contributedPart = new PartitionPart((SymExpression)part.Part,
                            new SymExpression(totalContributionToOrbits));
                        res.Add(contributedPart);
                    }
                }
                
                //handle case of times > 1
                var timesSimplified = PartitionHandler.SymbolicExpressionHandler.SimplifyTo01orOther(part.Times);
                if (timesSimplified.Expression != "0" && timesSimplified.Expression != "1")
                {
                    var orbitLength = part.Part;
                    var orbitTimes = part.Part;
                    var howManyPSuchOrbits = new SymExpression(String.Format("({0})*({0}-1)/2", part.Times.Expression));
                    var totalTimes =
                        PartitionHandler.SymbolicExpressionHandler.Multiply(orbitTimes, howManyPSuchOrbits);
                    var contribution = new PartitionPart((SymExpression)orbitLength, (SymExpression)totalTimes);
                    res.Add(contribution);
                }
            }

            //for each pair of orbits, add gcd orbits of cardinality lcm
            var pairs = parts.SelectMany((x, i) => parts.Skip(i + 1).Select(y => new { First = x, Second = y }));
            foreach (var pair in pairs)
            {
                var pairTimes =
                    PartitionHandler.SymbolicExpressionHandler.Multiply(pair.First.Times, pair.Second.Times);
                var gcd = CalculateGcd((SymExpression)pair.First.Part, (SymExpression)pair.Second.Part);
                var lcm = CalculateLcm((SymExpression)pair.First.Part, (SymExpression)pair.Second.Part);
                var times = PartitionHandler.SymbolicExpressionHandler.Multiply(pairTimes, gcd);
                var contribution = new PartitionPart((SymExpression)lcm, (SymExpression)times);
                res.Add(contribution);
            }

            return new Partition(res);

        }


        public ISymbolicExpression GetNumberOfOddOrbitsInSwaps(SnWrS2RamificationType wreathType)
        {
            var count = new SymExpression("0");

            var handler = PartitionHandler.SymbolicExpressionHandler;
            foreach (var branch in wreathType.BranchCycles)
            {
                if (branch.HasSwap())
                {
                    var lhs = branch.LeftHandPartition;
                    var timeListForOddCycles = lhs.Parts.Where(p => handler.AssessParity(p.Part) == Parity.Odd).Select(p=>p.Times);
                    foreach (var times in timeListForOddCycles)
                    {

                        count = (SymExpression)handler.Add(count, times);
                    }
                }
            }

            return count;
        }
    }
}
