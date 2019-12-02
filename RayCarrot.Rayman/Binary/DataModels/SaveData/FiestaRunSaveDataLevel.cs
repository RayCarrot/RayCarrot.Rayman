using System;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The level progression data for Rayman Fiesta Run
    /// </summary>
    public class FiestaRunSaveDataLevel : IBinarySerializable
    {
        /// <summary>
        /// The last amount of Lums earned in the level. Max is 100.
        /// </summary>
        public byte Lums { get; set; }

        /// <summary>
        /// The number of Teensies freed in the level. Max is 4.
        /// </summary>
        public byte TeensiesFreed { get; set; }

        /// <summary>
        /// Unknown byte. Is set to 6 when the level has a crown.
        /// </summary>
        public byte Unknown1 { get; set; }

        /// <summary>
        /// 11 unknown bytes
        /// </summary>
        public byte[] Unknown2 { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            Lums = reader.Read<byte>();
            TeensiesFreed = reader.Read<byte>();
            Unknown1 = reader.Read<byte>();
            Unknown2 = reader.ReadBytes(11);
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            writer.Write(Lums);
            writer.Write(TeensiesFreed);
            writer.Write(Unknown1);

            foreach (byte b in Unknown2)
                writer.Write(b);
        }

        public override string ToString()
        {
            return $"Teensies = {TeensiesFreed}, Unk1 = {Unknown1}";
        }
    }
}