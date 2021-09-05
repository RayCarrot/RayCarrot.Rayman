using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

                // The first bit is a flag. If it's 0 this is true, if it's 1 it's false.
                if ((lastByte & 0x80) == 0)
                {
                    // Write 0 the specified number of times. We don't need to remove the flag here since it's already 0.
                    for (var i = 0; i < lastByte; i++)
                        outputStream.WriteByte(0);
                }
                else
                {
                    // Get the size to write by removing the first bit, which is used as a flag
                    var size = lastByte & 0x7F;

                    // Create the byte array
                    var byteArray = new byte[size];

                    // Read the specified number of bytes in reverse
                    for (var i = size; i > 0; --i)
                        byteArray[i - 1] = DecryptByte();

                    // Write the bytes to the stream
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
            // Write the initial XOR key to be the same as the hard-coded key to make it 0, thus removing the encryption
            outputStream.Write(BitConverter.GetBytes(0xA55AA55A));

            // NOTE: We're not using the compression system here as it's irrelevant
            // Write in chunks of 127 (0111_1111)
            while (inputStream.Position < inputStream.Length)
            {
                // Create a buffer for the chunk of data
                var buffer = new List<byte>();

                for (int i = 0; i < 127; i++)
                {
                    var value = inputStream.ReadByte();

                    if (value == -1)
                        break;

                    buffer.Add((byte)value);
                }

                // Write the size and set the flag
                outputStream.WriteByte((byte)((byte)buffer.Count | 0x80));

                // Write the buffer, but reversed
                outputStream.Write(buffer.ToArray().Reverse().ToArray());
            }
        }
    }
}