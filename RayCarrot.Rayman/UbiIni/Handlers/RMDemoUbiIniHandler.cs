using RayCarrot.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Handles the Rayman M demo section of a ubi.ini file
    /// </summary>
    public class RMDemoUbiIniHandler : RMUbiIniHandler
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="path">The path of the ubi.ini file</param>
        public RMDemoUbiIniHandler(FileSystemPath path) : base(path, "Rayman M Nestle Demo")
        {

        }
    }
}