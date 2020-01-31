using System.Collections.Generic;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The encoder for Rayman 2 .sna data files
    /// </summary>
    public class Rayman2SNADataEncoder : IDataEncoder
    {
        /// <summary>
        /// Decodes the encrypted data
        /// </summary>
        /// <param name="dataStream">The encrypted data stream</param>
        /// <returns>The decrypted data</returns>
        public IEnumerable<byte> Decode(Stream dataStream)
        {
            // Get the initial magic key
            uint magic = 1790299257;

            // Return the initial magic key
            yield return 0x79;
            yield return 0xCC;
            yield return 0xB5;
            yield return 0x6A;

            // Set the position to skip the first 4 bytes
            dataStream.Position = 4;

            // TODO: Use enumerate extension method
            // Enumerate every byte
            for (long i = 4; i < dataStream.Length; i++)
            {
                // Read the byte
                var b = (byte)dataStream.ReadByte();
                
                // Decode the byte
                b ^= (byte)((magic >> 8) & 255);

                // Return the byte
                yield return b;

                // Update the magic key
                magic = 16807 * (magic ^ 0x75BD924) - 0x7FFFFFFF * ((magic ^ 0x75BD924u) / 0x1F31D);
            }
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