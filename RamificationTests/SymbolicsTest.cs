using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Symbolics;
using SnS2RamificationCheck;
using SnS2RamificationCheck.Interfaces;
using SnS2RamificationCheck.Objects;

namespace RamificationTests
{
    [TestClass]
    public class SymbolicsTest
    {


        [TestMethod]
        public void TestSimplify()
        {
            var str = "(4 + 2*(8 + 2*(-7 + n)))/2 - ((10 + 2*(-7 + n))/2 + (-2 + n + (-7 + n)/2 + (-1 + n)/2)/2)";
            var n = SymbolicExpression.Parse("n");
            var expr = SymbolicExpression.Parse(str);
            var res = expr.RationalSimplify(n).RationalNumberValue.IsOne;
            Assert.IsTrue(res);


        }



        [TestMethod]
        public void OddParityOneVariableTest()
        {
            var assumptions = new Dictionary<string, ParityAssumption>();
            assumptions.Add("n", new ParityAssumption(Parity.Odd));
            var handler = new SymbolicExpressionHandler("n", null, null, assumptions);

            var nCycle = new SymExpression("n");



            var res = handler.AssessParity(nCycle);

            Assert.AreEqual(res, Parity.Odd);

        }


        [TestMethod]
        public void EvenParityOneVariableTest()
        {
            var assumptions = new Dictionary<string, ParityAssumption>();
            assumptions.Add("n", new ParityAssumption(Parity.Even));
            var handler = new SymbolicExpressionHandler("n", null, null, assumptions);

            var nCycle = new SymExpression("n");



            var res = handler.AssessParity(nCycle);

            Assert.AreEqual(res, Parity.Even);

        }


        [TestMethod]
        public void NCyclePartitionSignTest()
        {
            var assumptions = new Dictionary<string, ParityAssumption>();
            assumptions.Add("n", new ParityAssumption(Parity.Even));


            var nCycle = new PartitionPart("n", "1");
            var partition = new Partition(new List<PartitionPart> { nCycle });


            var handler = new SymbolicExpressionHandler("n", null, null, assumptions);
            var partitionHandler = new PartitionHandler(handler);


            var partParity = partitionHandler.GetPartParity(nCycle);
            Assert.AreEqual(partParity, Parity.Odd);

            //   var res = partitionHandler.GetParity(partition, assumptions);
            //  Assert.AreEqual(res, Parity.Odd);

            // var res2 = partitionHandler.GetPartParity(nCycle, new Dictionary<string, Parity>() { { "n", Parity.Odd } });
            //  Assert.AreEqual(res2, Parity.Even);

            // var res3 = partitionHandler.GetParity(partition, new Dictionary<string, Parity>() { { "n", Parity.Odd } });
            // Assert.AreEqual(res2, Parity.Even);

        }

        [TestMethod]
        public void HalvedNCyclePartitionSignTest()
        {
            var assumptions = new Dictionary<string, ParityAssumption>() { { "n", new ParityAssumption(4, 0) } };
            var handler = new SymbolicExpressionHandler("n", null, null, assumptions);
            var partitionHandler = new PartitionHandler(handler);

            var halved_n_cycle = new PartitionPart("n/2", "2");
            var partition = new Partition(new List<PartitionPart> { halved_n_cycle });

            var partParity = partitionHandler.GetPartParity(halved_n_cycle);
            Assert.AreEqual(partParity, Parity.Even);
            var partitionParity = partitionHandler.GetParity(partition);
            Assert.AreEqual(partitionParity, Parity.Even);
        }

        [TestMethod]
        public void HalvedNCyclePartSignTest()
        {
            var assumptions = new Dictionary<string, ParityAssumption>() { { "n", new ParityAssumption(2, 0) } };
            var handler = new SymbolicExpressionHandler("n", null, null, assumptions);
            var partitionHandler = new PartitionHandler(handler);

            var halved_n_cycle = new PartitionPart("n/2", "1");
            var partParity = partitionHandler.GetPartParity(halved_n_cycle);
            Assert.AreEqual(partParity, Parity.Odd);
        }

        [TestMethod]
        public void TestFindSubstitute()
        {
            var handler = new SymbolicExpressionHandler(null, null, null, null, 8);
            var threeMod4 = handler.TryFindSubstitute("n", "(n-1)/2", Parity.Odd);
            Assert.AreEqual(threeMod4, 3);

            var oneMod4 = handler.TryFindSubstitute("n", "(n-1)/2", Parity.Even);
            Assert.AreEqual(oneMod4, 1);

            var halfN = handler.TryFindSubstitute("n", "n/2", Parity.Odd);
            Assert.AreEqual(halfN, 2);

            var zeroMod4 = handler.TryFindSubstitute("n", "n/2", Parity.Even);
            Assert.AreEqual(zeroMod4, 4);

            var oneMod8 = handler.TryFindSubstitute("n", "(n-1)/4", Parity.Even);
            Assert.AreEqual(oneMod4, 1);

            var fiveMod8 = handler.TryFindSubstitute("n", "(n-1)/4", Parity.Odd);
            Assert.AreEqual(fiveMod8, 5);

        }


        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestFindSubstituteFailure()
        {
            var handler = new SymbolicExpressionHandler(null, null, null, null, 4);
            var threeMod4 = handler.TryFindSubstitute("n", "(n-1)/4", Parity.Odd);
        }

        [TestMethod]
        public void TestFindSubstituteDenominator4()
        {
            var handler = new SymbolicExpressionHandler(null, null, null, null, 12);
            var fiveMod4 = handler.TryFindSubstitute("n", "(n-1)/4", Parity.Odd);
            Assert.AreEqual(fiveMod4 % 8, 5);
            var oneMod4 = handler.TryFindSubstitute("n", "(n-1)/4", Parity.Even);
            Assert.AreEqual(oneMod4 % 8, 1);
        }

        [TestMethod]
        public void TestFractionExpressionComparison()
        {
            var expr1 = new SymExpression("n/2-1");
            var expr2 = new SymExpression("(n-2)/2");
            var handler = new SymbolicExpressionHandler("n", null, null,null);
            Assert.IsTrue(handler.ExpressionsEqual(expr1, expr2));
        }

        [TestMethod]
        public void TranspositionPartitionSignTest()
        {
            var assumptions = new Dictionary<string, ParityAssumption>() { { "n", new ParityAssumption(Parity.Odd) } };
            var handler = new SymbolicExpressionHandler("n", null, null, assumptions);
            var partitionHandler = new PartitionHandler(handler);

            var transposition = new PartitionPart("2", "1");
            var partition = new Partition(new List<PartitionPart> { transposition, new PartitionPart("1", "n-2") });

            var partParity = partitionHandler.GetPartParity(transposition);
            Assert.AreEqual(partParity, Parity.Odd);
        }

        [TestMethod]
        public void DoubleTranspositionSignCheckTest()
        {
            var assumptions = new Dictionary<string, ParityAssumption>() { { "n", new ParityAssumption(Parity.Even) } };
            var handler = new SymbolicExpressionHandler("n", null, null, assumptions);
            var partitionHandler = new PartitionHandler(handler);

            var transposition = new PartitionPart("2", "2");
            var partition = new Partition(new List<PartitionPart> { transposition, new PartitionPart("1", "n-4") });

            var partParity = partitionHandler.GetPartParity(transposition);
            Assert.AreEqual(partParity, Parity.Even);



            var assumptions2 = new Dictionary<string, ParityAssumption>() { { "n", new ParityAssumption(Parity.Odd) } };
            var handler2 = new SymbolicExpressionHandler("n", null, null, assumptions2);
            var partitionHandler2 = new PartitionHandler(handler);

            partParity = partitionHandler2.GetPartParity(transposition);
            Assert.AreEqual(partParity, Parity.Even);
        }



        [TestMethod]
        public void TranspositionPartitionSignTestAssumptionsDontMatterForTranspositionTest()
        {
            var assumptions = new Dictionary<string, ParityAssumption>() { { "n", new ParityAssumption(Parity.Even) } };
            var handler = new SymbolicExpressionHandler("n", null, null, assumptions);
            var partitionHandler = new PartitionHandler(handler);

            var transposition = new PartitionPart("2", "1");
            var partition = new Partition(new List<PartitionPart> { transposition, new PartitionPart("1", "n-2") });

            var partParity = partitionHandler.GetPartParity(transposition);
            Assert.AreEqual(partParity, Parity.Odd);
        }

        [TestMethod]
        public void TwoVariablePartitionSignTest()
        {
            var assumptions = new Dictionary<string, ParityAssumption>() { { "n", new ParityAssumption(Parity.Odd) }, { "a", new ParityAssumption(Parity.Odd) } };
            var handler = new SymbolicExpressionHandler("n", "a", null, assumptions);
            var partitionHandler = new PartitionHandler(handler);


            var partition = new Partition(new List<PartitionPart> { new PartitionPart("a", "1"), new PartitionPart("n-a", "1") });

            var res = partitionHandler.GetParity(partition);
            Assert.AreEqual(res, Parity.Odd);

            //  var res2 = partitionHandler.GetParity(partition, new Dictionary<string, Parity>() { { "n", Parity.Even }, { "a", Parity.Odd } });
            //Assert.AreEqual(res2, Parity.Even);


            //var res3 = partitionHandler.GetParity(partition, new Dictionary<string, Parity>() { { "n", Parity.Odd }, { "a", Parity.Even } });
            // Assert.AreEqual(res3, Parity.Odd);


            //var res4 = partitionHandler.GetParity(partition, new Dictionary<string, Parity>() { { "n", Parity.Even }, { "a", Parity.Even } });
            // Assert.AreEqual(res4, Parity.Even);


        }

        [TestMethod]
        public void ExpressionWithTest()
        {
            var assumptions = new Dictionary<string, ParityAssumption>();
            assumptions.Add("n", new ParityAssumption(Parity.Even));

            var handler = new SymbolicExpressionHandler("n", null, null, assumptions);

            var nCycle = new SymExpression("n");

            var res = handler.AssessParity(nCycle);

            Assert.AreEqual(res, Parity.Even);

        }


        [TestMethod]
        public void ExpressionsEqualityTest()
        {
            var handler = new SymbolicExpressionHandler("n", null, null, null);
            var expr1 = new SymExpression("n/2+n/2");
            var expr2 = new SymExpression("n");
            var res = handler.ExpressionsEqual(expr1, expr2);
            Assert.IsTrue(res);
        }


        [TestMethod]
        public void ExpressionsEqualityTest2()
        {
            var handler = new SymbolicExpressionHandler("n", null, null, null);
            var expr1 = new SymExpression("n-2+2");
            var expr2 = new SymExpression("n");
            var res = handler.ExpressionsEqual(expr1, expr2);
            Assert.IsTrue(res);
        }



        [TestMethod]
        public void ExpressionsEqualityTest3()
        {
            var handler = new SymbolicExpressionHandler("n", "a", null, null);
            var expr1 = new SymExpression("n-a+a");
            var expr2 = new SymExpression("n");
            var res = handler.ExpressionsEqual(expr1, expr2);
            Assert.IsTrue(res);
        }


        [TestMethod]
        public void ExpressionsEqualityTest4()
        {
            var handler = new SymbolicExpressionHandler("n", "a", null, null);
            var expr1 = new SymExpression("n-a+a");
            var expr2 = new SymExpression("n/2");
            var res = handler.ExpressionsEqual(expr1, expr2);
            Assert.IsFalse(res);
        }



        [TestMethod]
        public void TestSimplifyBehavior()
        {
            var str = "(2 - n + (-3 + n)/2 + (-1 + n)/2)/2";
            var expr = MathNet.Symbolics.SymbolicExpression.Parse(str).Expand();
            var ratValue = expr.RationalNumberValue;
            Assert.IsTrue(ratValue.IsZero);

            var str2 = "(1 - n/2 + (-2 + n)/2)/2";
            var expr2 = MathNet.Symbolics.SymbolicExpression.Parse(str2).Expand().RationalNumberValue;
            Assert.IsTrue(expr2.IsZero);
            ;
        }


        [TestMethod]
        public void CheckExpressionsEqual()
        {
            var str1 = "(-2*a + 4*(-1 + a/2) + 2*n)/2";
            var str2 = "(-4 + 2*n)/2";

            var expr1 = SymbolicExpression.Parse(str1);
            var expr2 = SymbolicExpression.Parse(str2);

            var equal = expr1.Expression.Equals(expr2.Expression);
            Assert.IsTrue(equal);
            

        }
    }
}
