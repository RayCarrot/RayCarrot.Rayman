namespace RayCarrot.Rayman
{
    /// <summary>
    /// Used for objects which can be serialized using a <see cref="BinaryDataReader{Settings}"/> and <see cref="BinaryDataWriter{Settings}"/>
    /// </summary>
    /// <typeparam name="Settings">The type of serializer settings</typeparam>
    public interface IBinarySerializable<in Settings>
        where Settings : BinarySerializerSettings
    {
        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        void Deserialize(IBinaryDataReader<Settings> reader);

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        void Serialize(IBinaryDataWriter<Settings> writer);
    }
}