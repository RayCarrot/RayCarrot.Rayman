using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The binary serializer to use for standard serializable data
    /// </summary>
    /// <typeparam name="T">The type of data to serialize to. Either a supported value type of a class implementing <see cref="IBinarySerializable"/></typeparam>
    public abstract class StandardBinaryDataSerializer<T> : BinaryDataSerializer<T>
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="byteOrder">The byte order to use</param>
        /// <param name="textEncoding">The encoding to use</param>
        /// <param name="binaryStringEncoding">The binary string encoding to use when reading strings</param>
        protected StandardBinaryDataSerializer(ByteOrder byteOrder, TextEncoding? textEncoding = null, BinaryStringEncoding binaryStringEncoding = BinaryStringEncoding.LengthPrefixed)
        {
            ByteOrder = byteOrder;
            TextEncoding = textEncoding ?? TextEncoding.UTF8;
            BinaryStringEncoding = binaryStringEncoding;
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The byte order to use
        /// </summary>
        protected ByteOrder ByteOrder { get; }

        /// <summary>
        /// The encoding to use
        /// </summary>
        protected TextEncoding TextEncoding { get; }

        /// <summary>
        /// The binary string encoding to use when reading strings
        /// </summary>
        protected BinaryStringEncoding BinaryStringEncoding { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a new binary reader to use for the specified stream
        /// </summary>
        public override BinaryReader GetBinaryReader(Stream stream) => new StandardBinaryReader(stream, ByteOrder, TextEncoding.GetEncoding(), BinaryStringEncoding, true);

        /// <summary>
        /// Gets a new binary writer to use for the specified stream
        /// </summary>
        public override BinaryWriter GetBinaryWriter(Stream stream) => new StandardBinaryWriter(stream, ByteOrder, TextEncoding.GetEncoding(), BinaryStringEncoding, true);

        /// <summary>
        /// Deserializes the data from the serialized file as a stream
        /// </summary>
        /// <param name="stream">The file stream to deserialize</param>
        /// <returns>The deserialized object</returns>
        public override T Deserialize(Stream stream)
        {
            // Get the reader
            using var binaryReader = GetBinaryReader(stream);

            // Create the wrapper reader
            using var reader = new BinaryDataReader(binaryReader, false);

            // Deserialize the data
            return reader.Read<T>();
        }

        #endregion
    }
}