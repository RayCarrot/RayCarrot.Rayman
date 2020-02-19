using System;
using System.IO;

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
        /// The XOR key for the file name
        /// </summary>
        public byte XORKey { get; }

        /// <summary>
        /// The XOR key for the file contents
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