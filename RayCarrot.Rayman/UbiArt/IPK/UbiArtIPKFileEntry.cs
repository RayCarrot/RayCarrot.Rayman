using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The data used for a file entry within a .ipk file
    /// </summary>
    public class UbiArtIPKFileEntry : IBinarySerializable
    {
        #region Public Properties

        /// <summary>
        /// The .ipk version the entry belongs to
        /// </summary>
        public uint IPKVersion { get; set; }

        /// <summary>
        /// The number of offsets
        /// </summary>
        public uint OffsetCount { get; set; }

        /// <summary>
        /// The size of the uncompressed file in bytes
        /// </summary>
        public uint Size { get; set; }

        /// <summary>
        /// The size of the compressed file in bytes
        /// </summary>
        public uint CompressedSize { get; set; }

        public ulong TStamp { get; set; }

        /// <summary>
        /// The file offsets
        /// </summary>
        public ulong[] Offsets { get; set; }

        /// <summary>
        /// The file path
        /// </summary>
        public UbiArtPath Path { get; set; }

        /// <summary>
        /// Indicates if the file is compressed
        /// </summary>
        public bool IsCompressed => CompressedSize != 0;

        /// <summary>
        /// The size of the file within the archive
        /// </summary>
        public uint ArchiveSize => IsCompressed ? CompressedSize : Size;

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            // 3DS files can not be serialized separately
            if (IPKVersion == 4)
                throw new BinarySerializableException("File data for IPK 4 can not be serialized separately");

            // Read common values
            OffsetCount = s.Serialize<uint>(OffsetCount, name: nameof(OffsetCount));
            Size = s.Serialize<uint>(Size, name: nameof(Size));
            CompressedSize = s.Serialize<uint>(CompressedSize, name: nameof(CompressedSize));
            TStamp = s.Serialize<ulong>(TStamp, name: nameof(TStamp));

            Offsets = s.SerializeArray<ulong>(Offsets, (int)OffsetCount, name: nameof(Offsets));

            // For any game after Origins the path is in the standard format
            if (IPKVersion >= 5)
                Path = s.SerializeObject<UbiArtPath>(Path, name: nameof(Path));
            else
                Path = new UbiArtPath(s.SerializeLengthPrefixedString(Path?.FullPath, name: nameof(Path)));
        }

        #endregion
    }
}