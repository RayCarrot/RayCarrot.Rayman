using System.IO;
using SevenZip.Compression.LZMA;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Data encoder for 7-Zip compression
    /// </summary>
    public class SevenZipEncoder : IDataEncoder
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="decompressedSize">The size of the decompressed data, if available</param>
        public SevenZipEncoder(long decompressedSize)
        {
            DecompressedSize = decompressedSize;
        }

        /// <summary>
        /// The size of the decompressed data, if available
        /// </summary>
        public long DecompressedSize { get; }

        /// <summary>
        /// Decodes the encrypted data
        /// </summary>
        /// <param name="inputStream">The encrypted data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The decrypted data</returns>
        public void Decode(Stream inputStream, Stream outputStream)
        {
            SevenZipHelper.Decompress(inputStream, outputStream, DecompressedSize);
        }

        /// <summary>
        /// Encodes the raw data
        /// </summary>
        /// <param name="inputStream">The raw data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The encrypted data</returns>
        public void Encode(Stream inputStream, Stream outputStream)
        {
            SevenZipHelper.Compress(inputStream, outputStream);
        }
    }
}