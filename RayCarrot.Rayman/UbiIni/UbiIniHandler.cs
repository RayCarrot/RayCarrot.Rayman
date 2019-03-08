using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Parser;
using RayCarrot.CarrotFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The base class to inherit from for handling a ubi.ini file
    /// </summary>
    public abstract class UbiIniHandler
    {
        #region Default Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="path">The path of the ubi.ini file</param>
        /// <param name="sectionName">The name of the section to retrieve, usually the name of the game</param>
        protected UbiIniHandler(FileSystemPath path, string sectionName)
        {
            // Save the path
            Path = path;

            // Save the section name
            SectionName = sectionName;

            // Make sure the file exists
            if (!path.FileExists)
                throw new FileNotFoundException($"The file {path} could not be found");

            // Save the installed products as they are not formatted following
            // the .ini file standard and will be lost during the parsing
            using (var sr = new StreamReader(path))
            {
                // Temporarily save the retrieved products
                List<string> products = new List<string>();

                // Store the current line
                string line;

                // Save if inside the correct section
                bool inSection = false;

                // Go through each line
                while ((line = sr.ReadLine()) != null)
                {
                    // Exit the loop if it's the end of the section 
                    if (inSection && (line.IsNullOrWhiteSpace() || line.Contains('[')))
                        break;

                    // Save the product if in the section
                    else if (inSection)
                        products.Add(line);

                    // Check if in the section
                    else if (line == InstalledProductsSection)
                        inSection = true;
                }

                // Save the retrieved products
                InstalledProducts = products.ToArray();
            }

            // Get the ini data
            Data = new FileIniDataParser(new UbiIniDataParser()).ReadFile(path);
        }

        #endregion

        #region Private Properties

        /// <summary>
        /// The data
        /// </summary>
        private IniData Data { get; }

        /// <summary>
        /// The path of the ubi.ini file
        /// </summary>
        private FileSystemPath Path { get; }

        /// <summary>
        /// The installed product keys to save
        /// since they're formatted incorrectly
        /// </summary>
        private string[] InstalledProducts { get; }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The section to retrieve keys from
        /// </summary>
        protected KeyDataCollection Section => Data.Sections[SectionName];

        protected const string InstalledProductsSection = "[INSTALLED PRODUCTS]";

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the currently handled section
        /// </summary>
        public string SectionName { get; }

        /// <summary>
        /// True if the section exists
        /// </summary>
        public bool Exists => Section != null;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the <see cref="KeyDataCollection"/> for this handler's section
        /// </summary>
        /// <returns>The key data collection for the section</returns>
        public KeyDataCollection GetSectionData()
        {
            return Section;
        }

        /// <summary>
        /// Recreates the section and removes all of its keys
        /// </summary>
        public void ReCreate()
        {
            if (Exists)
                Data.Sections.RemoveSection(SectionName);
            Data.Sections.AddSection(SectionName);
        }

        /// <summary>
        /// Saves the file with the modified <see cref="KeyDataCollection"/> in the <see cref="Section"/>
        /// </summary>
        public void Save()
        {
            // Create the file stream
            using (FileStream fs = File.Open(Path, FileMode.Create, FileAccess.Write))
            {
                // Create the stream writer
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    // Get the ini data
                    string iniData = Data.ToString();

                    // Add installed products if any are saved
                    if (InstalledProducts.Length > 0)
                    {
                        // Create a string builder
                        StringBuilder data = new StringBuilder(iniData);

                        // Get the index
                        int index = iniData.IndexOf(InstalledProductsSection, StringComparison.Ordinal) + InstalledProductsSection.Length;

                        // Insert the products
                        data.Insert(index, Environment.NewLine + InstalledProducts.JoinItems(Environment.NewLine));

                        // Set the new data
                        iniData = data.ToString();
                    }

                    // Write the data
                    sr.Write(iniData);
                }
            }
        }

        #endregion
    }

    public class UbiIniDataParser : IniDataParser
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public UbiIniDataParser() : base(new IniParserConfiguration()
        {
            SkipInvalidLines = true
        })
        { }

        #endregion
    }
}