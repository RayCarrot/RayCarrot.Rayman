using System.IO;
using System.Linq;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The encoder for Tonic Trouble .sna data files
    /// </summary>
    public class TonicTroubleSNADataEncoder : IDataEncoder
    {
        /// <summary>
        /// Performs the encoding operation on the data
        /// </summary>
        /// <param name="inputStream">The input data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <param name="isDecoding">True if the data should be decoded, false if it should be encoded</param>
        protected void Encode(Stream inputStream, Stream outputStream, bool isDecoding)
        {
            var originalMaskBytes = new byte[] { 0x41, 0x59, 0xBE, 0xC7, 0x0D, 0x99, 0x1C, 0xA3, 0x75, 0x3F };
            var maskBytes = originalMaskBytes.ToArray();
            uint currentMaskByte = 0;

            // Get the length
            var length = inputStream.Length - inputStream.Position;

            // Enumerate every byte
            for (long i = 0; i < length; i++)
            {
                // Read the byte
                var b = (byte)inputStream.ReadByte();

                // Decode the byte
                byte decodedByte = (byte)(b ^ (maskBytes[currentMaskByte]));
                maskBytes[currentMaskByte] = (byte)(originalMaskBytes[currentMaskByte] + (isDecoding ? b : decodedByte));
                currentMaskByte = (uint)((currentMaskByte + 1) % maskBytes.Length);
                b = decodedByte;

                // Write the byte
                outputStream.WriteByte(b);
            }
        }

        /// <summary>
        /// Decodes the encrypted data
        /// </summary>
        /// <param name="inputStream">The encrypted data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The decrypted data</returns>
        public void Decode(Stream inputStream, Stream outputStream) => Encode(inputStream, outputStream, true);

        /// <summary>
        /// Encodes the raw data
        /// </summary>
        /// <param name="inputStream">The raw data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The encrypted data</returns>
        public void Encode(Stream inputStream, Stream outputStream) => Encode(inputStream, outputStream, false);
    }
}