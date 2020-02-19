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
        /// <param name="inputStream">The encrypted data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The decrypted data</returns>
        public void Decode(Stream inputStream, Stream outputStream)
        {
            // Get the initial magic key
            uint magic = 1790299257;

            // Return the initial magic key
            outputStream.WriteByte(0x79);
            outputStream.WriteByte(0xCC);
            outputStream.WriteByte(0xB5);
            outputStream.WriteByte(0x6A);

            // Get the length
            var length = inputStream.Length - inputStream.Position;

            // Set the position to skip the first 4 bytes
            inputStream.Position += 4;

            // Enumerate every byte
            for (long i = 4; i < length; i++)
            {
                // Read the byte
                var b = (byte)inputStream.ReadByte();
                
                // Decode the byte
                b ^= (byte)((magic >> 8) & 255);

                // Return the byte
                outputStream.WriteByte(b);

                // Update the magic key
                magic = 16807 * (magic ^ 0x75BD924) - 0x7FFFFFFF * ((magic ^ 0x75BD924u) / 0x1F31D);
            }
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