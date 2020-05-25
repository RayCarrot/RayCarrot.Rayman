using RayCarrot.Common;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Handles encoding using an XOR key
    /// </summary>
    public class XORDataEncoder : IDataEncoder
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="xorKey">The key to use</param>
        public XORDataEncoder(byte xorKey)
        {
            XorKey = xorKey;
        }

        /// <summary>
        /// The key to use
        /// </summary>
        public byte XorKey { get; }

        /// <summary>
        /// Decodes the encrypted data
        /// </summary>
        /// <param name="inputStream">The encrypted data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The decrypted data</returns>
        public void Decode(Stream inputStream, Stream outputStream)
        {
            foreach (var b in inputStream.EnumerateBytes())
                outputStream.WriteByte((byte)(b ^ XorKey));
        }

        /// <summary>
        /// Encodes the raw data
        /// </summary>
        /// <param name="inputStream">The raw data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The encrypted data</returns>
        public void Encode(Stream inputStream, Stream outputStream)
        {
            // Same as decoding...
            Decode(inputStream, outputStream);
        }
    }
}