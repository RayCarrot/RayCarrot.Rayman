using System.IO;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The binary writer to use for UbiArt game files
    /// </summary>
    public class UbiArtBinaryWriter : OrderedBinaryWriter
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="UbiArtBinaryWriter"/> with a stream input and the specified byte order to use with the encoding defaulted to UTF-8
        /// </summary>
        /// <param name="output">The stream output</param>
        /// <param name="byteOrder">The byte order to use</param>
        public UbiArtBinaryWriter(Stream output, ByteOrder byteOrder) : this(output, byteOrder, Encoding.UTF8)
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="UbiArtBinaryWriter"/> with a stream input, the specified byte order to use and a specific encoding
        /// </summary>
        /// <param name="output">The stream output</param>
        /// <param name="byteOrder">The byte order to use</param>
        /// <param name="encoding">The encoding to use</param>
        public UbiArtBinaryWriter(Stream output, ByteOrder byteOrder, Encoding encoding) : this(output, byteOrder, encoding, false)
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="UbiArtBinaryWriter"/> with a stream input, the specified byte order to use, a specific encoding and a value indicating if the stream should be kept open when disposing
        /// </summary>
        /// <param name="output">The stream output</param>
        /// <param name="byteOrder">The byte order to use</param>
        /// <param name="encoding">The encoding to use</param>
        /// <param name="leaveOpen">Indicates if the stream should be kept open when disposing</param>
        public UbiArtBinaryWriter(Stream output, ByteOrder byteOrder, Encoding encoding, bool leaveOpen) : base(output, byteOrder, encoding, leaveOpen)
        {

        }

        #endregion

        #region Public Override Methods

        /// <summary>
        /// Writes a length-prefixed string to this stream in the current encoding of the <see cref="T:System.IO.BinaryWriter" />, and advances the current position of the stream in accordance with the encoding used and the specific characters being written to the stream.
        /// </summary>
        /// <param name="value">The value to write. </param>
        public override void Write(string value)
        {
            // Get the string bytes
            var bytes = Encoding.GetBytes(value);

            // NOTE: The way the char count is obtained is not ideal
            // Write the item size
            Write(bytes.Length / Encoding.GetByteCount("A"));

            // Write the bytes to the stream
            Write(bytes);
        }

        #endregion
    }
}