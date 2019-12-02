using System;
using System.IO;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// A binary reader which supports a custom byte order
    /// </summary>
    public class OrderedBinaryReader : BinaryReader
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="OrderedBinaryReader"/> with a stream input and the specified byte order to use with the encoding defaulted to UTF-8
        /// </summary>
        /// <param name="input">The stream input</param>
        /// <param name="byteOrder">The byte order to use</param>
        public OrderedBinaryReader(Stream input, ByteOrder byteOrder) : this(input, byteOrder, Encoding.UTF8)
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="OrderedBinaryReader"/> with a stream input, the specified byte order to use and a specific encoding
        /// </summary>
        /// <param name="input">The stream input</param>
        /// <param name="byteOrder">The byte order to use</param>
        /// <param name="encoding">The encoding to use</param>
        public OrderedBinaryReader(Stream input, ByteOrder byteOrder, Encoding encoding) : this(input, byteOrder, encoding, false)
        {

        }

        /// <summary>
        /// Creates a new instance of <see cref="OrderedBinaryReader"/> with a stream input, the specified byte order to use, a specific encoding and a value indicating if the stream should be kept open when disposing
        /// </summary>
        /// <param name="input">The stream input</param>
        /// <param name="byteOrder">The byte order to use</param>
        /// <param name="encoding">The encoding to use</param>
        /// <param name="leaveOpen">Indicates if the stream should be kept open when disposing</param>
        public OrderedBinaryReader(Stream input, ByteOrder byteOrder, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen)
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
        /// Reads a 2-byte signed integer from the current stream and advances the current position of the stream by two bytes.
        /// </summary>
        /// <returns>A 2-byte signed integer read from the current stream.</returns>
        public override short ReadInt16()
        {
            // Read the bytes
            var bytes = ReadBytes(sizeof(short));

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Convert and return
            return BitConverter.ToInt16(bytes, 0);
        }

        /// <summary>
        /// Reads a 2-byte unsigned integer from the current stream using little-endian encoding and advances the position of the stream by two bytes.
        /// </summary>
        /// <returns>A 2-byte unsigned integer read from this stream.</returns>
        public override ushort ReadUInt16()
        {
            // Read the bytes
            var bytes = ReadBytes(sizeof(ushort));

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Convert and return
            return BitConverter.ToUInt16(bytes, 0);
        }

        /// <summary>
        /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns>A 4-byte signed integer read from the current stream.</returns>
        public override int ReadInt32()
        {
            // Read the bytes
            var bytes = ReadBytes(sizeof(int));

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Convert and return
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Reads a 4-byte unsigned integer from the current stream and advances the position of the stream by four bytes.
        /// </summary>
        /// <returns>A 4-byte unsigned integer read from this stream.</returns>
        public override uint ReadUInt32()
        {
            // Read the bytes
            var bytes = ReadBytes(sizeof(uint));

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Convert and return
            return BitConverter.ToUInt32(bytes, 0);
        }

        /// <summary>
        /// Reads an 8-byte signed integer from the current stream and advances the current position of the stream by eight bytes.</summary>
        /// <returns>An 8-byte signed integer read from the current stream.
        /// </returns>
        public override long ReadInt64()
        {
            // Read the bytes
            var bytes = ReadBytes(sizeof(long));

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Convert and return
            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// Reads an 8-byte unsigned integer from the current stream and advances the position of the stream by eight bytes.
        /// </summary>
        /// <returns>An 8-byte unsigned integer read from this stream.</returns>
        public override ulong ReadUInt64()
        {
            // Read the bytes
            var bytes = ReadBytes(sizeof(ulong));

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Convert and return
            return BitConverter.ToUInt64(bytes, 0);
        }

        /// <summary>
        /// Reads a 4-byte floating point value from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns>A 4-byte floating point value read from the current stream.</returns>
        public override float ReadSingle()
        {
            // Read the bytes
            var bytes = ReadBytes(sizeof(float));

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Convert and return
            return BitConverter.ToSingle(bytes, 0);
        }

        /// <summary>
        /// Reads an 8-byte floating point value from the current stream and advances the current position of the stream by eight bytes.
        /// </summary>
        /// <returns>An 8-byte floating point value read from the current stream.</returns>
        public override double ReadDouble()
        {
            // Read the bytes
            var bytes = ReadBytes(sizeof(double));

            // Reverse if different endian
            if (ShouldReverseBytes)
                Array.Reverse(bytes);

            // Convert and return
            return BitConverter.ToDouble(bytes, 0);
        }

        /// <summary>
        /// Reads a decimal value from the current stream and advances the current position of the stream by sixteen bytes.
        /// </summary>
        /// <returns>A decimal value read from the current stream.</returns>
        public override decimal ReadDecimal()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}