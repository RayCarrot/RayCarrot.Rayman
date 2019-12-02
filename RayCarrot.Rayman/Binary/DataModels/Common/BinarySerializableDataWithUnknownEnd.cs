namespace RayCarrot.Rayman
{
    /// <summary>
    /// A data model with a <see cref="IBinarySerializable"/> data and unknown remaining bytes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinarySerializableDataWithUnknownEnd<T> : IBinarySerializable
        where T : IBinarySerializable, new()
    {
        /// <summary>
        /// The primary data
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// The remaining bytes of the file
        /// </summary>
        public byte[] RemainingBytes { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            // Create primary data
            Data = new T();

            // Deserialize primary data
            Data.Deserialize(reader);

            // Read remaining bytes
            RemainingBytes = reader.ReadRemainingBytes();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            // Serialize primary data
            Data.Serialize(writer);

            // Write remaining bytes
            writer.Write(RemainingBytes);
        }
    }
}