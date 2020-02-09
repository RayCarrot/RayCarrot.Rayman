using Ionic.Zlib;
using RayCarrot.Extensions;
using RayCarrot.IO;
using SevenZip.Compression.LZMA;
using System;
using System.Collections.Generic;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The data used for a file within the .ipk files for UbiArt games
    /// </summary>
    public class UbiArtIPKFile : IBinarySerializable<UbiArtSettings>
    {
        #region Public Properties

        public int Type { get; set; }

        /// <summary>
        /// The size of the uncompressed file in bytes
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// The size of the compressed file in bytes
        /// </summary>
        public int CompressedSize { get; set; }

        public long TStamp { get; set; }

        /// <summary>
        /// The file offset
        /// </summary>
        public long Offset { get; set; }

        /// <summary>
        /// Unknown values
        /// </summary>
        public byte[] Unknown0 { get; set; }

        /// <summary>
        /// The full path including the directory path and file name
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// The directory path, not including the file name
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// The file name, including the full file extension
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Unknown values
        /// </summary>
        public byte[] Unknown1 { get; set; }

        /// <summary>
        /// Indicates if the file is compressed
        /// </summary>
        public bool IsCompressed => CompressedSize != 0;

        /// <summary>
        /// The file name, without any of the file extensions
        /// </summary>
        public string FileNameWithoutExtension => FileName.Remove(GetFileExtensions());

        /// <summary>
        /// The size of the file within the archive
        /// </summary>
        public int ArchiveSize => IsCompressed ? CompressedSize : Size;

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the file extensions used for the file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetFileExtensions() => new FileSystemPath(FileName).FileExtensions;

        /// <summary>
        /// Gets the TEX file data for the file
        /// </summary>
        /// <param name="ipkFileStream">The stream to get the file bytes from</param>
        /// <param name="baseOffset">The file base offset</param>
        /// <param name="serializerSettings">The serializer setting to use</param>
        /// <returns>The texture</returns>
        public UbiArtTEXFile GetTEXFile(Stream ipkFileStream, uint baseOffset, UbiArtSettings serializerSettings)
        {
            // Get the decompressed bytes and load them into a memory stream
            using var stream = new MemoryStream(GetFileBytes(ipkFileStream, baseOffset, serializerSettings));

            // Return the data
            return GetTEXFile(stream, serializerSettings);
        }

        /// <summary>
        /// Gets the TEX file data for the file
        /// </summary>
        /// <param name="fileStream">The stream to get the file bytes from</param>
        /// <param name="serializerSettings">The serializer setting to use</param>
        /// <returns>The texture</returns>
        public UbiArtTEXFile GetTEXFile(Stream fileStream, UbiArtSettings serializerSettings)
        {
            // Serialize the data
            var data = UbiArtTEXFile.GetSerializer(serializerSettings).Deserialize(fileStream);

            // Return the data
            return data;
        }

        /// <summary>
        /// Gets the file bytes for the IPK file item from the stream
        /// </summary>
        /// <param name="fileStream">The stream to get the file bytes from</param>
        /// <param name="baseOffset">The file base offset</param>
        /// <param name="serializerSettings">The serializer setting to use</param>
        /// <param name="decompress">Indicates if the bytes should be decompressed if they're compressed</param>
        /// <returns>The file bytes</returns>
        public byte[] GetFileBytes(Stream fileStream, uint baseOffset, UbiArtSettings serializerSettings, bool decompress = true)
        {
            // Set the position
            fileStream.Position = Offset + baseOffset;

            // Create the buffer
            byte[] buffer = new byte[ArchiveSize];

            // Read the bytes into the buffer
            fileStream.Read(buffer, 0, buffer.Length);

            // Return the bytes if they should not be decompressed
            if (!decompress || !IsCompressed) 
                return buffer;

            // Use LZMA
            if (serializerSettings.IPKVersion >= 8)
            {
                // Decompress the bytes
                return SevenZipHelper.Decompress(buffer, Size);
            }
            // Use ZLib
            else
            {
                // Decompress the bytes
                return ZlibStream.UncompressBuffer(buffer);
            }
        }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            // 3DS files can not be serialized separately
            if (reader.SerializerSettings.IPKVersion == 4)
                throw new Exception("File data for IPK 4 can not be serialized separately");

            // Read common values
            Type = reader.Read<int>();
            Size = reader.Read<int>();
            CompressedSize = reader.Read<int>();
            TStamp = reader.Read<long>();
            Offset = reader.Read<long>();

            // Read unknown values for type 2
            if (Type == 2)
                Unknown0 = reader.ReadBytes(8);

            // For 5 and above the file and directory paths are separate
            if (reader.SerializerSettings.IPKVersion >= 5)
            {
                // Just Dance reads the values in reverse
                if (reader.SerializerSettings.Game == UbiArtGame.JustDance2017)
                {
                    // Read the path
                    FileName = reader.Read<string>();
                    DirectoryPath = reader.Read<string>();
                }
                else
                {
                    // Read the path
                    DirectoryPath = reader.Read<string>();
                    FileName = reader.Read<string>();
                }

                // Get the full path
                FullPath = Path.Combine(DirectoryPath, FileName);

                // Read unknown values
                Unknown1 = reader.ReadBytes(8);
            }
            else
            {
                // Read the path
                FullPath = reader.Read<string>();

                // Get the separate paths
                FileSystemPath path = FullPath;
                DirectoryPath = path.Parent.FullPath.
                    // Fix the separator character
                    Replace('\\', '/');
                FileName = path.Name;
            }
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            // 3DS files can not be serialized separately
            if (writer.SerializerSettings.IPKVersion == 4)
                throw new Exception("File data for IPK 4 can not be serialized separately");

            // Write common values
            writer.Write(Type);
            writer.Write(Size);
            writer.Write(CompressedSize);
            writer.Write(TStamp);
            writer.Write(Offset);

            // Write unknown values for type 2
            if (Type == 2)
                writer.Write(Unknown0);

            // For 5 and above the file and directory paths are separate
            if (writer.SerializerSettings.IPKVersion >= 5)
            {
                // Just Dance reads the values in reverse
                if (writer.SerializerSettings.Game == UbiArtGame.JustDance2017)
                {
                    // Write the path
                    writer.Write(FileName);
                    writer.Write(DirectoryPath);
                }
                else
                {
                    // Write the path
                    writer.Write(DirectoryPath);
                    writer.Write(FileName);
                }

                // Write unknown values
                writer.Write(Unknown1);
            }
            else
            {
                // Write the path
                writer.Write(FullPath);
            }
        }

        #endregion
    }
}