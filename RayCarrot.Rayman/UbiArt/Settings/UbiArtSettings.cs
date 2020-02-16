namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// Settings for serializing UbiArt game formats
    /// </summary>
    public class UbiArtSettings : BinarySerializerSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        /// <param name="byteOrder">The byte order to use</param>
        /// <param name="textEncoding">The text encoding to use</param>
        public UbiArtSettings(UbiArtGame game, UbiArtPlatform platform, ByteOrder byteOrder, TextEncoding textEncoding)
        {
            // Set base properties
            ByteOrder = byteOrder;
            Encoding = textEncoding.GetEncoding();
            StringEncoding = BinaryStringEncoding.LengthPrefixed;
            BoolEncoding = BinaryBoolEncoding.Int32;

            // Set properties
            Game = game;
            Platform = platform;
            DeserializeMipmaps = true;
        }

        /// <summary>
        /// The current IPK version. This value gets set automatically when deserializing the IPK and is used for deserializing the IPK contents.
        /// </summary>
        public uint IPKVersion { get; set; }

        /// <summary>
        /// The game
        /// </summary>
        public UbiArtGame Game { get; }

        /// <summary>
        /// The platform
        /// </summary>
        public UbiArtPlatform Platform { get; }

        /// <summary>
        /// Indicates if mipmaps should be deserialized (if available). Setting this to false will improve deserializing performance for certain games, but will not allow the deserialized file to be serialized without new mipmaps being generated
        /// </summary>
        public bool DeserializeMipmaps { get; set; }
    }
}