using System;
using System.IO;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// A binary writer which supports a custom byte order
    /// </summary>
    public class OrderedBinaryWriter : BinaryWriter
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="OrderedBinaryReader"/> with a stream input and the specified byte order to use with the encoding defaulted to UTF-8
        /// </summary>
        /// <param name="output">The stream output</param>
        /// <param name="byteOrder">The byte order to use</param>
        public OrderedBinaryWriter(Stream output, ByteOrder byteOrder) : this(output, byteOrder, Encoding.UTF8)
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="OrderedBinaryReader"/> with a stream input, the specified byte order to use and a specific encoding
        /// </summary>
        /// <param name="output">The stream output</param>
        /// <param name="byteOrder">The byte order to use</param>
        /// <param name="encoding">The encoding to use</param>
        public OrderedBinaryWriter(Stream output, ByteOrder byteOrder, Encoding encoding) : this(output, byteOrder, encoding, false)
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="OrderedBinaryReader"/> with a stream input, the specified byte order to use, a specific encoding and a value indicating if the stream should be kept open when disposing
        /// </summary>
        /// <param name="output">The stream output</param>
        /// <param name="byteOrder">The byte order to use</param>
        /// <param name="encoding">The encoding to use</param>
        /// <param name="leaveOpen">Indicates if the stream should be kept open when disposing</param>
        public OrderedBinaryWriter(Stream output, ByteOrder byteOrder, Encoding encoding, bool leaveOpen) : base(output, encoding, leaveOpen)
        {
            // Set the encoding
            Encoding = encoding;

            // Set the byte order
            ByteOrder = byteOrder;

            // Check if the bytes should be reversed
            ShouldReverseBytes = BitConverter.IsLittleEndian && byteOrder == ByteOrder.BigEndian || !BitConverter.IsLittleEndian && byteOrder == ByteOrder.LittleEndian;
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The encoding to use
        /// </summary>
        protected Encoding Encoding { get; }

        /// <summary>
        /// The byte order to use
        /// </summary>
        protected ByteOrder ByteOrder { get; }

        /// <summary>
        /// Indicates if the byte order should be reversed when read
        /// </summary>
        protected bool ShouldReverseBytes { get; }

        #endregion

        #region Public Override Methods

        /// <summary>
        /// Writes a two-byte signed integer to the current stream and advances the stream position by two bytes.
        /// </summary>
        /// <param name="value">The two-byte signed integer to write. </param>
        public override void Write(short value)
        {
            // Get the bytes
            var bytes = BitConverter.GetBytes(value);

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Write the bytes
            Write(bytes);
        }

        /// <summary>
        /// Writes a two-byte unsigned integer to the current stream and advances the stream position by two bytes.
        /// </summary>
        /// <param name="value">The two-byte unsigned integer to write.</param>
        public override void Write(ushort value)
        {
            // Get the bytes
            var bytes = BitConverter.GetBytes(value);

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Write the bytes
            Write(bytes);
        }

        /// <summary>
        /// Writes a four-byte signed integer to the current stream and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte signed integer to write. </param>
        public override void Write(int value)
        {
            // Get the bytes
            var bytes = BitConverter.GetBytes(value);

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Write the bytes
            Write(bytes);
        }

        /// <summary>
        /// Writes a four-byte unsigned integer to the current stream and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte unsigned integer to write. </param>
        public override void Write(uint value)
        {
            // Get the bytes
            var bytes = BitConverter.GetBytes(value);

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Write the bytes
            Write(bytes);
        }

        /// <summary>
        /// Writes an eight-byte signed integer to the current stream and advances the stream position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte signed integer to write. </param>
        public override void Write(long value)
        {
            // Get the bytes
            var bytes = BitConverter.GetBytes(value);

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Write the bytes
            Write(bytes);
        }

        /// <summary>
        /// Writes an eight-byte unsigned integer to the current stream and advances the stream position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte unsigned integer to write. </param>
        public override void Write(ulong value)
        {
            // Get the bytes
            var bytes = BitConverter.GetBytes(value);

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Write the bytes
            Write(bytes);
        }

        /// <summary>
        /// Writes a four-byte floating-point value to the current stream and advances the stream position by four bytes.
        /// </summary>
        /// <param name="value">The four-byte floating-point value to write. </param>
        public override void Write(float value)
        {
            // Get the bytes
            var bytes = BitConverter.GetBytes(value);

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Write the bytes
            Write(bytes);
        }

        /// <summary>
        /// Writes an eight-byte floating-point value to the current stream and advances the stream position by eight bytes.
        /// </summary>
        /// <param name="value">The eight-byte floating-point value to write. </param>
        public override void Write(double value)
        {
            // Get the bytes
            var bytes = BitConverter.GetBytes(value);

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Write the bytes
            Write(bytes);
        }

        /// <summary>
        /// Writes a decimal value to the current stream and advances the stream position by sixteen bytes.
        /// </summary>
        /// <param name="value">The decimal value to write. </param>
        public override void Write(decimal value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}