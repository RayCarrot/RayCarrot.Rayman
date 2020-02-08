using System.Collections.Generic;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// A list with a fixed size which implements <see cref="IBinarySerializable"/>
    /// </summary>
    public class BinarySerializableList<T> : List<T>, IBinarySerializable
    {
        /// <summary>
        /// Default constructor for a fixed list where the first 4 bytes represent the capacity
        /// </summary>
        public BinarySerializableList()
        {
            ReadCapacity = true;
        }

        /// <summary>
        /// Default constructor for a fixed list with a set capacity
        /// </summary>
        /// <param name="capacity">The capacity of the list</param>
        public BinarySerializableList(int capacity) : base(capacity)
        {
            ReadCapacity = false;
        }

        /// <summary>
        /// Indicates if the capacity has to be read/written from/to the stream
        /// </summary>
        protected bool ReadCapacity { get; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            if (ReadCapacity)
                Capacity = reader.Read<int>();

            for (int i = 0; i < Capacity; i++)
                Add(reader.Read<T>());
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            if (ReadCapacity)
                writer.Write(Count);

            foreach (var levelData in this)
                writer.Write(levelData);
        }
    }
}