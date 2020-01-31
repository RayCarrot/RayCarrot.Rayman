using System;
using System.Collections.Generic;
using System.IO;
using RayCarrot.Extensions;

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
        /// <param name="dataStream">The encrypted data stream</param>
        /// <returns>The decrypted data</returns>
        public IEnumerable<byte> Decode(Stream dataStream)
        {
            // Read the initial key
            var XORKey = BitConverter.ToUInt32(dataStream.Read(4), 0) ^ 0xA55AA55A;

            // Keep track of the last byte
            byte lastByte;

            byte DecryptByte()
            {
                XORKey = (XORKey >> 3) | (XORKey << 29);
                lastByte = (byte)dataStream.ReadByte();
                lastByte ^= (byte)XORKey;
                return lastByte;
            }

            // Enumerate each byte
            while (dataStream.Position < dataStream.Length)
            {
                // Decrypt the byte
                DecryptByte();

                if ((lastByte & 0x80) == 0)
                {
                    for (var i = 0; i < lastByte; i++)
                        yield return 0;
                }
                else
                {
                    var size = lastByte & 0x7F;
                    var byteArray = new byte[size];

                    for (var i = size; i > 0; --i)
                        byteArray[i - 1] = DecryptByte();

                    foreach (var b in byteArray)
                        yield return b;
                }
            }
        }

        /// <summary>
        /// Encodes the raw data
        /// </summary>
        /// <param name="dataStream">The raw data stream</param>
        /// <returns>The encrypted data</returns>
        public IEnumerable<byte> Encode(Stream dataStream)
        {
            throw new NotImplementedException();
        }
    }
}