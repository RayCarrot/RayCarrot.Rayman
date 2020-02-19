using System;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The standard binary reader to use
    /// </summary>
    public class StandardBinaryReader : BinaryReader
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of <see cref="StandardBinaryReader"/> with a stream input, settings and a value indicating if the stream should be kept open when disposing
        /// </summary>
        /// <param name="input">The stream input</param>
        /// <param name="settings">The settings</param>
        /// <param name="leaveOpen">Indicates if the stream should be kept open when disposing</param>
        public StandardBinaryReader(Stream input, BinarySerializerSettings settings, bool leaveOpen) : base(input, settings.Encoding, leaveOpen)
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
        /// Indicates if the byte order should be reversed when read
        /// </summary>
        protected bool ShouldReverseBytes { get; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Reads a string from the stream using <see cref="BinaryStringEncoding.NullTerminated"/>
        /// </summary>
        /// <returns>The string</returns>
        protected virtual string ReadNullTerminatedString()
        {
            string str = "";

            char ch;

            while ((ch = ReadChar()) != 0)
                str += ch;

            return str;
        }

        /// <summary>
        /// Reads a string from the stream using <see cref="BinaryStringEncoding.LengthPrefixed"/>
        /// </summary>
        /// <returns>The string</returns>
        protected virtual string ReadLengthPrefixedString()
        {
            // Get the length
            var length = ReadInt32();

            // IDEA: Have a max length to avoid OutOfMemory exception

            // Read the bytes
            var bytes = ReadBytes(length * Settings.Encoding.GetByteCount("A"));

            // Return the string
            return Settings.Encoding.GetString(bytes);
        }

        #endregion

        #region Public Override Methods

        /// <summary>
        /// Reads a boolean as the specified integer length
        /// </summary>
        /// <returns>True if the value is 1 or false if it is 0</returns>
        public override bool ReadBoolean()
        {
            return Settings.BoolEncoding switch
            {
                BinaryBoolEncoding.Byte => (ReadByte() == 1),
                BinaryBoolEncoding.Int16 => (ReadInt16() == 1),
                BinaryBoolEncoding.Int32 => (ReadInt32() == 1),
                BinaryBoolEncoding.Int64 => (ReadInt64() == 1),
                _ => throw new ArgumentOutOfRangeException(nameof(Settings.BoolEncoding), Settings.BoolEncoding, null)
            };
        }

        /// <summary>
        /// Reads a string from the current stream based on the selected <see cref="BinaryStringEncoding"/>
        /// </summary>
        /// <returns>The string</returns>
        public override string ReadString()
        {
            return Settings.StringEncoding switch
            {
                BinaryStringEncoding.BinaryReaderStandard => base.ReadString(),
                BinaryStringEncoding.NullTerminated => ReadNullTerminatedString(),
                BinaryStringEncoding.LengthPrefixed => ReadLengthPrefixedString(),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

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