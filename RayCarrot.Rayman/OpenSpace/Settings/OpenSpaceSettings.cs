using System;
using System.Text;
using RayCarrot.Binary;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// Binary settings for serializing OpenSpace game formats
    /// </summary>
    public class OpenSpaceSettings : BinarySerializerSettings
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="endian">The endianness</param>
        /// <param name="textEncoding">The text encoding to use</param>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        public OpenSpaceSettings(Endian endian, Encoding textEncoding, OpenSpaceGame game, Platform platform) : base(endian, textEncoding)
        {
            // Set the properties
            Game = game;
            Platform = platform;

            // Get the engine version
            foreach (var version in EnumHelpers.GetValues<OpenSpaceEngineVersion>())
            {
                if ((int) game > (int) version) 
                    continue;

                EngineVersion = version;
                break;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The game
        /// </summary>
        public OpenSpaceGame Game { get; }

        /// <summary>
        /// The engine version
        /// </summary>
        public OpenSpaceEngineVersion EngineVersion { get; }

        /// <summary>
        /// The platform
        /// </summary>
        public Platform Platform { get; }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the default settings based on the game and platform
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        /// <returns>The settings</returns>
        public static OpenSpaceSettings GetDefaultSettings(OpenSpaceGame game, Platform platform)
        {
            Endian endian;

            // Set the byte order based on platform
            switch (platform)
            {
                case Platform.PC:
                case Platform.iOS:
                case Platform.DreamCast:
                case Platform.PlayStation2:
                case Platform.NintendoDS:
                case Platform.Nintendo3DS:
                    endian = Endian.Little;
                    break;

                case Platform.GameCube:
                case Platform.Nintendo64:
                    endian = Endian.Big;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }

            return new OpenSpaceSettings(endian, Encoding.GetEncoding(1252), game, platform);
        }

        #endregion
    }
}