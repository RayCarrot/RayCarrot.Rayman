using System.Collections.Generic;
using System.IO;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Handles encoding using separate encoders for separate segments
    /// </summary>
    public class SegmentedDataEncoder : IDataEncoder
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="segments">The segments</param>
        public SegmentedDataEncoder(IEnumerable<SegmentedDataInfo> segments)
        {
            Segments = segments;
        }

        /// <summary>
        /// The segments
        /// </summary>
        public IEnumerable<SegmentedDataInfo> Segments { get; }

        /// <summary>
        /// Decodes the encrypted data
        /// </summary>
        /// <param name="dataStream">The encrypted data stream</param>
        /// <returns>The decrypted data</returns>
        public IEnumerable<byte> Decode(Stream dataStream)
        {
            // Handle each segment
            foreach (var s in Segments)
            {
                // Read the segment into a memory stream
                using var segmentData = new MemoryStream(dataStream.Read(s.Length));

                // Decode the segment and return
                foreach (var b in s.Encoder.Decode(segmentData))
                    yield return b;
            }
        }

        /// <summary>
        /// Encodes the raw data
        /// </summary>
        /// <param name="dataStream">The raw data stream</param>
        /// <returns>The encrypted data</returns>
        public IEnumerable<byte> Encode(Stream dataStream)
        {
            // Handle each segment
            foreach (var s in Segments)
            {
                // Read the segment into a memory stream
                using var segmentData = new MemoryStream(dataStream.Read(s.Length));

                // Encode the segment and return
                foreach (var b in s.Encoder.Encode(segmentData))
                    yield return b;
            }
        }

        /// <summary>
        /// The info for a segment
        /// </summary>
        public class SegmentedDataInfo
        {
            /// <summary>
            /// Default constructor
            /// </summary>
            /// <param name="encoder">The encoder to use</param>
            /// <param name="length">The segment length</param>
            public SegmentedDataInfo(IDataEncoder encoder, int length)
            {
                Encoder = encoder;
                Length = length;
            }

            /// <summary>
            /// The encoder to use
            /// </summary>
            public IDataEncoder Encoder { get; }

            /// <summary>
            /// The segment length
            /// </summary>
            public int Length { get; }
        }
    }
}