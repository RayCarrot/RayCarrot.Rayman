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
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        public Ray1Settings(Endian endian, Encoding textEncoding, Ray1Game game, Platform platform) : base(endian, textEncoding)
        {
            Game = game;
            Platform = platform;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The game
        /// </summary>
        public Ray1Game Game { get; }
        
        /// <summary>
        /// The platform
        /// </summary>
        public Platform Platform { get; }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the default settings
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        /// <returns>The settings</returns>
        public static Ray1Settings GetDefaultSettings(Ray1Game game, Platform platform) => new Ray1Settings(Endian.Little, Encoding.GetEncoding(437), game, platform);

        #endregion
    }
}