using System;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The standard binary writer to use
    /// </summary>
    public class StandardBinaryWriter : BinaryWriter
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="StandardBinaryWriter"/> with a stream input, settings and a value indicating if the stream should be kept open when disposing
        /// </summary>
        /// <param name="output">The stream output</param>
        /// <param name="settings">The settings</param>
        /// <param name="leaveOpen">Indicates if the stream should be kept open when disposing</param>
        public StandardBinaryWriter(Stream output, BinarySerializerSettings settings, bool leaveOpen) : base(output, settings.Encoding, leaveOpen)
        {
            // Set the settings
            Settings = settings;

            // Check if the bytes should be reversed
            ShouldReverseBytes = BitConverter.IsLittleEndian && Settings.ByteOrder == ByteOrder.BigEndian || !BitConverter.IsLittleEndian && Settings.ByteOrder == ByteOrder.LittleEndian;

        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The settings
        /// </summary>
        protected BinarySerializerSettings Settings { get; }

        /// <summary>
        /// Indicates if the byte order should be reversed when written
        /// </summary>
        protected bool ShouldReverseBytes { get; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Reads a string from the stream using <see cref="BinaryStringEncoding.NullTerminated"/>
        /// </summary>
        /// <param name="value">The value to write</param>
        protected virtual void WriteNullTerminatedString(string value)
        {
            // Get the string bytes
            var bytes = Settings.Encoding.GetBytes(value);

            // Write the bytes to the stream
            Write(bytes);

            // Write the null value
            Write((byte)0x00);
        }

        /// <summary>
        /// Reads a string from the stream using <see cref="BinaryStringEncoding.LengthPrefixed"/>
        /// </summary>
        /// <param name="value">The value to write</param>
        protected virtual void WriteLengthPrefixedString(string value)
        {
            // Get the string bytes
            var bytes = Settings.Encoding.GetBytes(value);

            // NOTE: The way the char count is obtained is not ideal
            // Write the item size
            Write(bytes.Length / Settings.Encoding.GetByteCount("A"));

            // Write the bytes to the stream
            Write(bytes);
        }

        #endregion

        #region Public Override Methods

        /// <summary>
        /// Writes a boolean to the stream as the specified integer length
        /// </summary>
        /// <param name="value">The value to write</param>
        public override void Write(bool value)
        {
            switch (Settings.BoolEncoding)
            {
                case BinaryBoolEncoding.Byte:
                    Write((byte)(value ? 1 : 0));
                    break;

                case BinaryBoolEncoding.Int16:
                    Write((short)(value ? 1 : 0));
                    break;

                case BinaryBoolEncoding.Int32:
                    Write(value ? 1 : 0);
                    break;

                case BinaryBoolEncoding.Int64:
                    Write((long)(value ? 1 : 0));
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(Settings.BoolEncoding), Settings.BoolEncoding, null);
            }
        }

        /// <summary>
        /// Writes a string to the stream based on the selected <see cref="BinaryStringEncoding"/>
        /// </summary>
        /// <param name="value">The value to write</param>
        public override void Write(string value)
        {
            switch (Settings.StringEncoding)
            {
                case BinaryStringEncoding.BinaryReaderStandard:
                    base.Write(value);
                    break;

                case BinaryStringEncoding.NullTerminated:
                    WriteNullTerminatedString(value);
                    break;

                case BinaryStringEncoding.LengthPrefixed:
                    WriteLengthPrefixedString(value);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

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