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
        /// <param name="inputStream">The encrypted data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The decrypted data</returns>
        void Decode(Stream inputStream, Stream outputStream);

        /// <summary>
        /// Encodes the raw data
        /// </summary>
        /// <param name="inputStream">The raw data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The encrypted data</returns>
        void Encode(Stream inputStream, Stream outputStream);
    }
}