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
            // Get the initial key
            uint currentMask = 0x6AB5CC79;

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
                b ^= (byte)((currentMask >> 8) & 0xFF);

                // Write the byte
                outputStream.WriteByte(b);

                // Update the magic key
                currentMask = 16807 * (currentMask ^ 0x75BD924) - 0x7FFFFFFF * ((currentMask ^ 0x75BD924) / 0x1F31D);

                // Use this instead for the iOS version
                //currentMask = (uint)(16807 * ((currentMask ^ 0x75BD924u) % 0x1F31D) - 2836 * ((currentMask ^ 0x75BD924u) / 0x1F31D));
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