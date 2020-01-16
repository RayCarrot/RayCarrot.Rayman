using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The serializer for the Rayman 3 save files
    /// </summary>
    public class Rayman3SaveDataSerializer : StandardBinaryDataSerializer<Rayman3SaveData>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Rayman3SaveDataSerializer() : base(ByteOrder.LittleEndian, TextEncoding.UTF8)
        {
            
        }

        /// <summary>
        /// Gets a new binary reader to use for the specified stream
        /// </summary>
        public override BinaryReader GetBinaryReader(Stream stream)
        {
            return new StandardBinaryReader(new Rayman3SaveDataStream(stream, true), ByteOrder, TextEncoding.GetEncoding(), BinaryStringEncoding.NullTerminated, false);
        }

        /// <summary>
        /// Gets a new binary writer to use for the specified stream
        /// </summary>
        public override BinaryWriter GetBinaryWriter(Stream stream)
        {
            return new StandardBinaryWriter(new Rayman3SaveDataStream(stream, true), ByteOrder, TextEncoding.GetEncoding(), BinaryStringEncoding.NullTerminated, false);
        }
    }
}