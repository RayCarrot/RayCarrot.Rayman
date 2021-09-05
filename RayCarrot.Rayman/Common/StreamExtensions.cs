using System.Collections.Generic;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Extension methods for <see cref="Stream"/>
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// Reads the remaining bytes from a file stream
        /// </summary>
        /// <param name="stream">The file stream to read from</param>
        /// <returns>The remaining bytes</returns>
        public static byte[] ReadRemainingBytes(this Stream stream)
        {
            // Check if seeking is allowed
            if (stream.CanSeek)
            {
                // Create the buffer
                byte[] buffer = new byte[stream.Length - stream.Position];

                // Read into the buffer
                stream.Read(buffer, 0, buffer.Length);

                // Return the buffer
                return buffer;
            }
            else
            {
                // Create the output list
                var output = new List<byte>();

                int value;

                // Read until we reach the end
                while ((value = stream.ReadByte()) != -1)
                    output.Add((byte)value);

                // Return the output
                return output.ToArray();
            }
        }

        /// <summary>
        /// Enumerated the bytes in a stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <returns>The bytes</returns>
        public static IEnumerable<byte> EnumerateBytes(this Stream stream)
        {
            int value;

            // Read until we reach the end
            while ((value = stream.ReadByte()) != -1)
                yield return (byte)value;
        }

        /// <summary>
        /// Reads the specified amount of bytes from the stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="count">The amount of bytes to read</param>
        /// <returns>The read bytes</returns>
        public static byte[] Read(this Stream stream, int count)
        {
            var buffer = new byte[count];

            stream.Read(buffer, 0, count);

            return buffer;
        }

        /// <summary>
        /// Writes the bytes to the stream
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="bytes">The bytes to write</param>
        public static void Write(this Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}