using System.Collections.Generic;
using System.IO;
using System.Linq;
using RayCarrot.Extensions;

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
        /// <param name="dataStream">The encrypted data stream</param>
        /// <returns>The decrypted data</returns>
        public IEnumerable<byte> Decode(Stream dataStream)
        {
            return dataStream.EnumerateBytes().Select(b => (byte)(b ^ XorKey));
        }

        /// <summary>
        /// Encodes the raw data
        /// </summary>
        /// <param name="dataStream">The raw data stream</param>
        /// <returns>The encrypted data</returns>
        public IEnumerable<byte> Encode(Stream dataStream)
        {
            // Same as decoding...
            return Decode(dataStream);
        }
    }
}