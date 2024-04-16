/// <summary>
/// Author:    Man Wai Lam
/// Partner:   None
/// Date:      12-Jan-2022
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
/// This program is for the test the FormulaEvaluator class Project
///  Will test the all basic function and the error handling also, the complex input.
///  
///
///    
/// </summary>


using FormulaEvaluator;
using System;
using System.Globalization;
using System.Security.Cryptography;

namespace FormulaEvaluatorTester
{
    class FormulaEvaluatorTester
    {

        static void Main(string[] args)
        {

            //test 1-4 try the opreator befor the number
            //test1
            try
            {Evaluator.Evaluate("-1+1", s => 0);}
            catch (ArgumentException e)
            {Console.WriteLine("Test 1 " + e.Message);}

            //test 2
            try
            {Evaluator.Evaluate("*1+1", s => 0);}
            catch (ArgumentException e)
            {Console.WriteLine("Test 2 " + e.Message);}

            //test 3
            try
            {Evaluator.Evaluate("/1+1", s => 0);}
            catch (ArgumentException e)
            {Console.WriteLine("Test 3 " + e.Message);}

            //test 4
            try
            {Evaluator.Evaluate("+1+1", s => 0);}
            catch (ArgumentException e)
            { Console.WriteLine("Test 4 " + e.Message);}

            //test 5-8 try the operator is good for cal
            //test 5
            if (Evaluator.Evaluate("10+15", s => 0) == 25)
                Console.WriteLine("test 5 pass");

            //test 6
            if (Evaluator.Evaluate("30-15", s => 0) == 15)
                Console.WriteLine("test 6 pass");

            //test 7
            if (Evaluator.Evaluate("10*10", s => 0) == 100)
                Console.WriteLine("test 7 pass");

            //test 8
            if (Evaluator.Evaluate("10/2", s => 0) == 5)
                Console.WriteLine("test 8 pass");

            //test 9 - test div by 0
            try
            { Evaluator.Evaluate("1/0", s => 0); }

            catch (ArgumentException e)
            { Console.WriteLine("Test 9 " + e.Message); }

            //test 10 -Try the invalid variable name
            try
            { Evaluator.Evaluate("abc+a3", variableChecker); }

            catch (ArgumentException e)
            { Console.WriteLine("Test 10 " + e.Message); }


            //test 11 - parentheses test
            if (Evaluator.Evaluate("(10+15)*3", s => 0) == 75)
                Console.WriteLine("test 11 pass");

            //test 12 - parenthess Error1 test
            try
            { Evaluator.Evaluate("(1+1))", s => 0); }
            catch (ArgumentException e)
            { Console.WriteLine("Test 12 " + e.Message); }

            //test 13 - parenthess Error2 test
            try
            { Evaluator.Evaluate("(((1+1))", s => 0); }
            catch (ArgumentException e)
            { Console.WriteLine("Test 13 " + e.Message); }

            //test 14-Error Variable test
            try
            { Evaluator.Evaluate("10+abb", s => 0); }
            catch (ArgumentException e)
            { Console.WriteLine("Test 14 " + e.Message); }

            //test 15 - stress test
            if (Evaluator.Evaluate("1+ABB12+15/3*(3+30)+3+((30+5)*A3)", variableChecker) == 574)
                Console.WriteLine("test 15 pass");

            //test 16 - stress test
            if (Evaluator.Evaluate("(((3*3)+(TESt12345+16)-(55+77)+1000))", variableChecker) == 898)
                Console.WriteLine("test 16 pass");

            //test 17 - Negative number
            try
            { Evaluator.Evaluate("1-2", s => 0); }
            catch (ArgumentException e)
            { Console.WriteLine("Test 17 " + e.Message); }

            //test 18 test the parenthess 
            if(Evaluator.Evaluate("((3))", s => 0) ==3)
                Console.WriteLine("test 18 pass");

            //test 19 test the signal number
            if (Evaluator.Evaluate("10+(6-3)/244-1)*2", s=>0) == 15)
                Console.WriteLine("test 19 pass");

            int ans = Evaluator.Evaluate("10+(6-3)/244-1)*2", s=>0);
            global::System.Console.WriteLine();

            //test 20 test the repeat varlible
            if (Evaluator.Evaluate("a3+a3*a3-a3", s => 5) == 25)
                Console.WriteLine("test 20 pass");



        }
        /// <summary>
        /// variableChecker is for the test FormulaEvaluator delegate funtion and pass the string to int.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static int variableChecker(string s)
        {
            IDictionary<string, int> numberNames = new Dictionary<string, int>();
            numberNames.Add("c1", 1); //adding a key/value using the Add() method
            numberNames.Add("c2", 2);
            numberNames.Add("A3", 3);
            numberNames.Add("ABB12", 300);
            numberNames.Add("TESt12345", 5);
            if (numberNames.ContainsKey(s))
                return numberNames[s];
            else
                throw new ArgumentException("Unknown variable");


        }
    }
}