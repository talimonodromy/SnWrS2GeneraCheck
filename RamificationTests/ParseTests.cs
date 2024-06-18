using Microsoft.VisualStudio.TestTools.UnitTesting;
using SnS2RamificationCheck;
using SnS2RamificationCheck.Interfaces;
using SnS2RamificationCheck.Objects;
using System;
using System.Collections.Generic;
using System.Text;

namespace RamificationTests
{
    [TestClass]
   public class ParseTests
    {

        [TestMethod]
        public void TestTimesParsing()
        {

            var t1 = new SnRamificationType("I1.1", new List<SnBranchCycle>()
            {
                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("1","n")}))
            });

            var t2 = new SnRamificationType("I1.1", new List<SnBranchCycle>()
            {
                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("1","1")}))
            });

            //TODO without a logical handler, partition is identity only if parts are all 1. 
            Assert.IsTrue(t1.Equals(t2));
        }

        [TestMethod]
        public void TestParseSnTypeString()
        {
            var str = "[n],[a,n-a],[1^{n-4},2^2],[1^n]";

            var expectedType = new SnRamificationType("I1.1", new List<SnBranchCycle>()
            {
                new SnBranchCycle(new Partition(new List<PartitionPart>() { new PartitionPart("n","1")})),
                new SnBranchCycle(new Partition(new List<PartitionPart>() { new PartitionPart("a","1"), new PartitionPart("n-a","1") })),
                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("2","2"), new PartitionPart("1","n-4")})),
                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("1","n")}))
            });

            var parsedCycles = ParsingSetup.ParseBranchCyclesFromString(str);
            Assert.IsTrue(StaticAndBenEll.ListEquals<SnBranchCycle>(parsedCycles, expectedType.BranchCycles));

        }


        [TestMethod]
        public void TestParseWreathTypeString()
        {
            var str = "(([n],[1^n]),True), (([a,n-a],[1^n]),True), (([2,1^{n-2}], [1^n]),False)";
            var expectedType = new SnWrS2RamificationType("I1.1", new List<SnWrS2BranchCycleReducedForm>()
            {
                new SnWrS2BranchCycleReducedForm(new Partition(new List<PartitionPart>() { new PartitionPart("n","1")}),
                                                new Partition(new List<PartitionPart>() { new PartitionPart("1","n")}),
                                                true),
                    new SnWrS2BranchCycleReducedForm(new Partition(new List<PartitionPart>() { new PartitionPart("a","1"), new PartitionPart("n-a","1")}),
                                                new Partition(new List<PartitionPart>() { new PartitionPart("1","n")}),
                                                true),
                    new SnWrS2BranchCycleReducedForm(new Partition(new List<PartitionPart>() { new PartitionPart("2","1"), new PartitionPart("1","n-2")}),
                                                 new Partition(new List<PartitionPart>() { new PartitionPart("1","n")}),
                                                 false)
            });

            var parsedCycles = ParsingSetup.ParseWreathBranchCyclesFromString(str);
            Assert.IsTrue(StaticAndBenEll.ListEquals<SnWrS2BranchCycleReducedForm>(parsedCycles, expectedType.BranchCycles));
        }

    }
}
