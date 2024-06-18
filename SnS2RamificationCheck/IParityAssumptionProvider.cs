using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SnS2RamificationCheck.Objects;

namespace SnS2RamificationCheck
{
   
    public class ParityAssumptionProvider 
    {
        public ParityAssumptionProvider()
        {

        }

        private IEnumerable<Dictionary<string, ParityAssumption>> NAssumptions0Mod1 =
            new List<Dictionary<string, ParityAssumption>>()
            {
                new Dictionary<string, ParityAssumption>()
                {

                    { "n", new ParityAssumption(equivTo: 0,modulo: 4) },

                },
                new Dictionary<string, ParityAssumption>()
                {

                    { "n", new ParityAssumption(equivTo: 1,modulo: 4) },

                },
                new Dictionary<string, ParityAssumption>()
                {

                    { "n", new ParityAssumption(equivTo:2,modulo: 4) },


                },
                new Dictionary<string, ParityAssumption>()
                {

                    { "n", new ParityAssumption(equivTo: 3,modulo: 4) },

                }
            };

        private IEnumerable<Dictionary<string, ParityAssumption>> NAssumptions1mod2 = new List<Dictionary<string, ParityAssumption>>()
        {
            new Dictionary<string, ParityAssumption>()
            {

                { "n", new ParityAssumption(equivTo: 1, modulo: 8) },

            },
            new Dictionary<string, ParityAssumption>()
            {

                { "n", new ParityAssumption(equivTo: 3, modulo: 8) }
            },
            new Dictionary<string, ParityAssumption>()
            {

                { "n", new ParityAssumption(equivTo: 5, modulo: 8) },

            },
            new Dictionary<string, ParityAssumption>()
            {

                { "n", new ParityAssumption(equivTo:7, modulo: 8) },

            },
        };
        private IEnumerable<Dictionary<string, ParityAssumption>> NAssumptions0mod2 = new List<Dictionary<string, ParityAssumption>>()
        {
            new Dictionary<string, ParityAssumption>()
            {

                { "n", new ParityAssumption(equivTo: 0, modulo: 8) },

            },
            new Dictionary<string, ParityAssumption>()
            {

                { "n", new ParityAssumption(equivTo: 2, modulo: 8) }
            },
            new Dictionary<string, ParityAssumption>()
            {

                { "n", new ParityAssumption(equivTo: 4, modulo: 8) },

            },
            new Dictionary<string, ParityAssumption>()
            {

                { "n", new ParityAssumption(equivTo:6, modulo: 8) },

            },
        };


        private IEnumerable<Dictionary<string, ParityAssumption>> NaAssumptions0mod2 =
            new List<Dictionary<string, ParityAssumption>>()
            {
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 0, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 1, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 0, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 3, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 0, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 5, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 0, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 7, modulo: 8) }
                }, //2mod8
                
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 2, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 1, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 2, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 3, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 2, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 5, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 2, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 7, modulo: 8) }
                },//4mod8
                
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 4, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 1, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 4, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 3, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 4, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 5, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 4, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 7, modulo: 8) }
                },
                //6mod8
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 6, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 1, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 6, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 3, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 6, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 5, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 6, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 7, modulo: 8) }
                }

            };




        private IEnumerable<Dictionary<string, ParityAssumption>> NaAssumptions1mod2 =
            new List<Dictionary<string, ParityAssumption>>()
            {
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 1, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 1, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 1, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 3, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 1, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 5, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 1, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 7, modulo: 8) }
                },

                //even a's
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 1, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 0, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 1, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 2, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 1, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 4, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 1, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 6, modulo: 8) }
                },
                //3mod8
                
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 3, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 1, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 3, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 3, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 3, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 5, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 3, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 7, modulo: 8) }
                }, //even a's
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 3, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 0, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 3, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 2, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 3, modulo: 8) },
                    { "a", new ParityAssumption(equivTo:4, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 3, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 8, modulo: 8) }
                },
                
                
                //5mod8
                
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 5, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 1, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 5, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 3, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 5, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 5, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 5, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 7, modulo: 8) }
                }, //even a's

                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 5, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 0, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 5, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 2, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 5, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 4, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 5, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 6, modulo: 8) }
                },
                //7mod8
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 7, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 0, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 7, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 2, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 7, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 4, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 7, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 6, modulo: 8) }
                }, //odd a's
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 7, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 1, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 7, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 3, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 7, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 5, modulo: 8) }
                },
                new Dictionary<string, ParityAssumption>()
                {
                    { "n", new ParityAssumption(equivTo: 7, modulo: 8) },
                    { "a", new ParityAssumption(equivTo: 7, modulo: 8) }
                }

            };

        private IEnumerable<Dictionary<string, ParityAssumption>> NaAssumptions0mod1 =
            new List<Dictionary<string, ParityAssumption>>()
            {
                new Dictionary<string, ParityAssumption>() {
                { "n", new ParityAssumption(equivTo: 0,modulo: 4) },
                {"a", new ParityAssumption(equivTo:1, modulo:4)}
                },
                new Dictionary<string, ParityAssumption>() {
                    { "n", new ParityAssumption(equivTo: 0,modulo: 4) },
                    {"a", new ParityAssumption(equivTo:3, modulo:4)}
                },
                new Dictionary<string, ParityAssumption>() {
                    { "n", new ParityAssumption(equivTo: 1,modulo: 4) },
                    {"a", new ParityAssumption(equivTo:0, modulo:4)}
                },
                new Dictionary<string, ParityAssumption>() {
                    { "n", new ParityAssumption(equivTo: 1,modulo: 4) },
                    {"a", new ParityAssumption(equivTo:1, modulo:4)}
                },
                new Dictionary<string, ParityAssumption>() {
                    { "n", new ParityAssumption(equivTo: 1,modulo: 4) },
                    {"a", new ParityAssumption(equivTo:2, modulo:4)}
                },
                new Dictionary<string, ParityAssumption>() {
                    { "n", new ParityAssumption(equivTo: 1,modulo: 4) },
                    {"a", new ParityAssumption(equivTo:3, modulo:4)}
                },
                new Dictionary<string, ParityAssumption>() {
                    { "n", new ParityAssumption(equivTo: 2,modulo: 4) },
                    {"a", new ParityAssumption(equivTo:1, modulo:4)}
                },
                new Dictionary<string, ParityAssumption>() {
                    { "n", new ParityAssumption(equivTo: 2,modulo: 4) },
                    {"a", new ParityAssumption(equivTo:3, modulo:4)}
                },
                new Dictionary<string, ParityAssumption>() {
                    { "n", new ParityAssumption(equivTo: 3,modulo: 4) },
                    {"a", new ParityAssumption(equivTo:0, modulo:4)}
                },
                new Dictionary<string, ParityAssumption>() {
                    { "n", new ParityAssumption(equivTo: 3,modulo: 4) },
                    {"a", new ParityAssumption(equivTo:1, modulo:4)}
                },
                new Dictionary<string, ParityAssumption>() {
                    { "n", new ParityAssumption(equivTo: 3,modulo: 4) },
                    {"a", new ParityAssumption(equivTo:2, modulo:4)}
                },
                new Dictionary<string, ParityAssumption>() {
                    { "n", new ParityAssumption(equivTo: 3,modulo: 4) },
                    {"a", new ParityAssumption(equivTo:3, modulo:4)}
                }
            };

        
        private Dictionary<string, IEnumerable<Dictionary<string, ParityAssumption>>> _assumptions;

        private Dictionary<string, IEnumerable<Dictionary<string, ParityAssumption>>> _assumptionsSetsForParameterTypes;

        private IEnumerable<Dictionary<string, ParityAssumption>> _defaultAssumptions;

        public IEnumerable<Dictionary<string, ParityAssumption>> DefaultAssumptions
        {
            get
            {
                if (_defaultAssumptions != null)
                {
                    return _defaultAssumptions;
                }

                _defaultAssumptions = GetDefaultAssumptions();
                return _defaultAssumptions;
            }
        }
        public Dictionary<string, IEnumerable<Dictionary<string, ParityAssumption>>> TypesWithPossibleNAssumptionsBasedOnDenominator
        {
            get
            {
                if (_assumptions != null)
                {
                    return _assumptions;
                }

                _assumptions = ParseTypesWithPossibleNAssumptionsBasedOnDenominatorFromFile();
                return _assumptions;
            }
        }

        public Dictionary<string, IEnumerable<Dictionary<string, ParityAssumption>>>
            AssumptionsForTypesWithParameterDependentParts
        {
            get
            {
                if (_assumptionsSetsForParameterTypes == null)
                {
                   _assumptionsSetsForParameterTypes =  LoadAssumptionSetsForParameterTypes();
                }

                return _assumptionsSetsForParameterTypes;
            }
        }


        public IEnumerable<Dictionary<string,ParityAssumption>> GetAllPossibleAssumptionsForSnType(SnRamificationType snType)
        {

            //check if type contains parameter
            if (!snType.BranchCycles.Any(b =>
                    b.Partition.Parts.Any(p =>
                        p.Part.Expression.Contains("n") || p.Part.Expression.Contains("a")))) 
            {

                var possibleNAssumptionsBasedOnDenominator = TypesWithPossibleNAssumptionsBasedOnDenominator[snType.Name.Trim()];
                return possibleNAssumptionsBasedOnDenominator;
            }

            return AssumptionsForTypesWithParameterDependentParts[snType.Name.Trim()];
        }
        //it's an ienumerable because we want all possible assumptions. The trouble is that we load the ram type separately from the assumptions; we can either fix input or 
        public IEnumerable<Dictionary<string, ParityAssumption>> GetAllPossibleAssumptionsForType(SnWrS2RamificationType snwrs2Type)
        {

            //check if type contains parameter
            if (!snwrs2Type.BranchCycles.Any(b =>
                    b.LeftHandPartition.Parts.Any(p =>
                        p.Part.Expression.Contains("n") || p.Part.Expression.Contains("a")) || b.RightHandPartition.Parts.Any(p=>p.Part.Expression.Contains("n") || p.Part.Expression.Contains("a"))))
            {

                var possibleNAssumptionsBasedOnDenominator = TypesWithPossibleNAssumptionsBasedOnDenominator[snwrs2Type.Name];
                return possibleNAssumptionsBasedOnDenominator;
            }

            return AssumptionsForTypesWithParameterDependentParts[snwrs2Type.Name]; //TODO throw exception if key not found

        }


        public Dictionary<string, IEnumerable<Dictionary<String, ParityAssumption>>>
            LoadAssumptionSetsForParameterTypes()
        {
            var res = new Dictionary<string, IEnumerable<Dictionary<string, ParityAssumption>>>();
            var assumptionSetsForTypes = GetAssumptionSetsForParameterTypes();
            //now match strings to types
            foreach (var typeInfo in assumptionSetsForTypes)
            {
                var assumptionSet = GetAssumptionSetForAssumptionSetName(typeInfo.Item2); //IEnumerable<Dictionary<String,ParityAssumption>>
                res.Add(typeInfo.Item1,assumptionSet);
            }
            return res;
        }

        public IEnumerable<Dictionary<String, ParityAssumption>> GetAssumptionSetForAssumptionSetName(
            string assumptionSetName)
        {
            if (assumptionSetName == "na01")
            {
                return NaAssumptions0mod1;
            }

            if (assumptionSetName== "n01")
            {
                return NAssumptions0Mod1;
            }

            if (assumptionSetName == "n12")
            {
                return NAssumptions1mod2;
            }

            if (assumptionSetName == "n02")
            {
                return NAssumptions0mod2;
            }

            if (assumptionSetName == "na02")
            {
                return NaAssumptions0mod2; 
            }

            if (assumptionSetName == "na12")
            {
                return NaAssumptions1mod2;
            }

            throw new Exception("Assumption set does not exist " + assumptionSetName);
            return null; 
        }

            public Dictionary<string, IEnumerable<Dictionary<string, ParityAssumption>>> ParseTypesWithPossibleNAssumptionsBasedOnDenominatorFromFile()
        {
            var res = new Dictionary<string, IEnumerable<Dictionary<string, ParityAssumption>>>();
            var denominatorAssumptions = GetDenominatorAssumptions();
            foreach (var denominatorAssumption in denominatorAssumptions)
            {
                
                //we have one denominator assumption per type, now we want two - one so that n-k/d is odd, the other so that is is even, i.e. 
                var typeAssumptions = new List<Dictionary<string, ParityAssumption>>();

                var nAssumption1 = new ParityAssumption(2 * denominatorAssumption.Item3, denominatorAssumption.Item2);

                var nAssumption2 = new ParityAssumption(2 * denominatorAssumption.Item3,
                    denominatorAssumption.Item2 + denominatorAssumption.Item3);

                var assumptionsForType = new List<Dictionary<string, ParityAssumption>>()
                {
                    new Dictionary<string, ParityAssumption>() { { "n", nAssumption1 } },
                    new Dictionary<string, ParityAssumption>() { { "n", nAssumption2 } }

                };
                res.Add(denominatorAssumption.Item1,assumptionsForType);
            }

            return res;
        }
/*
        public Dictionary<string, IEnumerable<Dictionary<string, ParityAssumption>>> ParseTypesWithPossibleNAssumptionsBasedOnDenominatorFromFile()
        {
            var res = new Dictionary<string, IEnumerable<Dictionary<string, ParityAssumption>>>();
            var denominatorAssumptions = GetDenominatorAssumptions();
            foreach (var denominatorAssumption in denominatorAssumptions)
            {
                //we have one denominator assumption per type, now we want two
                var typeAssumptions = new List<Dictionary<string, ParityAssumption>>();

                var nAssumption1 = new ParityAssumption(2 * denominatorAssumption.Item3, denominatorAssumption.Item2);

                var nAssumption2 = new ParityAssumption(2 * denominatorAssumption.Item3,
                    denominatorAssumption.Item2 + denominatorAssumption.Item3);
                //TODO calculate a assumptions and add


                if ((nAssumption1.EquivTo + nAssumption1.Modulo) % 2 == 1) //n is odd 
                {
                    var aAssumptions1 = new Dictionary<string, ParityAssumption>();
                    aAssumptions1.Add("n", nAssumption1);
                    aAssumptions1.Add("a",
                        new ParityAssumption(2 * denominatorAssumption.Item3, denominatorAssumption.Item2));

                    typeAssumptions.Add(aAssumptions1);



                    var aAssumptions2 = new Dictionary<string, ParityAssumption>
                    {
                        { "n", nAssumption1 },
                        {
                            "a", new ParityAssumption(2 * denominatorAssumption.Item3,
                                denominatorAssumption.Item2 + denominatorAssumption.Item3)
                        }
                    };

                    typeAssumptions.Add(aAssumptions2);
                }
                else
                {
                    //add just the a assumptions where a is odd, and we know that nequiv+ nmodulo is even
                    var aAssumptions3 = new Dictionary<string, ParityAssumption>();
                    aAssumptions3.Add("n", nAssumption1);
                    aAssumptions3.Add("a",
                        new ParityAssumption(2 * denominatorAssumption.Item3, denominatorAssumption.Item3 + denominatorAssumption.Item2));

           //           aAssumptions3.Add("a",new ParityAssumption(2,1));
                    typeAssumptions.Add(aAssumptions3);
                }



                if ((nAssumption2.EquivTo + nAssumption2.Modulo) % 2 == 1) //n is odd 
                {
                    var aAssumptions12 = new Dictionary<string, ParityAssumption>();
                    aAssumptions12.Add("n", nAssumption2);
                    aAssumptions12.Add("a",
                        new ParityAssumption(2 * denominatorAssumption.Item3, denominatorAssumption.Item2));

                    typeAssumptions.Add(aAssumptions12);



                    var aAssumptions22 = new Dictionary<string, ParityAssumption>();
                    aAssumptions22.Add("n", nAssumption2);
                    aAssumptions22.Add("a",
                        new ParityAssumption(2 * denominatorAssumption.Item3,
                            denominatorAssumption.Item2 + denominatorAssumption.Item3));

                    typeAssumptions.Add(aAssumptions22);
                }
                else
                {
                    //add just the a assumptions where a is odd, and we know that nequiv+ nmodulo is even
                    var aAssumptions32 = new Dictionary<string, ParityAssumption>();
                    aAssumptions32.Add("n", nAssumption2);
                    aAssumptions32.Add("a",
                        new ParityAssumption(2 * denominatorAssumption.Item3, denominatorAssumption.Item3 + denominatorAssumption.Item2));
 
           //          aAssumptions32.Add("a", new ParityAssumption(2,1));
                    typeAssumptions.Add(aAssumptions32);
                }

                res.Add(denominatorAssumption.Item1, typeAssumptions);
            }

            return res;
        }
*/
        private IEnumerable<Dictionary<string, ParityAssumption>> GetDefaultAssumptions()
        {

            var assumptions = new List<Dictionary<string, ParityAssumption>>();
            for (var i = 1; i < 3; i++)
            {
                for (var j = 1; j < 3; j++)
                {
                    if (!(i % 2 == 0 && j % 2 == 0))
                        assumptions.Add(
                            new Dictionary<string, ParityAssumption>
                            {
                                { "n", new ParityAssumption(modulo: 8, equivTo: i) },
                                { "a", new ParityAssumption(modulo: 8, equivTo: j) }
                            }
                        );
                }
            }

            return assumptions;
        }

        public IEnumerable<Dictionary<String, ParityAssumption>> LoadAllAssumptionsForType(string typeName)
        {
            if (TypesWithPossibleNAssumptionsBasedOnDenominator.ContainsKey(typeName))
            {
                return TypesWithPossibleNAssumptionsBasedOnDenominator[typeName];
            }
            else
            {
                return DefaultAssumptions; //TODO?
            }
        }
    

        private IEnumerable<Dictionary<String, ParityAssumption>> GetPlaceholderForAssumptions()
        {
            var assumptions = new List<Dictionary<string, ParityAssumption>>();
            for (var i = 1; i < 13; i++)
            {
                for (var j = 1; j < 9; j++)
                {
                    if (!(i % 2 == 0 && j % 2 == 0))
                        assumptions.Add(
                            new Dictionary<string, ParityAssumption>
                            {
                                { "n", new ParityAssumption(modulo: 8, equivTo: i) },
                                { "a", new ParityAssumption(modulo: 8, equivTo: j) }
                            }
                        );
                }
            }

            return assumptions;
        }

        private IEnumerable<Tuple<string, string>> GetAssumptionSetsForParameterTypes()
        {
            string fileNmae = "parity_assumptions_for_parameter_types.txt";

            string solutionDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string filePath = Path.Combine(solutionDirectory, "Input", fileNmae);
            var lines = File.ReadAllLines(filePath);

            var res = new List<Tuple<string, string>>();
            foreach (var line in lines)
            {
                var strippedLine = Regex.Replace(line, @"\s+", ""); //remove whitespace
                var parts = strippedLine.Split("&");
                if (parts.Length != 2)
                {
                    throw new Exception("something wrong");
                }

                var typeName = parts[0];
                var assumption = parts[1];
                res.Add(new Tuple<string, string>(typeName, assumption));
            }

            return res;
        }
        private IEnumerable<Tuple<string, int, int>> GetDenominatorAssumptions()
        {
            string solutionDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            string filePath = Path.Combine(solutionDirectory, "Input", "i_mod_k_assumptions_for_n_parameter.txt");
            var lines = File.ReadAllLines(filePath);

            var res = new List<Tuple<string, int, int>>();
            foreach (var line in lines)
            {
                var strippedLine = Regex.Replace(line, @"\s+", ""); //remove whitespace
                var parts = strippedLine.Split("&");
                if (parts.Length != 3)
                {
                    throw new Exception("something wrong");
                }

                var typeName = parts[0];
                var assumption = Int32.Parse(parts[1]);
                var denominator = Int32.Parse(parts[2]);
                res.Add(new Tuple<string, int, int>(typeName, assumption, denominator));
            }

            return res;
        }
    }
}
