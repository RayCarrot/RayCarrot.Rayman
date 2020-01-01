using System;
using System.IO;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The serializer for the .lev files in Rayman 1
    /// </summary>
    public class Rayman1LevSerializer : BinaryDataSerializer<Rayman1LevData>
    {
        /// <summary>
        /// Gets a new binary reader to use for the specified stream
        /// </summary>
        protected override BinaryReader GetBinaryReader(Stream stream)
        {
            return new StandardBinaryReader(stream, ByteOrder.LittleEndian, Encoding.Unicode);
        }

        /// <summary>
        /// Gets a new binary writer to use for the specified stream
        /// </summary>
        protected override BinaryWriter GetBinaryWriter(Stream stream)
        {
            throw new NotImplementedException();
        }
    }
}