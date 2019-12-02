using System;
using System.Collections.Generic;
using System.Linq;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The archive data used for Rayman 2's .cnt files
    /// </summary>
    public class R2CntData : IBinarySerializable
    {
        #region Public Properties

        /// <summary>
        /// The file signature
        /// </summary>
        public short Signature { get; set; }

        /// <summary>
        /// The XOR key used to get the directory and file names
        /// </summary>
        public byte XORKey { get; set; }

        /// <summary>
        /// The available directories
        /// </summary>
        public string[] Directories { get; set; }

        /// <summary>
        /// The version ID
        /// </summary>
        public byte VersionID { get; set; }

        /// <summary>
        /// The available files
        /// </summary>
        public R2CntFile[] Files { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            // Read the directory and file count
            var dirCount = reader.Read<int>();
            var fileCount = reader.Read<int>();

            // Read header info
            Signature = reader.Read<short>();
            XORKey = reader.Read<byte>();

            // Create the list
            Directories = new string[dirCount];

            // Read directory paths
            for (int i = 0; i < dirCount; i++)
                Directories[i] = reader.ReadyEncryptedString(XORKey);

            // Read version ID
            VersionID = reader.Read<byte>();

            // Create the list
            Files = new R2CntFile[fileCount];

            // Read file info
            for (int i = 0; i < fileCount; i++)
            {
                // Create the new file data and pass in the XOR key
                var file = new R2CntFile(XORKey);

                // Deserialize the file data
                file.Deserialize(reader);

                // Add the file info
                Files[i] = file;
            }
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}