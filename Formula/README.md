```
Author:     Man Wai Lam
Partner:    None
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  matthewlam721
Repo:       https://github.com/uofu-cs3500-spring23/spreadsheet-matthewlam721
Date:       02/02/2023 11:10
Project:    Foumula
Copyright:  CS 3500 and [Man Wai Lam] - This work may not be copied for use in Academic Coursework.
```

# Comments to Formula:
This Formula class is refactoring the FormulaEvaluator class; first, the thing change return value from int to double, which mean
need to fix the floating point error, also, make sure the formula is Immutable, mean cannot be modified once it has been created, 
and before the calculation, if the formula format has an error, should throw the FoumulaFormatException, so this program needs to 
make surethe design is correct before calculating in the formula function if the logic of calculating error needs to return the object 
to FomulaError. Also, override some operators for use.

# Requirements
-Immutable  make sure that your Formula type is immutable so that there is no way to change a Formula after it has been created.
-FormulaFormatException class for reporting syntax errors, and a FormulaError struct for returning evaluation errors.
-The FormulaFormatException class is used to throw an exception when there is an error in the formula syntax.
-To make a class immutable, the properties should be set during object construction and not be allowed to change later.
-Two Formulas are considered equal if they've the same commemoratives values in the same order and ignore white spaces, 
after converting number commemoratives to a common format. 
-To compare two numbers for equality, they need to be converted to doubles, compared, 
and then converted back to strings using the Double.Parse or Double.TryParse and ToString method for double.


# Assignment Specific Topics
For this program, I use the regular expression extensions method to make the formula easy to find the match format
I delete the reply code in the Evaluator function to make sure they do the different thing( "compile" time and "run time"), also I have to override the operators
confirm the formula format can compare with different formulas and return is a match or not.


# Consulted Peers:

Tiffany Yau

# References:
https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/operator-overloading - Operator overloading
https://docs.oracle.com/cd/E19957-01/806-3568/ncg_goldberg.html -What Every Computer Scientist Should Know About Floating-Point Arithmetic
https://www.tutorialsteacher.com/csharp/csharp-func-delegate -C# - Func Delegate




