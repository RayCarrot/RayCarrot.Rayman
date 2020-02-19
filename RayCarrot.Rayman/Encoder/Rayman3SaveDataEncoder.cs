using RayCarrot.Extensions;
using System;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The encoder for Rayman 3 save data files
    /// </summary>
    public class Rayman3SaveDataEncoder : IDataEncoder
    {
        /// <summary>
        /// Decodes the encrypted data
        /// </summary>
        /// <param name="inputStream">The encrypted data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The decrypted data</returns>
        public void Decode(Stream inputStream, Stream outputStream)
        {
            // Read the initial key
            var XORKey = BitConverter.ToUInt32(inputStream.Read(4), 0) ^ 0xA55AA55A;

            // Keep track of the last byte
            byte lastByte;

            byte DecryptByte()
            {
                XORKey = (XORKey >> 3) | (XORKey << 29);
                lastByte = (byte)inputStream.ReadByte();
                lastByte ^= (byte)XORKey;
                return lastByte;
            }

            // Enumerate each byte
            while (inputStream.Position < inputStream.Length)
            {
                // Decrypt the byte
                DecryptByte();

                if ((lastByte & 0x80) == 0)
                {
                    for (var i = 0; i < lastByte; i++)
                        outputStream.WriteByte(0);
                }
                else
                {
                    var size = lastByte & 0x7F;
                    var byteArray = new byte[size];

                    for (var i = size; i > 0; --i)
                        byteArray[i - 1] = DecryptByte();

                    outputStream.Write(byteArray);
                }
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
            throw new NotImplementedException();
        }
    }
}