using RayCarrot.Binary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RayCarrot.Rayman.Ray1
{
    /// <summary>
    /// The archive data used for the .dat files from the Rayman 1 PC spin-offs
    /// </summary>
    public class Rayman1PCArchiveData : Rayman1PCBaseFile, IBinarySerializableArchive<Rayman1PCArchiveEntry>
    {
        #region Public Properties

        /// <summary>
        /// The available files
        /// </summary>
        public Rayman1PCArchiveEntry[] Files { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes every listed file entry based on its offset to the file, getting the contents from the generator
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="fileGenerator">The file generator</param>
        public void WriteArchiveContent(Stream stream, IArchiveFileGenerator<Rayman1PCArchiveEntry> fileGenerator)
        {
            // Make sure we have a generator for each file
            if (fileGenerator.Count != Files.Length)
                throw new BinarySerializableException("The .dat file can't be serialized without a file generator for each file");

            // Write the file contents
            foreach (var file in Files)
            {
                // Get the file stream
                using var fileStream = fileGenerator.GetFileStream(file);

                // Set the position to the pointer
                stream.Position = file.FileOffset;

                // Write the contents from the generator
                fileStream.CopyTo(stream);
            }
        }

        /// <summary>
        /// Gets a generator for the archive content
        /// </summary>
        /// <param name="stream">The archive stream</param>
        /// <returns>The generator</returns>
        public IArchiveFileGenerator<Rayman1PCArchiveEntry> GetArchiveContent(Stream stream)
        {
            return new Rayman1PCArchiveGenerator(stream);
        }

        /// <summary>
        /// Gets the size of the header information in the archive
        /// </summary>
        /// <param name="settings">The serializer settings</param>
        /// <returns>The size in bytes</returns>
        public uint GetHeaderSize(Ray1Settings settings)
        {
            // Create a temporary memory stream to determine the size
            using var stream = new MemoryStream();

            // Serialize the header only
            BinarySerializableHelpers.WriteToStream(this, stream, settings);

            // Get the position, which will be the size of the header
            return (uint)stream.Position;
        }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public override void Serialize(IBinarySerializer s)
        {
            // Serialize header
            base.Serialize(s);

            // Serialize the file table
            if (s.IsReading)
            {
                var tempList = new List<Rayman1PCArchiveEntry>();

                while (!tempList.Any() || tempList.Last().FileName != "ENDFILE")
                    tempList.Add(s.SerializeObject<Rayman1PCArchiveEntry>(null, name: $"{nameof(Files)}[{tempList.Count}]"));

                Files = tempList.Take(tempList.Count - 1).ToArray();
            }
            else
            {
                s.SerializeObjectArray<Rayman1PCArchiveEntry>(Files, Files.Length, name: nameof(Files));
                s.SerializeObject<Rayman1PCArchiveEntry>(new Rayman1PCArchiveEntry()
                {
                    FileName = "ENDFILE"
                }, name: $"ENDFILE");
            }
        }

        #endregion

        #region Classes

        /// <summary>
        /// The archive file generator for .cnt files
        /// </summary>
        protected class Rayman1PCArchiveGenerator : IArchiveFileGenerator<Rayman1PCArchiveEntry>
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="archiveStream">The archive file stream</param>
            public Rayman1PCArchiveGenerator(Stream archiveStream)
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
            /// Gets the file stream for the specified key
            /// </summary>
            /// <param name="fileEntry">The file entry to get the stream for</param>
            /// <returns>The stream</returns>
            public Stream GetFileStream(Rayman1PCArchiveEntry fileEntry)
            {
                // Set the position
                Stream.Position = fileEntry.FileOffset;

                // Create the buffer
                byte[] buffer = new byte[fileEntry.FileSize];

                // Read the bytes into the buffer
                Stream.Read(buffer, 0, buffer.Length);

                // Return the buffer
                return new MemoryStream(buffer);
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