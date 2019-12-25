﻿namespace RayCarrot.Rayman
{
    /// <summary>
    /// The save file data used for Rayman Fiesta Run Windows 10 Edition in the .dat file
    /// </summary>
    public class FiestaRunWin10EditionSaveData : IBinarySerializable
    {
        /// <summary>
        /// The file begins with two unknown bytes, presumably a 16-bit integer. The value is always 2.
        /// </summary>
        public short Unknown { get; set; }

        /// <summary>
        /// The data for the levels. The count is always 72.
        /// </summary>
        public FiestaRunWin10EditionSaveDataLevelCollection Levels { get; set; }

        /// <summary>
        /// Unknown bytes
        /// </summary>
        public byte[] Unknown1 { get; set; }

        /// <summary>
        /// The number of available Lums
        /// </summary>
        public int Lums { get; set; }

        /// <summary>
        /// The remaining unknown bytes in the file
        /// </summary>
        public byte[] Unknown2 { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            // Read the unknown value
            Unknown = reader.Read<short>();

            // Read the level collection
            Levels = reader.Read<FiestaRunWin10EditionSaveDataLevelCollection>();

            // Read unknown bytes
            Unknown1 = reader.ReadBytes(128);

            // Read Lums
            Lums = reader.Read<int>();

            // Read remaining bytes
            Unknown2 = reader.ReadRemainingBytes();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            // Write the unknown value
            writer.Write(Unknown);

            // Serialize the level collection
            writer.Write(Levels);

            // Write unknown bytes
            writer.Write(Unknown1);

            // Write Lums
            writer.Write(Lums);

            // Write remaining bytes
            writer.Write(Unknown2);
        }
    }
}