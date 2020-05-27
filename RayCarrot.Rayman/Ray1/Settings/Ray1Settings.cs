using RayCarrot.Binary;
using System.Text;

namespace RayCarrot.Rayman.Ray1
{
    /// <summary>
    /// Binary settings for serializing Rayman 1 game formats
    /// </summary>
    public class Ray1Settings : BinarySerializerSettings
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="endian">The endianness</param>
        /// <param name="textEncoding">The text encoding to use</param>
        public Ray1Settings(Endian endian, Encoding textEncoding) : base(endian, textEncoding)
        { }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the default settings
        /// </summary>
        /// <returns>The settings</returns>
        public static Ray1Settings GetDefaultSettings() => new Ray1Settings(Endian.Little, Encoding.GetEncoding(437));

        #endregion
    }
}