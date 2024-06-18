using SnS2RamificationCheck.Interfaces;
using SnS2RamificationCheck.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Data;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SymExpression = SnS2RamificationCheck.Interfaces.SymExpression;

namespace SnS2RamificationCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Parsing Sn types from txt file");
            var snTypes = ParsingSetup.LoadSnTypes();
            //     Console.WriteLine(String.Join(Environment.NewLine, snTypes.Select(t => t.ToString())));

            Console.WriteLine("Parsing Wreath types from txt file");
            var wreathTypes = ParsingSetup.LoadAllTypes();
            //   Console.WriteLine(String.Join(Environment.NewLine, wreathTypes.Select(t => t.ToString())));


            //option1: read all objects into a new "SymbolicX" object. (this contains the X object 
            // and then we will need all "equals" calculations coming up to involve a handler

            //TODO we either need our type objects to contain assumptions (i.e., translate from objects modeling input
            //to objects modeling the mathematical object, or we need calculations to be done by objects 
            //that handle the assumptions.
            //calculate symmetric tyeps based in kernel
            var kernelTypes = CalculateSymmetricTypesInKernel(wreathTypes);


            //run twice: first for n even, try find substitutes + keep track. Then for n odd.  


            Console.WriteLine("Calculating symmetric types matching to kernel");


            var resTable = new DataTable();
            resTable.Columns.Add("Wreath Type");
            resTable.Columns.Add("Mon");
            resTable.Columns.Add("n substitution");
            resTable.Columns.Add("a substitution");
            resTable.Columns.Add("assumptions");

            resTable.Columns.Add("Sn-1xSn-1 contribution");
            //  resTable.Columns.Add("assumptions applicable");

            resTable.Columns.Add("Kernel type");
            resTable.Columns.Add("(Sn # Sn) wr S2 type");
            //       resTable.Columns.Add("Contained in fiber");
            //       resTable.Columns.Add("Contained in AnSquared");


            resTable.Columns.Add("AnC4 type");
            resTable.Columns.Add("Sn#Sn type");
            resTable.Columns.Add("SnAn type");
            resTable.Columns.Add("AnAn type");

            //   resTable.Columns.Add("AnAn from fiber type");
            //genera
            foreach (var c2c2subgroup in (C2WrC2SubgroupName[])Enum.GetValues(typeof(C2WrC2SubgroupName)))
            {
                if (c2c2subgroup != C2WrC2SubgroupName.Undetermined)
                {
                    resTable.Columns.Add(c2c2subgroup.ToString());
                }
            }

            resTable.Columns.Add("Kernel genus");
            resTable.Columns.Add("Sn # Sn genus");
            resTable.Columns.Add("SnAn genus");
            resTable.Columns.Add("AnAn genus");
            //    resTable.Columns.Add("AnAn genus from fiber type");

            resTable.Columns.Add("pt stab genus");
            resTable.Columns.Add("S_n-1 x An genus");
            resTable.Columns.Add("SnAn-1 genus");

            // resTable.Columns.Add("AnAn-1 genus from fiber");
            resTable.Columns.Add("Sn # Sn-1 genus");
            resTable.Columns.Add("AnAn-1 genus");

            resTable.Columns.Add("an wr s2 match");
            resTable.Columns.Add("an wr s2 genus");

            resTable.Columns.Add("check");
            resTable.Columns.Add("accola check");

            resTable.Columns.Add("2 set genus kernel");
            resTable.Columns.Add("2 pt genus"); //(S_n-2 x Sn) cap G
            resTable.Columns.Add("kernel s2 x an-2 genus");
            resTable.Columns.Add("Sn # Sn 2 set stab genus");
            resTable.Columns.Add("Sn x An rhs 2set genus");
            resTable.Columns.Add("Sn x An lhs 2set genus");
            resTable.Columns.Add("S_{n-2}xAn"); //S_{n-2}xA_n
            resTable.Columns.Add("S_2xA_{n-2}xA_n");//S_2xA_{n-2}xA_n
            resTable.Columns.Add("AnxAn 2set genus"); //(S_2 # S_{n-2) x An
            resTable.Columns.Add("S2#Sn-2#Sn");//this is an 2st stabilizer in sn # sn lhs
            
            resTable.Columns.Add("a_{n-2}xS_n"); 
            

            resTable.Columns.Add("special1");
            resTable.Columns.Add("special2");
            resTable.Columns.Add("special3");
            resTable.Columns.Add("sn-2#sn");
            var assumptionsProvider = new ParityAssumptionProvider();




            var snsnMatched = new List<Tuple<String, SnRamificationType>>();
            var snAnMatched = new List<Tuple<String, SnRamificationType>>();
            var fiberMatched = new List<Tuple<String, SnRamificationType>>();
            var anAnMatched = new List<Tuple<string, SnRamificationType>>();


            string[] snWrS2TypeColumns = new String[]
                { "Wreath Type", "assumptions", "(Sn # Sn) wr S2 type", "AnC4 type", "an wr s2 match" };
            string[] snWrS2KernelPointStabGeneraColumns = new String[]
            {
                "Wreath Type", "assumptions", "pt stab genus", "S_n-1 x An genus", "Sn # Sn-1 genus", "AnAn-1 genus"
            };
            string[] snWrS2Matches = new String[] { "Wreath Type", "assumptions", "Kernel type" ,"Sn#Sn type", "SnAn type", "AnAn type" };

            string[] snWrS2_twoSet_genera_index_2 = new string[]
            {
                "Wreath Type", "assumptions", "2 pt genus", "kernel s2 x an-2 genus", "Sn # Sn 2 set stab genus", "Sn x An rhs 2set genus",
                "Sn x An lhs 2set genus", "special1", "special2"
            };

            string[] snWrS2_twoSet_genera_index4 = new string[]
            {
                "Wreath Type", "assumptions", "AnxAn 2set genus", "S_{n-2}xAn", "S_2xA_{n-2}xA_n", "a_{n-2}xS_n",
                "S2#Sn-2#Sn", "special3", "sn-2#sn"
            };
            string[] fiberKernelPointStabGeneraColumns = new String[]
            {
                "Wreath Type", "assumptions", "pt stab genus", "AnAn-1 genus"
            };

            string[] anc4typeColumns = new string[]
            {
                "Wreath Type", "assumptions",  "Kernel genus", "Kernel type", "an wr s2 match"
            };
            string[] anWrS2GenusColumns = new string[]
            {
                "Wreath Type", "assumptions", "Kernel genus", "Kernel type" , "pt stab genus", "AnAn-1 genus"
            };




            var twoSetRamifications = new List<Tuple<Dictionary<string,ParityAssumption>,SnRamificationType>>();
            foreach (var snType in snTypes)
            {
                var snAssumptions = assumptionsProvider.GetAllPossibleAssumptionsForSnType(snType);
                foreach (var assumption in snAssumptions)
                {
                    var handler = new SymbolicExpressionHandler(x: "n", y: "a", null, assumption);

                    var snCalc = new RamificationTypeCalculator(new PartitionHandler(handler));
                    var twoSetRamType = snCalc.Get2SetRamificationType(snType);
                    twoSetRamifications.Add(
                        new Tuple<Dictionary<string, ParityAssumption>, SnRamificationType>(assumption, twoSetRamType));
                }
            }
            //TODO print twoSetRamifications
            PrintTwoSetTypes(twoSetRamifications);

            foreach (var wreathType in wreathTypes)
            {
                var assumptions = assumptionsProvider.GetAllPossibleAssumptionsForType(wreathType);
                
                foreach (var assumption in assumptions)
                {
                    Debug.WriteLine("working on type" + wreathType.Name);
                    var row = resTable.NewRow();
                    row["Wreath Type"] = wreathType.Name;
                    row["n substitution"] = String.Format("{0} = {1} mod {2} ", "n", assumption["n"].EquivTo,
                        assumption["n"].Modulo);
                    if (assumption.ContainsKey("a"))
                    {
                        row["a substitution"] = String.Format("{0} = {1} mod {2}", "a", assumption["a"].EquivTo,
                            assumption["a"].Modulo);
                    }

                    row["assumptions"] = row["n substitution"] + " " + row["a substitution"];
                    //    row["assumptions applicable"] = "yes";
                    row["check"] = true;

                    var handler =
                        new SymbolicExpressionHandler("n", "a", null,
                            assumption); //initializing this at type-level although not sure this is what we want TODO 
                    var calculator = new RamificationTypeCalculator(new PartitionHandler(handler));

                    //TODO calculate (for sanity check) genus of symmetric point stabilizer here,for projection to RHS
                    try //assumptions dependent code should begin here
                    {
                        //calculate kernel type
                        var kernelType = calculator.KernelTypeFromSnS2Type(wreathType);


                        //calculate kernel genus
                        var numRamPointsKernelOverBottom =
                            calculator.GetNumberOfRamifiedPointsForKernelTypeFromSnS2Type(wreathType);
                        var kernelGenus = calculator.CalculateTopGenus(handler, new SymExpression("2"),
                            new SymExpression(numRamPointsKernelOverBottom.ToString()), new SymExpression("0"));
                        row["Kernel genus"] = kernelGenus;


                        //contribution to 1,1: count total number of odd orbits in elements with swaps
                        var oneOneContribution = calculator.GetNumberOfOddOrbitsInSwaps(wreathType);
                        row["Sn-1xSn-1 contribution"] = FormatGenusExpression(handler,oneOneContribution);
                        //projection to RHS
                        var projectionToRhs = calculator.ProjectToRhsType(kernelType);
                        var snSnPtGenus =
                            calculator.CalculateGenusOfSymmetricActionPointStab(new SymExpression("n"),
                                projectionToRhs,
                                kernelGenus);

                        row["pt stab genus"] = snSnPtGenus;

                        var rhsMatch = snTypes.SingleOrDefault(t =>
                            calculator.BranchCyclesEqual(t, projectionToRhs));
                        if (rhsMatch != null)
                        {
                            row["Kernel type"] = rhsMatch.Name;
                            snsnMatched.Add(
                                new Tuple<string, SnRamificationType>(kernelType.Name, rhsMatch));
                        }


                        var kernel2SetGenus = calculator.GetTwoSetStabilizerGenus(projectionToRhs, new SymExpression("n*(n-1)/2"), kernelGenus);
                        row["2 set genus kernel"] = FormatGenusExpression(handler, kernel2SetGenus);


                        var kernel2PtGenus = calculator.GetTwoPointStabilizer(projectionToRhs, kernel2SetGenus);
                        row["2 pt genus"] = FormatGenusExpression(handler,kernel2PtGenus);
                        //calculate projections
                        var c2wrc2Subgroup = calculator.GetC2WrC2SubgroupGeneratedBy(wreathType);
                        if (c2wrc2Subgroup == null)
                        {
                            throw new Exception("could not match projection to c2 wr c2");
                        }

                        row["Mon"] = c2wrc2Subgroup.Name.ToString();


                        switch (c2wrc2Subgroup.Name)
                        {
                            case C2WrC2SubgroupName.AnxAn:
                            case C2WrC2SubgroupName.AnxSn:
                            case C2WrC2SubgroupName.SnxAn:
                            case C2WrC2SubgroupName.Fiber:
                            case C2WrC2SubgroupName.SnSquared:
                            case C2WrC2SubgroupName.Undetermined:
                                throw new Exception("Unexpected monodromy for type " + wreathType.Name);
                                break;
                            case C2WrC2SubgroupName.AnWrS2:
                            case C2WrC2SubgroupName.AnWrS2Cong:
                                //the "kernel" calculations are sufficient here. 
                                break;
                            case C2WrC2SubgroupName.FiberWrS2:

                                var anwrs2type = calculator.CalculateAnwrS2TypeFromFiberType(wreathType);

                                var anwrs2Match = wreathTypes.SingleOrDefault(t =>
                                    calculator.SnwrS2BranchCyclesEqual(anwrs2type, t));
                                if (anwrs2Match != null)
                                {
                                    row["an wr s2 match"] = anwrs2Match.Name;
                                    //TODO calculate genus as well (no need for match)
                                }

                               
                                //if mon = fiber wr s2 and all elements are 00 except for two 00s's, then need to change two elements to 11s
                                var anwrS2overfiberS2NumberOfRamPoints =
                                    calculator.GetNumberOfRamifiedPointsAnwrSrOverFiberSw(wreathType);
                                var anwrS2FiberSwCaseGenus = calculator.CalculateTopGenus(new SymExpression("2"),
                                    new SymExpression(anwrS2overfiberS2NumberOfRamPoints.ToString()), new SymExpression("0"));
                                row["an wr s2 genus"] = anwrS2FiberSwCaseGenus;
                                goto case C2WrC2SubgroupName.AnC4;
                            case C2WrC2SubgroupName.AnC4:

                                var anAnTypeFiberKernelCase =
                                    calculator.GetAnAnTypeFromFiberType(
                                        kernelType); //here kerneltype is already fiber type
                                var ananTypeProjectionFiberKernelCase =
                                    calculator.ProjectToRhsType(anAnTypeFiberKernelCase);
                                var anAnFromFiberKernelMatch = snTypes.SingleOrDefault(t =>
                                    calculator.BranchCyclesEqual(t, ananTypeProjectionFiberKernelCase));
                                if (anAnFromFiberKernelMatch != null)
                                {
                                    row["AnAn type"] = anAnFromFiberKernelMatch.Name;
                                }

                                var anAnOverFiberKernelAnAnOverFiberNumOfRamPoints =
                                    calculator.GetNumberOfRamifiedPointsAnAnOverFiber(kernelType);
                                       var anAnKernelCaseGenus = calculator.CalculateTopGenus(handler, new SymExpression("2"),
                                    new SymExpression(anAnOverFiberKernelAnAnOverFiberNumOfRamPoints.ToString()),
                                    kernelGenus);

                                row["AnAn genus"] = anAnKernelCaseGenus;
                                var anAnFromKernelCasePtGenus =
                                    calculator.CalculateGenusOfSymmetricActionPointStab(new SymExpression("n"),
                                        ananTypeProjectionFiberKernelCase, anAnKernelCaseGenus);
                                row["AnAn-1 genus"] = anAnFromKernelCasePtGenus;


                                var anptstabGenuFromSymmetricAction =
                                    calculator.GetAnPtStabGenusFromSnRamificationType(handler, projectionToRhs,
                                        new SymExpression("n"), kernelGenus);

                                var ananCalculationsAgree =
                                    handler.ExpressionsEqual(anptstabGenuFromSymmetricAction,
                                        anAnFromKernelCasePtGenus);
                                if (!ananCalculationsAgree)
                                    row["check"] = false;
                                //TODO check if different genera calcaultions add up 


                                var twoSetgenusFiberKernelCase = calculator.GetTwoSetStabilizerGenus(sntype: projectionToRhs,
                                    new SymExpression("n*(n-1)/2"), kernelGenus);
                                var ananInFiber2setGenus = calculator.GetTwoSetStabilizerGenus(sntype: ananTypeProjectionFiberKernelCase,
                                    new SymExpression("n*(n-1)/2"), anAnKernelCaseGenus);
                                row["Sn # Sn 2 set stab genus"] =
                                    FormatGenusExpression(handler, twoSetgenusFiberKernelCase);
                                row["AnxAn 2set genus"] = FormatGenusExpression(handler,
                                    ananInFiber2setGenus);
                                //sn2ptgenus
                                var sn2ptGenusInFiber =
                                    calculator.GetTwoPointStabilizer(projectionToRhs, twoSetgenusFiberKernelCase);
                            //    row[""] = sn2ptGenusInFiber;
                                //an2ptgenus
                                var an2ptgenusInFiber =
                                    calculator.GetTwoPointStabilizer(ananTypeProjectionFiberKernelCase,
                                        ananInFiber2setGenus);
                                //an2setgenusFiberCase

                                //other 2set groups

                                var s2anGenusFiberCase = calculator.GetMiddleGenusByAccola(an2ptgenusInFiber,
                                    ananInFiber2setGenus, sn2ptGenusInFiber, kernel2SetGenus);

                                row["kernel s2 x an-2 genus"] = FormatGenusExpression(handler,s2anGenusFiberCase);
                                break;
                            case C2WrC2SubgroupName.SnWrS2:

                                //fiber type
                                var fiberS2Type = calculator.CalculateFiberS2TypeFromWreathType(wreathType);
                                //TODO fiber genus

                                var fiberS2match =
                                    wreathTypes.FirstOrDefault(t =>
                                        calculator.SnwrS2BranchCyclesEqual(t,
                                            fiberS2Type)); //TODO changed from single default to first default
                                if (fiberS2match != null)
                                {
                                    row["(Sn # Sn) wr S2 type"] = fiberS2match.Name;



                                    var anwrs2typeFromFiber = calculator.CalculateAnwrS2TypeFromFiberType(fiberS2Type);

                                    var anwrs2FromFiberMatch = wreathTypes.SingleOrDefault(t =>
                                        calculator.SnwrS2BranchCyclesEqual(anwrs2typeFromFiber, t));
                                    if (anwrs2FromFiberMatch != null)
                                    {
                                        row["an wr s2 match"] = anwrs2FromFiberMatch.Name;
                                    }

                                }

                                //anc4 type
                                var anC4Type = calculator.CalculateAnC4TypeFromWreathType(wreathType);
                                var anC4match =
                                    wreathTypes.SingleOrDefault(t => calculator.SnwrS2BranchCyclesEqual(t, anC4Type));
                                if (anC4match != null)
                                {
                                    row["AnC4 type"] = anC4match.Name;
                                }


                                foreach (var c2c2option in (C2WrC2SubgroupName[])Enum.GetValues(
                                             typeof(C2WrC2SubgroupName)))
                                {
                                    if (c2c2option != C2WrC2SubgroupName.Undetermined)
                                    {
                                        var projection = calculator.GetC2WrC2Type(wreathType);
                                        var c2c2genus = calculator.GetC2C2SubgroupGenus(projection, c2c2option);
                                        row[c2c2option.ToString()] = c2c2genus;
                                    }
                                }

                                //
                                var containedInFiber = calculator.ContainedInFiber(kernelType);
                                //    row["Contained in fiber"] = containedInFiber;
                                var containedInAnSquared = calculator.ContainedInAnSquared(kernelType);
                                //      row["Contained in AnSquared"] = containedInAnSquared;
                                //calculate snAn type & genus
                                var snAnType = calculator.SnAnTypeFromSnSquaredType(kernelType);
                                var snAnProjectionToRHS = calculator.ProjectToRhsType(snAnType);
                                var snAnOverKernelNumOfRamPoints =
                                    calculator.GetNumberOfRamPointsSnAnOverSnSn(kernelType);
                                var snAnGenus = calculator.CalculateTopGenus(handler, new SymExpression("2"),
                                    new SymExpression(snAnOverKernelNumOfRamPoints.ToString()), kernelGenus);
                                var snAnRhsPtGenus =
                                    calculator.CalculateGenusOfSymmetricActionPointStab(new SymExpression("n"),
                                        snAnProjectionToRHS, snAnGenus);

                                var snAnRhs2setGenus = calculator.GetTwoSetStabilizerGenus(sntype: snAnProjectionToRHS,
                                    degree: new SymExpression("n*(n-1)/2"), snAnGenus); //this is also S2#Sn-2 genus of kernel type

                         

                                var snAnLHSprojection = calculator.ProjectToLhsType(snAnType);
                                var snAnLhsPtGenus =
                                    calculator.CalculateGenusOfSymmetricActionPointStab(degree: new SymExpression("n"),
                                        snAnLHSprojection, snAnGenus);
                                var snAnLHS2setGenus = calculator.GetTwoSetStabilizerGenus(sntype: snAnLHSprojection,
                                    degree: new SymExpression("n*(n-1)/2"), bottomGenus: snAnGenus);


                                var snAnRHS2ptGenus =
                                    calculator.GetTwoPointStabilizer(sntype: snAnProjectionToRHS, snAnLHS2setGenus);
                                var snAnLHS2ptGenus =
                                    calculator.GetTwoPointStabilizer(sntype: snAnLHSprojection, snAnLHS2setGenus);

                                row["SnAn genus"] = snAnGenus;
                                row["SnAn-1 genus"] = FormatGenusExpression(handler,snAnRhsPtGenus);
                                row["S_n-1 x An genus"] = FormatGenusExpression(handler, snAnLhsPtGenus);
                                row["Sn x An rhs 2set genus"] = FormatGenusExpression(handler, snAnRhs2setGenus);
                                row["Sn x An lhs 2set genus"] = FormatGenusExpression(handler, snAnLHS2setGenus);

                                var snAnMatch =
                                    snTypes.SingleOrDefault(t => calculator.BranchCyclesEqual(t, snAnProjectionToRHS));
                                if (snAnMatch != null)
                                {
                                    row["SnAn type"] = snAnMatch.Name;
                                    snAnMatched.Add(
                                        new Tuple<string, SnRamificationType>(kernelType.Name, snAnMatch));
                                }

                                //anan type & genus
                                //first calculate from sn an type
                                var anAnType = calculator.GetAnAnTypeFromSnAnType(snAnType);
                                var anAnProjection = calculator.ProjectToRhsType(anAnType);
                                var anAnOverSnAnNumOfRamPoints =
                                    calculator.GetNumberOfRamPointsAnAnOverSnAn(snAnType);
                                var anAnGenus = calculator.CalculateTopGenus(handler, new SymExpression("2"),
                                    new SymExpression(anAnOverSnAnNumOfRamPoints.ToString()), snAnGenus);


                                var anAnPtGenus =
                                    calculator.CalculateGenusOfSymmetricActionPointStab(new SymExpression("n"),
                                        anAnProjection, anAnGenus);
                                var anan2SetGenus = calculator.GetTwoSetStabilizerGenus(sntype: anAnProjection,
                                    degree: new SymExpression("n*(n-1)/2"), bottomGenus: anAnGenus);

                                var anan2ptGenus =
                                    calculator.GetTwoPointStabilizer(sntype: anAnProjection, anan2SetGenus);
                                
                                
                                var an2setstabGenusOfKernelType = snAnRhs2setGenus;

                                var kernelan2ptgenus =
                                    calculator.GetTwoPointStabilizer(sntype: snAnProjectionToRHS, an2setstabGenusOfKernelType);
                                var s2anGenus = calculator.GetMiddleGenusByAccola(kernelan2ptgenus,
                                    an2setstabGenusOfKernelType, kernel2PtGenus, kernel2SetGenus);

                                
                                row["kernel s2 x an-2 genus"] = FormatGenusExpression(handler,s2anGenus);

                                row["AnAn genus"] = FormatGenusExpression(handler,anAnGenus);
                                row["AnAn-1 genus"] = FormatGenusExpression(handler, anAnPtGenus);
                                row["AnxAn 2set genus"] = FormatGenusExpression(handler, anan2SetGenus);

                                var anAnMatch =
                                    snTypes.SingleOrDefault(t => calculator.BranchCyclesEqual(t, anAnProjection));
                                if (anAnMatch != null)
                                {
                                    row["AnAn type"] = anAnMatch.Name;
                                    anAnMatched.Add(
                                        new Tuple<string, SnRamificationType>(kernelType.Name, anAnMatch));
                                }
                                //s2an-2xan genus
                                var s2ananGenus = calculator.GetMiddleGenusByAccola(anan2ptGenus, snAnLHS2ptGenus,
                                    anan2SetGenus, snAnLHS2setGenus);

                                row["S_2xA_{n-2}xA_n"] = FormatGenusExpression(handler, s2ananGenus);

                                var snan2ptStab = calculator.GetTwoPointStabilizer(snAnLHSprojection, snAnLHS2setGenus);
                                //S_{n-2}xAn
                                row["S_{n-2}xAn"] = FormatGenusExpression(handler,snan2ptStab);
                                //
                                row["a_{n-2}xS_n"] = FormatGenusExpression(handler,snAnRHS2ptGenus);
                                //(S(2)#sn with kernel hxan-2, h=s2xan-2 i.e. s2xsn-2#sn
                                var special1 = calculator.GetMiddleGenusByAccola(top: s2ananGenus, s2anGenus,
                                    snAnLHS2setGenus, kernel2SetGenus);

                                //s(2)#sn with kernel hxan for h=s_{n-2}??? i.e. sn-2xs2#sn
                                var special2 = calculator.GetMiddleGenusByAccola(top: snAnLHS2ptGenus, kernel2PtGenus,
                                    snAnLHS2setGenus, kernel2SetGenus);

                            
                                //special1#sn
                                var special3 = calculator.GetMiddleGenusByAccola(anan2ptGenus, special1,
                                    snAnRHS2ptGenus, s2anGenus);

                                //sn-2#sn
                                var special4 = calculator.GetMiddleGenusByAccola(anan2ptGenus, special2,
                                    snAnRHS2ptGenus, kernel2PtGenus);

                                var s2fibersnfibersn = calculator.GetMiddleGenusByAccola(anan2ptGenus, anan2SetGenus,
                                    snAnRHS2ptGenus, snAnRhs2setGenus);
                                row["S2#Sn-2#Sn"]=FormatGenusExpression(handler,s2fibersnfibersn);

                                row["special1"] = FormatGenusExpression(handler,special1);
                                row["special2"] = FormatGenusExpression(handler,special2);
                                row["special3"] = FormatGenusExpression(handler,special3);
                                row["sn-2#sn"] = FormatGenusExpression(handler,special4);

                                //fiber
                                var fiberType = calculator.FiberRamificationTypeFromSnSquaredType(kernelType);
                                var fiberProjection = calculator.ProjectToRhsType(fiberType);
                                var fiberOverKernelNumOfRamPoints =
                                    calculator.GetNumberOfRamPointsFiberOverKernel(kernelType);
                                var fiberGenus = calculator.CalculateTopGenus(handler, new SymExpression("2"),
                                    new SymExpression(fiberOverKernelNumOfRamPoints.ToString()), kernelGenus);
                                var fiberPtGenus =
                                    calculator.CalculateGenusOfSymmetricActionPointStab(new SymExpression("n"),
                                        fiberProjection, fiberGenus);


                               
                                var fiber2setGenus = calculator.GetTwoSetStabilizerGenus(sntype: fiberProjection,
                                    degree: new SymExpression("n*(n-1)/2"), fiberGenus);

                                row["Sn # Sn genus"] = FormatGenusExpression(handler, fiberGenus);
                                row["Sn # Sn-1 genus"] = FormatGenusExpression(handler, fiberPtGenus);

                                row["Sn # Sn 2 set stab genus"] = FormatGenusExpression(handler, fiber2setGenus);

                                var fiberMatch = snTypes.SingleOrDefault(t =>
                                    calculator.BranchCyclesEqual(t, fiberProjection));
                                if (fiberMatch != null)
                                {
                                    row["Sn#Sn type"] = fiberMatch.Name;
                                    fiberMatched.Add(
                                        new Tuple<string, SnRamificationType>(kernelType.Name, fiberMatch));
                                }


                                //AnAn genus from fiber type
                                var anAnTypeFromFiber = calculator.GetAnAnTypeFromFiberType(fiberType);
                                var anAnFromFiberProjection = calculator.ProjectToRhsType(anAnTypeFromFiber);
                                var anAnFromFiberMatch = snTypes.SingleOrDefault(t =>
                                    calculator.BranchCyclesEqual(t, anAnFromFiberProjection));
                                if (anAnFromFiberMatch != null)
                                {
                                    row["AnAn type"] = anAnFromFiberMatch.Name;
                                }

                                var anAnOverFiberNumOfRamPoints =
                                    calculator.GetNumberOfRamifiedPointsAnAnOverFiber(fiberType);
                                var anAnFromFiberGenus = calculator.CalculateTopGenus(handler, new SymExpression("2"),
                                    new SymExpression(anAnOverFiberNumOfRamPoints.ToString()), fiberGenus);

                                row["AnxAn"] = anAnFromFiberGenus;
                                var anAnFromFiberPtGenus =
                                    calculator.CalculateGenusOfSymmetricActionPointStab(new SymExpression("n"),
                                        anAnProjection, anAnGenus);
                                row["AnAn-1 genus"] = FormatGenusExpression(handler, anAnFromFiberPtGenus);

                                //this is the 2-set stabilizer of anan in fiber
                               
              
                                var accolaCheckSide1 =
                                    handler.Add(anAnPtGenus, handler.Multiply(new SymExpression("2"), snSnPtGenus));
                                var accolaCheckSide2 = handler.Add(handler.Add(snAnLhsPtGenus, snAnRhsPtGenus),
                                    fiberPtGenus);
                                var accolaCheck = handler.ExpressionsEqual(accolaCheckSide2, accolaCheckSide1);
                                row["accola check"] = accolaCheck;

                                //extra subgroups of 2set stabilizer obtained via accola - c'mon we can do this!!!
                                
                                break;
                        }
                    }
                    catch (SymbolicExpressionHandlingException exp)
                    {
                        row["assumptions applicable"] = "no";
                    }

                    resTable.Rows.Add(row);
                }
                //    Console.WriteLine(String.Join(Environment.NewLine, snsnMatched.Select(t => t.ToString())));
            }

            Console.WriteLine("sn an matches:");
            Console.WriteLine(String.Join(Environment.NewLine, snAnMatched.Select(t => t.ToString())));


            Console.WriteLine("an an matches:");
            Console.WriteLine(String.Join(Environment.NewLine, anAnMatched.Select(t => t.ToString())));


            Console.WriteLine("fiber matches:");
            Console.WriteLine(String.Join(Environment.NewLine, fiberMatched.Select(t => t.ToString())));



            var fileName = GenerateFileNameWithTimeStamp("results_");
            TableToCsv(resTable, fileName + ".csv");

            PrintColumnsToLatexTable(snWrS2Matches, resTable, C2WrC2SubgroupName.SnWrS2,"SnWrS2_Kernel_2SetStab");
            PrintColumnsToLatexTable(snWrS2KernelPointStabGeneraColumns, resTable, C2WrC2SubgroupName.SnWrS2,"SnWrS2_kernel_pointstabilizers");
            PrintColumnsToLatexTable(snWrS2_twoSet_genera_index4, resTable,C2WrC2SubgroupName.SnWrS2, "2set_cases_index_4");
            PrintColumnsToLatexTable(snWrS2TypeColumns, resTable, C2WrC2SubgroupName.SnWrS2,"SnWrS2_primitive_types");

            PrintColumnsToLatexTable(fiberKernelPointStabGeneraColumns,resTable,C2WrC2SubgroupName.FiberWrS2,"FiberS2_kernel_point_stabilizers");

            PrintColumnsToLatexTable(fiberKernelPointStabGeneraColumns, resTable,C2WrC2SubgroupName.AnC4,"An2C4_kernel_pint_stabilizers");
            PrintColumnsToLatexTable(anc4typeColumns,resTable,C2WrC2SubgroupName.AnC4,"An2C4_type_matches");

            PrintColumnsToLatexTable(anWrS2GenusColumns,resTable,C2WrC2SubgroupName.AnWrS2,"AnWrS2GenusResults");

            PrintColumnsToLatexTable(snWrS2_twoSet_genera_index_2,resTable,C2WrC2SubgroupName.SnWrS2,"2set_cases_index_2");
        }




        public static ISymbolicExpression FormatGenusExpression(ISymbolicExpressionHandler handler,
           ISymbolicExpression expression)
        {
            var simplifiedByHandler =  handler.SimplifyTo01orOther(expression);

            var leadingCoeffPositive = handler.LeadingCoefficientNonnegative(simplifiedByHandler.Expression,"n");

          Assert.IsTrue(leadingCoeffPositive);
       //     var simplifiedExpr = handler.Simplify(expression).Expression;
       //     return new SymExpression(simplifiedExpr);



            //    foreach (var attemptedParse in CommonGenusExpressions)
            //   {


            //     if (handler.ExpressionsEqual(expression, attemptedParse))
            //       return attemptedParse;
            // }
            return simplifiedByHandler;
        }

        public static String GenerateFileNameWithTimeStamp(string name)
        {

            var timestamp = DateTime.Now;
            return name  + timestamp.ToString("MMddyy_HHmmss");
        }

        public static void PrintTwoSetTypes(
            List<Tuple<Dictionary<string, ParityAssumption>, SnRamificationType>> ramTypes)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var typeChecked in ramTypes)
            {
                var assumption = typeChecked.Item1;
                var ramType = typeChecked.Item2;
                var sb2 = new StringBuilder();
                var nAssumption = String.Format("{0} = {1} mod {2} ", "n", assumption["n"].EquivTo, assumption["n"].Modulo);
                sb2.Append(ramType.Name);
                sb2.Append("& ");
                sb2.Append(nAssumption);
                if (assumption.ContainsKey("a"))
                {
                    var aAssumption = String.Format("{0} = {1} mod {2} ", "a", assumption["a"].EquivTo, assumption["a"].Modulo);
                    sb2.Append(aAssumption);
                    
                }

                sb2.Append( "&");

                var formattedRamType = ramType.ToLatexString();
                sb2.Append(formattedRamType);
                sb2.Append("\\\\");
                sb.AppendLine(sb2.ToString());
            }

            var filePath = GenerateFileNameWithTimeStamp("sn_2set_types");
            File.WriteAllText(filePath+".txt",sb.ToString());
        }

        public static void PrintColumnsToLatexTable(string[] columnNames, DataTable dt, C2WrC2SubgroupName monodromy, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Join("&", columnNames)+"\\\\");

            var monName = monodromy.ToString();
            var rowFilter = String.Format("Mon='{0}'", monName);
            var dv = new DataView(dt);
                dv.RowFilter = rowFilter;

                    var selectedData = dv.ToTable(false, columnNames);

          
            foreach (DataRow row in selectedData.Rows)
            {

                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join("&", fields)+"\\\\");
            }


            var filePath = GenerateFileNameWithTimeStamp(tableName);
            File.WriteAllText(filePath+".txt", sb.ToString());
        }
        public static void TableToCsv(DataTable dt, string filepath)
        {
            StringBuilder sb = new StringBuilder();
            IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }


            File.WriteAllText(filepath, sb.ToString());
        }

        public static void RamificationCalculations()
        {
            var assumptions = new Dictionary<String, ParityAssumption>()
            {
                { "n", new ParityAssumption(Parity.Odd) }, { "a", new ParityAssumption(Parity.Odd) }
            }; //just for funsies
            var symbolicExpressionHandler = new SymbolicExpressionHandler("n", "a", null, assumptions);
            var partitionHandler = new PartitionHandler(symbolicExpressionHandler);
            Console.WriteLine("Hello World!");

            //    Console.WriteLine("symmetric types");

            //calculate and print kernel types
            //         Console.WriteLine(String.Join(Environment.NewLine, StaticAndBenEll.ExceptionalSymmetricTypes.Select(t => t.ToString())));

            Console.WriteLine("wreath types to check");

            Console.WriteLine(String.Join(Environment.NewLine, StaticAndBenEll.WreathTypes.Select(t => t.ToString())));


            Console.WriteLine("Kernel types");
            var doer = new RamificationTypeCalculator(partitionHandler); //TODO add assumptions
            var kernel_types = doer.GetKernelTypes(StaticAndBenEll.WreathTypes);
            Console.WriteLine(String.Join(Environment.NewLine, kernel_types.Select(t => t.ToString())));

            Console.WriteLine("corresponding S_n x S_{n-1} types");
            var corresponding_symmetric_types = kernel_types.Select(t => doer.ProjectToRhsType(t));
            Console.WriteLine(String.Join(Environment.NewLine, corresponding_symmetric_types));
            var snTypesComparer = new SnRamificationTypeEqualityComparer();
            //lookup symmetric types
            var corresponding_exceptional_types = corresponding_symmetric_types.Where(t =>
                StaticAndBenEll.ExceptionalSymmetricTypes.Contains(t, snTypesComparer));
            Console.WriteLine("Corresponding symmetric types");
            Console.WriteLine(String.Join(Environment.NewLine,
                corresponding_exceptional_types.Select(t => t.ToString())));
            //now do the same with types based in AnSn
            Console.WriteLine("SnAnTypes");
            var snanTypes = doer.GetSnAnTypes(kernel_types);
            Console.WriteLine(String.Join(Environment.NewLine, snanTypes.Select(t => t.ToString())));
            //now do the same for fiber types
            Console.WriteLine("fiber types");
            var fiberTypes = doer.GetFiberTypes(kernel_types);
            Console.WriteLine(String.Join(Environment.NewLine, fiberTypes.Select(t => t.ToString())));
            Console.WriteLine("AnAn Types");
            var anAnTypes = doer.GetAnAnTypesFromSnAnTypes(snanTypes);
            Console.WriteLine(String.Join(Environment.NewLine, anAnTypes.Select(t => t.ToString())));
        }

        public IEnumerable<SnRamificationType> ParseSnTypes()
        {
            return ParsingSetup.LoadSnTypes();
        }

        //We want to return for every s
        public static IEnumerable<SnRamificationType> CalculateSymmetricTypesInKernel(
            IEnumerable<SnWrS2RamificationType> input_types)
        {
            //TODO pass handler to constructor
            var doer = new RamificationTypeCalculator(null);
            var kernel_types = doer.GetKernelTypes(input_types);


            //calculate symmetric types with base snsn & match against list
            var corresponding_symmetric_types = kernel_types.Select(t => doer.ProjectToRhsType(t));
            //TODO check against list + at this point can still check genus!)
            return corresponding_symmetric_types;
        }
    }
}