using System;
using System.IO;

namespace SevenZip.Compression.LZMA
{
    public static class SevenZipHelper
    {
        private static readonly CoderPropID[] propIDs =
        {
            CoderPropID.DictionarySize, 
            CoderPropID.PosStateBits, 
            CoderPropID.LitContextBits,
            CoderPropID.LitPosBits, 
            CoderPropID.Algorithm, 
            CoderPropID.NumFastBytes, 
            CoderPropID.MatchFinder,
            CoderPropID.EndMarker
        };

        // these are the default properties, keeping it simple for now:
        private static readonly object[] properties =
        {
            // Dictionary
            1 << 23,

            2,
            3,
            0,
            2,
            128,
            "bt4",
            false
        };

        public static byte[] Compress(byte[] inputBytes)
        {
            // Create the memory streams
            using MemoryStream inputStream = new MemoryStream(inputBytes);
            using MemoryStream outputStream = new MemoryStream();

            // Compress the data
            Compress(inputStream, outputStream);

            // Return the output stream
            return outputStream.ToArray();
        }

        public static void Compress(Stream inputStream, Stream outputStream)
        {
            Encoder encoder = new Encoder();

            encoder.SetCoderProperties(propIDs, properties);
            encoder.WriteCoderProperties(outputStream);

            encoder.Code(inputStream, outputStream, -1, -1, null);
        }

        public static byte[] Decompress(byte[] inputBytes, long outSize)
        {
            // Create the memory streams
            using MemoryStream inputStream = new MemoryStream(inputBytes);
            using MemoryStream outputStream = new MemoryStream();

            // Decompress the data
            Decompress(inputStream, outputStream, outSize);

            // Return the output stream
            return outputStream.ToArray();
        }

        public static void Decompress(Stream inputStream, Stream outputStream, long outputSize)
        {
            Decoder decoder = new Decoder();

            var properties2 = new byte[5];
            if (inputStream.Read(properties2, 0, 5) != 5)
                throw new Exception("Input LZMA is too short");

            decoder.SetDecoderProperties(properties2);

            var compressedSize = inputStream.Length - inputStream.Position;
            decoder.Code(inputStream, outputStream, compressedSize, outputSize, null);
        }
    }
}