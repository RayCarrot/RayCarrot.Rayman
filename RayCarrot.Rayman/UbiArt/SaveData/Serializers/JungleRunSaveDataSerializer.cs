﻿using System.IO;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The progression data serializer for Rayman Jungle Run
    /// </summary>
    public class JungleRunSaveDataSerializer : BinaryDataSerializer<JungleRunSaveData>
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