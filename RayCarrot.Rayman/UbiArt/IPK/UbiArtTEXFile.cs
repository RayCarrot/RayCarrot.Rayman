using RayCarrot.Binary;
using RayCarrot.CarrotFramework.Abstractions;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// Data for a UbiArt TEX file
    /// </summary>
    public class UbiArtTEXFile : IBinarySerializable
    {
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
        /// The size of the actual texture data, in bytes. This is always 0 on PS Vita. On Wii U this does not match the actual size.
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
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Version = s.Serialize<uint>(Version, name: nameof(Version));
            Signature = s.Serialize<uint>(Signature, name: nameof(Signature));

            if (Signature != 0x54455800)
                throw new BinarySerializableException("The file is not a valid UbiArt texture");

            HdrSize = s.Serialize<uint>(HdrSize, name: nameof(HdrSize));

            TextureSize = s.Serialize<uint>(TextureSize, name: nameof(TextureSize));
            Width = s.Serialize<ushort>(Width, name: nameof(Width));
            Height = s.Serialize<ushort>(Height, name: nameof(Height));
            UnknownX = s.Serialize<ushort>(UnknownX, name: nameof(UnknownX));
            UnknownY = s.Serialize<ushort>(UnknownY, name: nameof(UnknownY));

            if (Version == 16 || Version == 17)
                Unknown6 = s.Serialize<uint>(Unknown6, name: nameof(Unknown6));

            TextureSize2 = s.Serialize<uint>(TextureSize2, name: nameof(TextureSize2));

            Unknown0 = s.Serialize<uint>(Unknown0, name: nameof(Unknown0));
            Unknown1 = s.Serialize<uint>(Unknown1, name: nameof(Unknown1));
            Unknown2 = s.Serialize<uint>(Unknown2, name: nameof(Unknown2));

            if (Version > 10)
                Unknown3 = s.Serialize<uint>(Unknown3, name: nameof(Unknown3));

            Unknown4 = s.Serialize<uint>(Unknown4, name: nameof(Unknown4));

            if (Version > 10)
                Unknown5 = s.Serialize<uint>(Unknown5, name: nameof(Unknown5));

            TextureData = s.SerializeArray<byte>(TextureData, (int)(s.Stream.Length - s.Stream.Position), name: nameof(TextureData));

            if (TextureData.Length != TextureSize && TextureSize != 0)
                RCFCore.Logger?.LogDebugSource($"The TEX file length {TextureData.Length} doesn't match the set size of {TextureSize} and {TextureSize2}");
        }

        #endregion
    }
}