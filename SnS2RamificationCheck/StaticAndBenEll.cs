using SnS2RamificationCheck.Interfaces;
using SnS2RamificationCheck.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SnS2RamificationCheck
{
    public static class StaticAndBenEll
    {
        public static List<string> VariableNames = new List<String>() { "n", "a" };
        public static List<SnRamificationType> ExceptionalSymmetricTypes = new List<SnRamificationType>()
        {
            new SnRamificationType("I1.1", new List<SnBranchCycle>()
            {
                new SnBranchCycle(new Interfaces.Partition(new List<PartitionPart>() { new PartitionPart("n","1")})),
                new SnBranchCycle(new Interfaces.Partition(new List<PartitionPart>() { new PartitionPart("a","1"), new PartitionPart("n-a","1") })),
                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("2","1"), new PartitionPart("1","n-2")}))
            }),

            new SnRamificationType("I2.1", new List<SnBranchCycle>()
            {
                new SnBranchCycle(new Interfaces.Partition(new List<PartitionPart>() { new PartitionPart("n","1")})),
                new SnBranchCycle(new Interfaces.Partition(new List<PartitionPart>() { new PartitionPart("1","3"), new PartitionPart("2","(n-3)/2") })),
                new SnBranchCycle(new Partition(new List<PartitionPart>(){new PartitionPart("2","1"), new PartitionPart("1","n-2")}))
            })
        };


        public static List<SnWrS2RamificationType> WreathTypes = new List<SnWrS2RamificationType>() {
            new SnWrS2RamificationType("I1.1", new List<SnWrS2BranchCycleReducedForm>()
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
            }),
                 new SnWrS2RamificationType("I1A.1", new List<SnWrS2BranchCycleReducedForm>()
            {
                new SnWrS2BranchCycleReducedForm(new Partition(new List<PartitionPart>() { new PartitionPart("n","1")}),
                                                new Partition(new List<PartitionPart>() { new PartitionPart("n","1")}),
                                                false),
                    new SnWrS2BranchCycleReducedForm(new Partition(new List<PartitionPart>() { new PartitionPart("3","1"), new PartitionPart("1","n-3")}),
                                                new Partition(new List<PartitionPart>() { new PartitionPart("1","n")}),
                                                false),
                    new SnWrS2BranchCycleReducedForm(new Partition(new List<PartitionPart>() { new PartitionPart("1","n")}),
                                                 new Partition(new List<PartitionPart>() { new PartitionPart("1","n")}),
                                                 true),
                              new SnWrS2BranchCycleReducedForm(new Partition(new List<PartitionPart>() { new PartitionPart("1","n")}),
                                                 new Partition(new List<PartitionPart>() { new PartitionPart("1","n")}),
                                                 true)
            })
        };


        public static string ConcatenateToString<T>(this IEnumerable<T> list)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (T item in list)
            {
                stringBuilder.Append(item.ToString());
            }

            return stringBuilder.ToString();
        }

        public static bool ListEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {

            var list1Empty = !list1.Any();
            var list2Empty = !list2.Any();
            if (list1Empty && list2Empty)
            {
                return true;
            }

            if (list1.Count() != list2.Count())
            {
                return false;
            }
            //copy list2
            var tempList = list2.ToList();
            foreach(var item in list1)
            {
                //lookup in list2 and remove
                if (tempList.Contains(item))
                {
                    tempList.Remove(item);
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
    }
}
