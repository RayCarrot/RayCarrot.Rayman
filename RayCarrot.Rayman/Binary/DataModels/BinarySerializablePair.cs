namespace RayCarrot.Rayman
{
    /// <summary>
    /// A pair of two items which implements <see cref="IBinarySerializable"/>
    /// </summary>
    /// <typeparam name="T1">The type of the first item</typeparam>
    /// <typeparam name="T2">The type of the second item</typeparam>
    public class BinarySerializablePair<T1, T2> : IBinarySerializable
    {
        /// <summary>
        /// The first item
        /// </summary>
        public T1 Item1 { get; set; }

        /// <summary>
        /// The second item
        /// </summary>
        public T2 Item2 { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            Item1 = reader.Read<T1>();
            Item2 = reader.Read<T2>();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            writer.Write(Item1);
            writer.Write(Item2);
        }
    }
}