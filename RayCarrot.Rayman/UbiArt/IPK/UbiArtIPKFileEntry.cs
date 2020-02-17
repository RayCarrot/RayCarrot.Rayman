﻿using Ionic.Zlib;
using SevenZip.Compression.LZMA;
using System;
using System.IO;
using System.Linq;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The data used for a file entry within a .ipk file
    /// </summary>
    public class UbiArtIPKFileEntry : IBinarySerializable<UbiArtSettings>
    {
        #region Public Properties

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
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            // 3DS files can not be serialized separately
            if (reader.SerializerSettings.IPKVersion == 4)
                throw new BinarySerializableException("File data for IPK 4 can not be serialized separately");

            // Read common values
            OffsetCount = reader.Read<uint>();
            Size = reader.Read<uint>();
            CompressedSize = reader.Read<uint>();
            TStamp = reader.Read<ulong>();
            Offsets = new ulong[OffsetCount];

            for (int i = 0; i < OffsetCount; i++)
                Offsets[i] = reader.Read<ulong>();

            // For any game after Origins the path is in the standard format
            if (reader.SerializerSettings.IPKVersion >= 5)
                Path = reader.Read<UbiArtPath>();
            else
                Path = new UbiArtPath(reader.Read<string>());
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            // 3DS files can not be serialized separately
            if (writer.SerializerSettings.IPKVersion == 4)
                throw new BinarySerializableException("File data for IPK 4 can not be serialized separately");

            // Write common values
            writer.Write(OffsetCount);
            writer.Write(Size);
            writer.Write(CompressedSize);
            writer.Write(TStamp);

            // Write each offset
            foreach (var offset in Offsets)
                writer.Write(offset);

            // For any game after Origins the path is in the standard format
            if (writer.SerializerSettings.IPKVersion >= 5)
                writer.Write(Path);
            else
                writer.Write(Path.FullPath);
        }

        #endregion
    }
}