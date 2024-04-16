// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)
///// <summary>
/// Author:    Man Wai Lam
/// Partner:   None
/// Date:      20-Jan-2022
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
/// A DependencyGraph can be modeled as a set of ordered pairs of strings. for the futrue 
/// can store the cell name for calculation.
///    
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        //Use Dictionary string as key and the Hashset is value for the Depandency Graph data structure
        //Hashset for the key can contain multi value and make sure not duplicate values.
        private Dictionary<string, HashSet<string>> Dependents;
        private Dictionary<string, HashSet<string>> Dependee;
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            Dependents = new Dictionary<string, HashSet<string>>();
            Dependee = new Dictionary<string, HashSet<string>>();

        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            // using lambda expression to return Dependents Hashset count, which's number of ordered pairs in Dependents
            get { return Dependents.Values.Sum(HashSet => HashSet.Count); }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                //using ContainKey to check the value Count, when the Dictionary have the key than check the value count
                if (Dependee.ContainsKey(s))
                    return Dependee[s].Count();
                else return 0;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            //Using TryGetValue if can get the value in Dependents return true else return False
            if (Dependents.TryGetValue(s, out HashSet<string>? temp))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            //Using TryGetValue if can get the value in Dependee return true else return False
            if (Dependee.TryGetValue(s, out HashSet<string>? temp))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            //If the Dependents Contain the key return key contain value the hashset to IEumerble.
            if (Dependents.ContainsKey(s))
                return Dependents[s];

            return new HashSet<string>();

        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            //If the Dependees Contain the key return key contain value the hashset to IEumerble.
            if (Dependee.ContainsKey(s))
                return Dependee[s];

            return new HashSet<string>();

        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            if (Dependents.ContainsKey(s)) //Checking Dependent is already contain the key or not,if yes just add the value to hashset
                Dependents[s].Add(t);
            else
                Dependents.Add(s, new HashSet<string>() { t });//if not contain the key add the key and new hash set than add the value

            //when done add the dependents need update the Dependees simultaneously

            if (Dependee.ContainsKey(t))//Checking Dependee is already contain the key or not,if yes just add the value to hashset
                Dependee[t].Add(s);
            else
                Dependee.Add(t, new HashSet<string>() { s });//if not contain the key add the key and new hash set than add the value
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            //check Dependents and Dependee is contain the key or not basic on my add function suppost they both have the key when I add
            //also if the value count equal zero remove the key
            if (Dependents.ContainsKey(s))
            {
                Dependents[s].Remove(t);

                if (Dependents[s].Count == 0) { Dependents.Remove(s); }
            }

            if (Dependee.ContainsKey(t))
            {
                Dependee[t].Remove(s);

                if (Dependee[t].Count == 0) { Dependee.Remove(t); }
            }

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            //using back my RemoveDependency and AddDependency function to make sure
            //the ReplaceDependents function simultaneously update dependents and dependees
            if (Dependents.ContainsKey(s))
            {
                foreach (string temp in Dependents[s])
                {
                    RemoveDependency(s, temp);
                }
                foreach (string temp in newDependents)
                {
                    AddDependency(s, temp);

                }
            }
            //If not contain the key using AddDependency to add the key and value
            else
                foreach (string temp in newDependents)
                {
                    AddDependency(s, temp);
                }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            //using back my RemoveDependency and AddDependees function to make sure
            //the ReplaceDependents function simultaneously update dependents and dependees
            if (Dependee.ContainsKey(s))
            {
                foreach (string temp in Dependee[s])
                {
                    RemoveDependency(temp, s);
                }
                foreach (string temp in newDependees)
                {
                    AddDependency(temp, s);
                }

            }
            //If not contain the key using AddDependency to add the key and value
            else
                foreach (string temp in newDependees)
                {
                    AddDependency(temp, s);
                }

        }

    }
}
