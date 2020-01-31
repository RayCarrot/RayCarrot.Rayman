using System.IO;
using System.Linq;

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
            // Create a memory stream
            using var stream = new MemoryStream(data);

            // Decode the data and return as an array
            return encoder.Decode(stream).ToArray();
        }

        /// <summary>
        /// Encodes the raw data
        /// </summary>
        /// <param name="encoder">The encoder</param>
        /// <param name="data">The raw data</param>
        /// <returns>The encrypted data</returns>
        public static byte[] Encode(this IDataEncoder encoder, byte[] data)
        {
            // Create a memory stream
            using var stream = new MemoryStream(data);

            // Decode the data and return as an array
            return encoder.Encode(stream).ToArray();
        }
    }
}