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
///    In this assignment I using the ExtensionsMethod to save much time,
///    and using the regular expression set the rule to check the variable.
///    also I make sure the formula is not any invalid input after the foreach
///
///    
/// </summary>


using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq.Expressions;
using System;
using System.Windows.Markup;

namespace FormulaEvaluator
{
    public class Evaluator
    {
        public delegate int Lookup(String variable_name);
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            if (expression == "")
                throw new ArgumentException("Cannot run with Empty string");
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            /*
             * Value and Operator stack for the save the token operators and value for the calculate
             * The int openParentheses is for the count how many Parentheses has open.
             */
            Stack<int> value = new Stack<int>();
            Stack<string> operators = new Stack<string>();
            int openParentheses = 0;


            foreach (string token in substrings)
            {
                int Number;
                bool isNumber = int.TryParse(token, out Number); //check the token is number or not if is number will be true and save to the int Number
                if (token.Trim() == "")
                    continue;

                if (token == "*" || token == "/")
                {
                    if (value.Count == 0)   //check the have any value before the operators if dones't have any value that's invalid formula
                        throw new ArgumentException("Not any number before the opreate");
                    else
                        operators.Push(token);  //Push the token to operatora stack
                    continue;
                }


                else if (isNumber == true) //push the number stack
                {
                    value.Push(Number);
                    isNumber = false; // when the number been pushed isNumber change to false
                    if (operators.isONtop("*") || operators.isONtop("/"))
                        value.Push(ExtensionsMethod.pushresult(value, operators, value));
                    continue;
                }


                else if (token == "+" || token == "-")
                {
                    if (value.Count == 0)   //check the have any value before the operators if dones't have any value that's invalid formula
                        throw new ArgumentException("Not any number before the opreate");

                    if (operators.isONtop("+") || operators.isONtop("-"))
                    {
                        value.Push(ExtensionsMethod.pushresult(value, operators, value));
                        operators.Push(token);
                    }
                    else
                        operators.Push(token);
                    continue;
                }

                else if (token == "(")
                {
                    openParentheses++;
                    operators.Push(token);
                    continue;
                }

                else if (token == ")")
                {
                    if (openParentheses == 0)
                        throw new ArgumentException("cannot have close parentheses without openparentheses"); //check before the close parentheses to make sure have the open parentheses. If no then throw the ArgumentException
                    openParentheses--;
                    while (!operators.isONtop("("))  //using while loop make sure the parentheses has been done with calculate
                    {
                        if (operators.isONtop("+") || operators.isONtop("-"))
                        {
                            value.Push(ExtensionsMethod.pushresult(value, operators, value));


                        }
                        else if (operators.isONtop("*") || operators.isONtop("/"))
                        {
                            value.Push(ExtensionsMethod.pushresult(value, operators, value));

                        }



                    }
                    operators.Pop();

                    if (operators.isONtop("*") || operators.isONtop("/"))
                    {
                        value.Push(ExtensionsMethod.pushresult(value, operators, value));

                    }
                    continue;
                }
                else if (Regex.IsMatch(token, @"^[a-zA-Z]+[0-9]+$") && isNumber == false) //check the variable using regular expression make sure the variable name start with a-z and A-Z 
                {
                    Number = variableEvaluator(token);
                    value.Push(Number);
                    if (operators.isONtop("*") || operators.isONtop("/"))
                        value.Push(ExtensionsMethod.pushresult(value, operators, value));
                    continue;
                }
                else
                    throw new ArgumentException(token + " is invalid input"); //other input should be invalid input so throw the ArgumentException

            }

            if (operators.Count > 0 && value.Count == 1)  //if left the operators but only have one value.
                throw new ArgumentException("have extra operator not be cal");

            else if (operators.Count == 0 && value.Count > 1)  //if the operators equal zero should only have one value.
                throw new ArgumentException("If the don't have any operator, value should be have one");


            else if (operators.Count == 1 && value.Count == 2) //If have two values and one operator the operators in the stack must be + or -
            {
                if (operators.isONtop("+") || operators.isONtop("-"))
                    value.Push(ExtensionsMethod.pushresult(value, operators, value));
            }
            else if (operators.Count == 1 && value.Count > 2)
                throw new ArgumentException("formula error only have the one operators but have more than two value");


            if (openParentheses != 0) //Check whether the Parentheses are equal to zero or not. If not means the Parentheses is not match, which's an invalid formula.
                throw new ArgumentException("Parentheses number not match");

            if (value.Peek() < 0)//check the number is negative if yes throw the argumentException
                throw new ArgumentException("Cannot have negative number");

            return value.Pop();
        }
    }
    /// <summary>
    /// The ExtensionsMethod is for the calculate method and peek, using the function
    /// Trying don't repeat me code and make the code easier to read
    /// </summary>

    public static class ExtensionsMethod
    {
        /*
         * IsOntop is for checking which operator is on the stack, also make sure the operator stack is greater than 0 
         */
        public static bool isONtop(this Stack<string> stack, string c)
        {
            return stack.Count > 0 && stack.Peek() == c;
        }

        /*
         * push result function is for calculating the formula, using the three stack input, and setting the conditional to make sure to fulfill the request.
         * Also don't need to repeat these codes too many times, make the code clearly
         */
        public static int pushresult(this Stack<int> stackvalue, Stack<string> stackopt, Stack<int> stackvalue2)
        {
            if (stackvalue.Count < 2) //make sure have the value in the stack for calculate if not throw the ArgumentException
                throw new ArgumentException("no number can cal");
            if (stackopt.Count < 1) //make sure have operator in stack
                throw new ArgumentException("No operator in stack");
            int ans = 0;
            int value1;
            int value2;
            string opt;

            stackvalue2.TryPop(out value2);
            stackvalue.TryPop(out value1);
            stackopt.TryPop(out opt);

            if (opt == "*")
            {
                ans = value1 * value2;
                return ans;
            }
            else if (opt == "/")
            {
                if (value2 == 0)
                    throw new ArgumentException("cannot div by 0"); //make sure not div by 0, if div by 0 throw the ArgumentException
                else
                    ans = value1 / value2;
                return ans;
            }
            else if (opt == "+")
            {
                ans = value1 + value2;
                return ans;
            }

            else if (opt == "-")
            {
                ans = value1 - value2;
                return ans;
            }
            else
                throw new Exception("");
        }
    }
}