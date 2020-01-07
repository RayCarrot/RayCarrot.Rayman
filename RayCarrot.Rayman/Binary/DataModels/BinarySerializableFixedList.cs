using System.Collections.Generic;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// A list with a fixed size which implements <see cref="IBinarySerializable"/>
    /// </summary>
    public class BinarySerializableFixedList<T> : List<T>, IBinarySerializable
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="capacity">The capacity of the list</param>
        public BinarySerializableFixedList(int capacity) : base(capacity)
        {

        }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            for (int i = 0; i < Capacity; i++)
                Add(reader.Read<T>());
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            foreach (var levelData in this)
                writer.Write(levelData);
        }
    }
}