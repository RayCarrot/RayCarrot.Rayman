using System.Collections.Generic;
using System.IO;

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
}