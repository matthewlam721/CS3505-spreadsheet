/// <summary>
/// Author:    Man Wai Lam
/// Partner:   None
/// Date:      16-Feb-2023 11:30pm
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
///     This class is for Test  "Complete" Spreadsheet Model which need test save, load and calculate
///     function, and make sure the exception throw the right way, and need the stresstest.
///
///    
/// </summary>
using SpreadsheetUtilities;
using SS;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Xml;

namespace SpreadsheetTests
{
    /// <summary>
    /// Test class for the spreadsheet class.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests : Spreadsheet
    {


        /// <summary>
        /// Test add function
        /// </summary>
        [TestMethod()]
        public void TestaddValue()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "=1+1");
            Assert.AreEqual((double)2, s.GetCellValue("B1"));
        }


        /// <summary>
        /// Test string value
        /// </summary>
        [TestMethod()]
        public void TestStringValue()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "HIHI");
            Assert.AreEqual("HIHI", s.GetCellContents("B1"));
        }

        /// <summary>
        /// Test double value
        /// </summary>
        [TestMethod()]
        public void TestDoubleValue()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1234567889", "1.58");
            Assert.AreEqual(1.58, s.GetCellContents("B1234567889"));
        }

        /// <summary>
        /// Test Formula value
        /// </summary>
        [TestMethod()]
        public void TestFormulaValue()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "=A1+A2");
            Formula f = (Formula)s.GetCellContents("B1");
            Assert.AreEqual(new Formula("A1+A2"), f);
        }

        /// <summary>
        /// Test the cell vaild name 
        /// </summary>
        [TestMethod()]
        public void TestSetCellContentsWithString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("___", "1+1");
            s.SetContentsOfCell("o_11", "5+1");
            Assert.AreEqual("1+1", s.GetCellContents("___"));
            Assert.AreEqual("5+1", s.GetCellContents("o_11"));
        }

        /// <summary>
        /// test the circular error
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(CircularException))]
        public void TestCircularException()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A2", "=A1+A3");
        }

        /// <summary>
        /// test the empty string input
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestEmptyStringException()
        {
            Spreadsheet s = new Spreadsheet();
            string? a = null;
            s.SetContentsOfCell("A1", a);
        }

        /// <summary>
        /// Test null formula
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmptyFormulaException()
        {
            new Spreadsheet().SetContentsOfCell("A1", "=");
        }

        /// <summary>
        /// Test null value in Cell will not be get with GetNamesOfAllNonemptyCells method.
        /// </summary>
        [TestMethod()]
        public void TestNullValueInCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "");
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        /// <summary>
        /// test the invaild cell name with GetCellContents throw the InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContentsInvalidNameTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("1A1");
        }

        /// <summary>
        /// test the invaild cell name with SetCellContents throw the InvalidNameException
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameException()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("123A", "5+5");
        }

        /// <summary>
        /// test the invaild cell name with symbol with SetCellContents throw the InvalidNameException
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidNameException2()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("&A1", "5+5");
        }

        /// <summary>
        /// Test the Denpendency make sure return list have all dependency 
        /// </summary>
        [TestMethod()]
        public void DenpendencyTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "=A1*2");
            s.SetContentsOfCell("C1", "=B1*A1");
            Assert.IsTrue(s.SetContentsOfCell("A1", "100").SequenceEqual(new List<string>() { "A1", "B1", "C1" }));
        }

        /// <summary>
        /// Test the GetNamesOfAllNonemptyCells function
        /// </summary>
        [TestMethod()]
        public void GetNamesOfAllNonemptyCellsTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "30");
            s.SetContentsOfCell("A2", "=A1+C3");
            s.SetContentsOfCell("A3", "eee");
            Assert.AreEqual(3, s.GetNamesOfAllNonemptyCells().Count());
        }

        /// <summary>
        /// Test the empty cell should not throw any error in GetCellContents method
        /// </summary>
        [TestMethod()]
        public void TestEmptyCell()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual("", s.GetCellContents("A1"));
        }


        /// <summary>
        /// Test complex cell
        /// </summary>
        [TestMethod()]
        public void TestcomplexCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=C1+B1");
            s.SetContentsOfCell("B1", "=C2+B2");
            s.SetContentsOfCell("B2", "=B3+A3");
            s.SetContentsOfCell("A1", "33");
            Assert.AreEqual((double)33, s.GetCellContents("A1"));
        }

        //add for PS5

        /// <summary>
        /// Test set and get cee contents
        /// </summary>
        [TestMethod]
        public void TestSetAndGetCellContents()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "5");
            Assert.AreEqual(5.0, s.GetCellValue("A1"));
            s.SetContentsOfCell("B2", "Hello World");
            Assert.AreEqual("Hello World", s.GetCellValue("B2"));
            s.SetContentsOfCell("C3", "=A1+B2");
            Assert.AreEqual(new Formula("A1+B2").Evaluate(s.lookupdouble), s.GetCellValue("C3"));
            Assert.AreEqual("", s.GetCellValue("A12"));
        }

        /// <summary>
        /// Test recalcaulate cell
        /// </summary>
        [TestMethod()]
        public void TestRecalculateCell()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "6");
            s.SetContentsOfCell("c1", "=a1+5");
            s.SetContentsOfCell("c3", "10");
            s.SetContentsOfCell("c2", "=c1+c3");
            s.SetContentsOfCell("B1", "=c2");
            Assert.AreEqual((double)21, s.GetCellValue("B1"));


        }
        /// <summary>
        /// Save test
        /// </summary>
        [TestMethod]
        public void SaveTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.Save("testsave.xml");
            Assert.AreEqual("default", s.GetSavedVersion("testsave.xml"));
        }

        /// <summary>
        /// Test null file name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]

        public void TestSave_NullFileName()
        {
            Spreadsheet s = new Spreadsheet();
                s.Save(null);
        }

        /// <summary>
        /// Test save multiiple type
        /// </summary>
        [TestMethod]
        public void TestSaveWithMultipleTypes()
        {
            Spreadsheet s = new Spreadsheet();

            s.SetContentsOfCell("A1", "PS5");
            s.SetContentsOfCell("B1", "5.8");
            s.SetContentsOfCell("C1", "=5*6");

            string filename = "test-spreadsheet.xml";
            s.Save(filename);

            string fileContents = File.ReadAllText(filename);

            Assert.IsTrue(fileContents.Contains("<spreadsheet version=\"default\">"));
            Assert.IsTrue(fileContents.Contains("<cell><name>A1</name><contents>PS5</contents></cell>"));
            Assert.IsTrue(fileContents.Contains("<cell><name>B1</name><contents>5.8</contents></cell>"));
            Assert.IsTrue(fileContents.Contains("<cell><name>C1</name><contents>=5*6</contents></cell>"));

            File.Delete(filename);
        }

        /// <summary>
        /// Test save and read
        /// </summary>
        [TestMethod]
        public void TestSaveAndRead()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "1");
            s.SetContentsOfCell("B1", "=A1+1");
            s.SetContentsOfCell("C1", "hello world");

            string filename = "test-spreadsheet.xml";
            s.Save(filename);

            Spreadsheet newSheet = new Spreadsheet(filename, s => true, s => s.ToUpper(), "default");

            Assert.AreEqual(1.0, newSheet.GetCellValue("A1"));
            Assert.AreEqual(2.0, newSheet.GetCellValue("B1"));
            Assert.AreEqual("hello world", newSheet.GetCellValue("C1"));

            File.Delete(filename);
        }


        /// <summary>
        /// Test Constructor Invalid Version
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestConstructorInvalidVersion()
        {
            string path = "testinvaild.xml";
            File.Create(path).Close();
            using (XmlWriter writer = XmlWriter.Create(path))
            {
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "abcd");
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }

            Spreadsheet spreadsheet = new Spreadsheet(path, s => true, s => s, "dcba");

        }

        /// <summary>
        /// Test Constructor Null Path
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestConstructorNullPath()
        {
            Spreadsheet spreadsheet = new Spreadsheet(null, s => true, s => s, "dcba");

        }
        /// <summary>
        /// Test Constructor With Invalid Path
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestConstructorWithInvalidPath()
        {
            string version = "1.0";
            string path = "no this path.xml";
            Spreadsheet spreadsheet = new Spreadsheet(path, s => true, s => s, version);
        }

        /// <summary>
        /// Test Constructor With Non Existent File
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestConstructorWithNonExistentFile()
        {
            string path = "abbadf.xml";
            Spreadsheet sheet = new Spreadsheet(path, s => true, s => s, "default");
        }

        // Test for file not found exception
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSaveFileNotFound()
        {
            Spreadsheet s = new Spreadsheet();
            s.Save(@"C:\miss.xml");
        }

        // Test for invalid XML format exception
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSaveInvalidXML()
        {
            Spreadsheet s = new Spreadsheet();
            s.Save("invalid.xml");
            File.WriteAllText("invalid.xml", "Whatever");
            s = new Spreadsheet("invalid.xml", s.IsValid, s.Normalize, s.Version);
        }


        /// <summary>
        /// Test Save IOException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestSaveIOException()
        {
            var path = "path/for/test";
            var version = "1.0";
            var s = new Spreadsheet(s=>true, s=>s, version);
            s.SetContentsOfCell("A1", "Hello World");
            s.Save(path);


        }

        /// <summary>
        /// Save to Unauthorized Access Exception
        /// </summary>
        [TestMethod]
        public void Save_UnauthorizedAccessException()
        {
            var spreadsheet = new Spreadsheet();
            var filename = @"C:\Windows\System32\test.xml";
            Assert.ThrowsException<SpreadsheetReadWriteException>(() => spreadsheet.Save(filename), $"Access to the path '{filename}' is denied.");
        }

        /// <summary>
        /// Test get save version
        /// </summary>
        [TestMethod]
        public void TestGetSavedVersion()
        {
            string filename = "testSpreadsheet.xml";
            string version = "1.0";
            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename))
                {
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", version);
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                Spreadsheet sheet = new Spreadsheet();
                Assert.AreEqual(version, sheet.GetSavedVersion(filename));
            }
            finally
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }
        }
        /// <summary>
        /// Test change method
        /// </summary>
        [TestMethod]
        public void TestChangedProperty()
        {
            var spreadsheet = new Spreadsheet();
            bool changed1 = spreadsheet.Changed;

            spreadsheet.SetContentsOfCell("A1", "42");
            bool changed2 = spreadsheet.Changed;

            spreadsheet.Save("testChange.xml");
            bool changed3 = spreadsheet.Changed;

            Assert.IsFalse(changed1);
            Assert.IsTrue(changed2);
            Assert.IsFalse(changed3);
        }

        /// <summary>
        /// Get Saved Version No Version Exception
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedVersionNoVersionException()
        {
            string path = "test.xml";
            using (XmlWriter writer = XmlWriter.Create(path))
            {
                writer.WriteStartElement("spreadsheet");
                writer.WriteEndElement();
            }

            Spreadsheet s = new Spreadsheet();
            s.GetSavedVersion(path);
        }

        /// <summary>
        /// tesl null exception with constructor
        /// </summary>
        [TestMethod]
        public void SpreadsheetThrowsArgumentNullException()
        {

            Assert.ThrowsException<ArgumentNullException>(() => new Spreadsheet(null, s=>s, "abc"));
            Assert.ThrowsException<ArgumentNullException>(() => new Spreadsheet(s=>true, null, "abc"));
            Assert.ThrowsException<ArgumentNullException>(() => new Spreadsheet(s=>true, s=>s, null));
        }


        /// <summary>
        /// Calculate stress test
        /// </summary>
        [TestMethod]
        public void stresstest1()
        {
            Spreadsheet s = new Spreadsheet();

            string formula = "1";
            for (int i = 1; i <= 10000; i++)
            {
                string cellName = "A" + i;
                s.SetContentsOfCell(cellName, i.ToString());
                formula += "+" + cellName;
            }

            s.SetContentsOfCell("B1", "=" + formula);

            double expected = 50005001;
            object actual = s.GetCellValue("B1");
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Dependency cell stress test
        /// </summary>
        [TestMethod]
        public void stresstest2()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 0; i <= 1000; i++)
            {
                string cellName = "B" + i;
                if (i == 0)
                    s.SetContentsOfCell(cellName, "1.3");
                else
                {
                    int a = i - 1;
                    string cellName2 = "B" + a;
                    s.SetContentsOfCell(cellName, "="+cellName2);
            
                }   
            }
            for (int i = 0; i <= 1000; i++)
            {
                string cellName = "B" + i;
                Assert.AreEqual(1.3, s.GetCellValue(cellName));
            }
        }

        /// <summary>
        /// Save method stress Test
        /// </summary>
        [TestMethod]
        public void stresstest3()
        {
            Spreadsheet s = new Spreadsheet();
            for (int i = 0; i <= 1000; i++)
            {
                string cellName = "B" + i;
                if (i == 0)
                    s.SetContentsOfCell(cellName, "1.3");
                else
                {
                    int a = i - 1;
                    string cellName2 = "B" + a;
                    s.SetContentsOfCell(cellName, "=" + cellName2);

                }
            }
            string filename = "test-spreadsheet.xml";
            s.Save(filename);

            Spreadsheet newSheet = new Spreadsheet(filename, s => true, s => s.ToUpper(), "default");
            for (int i = 0; i <= 1000; i++)
            {
                string cellName = "B" + i;
                Assert.AreEqual(1.3, newSheet.GetCellValue(cellName));
            }
            File.Delete(filename);
        }





    }

}