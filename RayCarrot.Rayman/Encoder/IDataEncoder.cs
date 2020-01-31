using System.Collections.Generic;
using System.IO;
using System.Linq;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Interface for decoding/encoding encrypted data
    /// </summary>
    public interface IDataEncoder
    {
        /// <summary>
        /// Decodes the encrypted data
        /// </summary>
        /// <param name="dataStream">The encrypted data stream</param>
        /// <returns>The decrypted data</returns>
        IEnumerable<byte> Decode(Stream dataStream);

        /// <summary>
        /// Encodes the raw data
        /// </summary>
        /// <param name="dataStream">The raw data stream</param>
        /// <returns>The encrypted data</returns>
        IEnumerable<byte> Encode(Stream dataStream);
    }

    // TODO: Finish this up and use
    public class XORDataEncoder : IDataEncoder
    {
        public XORDataEncoder(byte xorKey)
        {
            XorKey = xorKey;
        }

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