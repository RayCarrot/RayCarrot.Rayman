namespace RayCarrot.Rayman
{
    /// <summary>
    /// The data for a Rayman 3 save file level
    /// </summary>
    public class Rayman3SaveDataLevel : IBinarySerializable
    {
        /// <summary>
        /// The cages
        /// </summary>
        public int Cages { get; set; }

        /// <summary>
        /// The score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            Cages = reader.Read<int>();
            Score = reader.Read<int>();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            writer.Write(Cages);
            writer.Write(Score);
        }
    }
}