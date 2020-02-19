using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Extension methods for <see cref="IDataEncoder"/>
    /// </summary>
    public static class DataEncoderExtensions
    {
        /// <summary>
        /// Decodes the encrypted data
        /// </summary>
        /// <param name="encoder">The encoder</param>
        /// <param name="data">The encrypted data</param>
        /// <returns>The decrypted data</returns>
        public static byte[] Decode(this IDataEncoder encoder, byte[] data)
        {
            // Create memory streams
            using var inputStream = new MemoryStream(data);
            using var outputStream = new MemoryStream();

            // Decode the data and return as an array
            encoder.Decode(inputStream, outputStream);

            // Return the output stream
            return outputStream.ToArray();
        }

        /// <summary>
        /// Encodes the raw data
        /// </summary>
        /// <param name="encoder">The encoder</param>
        /// <param name="data">The raw data</param>
        /// <returns>The encrypted data</returns>
        public static byte[] Encode(this IDataEncoder encoder, byte[] data)
        {
            // Create memory streams
            using var inputStream = new MemoryStream(data);
            using var outputStream = new MemoryStream();

            // Decode the data and return as an array
            encoder.Encode(inputStream, outputStream);

            // Return the output stream
            return outputStream.ToArray();
        }
    }
}