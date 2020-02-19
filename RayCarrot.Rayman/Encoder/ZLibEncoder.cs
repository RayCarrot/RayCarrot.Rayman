using System.IO;
using Ionic.Zlib;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Data encoder for ZLib
    /// </summary>
    public class ZLibEncoder : IDataEncoder
    {
        /// <summary>
        /// Decodes the encrypted data
        /// </summary>
        /// <param name="inputStream">The encrypted data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The decrypted data</returns>
        public void Decode(Stream inputStream, Stream outputStream)
        {
            using var zStream = new ZlibStream(inputStream, CompressionMode.Decompress);

            zStream.CopyTo(outputStream);
        }

        /// <summary>
        /// Encodes the raw data
        /// </summary>
        /// <param name="inputStream">The raw data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The encrypted data</returns>
        public void Encode(Stream inputStream, Stream outputStream)
        {
            using var zStream = new ZlibStream(inputStream, CompressionMode.Compress);

            zStream.CopyTo(outputStream);
        }
    }
}