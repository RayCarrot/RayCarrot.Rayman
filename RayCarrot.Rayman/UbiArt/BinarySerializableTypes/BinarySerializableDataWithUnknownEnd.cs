using System.IO;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// A data model with a <see cref="IBinarySerializable"/> data and unknown remaining bytes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BinarySerializableDataWithUnknownEnd<T> : BinarySerializableDataWithUnknownEnd, IBinarySerializable
        where T : IBinarySerializable, new()
    {
        /// <summary>
        /// The primary data
        /// </summary>
        public new T Data
        {
            get => base.Data.CastTo<T>();
            set => base.Data = value;
        }

        /// <summary>
        /// Deserializes the data from the serializer into this instance
        /// </summary>
        /// <summary>
        /// Deserializes the data from the serializer into this instance
        /// </summary>
        /// <param name="stream">The stream to deserialize from</param>
        /// <param name="deserializer">The deserializer to get the data from</param>
        public virtual void Deserialize(FileStream stream, IBinaryDeserializer deserializer)
        {
            // Create primary data
            Data = new T();

            // Deserialize primary data
            Data.Deserialize(stream, deserializer);

            // Read remaining bytes
            RemainingBytes = stream.ReadRemainingBytes();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="stream">The stream to serialize to</param>
        /// <param name="serializer">The serializer</param>
        public void Serialize(FileStream stream, IBinarySerializer serializer)
        {
            // Serialize primary data
            serializer.Serialize(stream, Data);

            // Write remaining bytes
            stream.Write(RemainingBytes, 0, RemainingBytes.Length);
        }
    }

    /// <summary>
    /// A non-generic data model for a serializable binary data with unknown bytes remaining
    /// </summary>
    public class BinarySerializableDataWithUnknownEnd
    {
        /// <summary>
        /// The primary data
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// The remaining bytes of the file
        /// </summary>
        public byte[] RemainingBytes { get; set; }
    }
}