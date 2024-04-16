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
/// 
///   This program is A "Complete" Spreadsheet Model which mean all
///  spreadsheet method should be via this class, also is "MVC" model
///  this program is added save load, constructors for the get the spreadsheet
///  also make the protected make sure only inheritance can modify the code.
///    
/// </summary>
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using SpreadsheetUtilities;
using static System.Net.Mime.MediaTypeNames;
using System.Xml;
using System.Diagnostics.Metrics;
using System.Reflection.PortableExecutable;

namespace SS
{
    /// <summary>
    /// This spreadsheet class is inheritance AbstractSpreadsheet, AbstractSpreadsheet is an abstract class, 
    /// that has defined the abstract method for common behavior for the spreadsheet, 
    /// which in this class needs to make the abstract method complete work.
    /// </summary>
    public class Spreadsheet: AbstractSpreadsheet
    {
        /// <summary>
        /// This Cell class is for the future use now just add getter and setter
        /// and constructor to here and Content object is for save object and display in the cell variable
        /// because have setcellofcontent method so if not formula that must is double or string
        /// </summary>
        private class Cell
        {
            public string cellname { get; set; }
            public object Content { get; set; }
            public object value { get; set; }
            public Func<string, double> Lookup { get; private set; }

            // Constructor that initializes the cell's name, content, and lookup function.
            public Cell(string Cellname, object value, Func<string, double> lookup)
            {

                this.cellname = Cellname;
                this.Content = value;
                this.Lookup = lookup;
                if (value is Formula)
                {
                    Formula formula = (Formula)value;
                    this.value = formula.Evaluate(lookup);
                }
                else
                    this.value = value;
            }

        }


        /// This dictionary is for save the cell value of each cell.
        private Dictionary<string, Cell> Cellvalue = new Dictionary<string, Cell>();
        private DependencyGraph DependencyGraph = new DependencyGraph();
        private bool isChanged;


        /// <summary>
        /// Constructs a new spreadsheet with default settings.
        /// </summary>
        public Spreadsheet()
           : base(s => true, s => s, "default")
        {
            Cellvalue = new Dictionary<string, Cell>();
            DependencyGraph = new DependencyGraph();
            isChanged = false;
        }
        /// <summary>
        /// This constructor creates a new Spreadsheet object with the specified settings.
        /// </summary>
        /// <param name="isValid">A delegate that determines whether the given cell name is valid.</param>
        /// <param name="normalize">A delegate that normalizes cell names.</param>
        /// <param name="version">The version of the Spreadsheet.</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
           : base(isValid, normalize, version)
        {
            // If any of the arguments are null, throw an ArgumentNullException.
            if (isValid == null || normalize == null || version == null)
                throw new ArgumentNullException();
            Cellvalue = new Dictionary<string, Cell>();
            DependencyGraph = new DependencyGraph();
            isChanged = false;
        }

        /// <summary>
        /// Constructs a new Spreadsheet from an XML file. The contents of the file are used to populate the
        /// spreadsheet. All previous contents of the spreadsheet are lost when reading from the file.
        /// </summary>
        /// <param name="path">The path to the XML file.</param>
        /// <param name="isValid">The delegate used to determine whether a given cell name is valid.</param>
        /// <param name="normalize">The delegate used to normalize cell names.</param>
        /// <param name="version">The version of the file to be read.</param>
        /// <exception cref="ArgumentException">The provided file path is null or empty.</exception>
        /// <exception cref="SpreadsheetReadWriteException">The specified file path does not exist or the file version does not match.</exception>
        public Spreadsheet(string path, Func<string, bool> isValid, Func<string, string> normalize, string version) :
           this(isValid, normalize, version)
        {
            Cellvalue = new Dictionary<string, Cell>();
            DependencyGraph = new DependencyGraph();
            isChanged = false;
            string? TempCellname=null;
            string? TempContents=null;
            // Check that the path is not null or empty
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Not allow null file path");
            // Check that the file exists
            if (!File.Exists(path))
                throw new SpreadsheetReadWriteException("The specified file path does not exist");
            // Check that the saved version matches the version parameter
            if (GetSavedVersion(path) != version)
                throw new SpreadsheetReadWriteException("Not same version");
            XmlReader? reader = null;
            try
            {
                using (reader = XmlReader.Create(path))
                {

                    while (reader.Read())
                    {
                        if (reader.Name == "spreadsheet")
                            GetSavedVersion(path);
                        else if (reader.Name == "cell")
                            continue;
                        else if (reader.Name == "name")
                        {
                            TempCellname = reader.ReadElementContentAsString();
                            if (!String.IsNullOrEmpty(TempCellname))
                            {
                                TempContents = reader.ReadElementContentAsString();
                                SetContentsOfCell(TempCellname, TempContents);
                                TempCellname = null;
                                TempContents = null;
                            }

                        }
                    }
                }
            }

            finally
            {
                reader?.Dispose();
            }

        }
        ///<inheritdoc/>
        public override IEnumerable<String> GetNamesOfAllNonemptyCells()
        {
            HashSet<string> CellNameList = new HashSet<string>();
            foreach (string key in Cellvalue.Keys)
            {
                if (!String.IsNullOrEmpty(Cellvalue[key].value.ToString()))
                { CellNameList.Add(key); }
            }

            return CellNameList;
        }

        ///<inheritdoc/>
        public override bool Changed{get => isChanged; protected set => isChanged = value;}

        ///<inheritdoc/>
        public override object GetCellContents(String name)
        {
            TheRightNameOfCell(name);
            return Cellvalue.ContainsKey(Normalize(name)) ? Cellvalue[Normalize(name)].Content : new string("");
        }




        ///<inheritdoc/>
        protected override IList<String> SetCellContents(String name, double number)
        { return AddValue(name, number);}

        ///<inheritdoc/>
        protected override IList<String> SetCellContents(String name, String text)
        {return AddValue(name, text);}
        
        ///<inheritdoc/>
        protected  override IList<String> SetCellContents(String name, Formula formula)
        {
            foreach (string variable in formula.GetVariables())
            {
                if (TheRightNameOfCell(variable) == true)
                    DependencyGraph.AddDependency(variable, name);
            } 
            return AddValue(name, formula);
        }


        ///<inheritdoc/>
        protected override IEnumerable<String> GetDirectDependents(String name)
        {
            TheRightNameOfCell(name);
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException();
            return DependencyGraph.GetDependents(name);
        }

        ///<inheritdoc/>
        public override IList<String> SetContentsOfCell(String name, String content)
        {
            TheRightNameOfCell(name);
            name = Normalize(name);
            double temp;
            isChanged = true;
            if (content == null)
                throw new ArgumentNullException();

            if (content.StartsWith("="))
            {
                return SetCellContents(name, new Formula(content.Substring(1),Normalize,IsValid));
            }
            else if (double.TryParse(content, out temp))
            {
                return SetCellContents(name, temp);
            }
            else
                return SetCellContents(name, content);
                    
        }
        ///<inheritdoc/>
        public override string GetSavedVersion(String filename)
        {
            try
            {
                XmlReader reader = XmlTextReader.Create(filename);
                while (reader.Read())
                {
                    if (reader.Name != null && reader.Name.Equals("spreadsheet"))
                    {
                        if (reader.HasAttributes)
                        {
                            string? version = null;
                            if (reader.MoveToAttribute("version"))
                                version = reader.Value;
                            if (version != null)
                            {
                                reader.Dispose();
                                return version;
                            }
                        }
                         throw new SpreadsheetReadWriteException("Version name doesn't exist");
                    }
                }
                throw new SpreadsheetReadWriteException("Does not have a version name");
            }
            catch (System.Xml.XmlException)
            { throw new SpreadsheetReadWriteException("File does not contain proper XML Format"); }
 
        }

        ///<inheritdoc/>
        public override void Save(String filename)
        {
            if(string.IsNullOrEmpty(filename))
                throw new SpreadsheetReadWriteException("File name cannot null or empty");
            XmlWriter? writer = null;

            try
            {
                using (writer = XmlWriter.Create(filename))
                {
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    foreach (string name in Cellvalue.Keys)
                    {
                        object contents = Cellvalue[name].Content;

                        if (contents is double)
                        {
                            writer.WriteStartElement("cell");
                            writer.WriteElementString("name", name);
                            writer.WriteElementString("contents", contents.ToString());
                            writer.WriteEndElement();
                        }
                        else if (contents is Formula)
                        {
                            writer.WriteStartElement("cell");
                            writer.WriteElementString("name", name);
                            writer.WriteElementString("contents", "=" + contents.ToString());
                            writer.WriteEndElement();
                        }
                        else if (contents is string && !string.IsNullOrEmpty((string)contents))
                        {
                            writer.WriteStartElement("cell");
                            writer.WriteElementString("name", name);
                            writer.WriteElementString("contents", contents.ToString());
                            writer.WriteEndElement();
                        }
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }


            catch (IOException)
            {
                throw new SpreadsheetReadWriteException("Have I/O error while saving the file");
            }
            catch (UnauthorizedAccessException)
            {
                throw new SpreadsheetReadWriteException($"Access to the path '{filename}' is denied.");
            }
            finally 
            {
                writer?.Dispose();
            }

            isChanged = false;
        }

        ///<inheritdoc/>
        public override object GetCellValue(String name)
        {
            if (Cellvalue.ContainsKey(name))
            {
                TheRightNameOfCell(name);
                recalcular(name);
                return Cellvalue[name].value;
            }
            else
                return Cellvalue.ContainsKey(Normalize(name)) ? Cellvalue[Normalize(name)].Content : new string("");
        }




        /// <summary>
        /// This helper method is checking the cell name is not illegal or not null
        /// if not pass will be throw the InvalidNameException
        /// </summary>
        /// <param name="name">The cell name</param>
        /// <returns>Vaild cell name</returns>
        /// <exception cref="InvalidNameException"> throw exception if the name is not valid. </exception>
        private bool TheRightNameOfCell(string name)
        {
            if (Regex.IsMatch(name, @"^[a-zA-Z_]+[0-9_]+$") && name is not null&& IsValid(name))
                return true;
            else
                throw new InvalidNameException();
        }



        /// <summary>
        /// This helper method is for SetCellContents function use to add value to cellvalue dictionary and check have any Circular error
        /// if is formula this method will check the circular error and remove dependency make sure the cell have legal dependency graph
        /// will return GetCellsToRecalculate linkedlist to hashset make sure the all dependecy in the hashset.
        /// </summary>
        /// <param name="name">The cell name</param>
        /// <param name="value">The cell value</param>
        /// <returns>All dependency of cell name</returns>
        private IList<string> AddValue(String name,object value)
        {
        
            Cell cells = new Cell(name, value,lookupdouble);

            if (Cellvalue.ContainsKey(name))
            {
       
                if (Cellvalue[name].Content is Formula)
                {

                    Formula formula = (Formula)Cellvalue[name].Content;
                    foreach (string variable in formula.GetVariables())
                    {
                       DependencyGraph.RemoveDependency(variable, name);
                    }

                }
                Cellvalue[name] = cells;
            }
            else
                Cellvalue.Add(name, cells);

            recalcular(name);
            isChanged = true;
            return GetCellsToRecalculate(name).ToList();
        }

        /// <summary>
        ///This helper method  Recalculates the values of all cells that depend on the specified cell.
        /// The method evaluates all dependent cells' Formula objects, updating the values in the Cellvalue dictionary.
        /// </summary>
        /// <param name="name">The name of the cell to recalculate and all cells that depend on it.</param>
        private void recalcular(string name)
        {
            if (Cellvalue[name].Content is not string)
            {
                List<string> alldees = GetCellsToRecalculate(name).ToList();
                foreach (string dees in alldees)
                {
                    if (Cellvalue[dees].Content is Formula && Cellvalue.ContainsKey(dees))
                    {
                        Formula formula = (Formula)Cellvalue[dees].Content;
                        Cellvalue[dees].value = formula.Evaluate(lookupdouble);
                    }
                }
            }
        }


        /// <summary>
        /// Looks up a cell with the given name and returns its value as a double if it exists and is a double,
        /// otherwise throws an ArgumentException with the message "No double".
        /// </summary>
        /// <param name="name">The name of the cell to look up</param>
        /// <returns>The value of the cell as a double, if it exists and is a double</returns>
        /// <exception cref="ArgumentException">Thrown if the cell with the given name doesn't exist or isn't a double</exception>
        public double lookupdouble(string name)
        {
            if (Cellvalue.ContainsKey(name))
            {
                object value = Cellvalue[name].value;
                if (value is double)
                    return (double)value;
            }
            throw new ArgumentException("No double");
        }
    }
}
