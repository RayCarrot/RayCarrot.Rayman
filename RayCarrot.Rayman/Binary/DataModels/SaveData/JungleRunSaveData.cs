namespace RayCarrot.Rayman
{
    /// <summary>
    /// The save file data used for Rayman Jungle Run in the three .dat files
    /// </summary>
    public class JungleRunSaveData : IBinarySerializable
    {
        /// <summary>
        /// The file begins with two unknown bytes, presumably a 16-bit integer. The value is always 3.
        /// </summary>
        public short Unknown { get; set; }

        /// <summary>
        /// The data for the levels. The count is always 70.
        /// </summary>
        public JungleRunSaveDataLevelCollection Levels { get; set; }

        /// <summary>
        /// The remaining bytes in the file. Currently there is 1 known remaining byte which is always 0.
        /// </summary>
        public byte[] RemainingBytes { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            // Read the unknown value
            Unknown = reader.Read<short>();

            // Read the level collection
            Levels = reader.Read<JungleRunSaveDataLevelCollection>();

            // Read remaining bytes
            RemainingBytes = reader.ReadRemainingBytes();
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

            // Write remaining bytes
            writer.Write(RemainingBytes);
        }
    }
}