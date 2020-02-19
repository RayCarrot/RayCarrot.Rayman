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
        /// <param name="inputStream">The encrypted data stream</param>
        /// <param name="outputStream">The output stream</param>
        /// <returns>The decrypted data</returns>
        public void Decode(Stream inputStream, Stream outputStream)
        {
            // Handle each segment
            foreach (var s in Segments)
            {
                // Read the segment into a memory stream
                using var segmentData = new MemoryStream(inputStream.Read(s.Length));

                // Decode the segment
                s.Encoder.Decode(segmentData, outputStream);
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
            // Handle each segment
            foreach (var s in Segments)
            {
                // Read the segment into a memory stream
                using var segmentData = new MemoryStream(inputStream.Read(s.Length));

                // Encode the segment
                s.Encoder.Encode(segmentData, outputStream);
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