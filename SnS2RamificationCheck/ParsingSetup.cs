using SnS2RamificationCheck.Interfaces;
using SnS2RamificationCheck.Objects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SnS2RamificationCheck
{
    public class ParsingSetup
    {

        public static IEnumerable<SnWrS2RamificationType> LoadFullWreathTypes()
        {
            string solutionDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string filePath = Path.Combine(solutionDirectory, "Input", "FullWreathTypes.txt");
            return LoadWreathTypes(filePath);
        }

        public static IEnumerable<SnWrS2RamificationType> LoadAllTypes()
        {
            string solutionDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string filePath = Path.Combine(solutionDirectory, "Input", "WreathTypes.txt");
            return LoadWreathTypes(filePath);
        }
        public static IEnumerable<SnWrS2RamificationType> LoadWreathTypes(string filePath)
        {       
            var lines = File.ReadAllLines(filePath);

            var res = new List<SnWrS2RamificationType>();
            foreach (var line in lines)
            {
                var strippedLine = Regex.Replace(line, @"\s+", ""); //remove whitespace
                var parts = strippedLine.Split("&");
                if (parts.Length != 2)
                {
                    throw new Exception("something wrong");
                }

                var name = parts[0];
                Debug.WriteLine(name);
                var typeString = parts[1];
                var cycles = ParseWreathBranchCyclesFromString(typeString);

                res.Add(new SnWrS2RamificationType(name, cycles));
            }
            return res;
        }

      

        public static IEnumerable<SnRamificationType> LoadSnTypes()
        {

            string solutionDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string filePath = Path.Combine(solutionDirectory, "Input", "SymmetricTypesLatex.txt");



            var lines = File.ReadAllLines(filePath);
            var res = new List<SnRamificationType>();
            foreach (var line in lines)
            {

                var parts = line.Split("&");
                if (parts.Length != 2)
                {
                    throw new Exception("something wrong");
                }
                var name = parts[0];
                var typeString = parts[1];
                var cycles = ParseBranchCyclesFromString(typeString);
               
                res.Add(new SnRamificationType(name,cycles));
            }
            return res;
        }

        public static IEnumerable<SnWrS2BranchCycleReducedForm> ParseWreathBranchCyclesFromString(string typeString)
        {
     

            var cycleStrings = SplitByCommasOutsideParentheses(typeString);
            var cycles = new List<SnWrS2BranchCycleReducedForm>();
            foreach (var str in cycleStrings)
            {
                var trimmed = str.Trim('(');
                trimmed = trimmed.Trim(')');
                var parsedCycle = ParseSnWrS2BranchCycle(trimmed);
                cycles.Add(parsedCycle);
            }
            return cycles;
        }


        private static IEnumerable<string> SplitByCommasOutsideParentheses(string inputString, char startP = '(', char endP = ')')
        {
            List<string> result = new List<string>();
            string currentChunk = "";
            Stack<char> stack = new Stack<char>();

            foreach (char c in inputString)
            {
                if (c == ',' && stack.Count == 0)
                {
                    result.Add(currentChunk.Trim());
                    currentChunk = "";
                }
                else
                {
                    currentChunk += c;

                    if (c == startP)
                    {
                        stack.Push(startP);
                    }
                    else if (c == endP)
                    {
                        if (stack.Count > 0)
                        {
                            stack.Pop();
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(currentChunk))
            {
                result.Add(currentChunk.Trim());
            }

            return result;
        }

        //TODO assumes input has only "legal" parenthesis

        public static string TrimOuterParenthesis(string str, char leftP, char rightP)
        {
            var trimmed = str.Trim(leftP);
            return trimmed.Trim(rightP);
        }
        public static SnWrS2BranchCycleReducedForm ParseSnWrS2BranchCycle(string str)
        {
            //(([n],[1^n]),True) // (([a,n-a],[1^n]),True) // (([2,1^{n-2}], [1^n]),False) 
            //need to split into LHS and RHS and bool  
            if (str.Contains('['))
            {
                var elements = SplitByCommasOutsideParentheses(str, '[', ']');
                if (elements.Count()!=3)
                {
                    throw new Exception("something wrong while parsing wreath types"+str);
                }
                var lhs = TrimOuterParenthesis(TrimOuterParenthesis(elements.ElementAt(0),'(',')'),'[',']');
                var rhs = TrimOuterParenthesis(TrimOuterParenthesis(elements.ElementAt(1), '(', ')'),'[',']');
                var swap = TrimOuterParenthesis(elements.ElementAt(2),'(',')'); //TODO maybe change par method to return array
                

                var lhsBranch = ParseSingleBranchCycle(lhs);
                var rhsBranch = ParseSingleBranchCycle(rhs);
                var parsedSwap = false;
                var trimmedSwap = Regex.Replace(swap, @"\s+", "");
                if (string.IsNullOrEmpty(trimmedSwap) || string.Equals(trimmedSwap,"False")) {
                    parsedSwap = false;
                }
                else if (string.Equals(trimmedSwap, "True"))
                {
                    parsedSwap = true;
                }
                else
                {
                    throw new Exception("error parsing wreath string" + str);
                }
                return new SnWrS2BranchCycleReducedForm(lhsBranch, rhsBranch, parsedSwap);
            }
            else
            {
                throw new Exception("something wrong parsing sn wr s2 type" + str);
                
            }
        }

        public static SnBranchCycle ParseSingleBranchCycle(string cycle)
        {
            var parts = cycle.Split(",");

            var partitionParts = new List<PartitionPart>();
            //parse each part into part and times
            foreach (var part in parts)
            {
                string times = "1";
                string part_expression = "";
                if (part.Contains("^")) //part appears more than once
                {
                    var parts_of_part = part.Split("^");
                    part_expression = parts_of_part[0];

                    //times is what appears in {}
                    var timesMatch = Regex.Match(part, @"\{([^}]+)\}");
                    if (timesMatch.Success)
                    {
                        times = timesMatch.Groups[1].Value;
                    }
                    else
                    {
                        times = parts_of_part[1];
                    }
                }

                else
                {
                    part_expression = part; //some lazy logic here because we assume that if {} exists necessarily it's of the form ^{}
                }


                partitionParts.Add(new PartitionPart(part_expression, times));
            }
            return new SnBranchCycle(new Partition(partitionParts));
        }
        public static IEnumerable<SnBranchCycle> ParseBranchCyclesFromString(string str)
        { 
            str =  Regex.Replace(str, @"\s+", "");
            //split into cycles contained in []
            var cycles =
                  Regex.Matches(str, @"\[([^]]*)\]")
                      .Cast<Match>()
                      .Select(x => x.Groups[1].Value)
                      .ToList();

            var branchCycles = new List<SnBranchCycle>();
            //parse each cycle
            foreach (var cycle in cycles)
            {              
                branchCycles.Add(ParseSingleBranchCycle(cycle));
            }
            return branchCycles;

        }
    }
}
