///// <summary>
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
/// File Contents:
/// This Formula class is refactoring the FormulaEvaluator class,the requirement detial
/// in the ReadMe file, also I use ExtensionsMethod to match the Regex let the less error
/// for reply.
///    
/// </summary>

using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary
    public class Formula
    {
        private List<string> Tokens;
        private HashSet<string> normalizeT;


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
        this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            if (formula.Length == 0)
            {
                throw new FormulaFormatException("no empty");
            }
            int OpenParentheses = 0;
            int CloseParentheses = 0;
            double Number;
            string previouslyValue = "";
            Tokens = new List<string>(GetTokens(formula));
            normalizeT = new HashSet<string>();

            for (int i = 0; i < Tokens.Count; i++)
            {

                string token = Tokens[i];
                bool isNumber = false;
                isNumber = double.TryParse(token, out Number);
                if (i == 0) //check the first token in formula make sure is openparentheses,number and variable
                {
                    if (ExtensionsMethod.RegexExtension(token) == "operator" || ExtensionsMethod.RegexExtension(token) == "closeparen") //if the first token is operator throw the FormulaFormatException
                        throw new FormulaFormatException("The First token must be a number or open parentheses");
                    else if (isNumber == true)//if is number tostring and copy back to Tokens list and save as previously value
                    {
                        Tokens[i] = Number.ToString();
                        previouslyValue = Tokens[i];
                        continue;
                    }
                    else if (ExtensionsMethod.RegexExtension(token) == "variable" && isValid(normalize(token)) && isNumber == false)
                    {
                        normalizeT.Add(normalize(token));//if is variable check normalize and is vaild if pass put the hashset and save to previously value 
                        Tokens[i] = normalize(token);
                        previouslyValue = Tokens[i];
                        continue;
                    }
                    else if (!isValid(normalize(token)))
                        throw new FormulaFormatException("No legal variable");//if not pass throw the FormulaFormatException

                }

                //check if is open Parentheses
                if (ExtensionsMethod.RegexExtension(token) == "openparen")
                {
                    OpenParentheses++;//count the OpenParentheses
                }

                //check if is close Parentheses, also check before close parenthese have any open Open Parentheses.
                if (ExtensionsMethod.RegexExtension(token) == "closeparen" && OpenParentheses > CloseParentheses)
                {
                    CloseParentheses++;// count the CloseParentheses
                }
                else if (ExtensionsMethod.RegexExtension(token) == "closeparen" && OpenParentheses <= CloseParentheses)
                    throw new FormulaFormatException("Cannot have CloseParentheses before OpenParentheses"); //throw the FormulaFormatException if closeparenthese more than openparenthese



                //check if number before is not openparenthese or operator throw FormulaFormatException
                if (isNumber == true && ExtensionsMethod.RegexExtension(previouslyValue) != "operator" && ExtensionsMethod.RegexExtension(previouslyValue) != "openparen")
                    throw new FormulaFormatException("Invalid formula before the number must have the operator or OpenParentheses");
   

                else if (isNumber == true)
                {
                    Tokens[i] = Number.ToString(); //If is number tostring and add back to list
                }

                //check if variable before is not openparenthese or operator throw FormulaFormatException and check isvalid and normalize
                else if (ExtensionsMethod.RegexExtension(token) == "variable" && isValid(normalize(token)) && isNumber == false)
                {
                    if (ExtensionsMethod.RegexExtension(previouslyValue) != "operator" && ExtensionsMethod.RegexExtension(previouslyValue) != "openparen")
                    {
                        throw new FormulaFormatException("Invalid formula before the number must have the operator or OpenParentheses");
                    }
                    else
                    {
                        normalizeT.Add(normalize(token));
                        Tokens[i] = normalize(token);
                    }
                }
                else if (ExtensionsMethod.RegexExtension(token) == "variable" && !isValid(normalize(token)))
                    throw new FormulaFormatException("No legal variable");

                //check the operator make sure before is closeparenthese number or variable. 
                else if (ExtensionsMethod.RegexExtension(token) == "operator")
                {
                    if (ExtensionsMethod.RegexExtension(previouslyValue) != "variable" && ExtensionsMethod.RegexExtension(previouslyValue) != "num"
                        && ExtensionsMethod.RegexExtension(previouslyValue) != "closeparen")
                    {
                        throw new FormulaFormatException("before the operator must have number, variable or closeparenthese");
                    }

                }

                //check the last token must be closeparenthese, variable or number
                if (i == Tokens.Count - 1)
                {
                    if (ExtensionsMethod.RegexExtension(token) != "variable" && ExtensionsMethod.RegexExtension(token) != "num" &&
                        ExtensionsMethod.RegexExtension(token) != "closeparen")
                        throw new FormulaFormatException("last one must be a variable, number or closeparenthese");
                }




                previouslyValue = Tokens[i]; //copy value for next loop use


            }
            if (OpenParentheses != CloseParentheses) //checl the parenthese if not match throw FormulaFormatException
            {
                throw new FormulaFormatException("Parentheses not match");
            }




        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            /*
             * Value and Operator stack for the save the token operators and value for the calculate
             * The int openParentheses is for the count how many Parentheses has open.
             */
            Stack<double> value = new Stack<double>();
            Stack<string> operators = new Stack<string>();


            foreach (string token in Tokens)
            {
                bool iszero = false;
                double Number;
                bool isNumber = double.TryParse(token, out Number); //check the token is number or not if is number will be true and save to the int Number

                if (token == "*" || token == "/")
                {
                        operators.Push(token);  //Push the token to operatora stack
                    continue;
                }



                else if (isNumber == true) //push the number stack
                {
                    value.Push(Number);
                    isNumber = false; // when the number been pushed isNumber change to false
                    if (operators.isONtop("*") || operators.isONtop("/"))
                    {
                        iszero = ExtensionsMethod.isZero(value, operators);
                        if (iszero == true) { return new FormulaError("cannot div by 0"); }
                        value.Push(ExtensionsMethod.pushresult(value, operators, value));
                    }
                    continue;
                }


                else if (token == "+" || token == "-")
                {


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
                    operators.Push(token);
                    continue;
                }

                else if (token == ")")
                {
                    while (!operators.isONtop("("))  
                    {
                        if (operators.isONtop("+") || operators.isONtop("-"))
                        {
                            value.Push(ExtensionsMethod.pushresult(value, operators, value));

                        }
                    }
                    operators.Pop();

                    if (operators.isONtop("*") || operators.isONtop("/"))
                    {
                        iszero=ExtensionsMethod.isZero(value, operators);
                        if(iszero==true) { return new FormulaError("cannot div by 0"); }
                        value.Push(ExtensionsMethod.pushresult(value, operators, value));

                    }
                    continue;
                }
                else  
                {
                    try
                    {
                        Number = (double)lookup(token);
                    }
                    catch (ArgumentException)
                    {
                        return new FormulaError("This variable is no value");
                    }

                    Number = lookup(token);
                    value.Push(Number);
                    if (operators.isONtop("*") || operators.isONtop("/"))
                    {
                        iszero = ExtensionsMethod.isZero(value, operators);
                        if (iszero == true) { return new FormulaError("cannot div by 0"); }
                        value.Push(ExtensionsMethod.pushresult(value, operators, value));
                    }
                    continue;
                }
            }

            if (operators.Count == 1 && value.Count == 2) //If have two values and one operator the operators in the stack must be + or -
            {
                if (operators.isONtop("+") || operators.isONtop("-"))
                    value.Push(ExtensionsMethod.pushresult(value, operators, value));
            }

            return value.Pop();

        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<string> enumerableList = normalizeT;
            return enumerableList;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string? formula = "";
            for (int i = 0; i < Tokens.Count; i++)
            {
                formula += Tokens[i];
            }
            return formula;
        }

        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj == null || (obj is not Formula))
                return false;
            return ToString().Equals(obj.ToString());
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1 == f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            int hashcode = ToString().GetHashCode();
            return hashcode;
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }


    }

    public static class ExtensionsMethod
    {
        /// <summary>
        /// This function is for easy to find the match regular expression which's just type what u want to match less error for the program
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string RegexExtension(string s)
        {
            if (Regex.IsMatch(s, @"[a-zA-Z_](?: [a-zA-Z_]|\d)*"))
                return "variable";
            else if (Regex.IsMatch(s, @"[\+\-*/]"))
                return "operator";
            else if (Regex.IsMatch(s, @"\("))
                return "openparen";
            else if (Regex.IsMatch(s, @"\)"))
                return "closeparen";
            else if (Regex.IsMatch(s, "\\d+(\\.\\d+)?"))
                return "num";

            return "NoResult";
        }

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


        public static double pushresult(this Stack<double> stackvalue, Stack<string> stackopt, Stack<double> stackvalue2)
        {
            double ans = 0;
            double value1;
            double value2;
            string? opt;

            stackvalue2.TryPop(out value2);
            stackvalue.TryPop(out value1);
            stackopt.TryPop(out opt);

            if (opt == "*")
            {
                ans = value1 * value2;
                return (ans);
            }
            else if (opt == "/")
            {
                ans = value1 / value2;
                return ans;
            }
            else if (opt == "+")
            {
                ans = value1 + value2;
                return ans;
            }

            else
            {
                ans = value1 - value2;
                return ans;
            }
        }
        /// <summary>
        /// Make sure not div by 0 if the value is zero return false than return the FormalaError
        /// </summary>
        /// <param name="stackvalue"></param>
        /// <param name="stackopt"></param>
        /// <returns></returns>
        public static bool isZero(this Stack<double> stackvalue, Stack<string> stackopt)
        {
            if(stackvalue.Peek()==0 && stackopt.Peek()=="/")
            {
                return true;
            }
            else 
                return false;
 
        }



    }


}


// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>


