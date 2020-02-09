using System;
using System.Text;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Settings for serializing OpenSpace game formats
    /// </summary>
    public class OpenSpaceSettings : BinarySerializerSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        public OpenSpaceSettings(OpenSpaceGame game, OpenSpacePlatform platform)
        {
            // Set base properties
            Encoding = Encoding.UTF8;
            StringEncoding = BinaryStringEncoding.LengthPrefixed;
            BoolEncoding = BinaryBoolEncoding.Int32;

            // Set the properties
            Game = game;
            Platform = platform;
            DeserializeMipmaps = true;

            // Set the byte order based on platform
            switch (platform)
            {
                case OpenSpacePlatform.PC:
                case OpenSpacePlatform.iOS:
                case OpenSpacePlatform.DreamCast:
                case OpenSpacePlatform.PlayStation2:
                case OpenSpacePlatform.NintendoDS:
                case OpenSpacePlatform.Nintendo3DS:
                    ByteOrder = ByteOrder.LittleEndian;
                    break;

                case OpenSpacePlatform.GameCube:
                case OpenSpacePlatform.Nintendo64:
                    ByteOrder = ByteOrder.BigEndian;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }

            // Get the engine version
            foreach (var version in EnumHelpers.GetValues<OpenSpaceEngineVersion>())
            {
                if ((int) game > (int) version) 
                    continue;

                EngineVersion = version;
                break;
            }
        }

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
        public OpenSpacePlatform Platform { get; }

        /// <summary>
        /// Indicates if mipmaps should be deserialized (if available). Setting this to false will improve deserializing performance for certain games, but will not allow the deserialized file to be serialized without new mipmaps being generated
        /// </summary>
        public bool DeserializeMipmaps { get; set; }
    }
}