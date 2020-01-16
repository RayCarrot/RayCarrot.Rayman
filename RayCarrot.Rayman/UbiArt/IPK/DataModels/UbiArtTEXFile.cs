namespace RayCarrot.Rayman
{
    /// <summary>
    /// Data for a UbiArt TEX file
    /// </summary>
    public class UbiArtTEXFile : IBinarySerializable
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        public UbiArtTEXFile(UbiArtSettings settings)
        {
            Settings = settings;
            IPKVersion = settings.IPKVersion;
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The settings when serializing the data
        /// </summary>
        protected UbiArtSettings Settings { get; }

        /// <summary>
        /// The IPK archive version
        /// </summary>
        protected uint IPKVersion { get; }

        #endregion

        #region Public Properties

        /// <summary>
        /// The texture version
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// The file signature
        /// </summary>
        public uint Signature;

        /// <summary>
        /// The HDR size
        /// </summary>
        public uint HdrSize { get; set; }

        /// <summary>
        /// The size of the actual texture data, in bytes. This is always 0 on PS Vita.
        /// </summary>
        public uint TextureSize { get; set; }

        /// <summary>
        /// The texture width
        /// </summary>
        public ushort Width { get; set; }

        /// <summary>
        /// The texture height
        /// </summary>
        public ushort Height { get; set; }

        public ushort UnknownX { get; set; }

        public ushort UnknownY { get; set; }

        public uint Unknown6 { get; set; }

        /// <summary>
        /// Same as <see cref="TextureSize"/>
        /// </summary>
        public uint TextureSize2 { get; set; }

        public uint Unknown0 { get; set; }

        public uint Unknown1 { get; set; }

        public uint Unknown2 { get; set; }

        public uint Unknown3 { get; set; }

        public uint Unknown4 { get; set; }

        public uint Unknown5 { get; set; }

        /// <summary>
        /// The texture data
        /// </summary>
        public byte[] TextureData { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            Version = reader.Read<uint>();
            Signature = reader.Read<uint>();

            if (Signature != 0x54455800)
                throw new BinarySerializableException("The file is not a valid UbiArt texture");

            HdrSize = reader.Read<uint>();

            TextureSize = reader.Read<uint>();
            Width = reader.Read<ushort>();
            Height = reader.Read<ushort>();
            UnknownX = reader.Read<ushort>();
            UnknownY = reader.Read<ushort>();

            if (Version == 16 || Version == 17)
                Unknown6 = reader.Read<uint>();

            TextureSize2 = reader.Read<uint>();

            Unknown0 = reader.Read<uint>();
            Unknown1 = reader.Read<uint>();
            Unknown2 = reader.Read<uint>();

            if (Version > 10)
                Unknown3 = reader.Read<uint>();

            Unknown4 = reader.Read<uint>();

            if (Version > 10)
                Unknown5 = reader.Read<uint>();

            TextureData = reader.ReadRemainingBytes();
        }

        public void Serialize(BinaryDataWriter writer)
        {
            if (Settings.Game != UbiArtGame.RaymanOrigins)
            {
                writer.Write(Version);
                writer.Write(Signature);

                writer.Write(HdrSize);

                writer.Write(TextureSize);
                writer.Write(Width);
                writer.Write(Height);
                writer.Write(UnknownX);
                writer.Write(UnknownY);

                if (Version == 16 || Version == 17)
                    writer.Write(Unknown6);

                writer.Write(TextureSize2);
                writer.Write(Unknown0);
                writer.Write(Unknown1);
                writer.Write(Unknown2);

                if (Version > 10)
                    writer.Write(Unknown3);

                writer.Write(Unknown4);

                if (Version > 10) 
                    writer.Write(Unknown5);
            }

            writer.Write(TextureData);
        }

        #endregion
    }
}