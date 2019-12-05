using System.IO;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The serializer for Rayman 2 .cnt files
    /// </summary>
    public class R2CntSerializer : BinaryDataSerializer<R2CntData>
    {
        /// <summary>
        /// Gets a new binary reader to use for the specified stream
        /// </summary>
        protected override BinaryReader GetBinaryReader(Stream stream)
        {
            return new UbiArtBinaryReader(stream, ByteOrder.LittleEndian, Encoding.UTF8, true);
        }

        /// <summary>
        /// Gets a new binary writer to use for the specified stream
        /// </summary>
        protected override BinaryWriter GetBinaryWriter(Stream stream)
        {
            return new UbiArtBinaryWriter(stream, ByteOrder.LittleEndian, Encoding.UTF8, true);
        }
    }
}