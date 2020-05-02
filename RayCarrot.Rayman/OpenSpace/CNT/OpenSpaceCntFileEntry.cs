using System;
using System.IO;
using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// The data used for a file entry within a .cnt file
    /// </summary>
    public class OpenSpaceCntFileEntry : IBinarySerializable
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public OpenSpaceCntFileEntry()
        {
            FileXORKey = new byte[]
            {
                0, 0, 0, 0
            };
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
        public byte XORKey { get; set; }

        /// <summary>
        /// The XOR key for the file contents
        /// </summary>
        public byte[] FileXORKey { get; set; }

        /// <summary>
        /// The file checksum
        /// </summary>
        public uint Checksum { get; set; }

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
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{DirectoryIndex}: {FileName}";
        }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            DirectoryIndex = s.Serialize<int>(DirectoryIndex, name: nameof(DirectoryIndex));
            FileName = s.SerializeOpenSpaceEncryptedString(FileName, XORKey, name: nameof(FileName));
            FileXORKey = s.SerializeArray<byte>(FileXORKey, 4, name: nameof(FileXORKey));
            Checksum = s.Serialize<uint>(Checksum, name: nameof(Checksum));
            Pointer = s.Serialize<int>(Pointer, name: nameof(Pointer));
            Size = s.Serialize<int>(Size, name: nameof(Size));
        }

        #endregion
    }
}