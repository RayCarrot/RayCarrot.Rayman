using System.Collections.Generic;
using System.IO;

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
        /// <param name="stream">The stream to deserialize from</param>
        /// <param name="deserializer">The deserializer</param>
        public virtual void Deserialize(FileStream stream, IBinaryDeserializer deserializer)
        {
            // Read the dictionary length
            var length = deserializer.Deserialize<int>(stream);

            // Enumerate each item
            for (int i = 0; i < length; i++)
            {
                // Get the key
                var key = deserializer.Deserialize<TKey>(stream);

                // Get the value
                var value = deserializer.Deserialize<TValue>(stream);

                // Add the item
                Add(key, value);
            }
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="stream">The stream to serialize to</param>
        /// <param name="serializer">The serializer</param>
        public void Serialize(FileStream stream, IBinarySerializer serializer)
        {
            // Serialize the dictionary length
            serializer.Serialize(stream, Count);

            // Enumerate each item
            foreach (var item in this)
            {
                // Serialize the key
                serializer.Serialize(stream, item.Key);

                // Serialize the value
                serializer.Serialize(stream, item.Value);
            }
        }
    }
}