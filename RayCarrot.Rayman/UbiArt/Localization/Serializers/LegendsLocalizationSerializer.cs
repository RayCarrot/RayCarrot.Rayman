using System.IO;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The localization serializer for Rayman Legends
    /// </summary>
    public class LegendsLocalizationSerializer : BinaryDataSerializer<StandardUbiArtLocalizationData>
    {
        /// <summary>
        /// Gets a new binary reader to use for the specified stream
        /// </summary>
        protected override BinaryReader GetBinaryReader(Stream stream)
        {
            return new StandardBinaryReader(stream, ByteOrder.BigEndian, Encoding.UTF8, true);
        }

        /// <summary>
        /// Gets a new binary writer to use for the specified stream
        /// </summary>
        protected override BinaryWriter GetBinaryWriter(Stream stream)
        {
            return new StandardBinaryWriter(stream, ByteOrder.BigEndian, Encoding.UTF8, true);
        }
    }
}