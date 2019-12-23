using System.Collections.Generic;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// A <see cref="Dictionary{TKey,TValue}"/> which implements <see cref="IBinarySerializable"/>
    /// </summary>
    /// <typeparam name="TKey">The type of key</typeparam>
    /// <typeparam name="TValue">The type of value</typeparam>
    public class BinarySerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IBinarySerializable
    {
        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            // Read the dictionary length
            var length = reader.Read<int>();

            // Enumerate each item
            for (int i = 0; i < length; i++)
            {
                // Get the key
                var key = reader.Read<TKey>();

                // Get the value
                var value = reader.Read<TValue>();

                // Add the item
                Add(key, value);
            }
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            // Serialize the dictionary length
            writer.Write(Count);

            // Enumerate each item
            foreach (var item in this)
            {
                // Serialize the key
                writer.Write(item.Key);

                // Serialize the value
                writer.Write(item.Value);
            }
        }
    }
}