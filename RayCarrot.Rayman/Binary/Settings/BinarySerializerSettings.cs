using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Common settings for serializing with an additional configuration
    /// </summary>
    public class BinarySerializerSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BinarySerializerSettings()
        {
            ByteOrder = ByteOrder.LittleEndian;
            Encoding = Encoding.UTF8;
            StringEncoding = BinaryStringEncoding.LengthPrefixed;
            BoolEncoding = BinaryBoolEncoding.Byte;
        }

        /// <summary>
        /// The byte order to use
        /// </summary>
        public ByteOrder ByteOrder { get; set; }

        /// <summary>
        /// The encoding to use
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// The binary string encoding to use when serializing strings
        /// </summary>
        public BinaryStringEncoding StringEncoding { get; set; }

        /// <summary>
        /// The binary boolean encoding to use when serializing booleans
        /// </summary>
        public BinaryBoolEncoding BoolEncoding { get; set; }
    }
}