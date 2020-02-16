namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The save file data used for Rayman Jungle Run in the three .dat files on PC
    /// </summary>
    public class JungleRunPCSaveData : IBinarySerializable<BinarySerializerSettings>
    {
        /// <summary>
        /// Gets the default serializer
        /// </summary>
        public static BinaryDataSerializer<JungleRunPCSaveData, BinarySerializerSettings> GetSerializer() => new BinaryDataSerializer<JungleRunPCSaveData, BinarySerializerSettings>(new BinarySerializerSettings()
        {
            ByteOrder = ByteOrder.LittleEndian
        });

        /// <summary>
        /// The file begins with two unknown bytes, presumably a 16-bit integer. The value is always 3.
        /// </summary>
        public short Unknown { get; set; }

        /// <summary>
        /// The data for the levels. The count is always 70.
        /// </summary>
        public BinarySerializableList<JungleRunPCSaveDataLevel> Levels { get; set; }

        /// <summary>
        /// The remaining bytes in the file. Currently there is 1 known remaining byte which is always 0.
        /// </summary>
        public byte[] RemainingBytes { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<BinarySerializerSettings> reader)
        {
            // Read the unknown value
            Unknown = reader.Read<short>();

            // Create the level collection
            Levels = new BinarySerializableList<JungleRunPCSaveDataLevel>(70);

            // Read the level collection
            Levels.Deserialize(reader);

            // Read remaining bytes
            RemainingBytes = reader.ReadRemainingBytes();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<BinarySerializerSettings> writer)
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