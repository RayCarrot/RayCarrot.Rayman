using System.IO;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The progression data serializer for Rayman Fiesta Run Windows 10 Edition
    /// </summary>
    public class FiestaRunWin10EditionSaveDataSerializer : BinaryDataSerializer<FiestaRunWin10EditionSaveData>
    {
        /// <summary>
        /// Gets a new binary reader to use for the specified stream
        /// </summary>
        protected override BinaryReader GetBinaryReader(Stream stream)
        {
            return new StandardBinaryReader(stream, ByteOrder.LittleEndian, Encoding.UTF8, true);
        }

        /// <summary>
        /// Gets a new binary writer to use for the specified stream
        /// </summary>
        protected override BinaryWriter GetBinaryWriter(Stream stream)
        {
            return new StandardBinaryWriter(stream, ByteOrder.LittleEndian, Encoding.UTF8, true);
        }
    }
}