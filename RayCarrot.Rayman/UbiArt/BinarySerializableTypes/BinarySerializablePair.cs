using System.IO;

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
        /// <param name="stream">The stream to deserialize from</param>
        /// <param name="deserializer">The deserializer</param>
        public virtual void Deserialize(FileStream stream, IBinaryDeserializer deserializer)
        {
            Item1 = deserializer.Deserialize<T1>(stream);
            Item2 = deserializer.Deserialize<T2>(stream);
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="stream">The stream to serialize to</param>
        /// <param name="serializer">The serializer</param>
        public void Serialize(FileStream stream, IBinarySerializer serializer)
        {
            serializer.Serialize(stream, Item1);
            serializer.Serialize(stream, Item2);
        }
    }
}