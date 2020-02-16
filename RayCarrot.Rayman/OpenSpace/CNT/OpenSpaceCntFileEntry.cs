﻿using System;
using System.IO;
using System.Linq;
using RayCarrot.CarrotFramework.Abstractions;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// The data used for a file entry within a .cnt file
    /// </summary>
    public class OpenSpaceCntFileEntry : IBinarySerializable<OpenSpaceSettings>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="xorKey">The XOR key</param>
        public OpenSpaceCntFileEntry(byte xorKey)
        {
            XORKey = xorKey;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The index of the file in the directory
        /// </summary>
        public int DirectoryIndex { get; set; }

        /// <summary>
        /// The file name, including the extension
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The XOR key
        /// </summary>
        public byte XORKey { get; }

        /// <summary>
        /// The XOR key for the file
        /// </summary>
        public byte[] FileXORKey { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public int Unknown1 { get; set; }

        /// <summary>
        /// The file data pointer
        /// </summary>
        public int Pointer { get; set; }

        /// <summary>
        /// The file data size, in bytes
        /// </summary>
        public int Size { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the full path for the file from the list of available directories
        /// </summary>
        /// <param name="directories">The directories</param>
        /// <returns>The full path of the file</returns>
        public string GetFullPath(string[] directories)
        {
            return Path.Combine(DirectoryIndex == -1 ? String.Empty : directories[DirectoryIndex], FileName);
        }

        /// <summary>
        /// Gets the file content for the CNT file item from the stream
        /// </summary>
        /// <param name="cntStream">The stream to get the file content from</param>
        /// <param name="settings">The settings when serializing the data</param>
        /// <returns>The file content</returns>
        public OpenSpaceGFFile GetFileContent(Stream cntStream, OpenSpaceSettings settings)
        {
            // Get the bytes
            var bytes = GetFileBytes(cntStream);

            // Return the content
            return GetFileContent(bytes, settings);
        }

        /// <summary>
        /// Gets the file content for the CNT file item from the file bytes
        /// </summary>
        /// <param name="fileBytes">The bytes to get the file content from</param>
        /// <param name="settings">The settings when serializing the data</param>
        /// <returns>The file content</returns>
        public OpenSpaceGFFile GetFileContent(byte[] fileBytes, OpenSpaceSettings settings)
        {
            // Load the bytes into a memory stream
            using var stream = new MemoryStream(fileBytes);
            
            // Serialize the data
            var data = OpenSpaceGFFile.GetSerializer(settings).Deserialize(stream);

            // Make sure we read the entire file
            if (stream.Position != stream.Length)
                RCFCore.Logger?.LogWarningSource($"The GF file {FileName} was not fully read");

            // Return the data
            return data;
        }

        /// <summary>
        /// Gets the file bytes for the CNT file item from the stream
        /// </summary>
        /// <param name="fileStream">The stream to get the file bytes from</param>
        /// <param name="decrypt">Indicates if the bytes should be decrypted</param>
        /// <returns>The file bytes</returns>
        public byte[] GetFileBytes(Stream fileStream, bool decrypt = true)
        {
            // Set the position
            fileStream.Position = Pointer;

            // Create the buffer
            byte[] buffer = new byte[Size];

            // Read the bytes into the buffer
            fileStream.Read(buffer, 0, buffer.Length);

            // Decrypt if set to do so and there is an encryption
            if (decrypt && FileXORKey.Any(x => x != 0))
            {
                // Enumerate each byte
                for (int i = 0; i < Size; i++)
                {
                    if ((Size % 4) + i < Size)
                        buffer[i] = (byte)(buffer[i] ^ FileXORKey[i % 4]);
                }
            }

            // Return the buffer
            return buffer;
        }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<OpenSpaceSettings> reader)
        {
            DirectoryIndex = reader.Read<int>();
            FileName = OpenSpaceSerializerHelpers.ReadEncryptedString(reader, XORKey);
            FileXORKey = reader.ReadBytes(4);
            Unknown1 = reader.Read<int>();
            Pointer = reader.Read<int>();
            Size = reader.Read<int>();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<OpenSpaceSettings> writer)
        {
            writer.Write(DirectoryIndex);
            OpenSpaceSerializerHelpers.WriteEncryptedString(writer, FileName, XORKey);
            writer.Write(FileXORKey);
            writer.Write(Unknown1);
            writer.Write(Pointer);
            writer.Write(Size);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{DirectoryIndex}: {FileName}";
        }

        #endregion
    }
}