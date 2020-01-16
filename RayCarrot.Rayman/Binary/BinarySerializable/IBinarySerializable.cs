using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Used for objects which can be serialized using a <see cref="BinaryReader"/> and <see cref="BinaryWriter"/>
    /// </summary>
    public interface IBinarySerializable
    {
        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        void Deserialize(BinaryDataReader reader);

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        void Serialize(BinaryDataWriter writer);
    }
}