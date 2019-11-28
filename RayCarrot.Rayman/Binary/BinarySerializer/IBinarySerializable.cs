using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Used for objects which can be serialized using a <see cref="IBinarySerializer"/>
    /// </summary>
    public interface IBinarySerializable
    {
        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="stream">The stream to deserialize from</param>
        /// <param name="deserializer">The deserializer</param>
        void Deserialize(FileStream stream, IBinaryDeserializer deserializer);

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="stream">The stream to serialize to</param>
        /// <param name="serializer">The serializer</param>
        void Serialize(FileStream stream, IBinarySerializer serializer);
    }
}