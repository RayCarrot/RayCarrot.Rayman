namespace RayCarrot.Rayman
{
    /// <summary>
    /// Settings for serializing UbiArt game formats
    /// </summary>
    public class UbiArtSettings : IBinarySerializableSettings
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
            // Set the properties
            Game = game;
            Platform = platform;
            DeserializeMipmaps = true;
            ByteOrder = byteOrder;
            TextEncoding = textEncoding;
        }

        /// <summary>
        /// The current IPK version. This value gets set automatically when deserializing the IPK and is used for deserializing the IPK contents.
        /// </summary>
        public uint IPKVersion { get; set; }

        /// <summary>
        /// The byte order to use
        /// </summary>
        public ByteOrder ByteOrder { get; }

        /// <summary>
        /// The text encoding to use
        /// </summary>
        public TextEncoding TextEncoding { get; }

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