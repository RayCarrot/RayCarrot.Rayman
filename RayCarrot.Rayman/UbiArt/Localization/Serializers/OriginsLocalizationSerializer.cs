using System.IO;
using System.Text;

namespace RayCarrot.Rayman
{
    // NOTE: Origins on 3DS is encoded in little endian
    /// <summary>
    /// The localization serializer for Rayman Origins
    /// </summary>
    public class OriginsLocalizationSerializer : BinaryDataSerializer<StandardUbiArtLocalizationData>
    {
        /// <summary>
        /// Gets a new binary reader to use for the specified stream
        /// </summary>
        protected override BinaryReader GetBinaryReader(Stream stream)
        {
            return new StandardBinaryReader(stream, ByteOrder.BigEndian, Encoding.BigEndianUnicode, true);
        }

        /// <summary>
        /// Gets a new binary writer to use for the specified stream
        /// </summary>
        protected override BinaryWriter GetBinaryWriter(Stream stream)
        {
            return new UbiArtBinaryWriter(stream, ByteOrder.BigEndian, Encoding.BigEndianUnicode, true);
        }
    }
}