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
/// This program is for the test DependencyGraph, make sure all function work
/// and update whole DependencyGraph when add , remove and replace
///    
/// </summary>

using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;


namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }


        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
        }



        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }




        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }


        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }




        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void ReplaceThenEnumerate()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.AddDependency("a", "z");
            t.ReplaceDependents("b", new HashSet<string>());
            t.AddDependency("y", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "c" });
            t.AddDependency("w", "d");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            t.ReplaceDependees("d", new HashSet<string>() { "b" });

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }



        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }

          
        }
        /*
         * Self code for the test
         */

        /// <summary>
        /// Test HasDependents function
        /// </summary>
        [TestMethod()]
        public void HasDependentsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            Assert.IsTrue (t.HasDependents("x"));
            Assert.IsFalse (t.HasDependents("b"));

        }

        /// <summary>
        /// Test HasDependees function
        /// </summary>
        [TestMethod()]
        public void HasDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            Assert.IsTrue(t.HasDependees("b"));
            Assert.IsFalse(t.HasDependees("x"));

        }

        /// <summary>
        /// Test ReplaceDependents function
        /// </summary>
        [TestMethod()]
        public void ReplaceDependentsTest() 
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.ReplaceDependents("x", new HashSet<string>() { "c","a" });
            Assert.AreEqual(2,t.Size);
            t.ReplaceDependents("a",new HashSet<string>() { "c"});
            Assert.IsTrue(t.HasDependents("a"));
            Assert.AreEqual(2, t["c"]);
        }

        /// <summary>
        /// Test ReplaceDepeedees function
        /// </summary>
        [TestMethod()]
        public void ReplaceDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.ReplaceDependees("b", new HashSet<string>() { "a", "c" });
            Assert.AreEqual(2, t["b"]);
            t.ReplaceDependees("t", new HashSet<string>() {"x","o"});
            Assert.AreEqual(4, t.Size);

        }

        /// <summary>
        /// Test No this key in dependees
        /// </summary>
        [TestMethod()]
        public void TestNoDependeesKey()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t["a"]);
        }

        [TestMethod()]
        public void TestRemoveKey()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "b");
            t.RemoveDependency("x", "b");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        /// Test complex input with Eumerator
        /// </summary>
        [TestMethod()]
        public  void TestComplexWithEnumerator()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependents("a", new HashSet<string>() { "b", "c", "d", "e", "f" });
            Assert.AreEqual(5, t.Size);
            IEnumerator<string> i = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(i.MoveNext());
            Assert.AreEqual("b", i.Current);
            t.ReplaceDependents("a", new HashSet<string>() { "x","y" });
            i = t.GetDependents("a").GetEnumerator();
            Assert.IsTrue(i.MoveNext());
            Assert.AreEqual("x", i.Current);
            Assert.AreEqual(2, t.Size);
            i=t.GetDependees("x").GetEnumerator();
            Assert.IsTrue(i.MoveNext());
            Assert.AreEqual("a", i.Current);
            i=t.GetDependees("c").GetEnumerator();
            Assert.IsFalse(i.MoveNext());
        }

        /// <summary>
        /// Big size dependents test
        /// </summary>
        [TestMethod()]
        public void BigSizeDependentsTest()
        {
            DependencyGraph t = new DependencyGraph();
            for(int i=0; i<10000; i++)
            {
                string test = i.ToString();
                t.AddDependency("a",test);
            }
            Assert.AreEqual(10000, t.Size);
            for(int i =10000; i>0; i--)
            {
                string test = i.ToString();
                t.ReplaceDependents("a", new HashSet<string>() { test});
            }
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        /// Big size dependees test
        /// </summary>
        [TestMethod()]
        public void BigSizeDependeeTest()
        {
            DependencyGraph t = new DependencyGraph();
            for (int i = 0; i < 10000; i++)
            {
                string test = i.ToString();
                t.AddDependency(test, "a");
            }
            Assert.AreEqual(10000, t["a"]);
            for (int i = 10000; i > 0; i--)
            { 
                t.ReplaceDependees("a", new HashSet<string>() { "b" });
            }
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        /// Test the hashset make sure the hashset is working
        /// </summary>
        [TestMethod()]
        public void HashsetTest()
        {
            string test = "1";
            DependencyGraph t = new DependencyGraph();
            for (int i = 0; i < 1000; i++)
            {
                t.AddDependency("a", test);
            }
            Assert.AreEqual(1, t.Size);
            Assert.AreEqual(1, t["1"]);
        }

        /// <summary>
        /// complex add, remove and replace make sure get the right return 
        /// </summary>
        [TestMethod()]
        public void ComplexTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "k");
            t.AddDependency("k", "b");
            t.AddDependency("b", "a");
            t.AddDependency("c", "b");
            Assert.AreEqual(2, t["b"]);
            
            t.ReplaceDependees("a", new HashSet<string>() {"b","k","c"});
            Assert.AreEqual(6, t.Size);
            t.ReplaceDependees("a", new HashSet<string>() { "b"});
            t.RemoveDependency("b","a");
            Assert.IsFalse(t.HasDependents("b"));
            Assert.IsFalse(t.HasDependees("a"));
            IEnumerator<string> i = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(i.MoveNext());
       

        }

    }
}
