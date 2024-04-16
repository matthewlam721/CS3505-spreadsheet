```
Author:     Man Wai Lam
Partner:    None
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  matthewlam721
Repo:       https://github.com/uofu-cs3500-spring23/spreadsheet-matthewlam721
Date:       25-Jan-2023 4:55pm 
Project:    DependencyGraph
Copyright:  CS 3500 and [Man Wai Lam] - This work may not be copied for use in Academic Coursework.
```

# Comments to DependencyGraph:
The DependencyGraph Program is for building a spreadsheet and getting the cell value for the calculation, A1="1+1", A2="A1+3", A3=A1+A2 
which means A3's dependents is {A2, A1}. A1 is dependees {A2, A3}, This Program is making the Dependency Graph to make sure the spreadsheet 
gets the right cell and save the cell depend which cell.

## Requirements
When updating whatever dependents or dependees, they sure are updated simultaneously.
Make sure constant time for all function exceptions of (ReplaceDependents and ReplaceDependees).
Refrain from Replying with code for fewer errors. If can use a funtion, remember to call the function, do not type it again.
Make sure when RemoveDependency if the key is no value inside, remove the key.
Make the program easy to look at and read.


# Assignment Specific Topics
For this program, I use the dictionary key type is a string, and the value type is HashSet. Since the DependencyGraph, 
cell contains multiple cell needs, I use the HashSet and can save the same cell only once for errors and memory leaks.
Also, I tried ReplaceDependents and ReplaceDependees methods using back AddDependency and RemoveDependency functions to 
make both dictionary can update simultaneously also, which will look clear the code and fewer error.


# Consulted Peers:

Tiffany Yau

# References:

    1. C# - Dictionary<TKey, TValue>- https://www.tutorialsteacher.com/csharp/csharp-dictionary
    2. 5 C# Collections that Every C# Developer Must Know - https://programmingwithmosh.com/net/csharp-collections/
    3. HashSet<T>.Count Property - https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.hashset-1.count?view=net-7.0




