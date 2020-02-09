using System.Collections.Generic;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// A list which implements <see cref="IBinarySerializable{Settings}"/>, with the capacity determined by a 32-bit integer
    /// </summary>
    public class UbiArtSerializableList<T> : List<T>, IBinarySerializable<BinarySerializerSettings>
    {
        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<BinarySerializerSettings> reader)
        {
            var count = reader.Read<int>();

            Capacity = count;

            for (int i = 0; i < count; i++)
                Add(reader.Read<T>());
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<BinarySerializerSettings> writer)
        {
            writer.Write(Count);

            foreach (var levelData in this)
                writer.Write(levelData);
        }
    }
}