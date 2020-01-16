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
            using MemoryStream inStream = new MemoryStream(inputBytes);
            using MemoryStream outStream = new MemoryStream();
            
            Encoder encoder = new Encoder();
            
            encoder.SetCoderProperties(propIDs, properties);
            encoder.WriteCoderProperties(outStream);
            
            encoder.Code(inStream, outStream, -1, -1, null);
            return outStream.ToArray();
        }

        public static byte[] Decompress(byte[] inputBytes, long outSize)
        {
            using MemoryStream newInStream = new MemoryStream(inputBytes);
            using MemoryStream newOutStream = new MemoryStream();

            Decoder decoder = new Decoder();

            var properties2 = new byte[5];
            if (newInStream.Read(properties2, 0, 5) != 5) 
                throw new Exception("Input LZMA is too short");
            
            decoder.SetDecoderProperties(properties2);

            var compressedSize = newInStream.Length - newInStream.Position;
            decoder.Code(newInStream, newOutStream, compressedSize, outSize, null);

            var b = newOutStream.ToArray();

            return b;
        }
    }
}