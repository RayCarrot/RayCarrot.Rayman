using System.Collections.Generic;

namespace RayCarrot.Rayman
{
    // IDEA: Make an attribute which the reader uses to determine the size of the list. If the size comes from a previous value, have overload where a string (propertyName) is passed in
    /// <summary>
    /// A list with a fixed size which implements <see cref="IBinarySerializable{Settings}"/>
    /// </summary>
    public class BinarySerializableList<T> : List<T>, IBinarySerializable<BinarySerializerSettings>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public BinarySerializableList()
        {
            
        }

        /// <summary>
        /// Default constructor for a fixed size
        /// </summary>
        /// <param name="capacity">The capacity of the list</param>
        public BinarySerializableList(int capacity) : base(capacity)
        { }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<BinarySerializerSettings> reader)
        {
            for (int i = 0; i < Capacity; i++)
                Add(reader.Read<T>());
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<BinarySerializerSettings> writer)
        {
            foreach (var levelData in this)
                writer.Write(levelData);
        }
    }
}