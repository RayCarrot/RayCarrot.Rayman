using RayCarrot.Common;
using System;
using System.IO;
using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// The archive data used for the .cnt files from OpenSpace games
    /// </summary>
    public class OpenSpaceCntData : IBinarySerializableArchive<OpenSpaceCntFileEntry>
    {
        #region Public Properties

        /// <summary>
        /// Indicates if the XOR keys should be used
        /// </summary>
        public bool IsXORUsed { get; set; }

        /// <summary>
        /// Indicates if the checksum is used
        /// </summary>
        public bool IsChecksumUsed { get; set; }

        /// <summary>
        /// The XOR key used to get the directory and file names
        /// </summary>
        public byte XORKey { get; set; }

        /// <summary>
        /// The available directories
        /// </summary>
        public string[] Directories { get; set; }

        /// <summary>
        /// The checksum for the directories
        /// </summary>
        public byte DirChecksum { get; set; }

        /// <summary>
        /// The available files
        /// </summary>
        public OpenSpaceCntFileEntry[] Files { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes every listed file entry based on its offset to the file, getting the contents from the generator
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="fileGenerator">The file generator</param>
        public void WriteArchiveContent(Stream stream, IArchiveFileGenerator<OpenSpaceCntFileEntry> fileGenerator)
        {
            // Make sure we have a generator for each file
            if (fileGenerator.Count != Files.Length)
                throw new BinarySerializableException("The .cnt file can't be serialized without a file generator for each file");

            // Write the file contents
            foreach (var file in Files)
            {
                // Get the bytes
                var bytes = fileGenerator.GetBytes(file);

                // Set the position to the pointer
                stream.Position = file.Pointer;

                // Write the contents from the generator
                stream.Write(bytes);
            }
        }

        /// <summary>
        /// Gets a generator for the archive content
        /// </summary>
        /// <param name="stream">The archive stream</param>
        /// <returns>The generator</returns>
        public IArchiveFileGenerator<OpenSpaceCntFileEntry> GetArchiveContent(Stream stream)
        {
            return new CNTFileGenerator(stream);
        }

        /// <summary>
        /// Gets the size of the header information in the archive
        /// </summary>
        /// <param name="settings">The serializer settings</param>
        /// <returns>The size in bytes</returns>
        public int GetHeaderSize(OpenSpaceSettings settings)
        {
            // Create a temporary memory stream to determine the size
            using var stream = new MemoryStream();

            // Serialize the header only
            BinarySerializableHelpers.WriteToStream(this, stream, settings);

            // Get the position, which will be the size of the header
            return (int)stream.Position;
        }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            // Serialize the directory and file array sizes
            Directories = s.SerializeArraySize<string, int>(Directories, name: nameof(Directories));
            Files = s.SerializeArraySize<OpenSpaceCntFileEntry, int>(Files, name: nameof(Files));

            // Serialize header info
            IsXORUsed = s.Serialize<bool>(IsXORUsed, name: nameof(IsXORUsed));
            IsChecksumUsed = s.Serialize<bool>(IsChecksumUsed, name: nameof(IsChecksumUsed));
            XORKey = s.Serialize<byte>(XORKey, name: nameof(XORKey));

            // Serialize the directory paths
            for (int i = 0; i < Directories.Length; i++)
                Directories[i] = s.SerializeOpenSpaceEncryptedString(Directories[i], IsXORUsed ? XORKey : (byte)0, name: $"{nameof(Directories)}[{i}]");

            // Serialize the directory checksum
            DirChecksum = s.Serialize<byte>(DirChecksum, name: nameof(DirChecksum));

            // Serialize the file info
            Files = s.SerializeObjectArray<OpenSpaceCntFileEntry>(Files, Files.Length, (s, o) => o.XORKey = IsXORUsed ? XORKey : (byte)0, name: nameof(Files));
        }

        #endregion

        #region Classes

        /// <summary>
        /// The archive file generator for .cnt files
        /// </summary>
        protected class CNTFileGenerator : IArchiveFileGenerator<OpenSpaceCntFileEntry>
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="archiveStream">The archive file stream</param>
            public CNTFileGenerator(Stream archiveStream)
            {
                // Get the stream
                Stream = archiveStream;
            }

            /// <summary>
            /// The stream
            /// </summary>
            protected Stream Stream { get; }

            /// <summary>
            /// Gets the number of files which can be retrieved from the generator
            /// </summary>
            public int Count => throw new InvalidOperationException("The count can not be retrieved for this generator");

            /// <summary>
            /// Gets the bytes for the specified key
            /// </summary>
            /// <param name="fileEntry">The file entry to get the bytes for</param>
            /// <returns>The bytes</returns>
            public byte[] GetBytes(OpenSpaceCntFileEntry fileEntry)
            {
                // Set the position
                Stream.Position = fileEntry.Pointer;

                // Create the buffer
                byte[] buffer = new byte[fileEntry.Size];

                // Read the bytes into the buffer
                Stream.Read(buffer, 0, buffer.Length);

                // Return the buffer
                return buffer;
            }

            /// <summary>
            /// Disposes the generator
            /// </summary>
            public void Dispose()
            { }
        }

        #endregion
    }
}