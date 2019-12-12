using System.IO;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The standard binary reader to use
    /// </summary>
    public class StandardBinaryReader : OrderedBinaryReader
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="StandardBinaryReader"/> with a stream input and the specified byte order to use with the encoding defaulted to UTF-8
        /// </summary>
        /// <param name="input">The stream input</param>
        /// <param name="byteOrder">The byte order to use</param>
        public StandardBinaryReader(Stream input, ByteOrder byteOrder) : this(input, byteOrder, Encoding.UTF8)
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="StandardBinaryReader"/> with a stream input, the specified byte order to use and a specific encoding
        /// </summary>
        /// <param name="input">The stream input</param>
        /// <param name="byteOrder">The byte order to use</param>
        /// <param name="encoding">The encoding to use</param>
        public StandardBinaryReader(Stream input, ByteOrder byteOrder, Encoding encoding) : this(input, byteOrder, encoding, false)
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="StandardBinaryReader"/> with a stream input, the specified byte order to use, a specific encoding and a value indicating if the stream should be kept open when disposing
        /// </summary>
        /// <param name="input">The stream input</param>
        /// <param name="byteOrder">The byte order to use</param>
        /// <param name="encoding">The encoding to use</param>
        /// <param name="leaveOpen">Indicates if the stream should be kept open when disposing</param>
        public StandardBinaryReader(Stream input, ByteOrder byteOrder, Encoding encoding, bool leaveOpen) : base(input, byteOrder, encoding, leaveOpen)
        {

        }

        #endregion

        #region Public Override Methods

        /// <summary>
        /// Reads a string from the current stream. The string is prefixed with the length, encoded as an integer.
        /// </summary>
        /// <returns>The string being read.</returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached. </exception>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override string ReadString()
        {
            // Get the length
            var length = ReadInt32();

            // NOTE: The way the char count is obtained is not ideal
            // Read the bytes
            var bytes = ReadBytes(length * Encoding.GetByteCount("A"));

            // Return the string
            return Encoding.GetString(bytes);
        }

        #endregion
    }
}