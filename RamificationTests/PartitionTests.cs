using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnS2RamificationCheck;
using SnS2RamificationCheck.Interfaces;
using SnS2RamificationCheck.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace RamificationTests
{
    [TestClass]
    public class PartitionTests
    {

        [TestMethod]
        public void TestPartitionIsIdentity()
        {
            var handler = new SymbolicExpressionHandler("n", null, null,null);
            var partitionHandler = new PartitionHandler(handler);

            var transposition = new PartitionPart("2", "1");
            var transpositionPartition = new Partition(new List<PartitionPart> { transposition, new PartitionPart("1", "n-2") });

            Assert.IsFalse(transpositionPartition.IsIdentity());



            var part1 = new PartitionPart("1", "2");
            var partition2 = new Partition(new List<PartitionPart> { part1, new PartitionPart("1", "n-2") });
            Assert.IsTrue(partition2.IsIdentity());
        }

        [TestMethod]
        public void TestPartitionEquality()
        {
            var handler = new SymbolicExpressionHandler("n", null, null, null);
            var partitionHandler = new PartitionHandler(handler);

            var partition1 = new Partition(new List<PartitionPart> { new PartitionPart("1","2"), new PartitionPart("1", "n-2") });

            var partition2 = new Partition(new List<PartitionPart> { new PartitionPart("1", "n") });

            Assert.IsTrue(partitionHandler.PartitionEquals(partition1, partition2));
        }

        [TestMethod]
        public void TestSnRamificationTypeEqualityComparer()
        {
            var comparer = new SnS2RamificationCheck.Objects.SnRamificationTypeEqualityComparer();

            var strippedType = new SnRamificationType("I1.1", new List<SnBranchCycle>()
            {
                new SnBranchCycle(new SnS2RamificationCheck.Interfaces.Partition(new List<PartitionPart>() { new PartitionPart("n","1")})),
                new SnBranchCycle(new SnS2RamificationCheck.Interfaces.Partition(new List<PartitionPart>() { new PartitionPart("a","1"), new PartitionPart("n-a","1") })),
                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("2","1"), new PartitionPart("1","n-2")}))
            });
            var typeWithExtraIdentities = new SnRamificationType("I1.1", new List<SnBranchCycle>()
            {
                new SnBranchCycle(new SnS2RamificationCheck.Interfaces.Partition(new List<PartitionPart>() { new PartitionPart("n","1")})),
                new SnBranchCycle(new SnS2RamificationCheck.Interfaces.Partition(new List<PartitionPart>() { new PartitionPart("a","1"), new PartitionPart("n-a","1") })),
                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("2","1"), new PartitionPart("1","n-2")})),
                                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("1","2"), new PartitionPart("1","n-2")}))
            });

            Assert.IsTrue(strippedType.Equals(typeWithExtraIdentities));

        }



        [TestMethod]
        public void Test2OrbitsForRegularType()
        {
            var transposition = new Partition(new List<PartitionPart>()
                { new PartitionPart("2", "1"), new PartitionPart("1", "n-2") });

            var expectedPartition = new Partition(new List<PartitionPart>()
            {
                new PartitionPart("1", "1"), new PartitionPart("1", "(n-2)*(n-3)/2"), new PartitionPart("2", "n-2")
            });

            var handler = new SymbolicExpressionHandler("n", null, null, new Dictionary<string, ParityAssumption>() { {"n",new ParityAssumption(0,2)}});
            var partitionHandler = new PartitionHandler(handler);
            var calculator = new RamificationTypeCalculator(partitionHandler);
            var twoOrbits = calculator.GetOrbitsInTwoSetAction(new SnBranchCycle(transposition));

            var res = partitionHandler.PartitionEquals(expectedPartition, twoOrbits);
            Assert.IsTrue(res);
        }

        [TestMethod]
        public void TestHardcoded2Orbits()
        {
            var nCycle = new Partition(new List<PartitionPart>() { new PartitionPart("n", "1") });

            var expectedPartition = new Partition(new List<PartitionPart>() { new PartitionPart("n", "(n-1)/2") });
            var assumeNOdd = new ParityAssumption(modulo: 2, equivTo: 1);

            var handler1 = new SymbolicExpressionHandler("n", null, null,
                new Dictionary<string, ParityAssumption>() { { "n", assumeNOdd } });
            var partitionHandler = new PartitionHandler(handler1);

            var calculator = new RamificationTypeCalculator(partitionHandler);
            var twoOrbits = calculator.GetOrbitsInTwoSetAction(new SnBranchCycle(nCycle));

            var res = partitionHandler.PartitionEquals(twoOrbits, expectedPartition);
            Assert.IsTrue(res);
            
        }

        [TestMethod]
        public void TestHardcoded2Orbits2()
        {
            var nCycle = new Partition(new List<PartitionPart>()
                { new PartitionPart("a", "1"), new PartitionPart("n-a", "1") });

            var expectedPartition =
                new Partition(
                    new List<PartitionPart>()
                    {
                        new PartitionPart("a", "(a-1)/2"), new PartitionPart("(n-a)/2", "1"),
                        new PartitionPart("n-a", "(n-a)/2-1"),
                    new PartitionPart("(n-a)*a", "1")
                    });

            var assumeNOdd = new ParityAssumption(modulo: 4, equivTo: 1);
            var assumeAOdd = new ParityAssumption(modulo: 2, equivTo: 1);

            var handler1 = new SymbolicExpressionHandler("n", "a", null,
                new Dictionary<string, ParityAssumption>() { { "n", assumeNOdd } , {"a", assumeAOdd}});
            var partitionHandler = new PartitionHandler(handler1);

            var calculator = new RamificationTypeCalculator(partitionHandler);
            var twoOrbits = calculator.GetOrbitsInTwoSetAction(new SnBranchCycle(nCycle));

            var res = partitionHandler.PartitionEquals(twoOrbits, expectedPartition);
            Assert.IsTrue(res);

        }

        [TestMethod]
        public void TestIntegerGCD()
        {
            var gcd1 = RamificationTypeCalculator.CalculateIntegerGCD(2, 4);
            Assert.AreEqual(2,gcd1);
            
            Assert.AreEqual(1,RamificationTypeCalculator.CalculateIntegerGCD(3,4));
            Assert.AreEqual(1,RamificationTypeCalculator.CalculateIntegerGCD(4,3));

            Assert.AreEqual(2,RamificationTypeCalculator.CalculateIntegerGCD(2,2));

          
        }

        [TestMethod]
        public void TestAssumptionsCompatible()
        {
            var assumptions1 = new ParityAssumption(2, 1);
            var assumption2 = new ParityAssumption(4, 3);

            var compatible = RamificationTypeCalculator.AssumptionsCompatible(assumptions1, assumption2);

            Assert.IsTrue(compatible);
        }

        [TestMethod]
        public void Test2PtGenusCalculation()
        {
            var inputCycle = new SnRamificationType("I1.1", new List<SnBranchCycle>()
            {
                new SnBranchCycle(new Partition(new List<PartitionPart>() { new PartitionPart("n","1")})),
                new SnBranchCycle(new Partition(new List<PartitionPart>() { new PartitionPart("a","1"), new PartitionPart("n-a","1") })),
                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("2","1"), new PartitionPart("1","n-2")})),
                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("1","n")}))
            });
            var assumeNOdd = new ParityAssumption(modulo: 4, equivTo: 1);
            var assumeAOdd = new ParityAssumption(modulo: 2, equivTo: 1);
            var handler1 = new SymbolicExpressionHandler("n", "a", null,
                new Dictionary<string, ParityAssumption>() { { "n", assumeNOdd }, { "a", assumeAOdd } });
            var partitionHandler = new PartitionHandler(handler1);

            var ramCalculator = new RamificationTypeCalculator(partitionHandler);
            var twoSetgenus = ramCalculator.GetTwoSetStabilizerGenus(inputCycle,
                new SymExpression("n*(n-1)/2"), new SymExpression("0"));

            var twoPointGenus = ramCalculator.GetTwoPointStabilizer(inputCycle, twoSetgenus);

            var res = partitionHandler.SymbolicExpressionHandler.ExpressionsEqual(twoPointGenus, new SymExpression("0"));
            Assert.IsTrue(res);

        }
        [TestMethod]
        public void Test2setGenusCalculation()
        {
            var inputCycle = new SnRamificationType("I1.1", new List<SnBranchCycle>()
            {
                new SnBranchCycle(new Partition(new List<PartitionPart>() { new PartitionPart("n","1")})),
                new SnBranchCycle(new Partition(new List<PartitionPart>() { new PartitionPart("a","1"), new PartitionPart("n-a","1") })),
                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("2","1"), new PartitionPart("1","n-2")})),
                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("1","n")}))
            });

            var assumeNOdd = new ParityAssumption(modulo: 4, equivTo: 1);
            var assumeAOdd = new ParityAssumption(modulo: 2, equivTo: 1);
            var handler1 = new SymbolicExpressionHandler("n", "a", null,
                new Dictionary<string, ParityAssumption>() { { "n", assumeNOdd }, { "a", assumeAOdd } });
            var partitionHandler = new PartitionHandler(handler1);

            var ramCalculator = new RamificationTypeCalculator(partitionHandler);
            var genus = ramCalculator.GetTwoSetStabilizerGenus(inputCycle,
                new SymExpression("n*(n-1)/2"), new SymExpression("0"));

            var res = partitionHandler.SymbolicExpressionHandler.ExpressionsEqual(genus, new SymExpression("0"));
            Assert.IsTrue(res);
        }
    }



}
