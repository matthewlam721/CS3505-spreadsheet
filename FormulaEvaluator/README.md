```
Author:     Man Wai Lam
Partner:    None
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  matthewlam721
Repo:       https://github.com/uofu-cs3500-spring23/spreadsheet-matthewlam721
Date:       18-Jan-2023 4:55pm  
Project:    Formula Evaluator
Copyright:  CS 3500 and [Man Wai Lam] - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:
The Evaluator program is used for the spreadsheet to calculate basic function
## Requirements
-The only legal tokens are the four operator symbols: + - * /, left parentheses, right parentheses, non-negative integers, whitespace, 
and variables consisting of one or more letters followed by one or more digits. Letters can be lowercase or uppercase. 

-If the token is int, peek the stack if is * or / and the value.count>1 pop it and calculate push the result to the value stack else push the int to the value stack

-If the token is variable use the lookup function to check if the variable is no value then throw the ArgumentException by the delegate function.

-if the token is +/- check the value stack, if the value stack >2 pop the value stack twice and calculate then push the result to the value stack.

-if the token is "*"/"/"/"(" push to the operator stack.

-if the token is ) first need to check whether the left parenthesis exists or not, if not, throw the ArgumentException, otherwise pop the operator calculate it until the "(" on the stack top, after pop the "(",
need to peek at the operator if it is "*"/"/"  pop the value and calculate it.

-Make sure when the function return only one value in the value stack and empty operator stack, otherwise should calculate it or throw the ArgumentException

# Assignment Specific Topics
I using the ExtensionsMethod to help me with some reply function isOnTop is for replace the peek function and count and the pushresult is help me to calculate the result and check the value stack have enough 
element to calculate it and then push it, is very helpful to clear the code view and don't reply the code any time.

Also, I use the regular expression to check whether the variable is legal or not, that's much easier than the loop checker.



