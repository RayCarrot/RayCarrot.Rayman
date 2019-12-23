using System.IO;
using System.Linq;
using RayCarrot.CarrotFramework.Abstractions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The data used for a file within the .cnt files for OpenSpace games
    /// </summary>
    public class OpenSpaceCntFile : IBinarySerializable
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="xorKey">The XOR key</param>
        public OpenSpaceCntFile(byte xorKey)
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
        /// Gets the file content for the CNT file item from the stream
        /// </summary>
        /// <param name="fileStream">The stream to get the file content from</param>
        /// <param name="settings">The settings when serializing the data</param>
        /// <returns>The file content</returns>
        public OpenSpaceGFFile GetFileContent(Stream fileStream, OpenSpaceSettings settings)
        {
            // Get the bytes and load them into a memory stream
            using var stream = new MemoryStream(GetFileBytes(fileStream));
            
            // Serialize the data
            var data = new OpenSpaceGfSerializer(settings).Deserialize(stream, new OpenSpaceGFFile(settings));

            RCFCore.Logger?.LogWarningSource($"{data.RepeatByte}");

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
        public void Deserialize(BinaryDataReader reader)
        {
            DirectoryIndex = reader.Read<int>();
            FileName = reader.ReadEncryptedString(XORKey);
            FileXORKey = reader.ReadBytes(4);
            Unknown1 = reader.Read<int>();
            Pointer = reader.Read<int>();
            Size = reader.Read<int>();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            writer.Write(DirectoryIndex);
            writer.WriteEncryptedString(FileName, XORKey);
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