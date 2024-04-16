/// <summary>
/// Author:    Man Wai Lam
/// Partner:   None
/// Date:      03-Feb-2022
/// Course:    CS 3500, University of Utah, School of Computing
/// Copyright: CS 3500 and [Man Wai Lam] - This work may not 
///            be copied for use in Academic Coursework.
///
/// I, [Man Wai Lam], certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All 
/// references used in the completion of the assignments are cited 
/// in my README file.
///
/// File Contents
///
///    This test program is for the formula funtion Test
///    
/// </summary>


using SpreadsheetUtilities;
using System.Text.RegularExpressions;

namespace FormulaTests
{
    /// <summary>
    /// Have 21 Testmethod for the formula class
    /// </summary>
    [TestClass]
    public class FormulaTests
    {
        //test the basic calculate function
        [TestMethod]
        public void TestCalculateFunction()
        {
            Formula f = new Formula("1*1/1+1+3+6-1-2");
            Assert.AreEqual(8,(double)f.Evaluate(s=>0));
        }

        // test div by 0
        [TestMethod]
        public void TestDivby0()
        {
            Formula t = new Formula("1/0");
            Assert.IsInstanceOfType(t.Evaluate(s => 0),typeof(FormulaError));
        }

        [TestMethod]
        //test div by 0 via variable
        public void TestDivby0byVar()
        {
            Formula t = new Formula("1/a1");
            Assert.IsInstanceOfType(t.Evaluate(s => 0), typeof(FormulaError));
        }

        //test Invalid Variable during calcalate
        [TestMethod]
        public void TestInValidVariable()
        {
            Formula t = new Formula("1/x1");
            Assert.IsInstanceOfType(t.Evaluate(s => throw new ArgumentException("")), typeof(FormulaError));
        }

        //test complex formula
        [TestMethod]
        public void TestComplexFormula()
        {
            Formula t = new Formula("(((3*3)+1*(a5+16)-(55+77)+1000))*a1");
            Assert.AreEqual(4490, (double)t.Evaluate(s=>5));
        }

        //test div0 with parentheses
        [TestMethod]
        public void TestDiv0withParentheses()
        {
            Formula t = new Formula("(3+3)/(3-3)");
            Assert.IsInstanceOfType(t.Evaluate(s => 0), typeof(FormulaError));
        }

        //test parentheses not match
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ParenthesesTest()
        {Formula t = new Formula("((((3+3)))");}


        //test parentheses not match2
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ParenthesesTest2()
        {Formula t = new Formula("((((3+3)))))");}

        //test the last is operator
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestLastOneIsNoNumberOrVal()
        {Formula t = new Formula("3+3+");}

        //test invalid two operator
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInavlidOperatorset()
        { Formula t = new Formula("3++3"); }

        //test variable without operator or open parel
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInavlidOperatorsetwithVar()
        { Formula t = new Formula("(3+3)a1"); }

        //test number without operator or open parel
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestInavlidOperatorsetwithNum()
        { Formula t = new Formula("(3+3)41"); }

        //test Invaild variable format
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void isVaildTest()
        {Formula t = new Formula("X3+3x",s=>s.ToUpper(),isVaild);}


        //test vaild varible
        [TestMethod]
        public void isVaildTest2()
        {
            Formula t = new Formula("x3+x3", s => s.ToUpper(), isVaild);
            Assert.AreEqual(5.2, (double)t.Evaluate(s=>2.6));
        }

        //test the normalize
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void isVaildTest3()
        {Formula t = new Formula("X+3x", s => s.ToUpper(), isVaild);}

        //test the operator first
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOperatorFirst()
        {Formula t = new Formula("+x3+x3"); }

        //test the empty formula
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmptyFoumula()
        {Formula t = new Formula(""); }

        //test the equal and function
        [TestMethod]
        public void Equaltest()
        {
            string s = "a";
            string? s2= null;
            Formula t = new Formula("1+3+4");
            Formula t1 = new Formula("1.0+3+4.0");
            Assert.AreEqual(t.Equals(t1), true);
            Assert.AreEqual(t.Equals(s), false);
            Assert.AreEqual(t.Equals(s2), false);
        }

        //test the hashcode and try compare with operator
        [TestMethod]
        public void getHashCodeAndEqualTest()
        {
            Formula t = new Formula("1.5+7+4.3");
            Formula t1 = new Formula("1.5+7+4.3");
            Formula t3 = new Formula("1.9+1.3");
            Assert.AreEqual(t.GetHashCode(), t1.GetHashCode());
            Assert.IsTrue(t == t1);
            Assert.IsTrue(t != t3);
        }

        //test get variable
        [TestMethod]
        public void TestGetVariables()
        {
            Formula t = new Formula("a1+c2+b3", s => s.ToUpper(), isVaild);
            IEnumerable<string> test=t.GetVariables();
            Assert.AreEqual(3, test.Count());
            IEnumerator<string> e=test.GetEnumerator();
            e.MoveNext();
            Assert.AreEqual("A1",e.Current);
            e.MoveNext() ;
            Assert.AreEqual("C2", e.Current);
            e.MoveNext();
            Assert.AreEqual("B3", e.Current);
            Assert.IsFalse(e.MoveNext());
        }

        //test double value compare
        [TestMethod]
        public void Testdouble()
        {
            Formula t = new Formula("1e-1+1");
            Formula t2 = new Formula("0.1+1");
            
            Assert.IsTrue(t2.Equals(t));
            Assert.AreEqual(1.1,(double)t.Evaluate(s=>0));
        }

        /// <summary>
        /// this isVaild funtion is for the set condition for the formula isVaild test
        /// I set the condition is start with letter and capital and end with number.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static bool isVaild(string s)
        {
            if(Regex.IsMatch(s, @"^[A-Z]+[0-9]+$"))
                return true;
            else
                return false;
        }
    }

}