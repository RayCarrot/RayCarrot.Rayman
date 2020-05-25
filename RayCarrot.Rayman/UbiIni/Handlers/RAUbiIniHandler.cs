using System;
using RayCarrot.Common;
using RayCarrot.IO;

namespace RayCarrot.Rayman.UbiIni
{
    /// <summary>
    /// Handles the Rayman Arena section of a ubi.ini file
    /// </summary>
    public class RAUbiIniHandler : RMUbiIniHandler
    {
        #region Constructor

        /// <summary>
        /// Constructor for a custom section name
        /// </summary>
        /// <param name="path">The path of the ubi.ini file</param>
        /// <param name="sectionKey">The name of the section to retrieve</param>
        public RAUbiIniHandler(FileSystemPath path, string sectionKey) : base(path, sectionKey)
        {
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="path">The path of the ubi.ini file</param>
        public RAUbiIniHandler(FileSystemPath path) : base(path, SectionName)
        {

        }

        #endregion

        #region Constant Fields

        /// <summary>
        /// The section name
        /// </summary>
        public new const string SectionName = "Rayman Arena";

        #endregion

        #region Properties

        /// <summary>
        /// The ModemQuality key
        /// </summary>
        public string ModemQuality
        {
            get => Section?["ModemQuality"];
            set => Section["ModemQuality"] = value;
        }

        /// <summary>
        /// The UDPPort key
        /// </summary>
        public string UDPPort
        {
            get => Section?["UDPPort"];
            set => Section["UDPPort"] = value;
        }

        #endregion

        #region Formatted Properties

        /// <summary>
        /// The formatted Language key
        /// </summary>
        public RALanguages? FormattedRALanguage => Enum.TryParse(Language, out RALanguages r2Languages) ? r2Languages.CastTo<RALanguages?>() : null;

        /// <summary>
        /// The formatted ModemQuality key
        /// </summary>
        public int? FormattedModemQuality => Int32.TryParse(ModemQuality, out int result) ? result.CastTo<int?>() : null;

        /// <summary>
        /// The formatted UDPPort key
        /// </summary>
        public int? FormattedUDPPort => Int32.TryParse(UDPPort, out int result) ? result.CastTo<int?>() : null;

        #endregion
    }
}