using RayCarrot.Extensions;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Handles encoding using an XOR key consisting of multiple bytes
    /// </summary>
    public class MultiXORDataEncoder : IDataEncoder
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="xorKey">The key to use</param>
        /// <param name="skipIncompleteSequences">Indicates if sequences which are smaller than the size of the key should be skipped</param>
        public MultiXORDataEncoder(byte[] xorKey, bool skipIncompleteSequences)
        {
            XorKey = xorKey;
            SkipIncompleteSequences = skipIncompleteSequences;
        }

        /// <summary>
        /// The key to use
        /// </summary>
        public byte[] XorKey { get; }

        /// <summary>
        /// Indicates if sequences which are smaller than the size of the key should be skipped
        /// </summary>
        public bool SkipIncompleteSequences { get; }

        /// <summary>
        /// Decodes the encrypted data
        /// </summary>
        /// <param name="inputStream">The encrypted data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The decrypted data</returns>
        public void Decode(Stream inputStream, Stream outputStream)
        {
            var index = 0;
            var xorLength = XorKey.Length;
            var inputLength = inputStream.Length - inputStream.Position;

            foreach (var b in inputStream.EnumerateBytes())
            {
                if (!SkipIncompleteSequences || (inputLength % xorLength) + index < inputLength)
                    outputStream.WriteByte((byte)(b ^ XorKey[index % xorLength]));
                else
                    outputStream.WriteByte(b);

                index++;
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