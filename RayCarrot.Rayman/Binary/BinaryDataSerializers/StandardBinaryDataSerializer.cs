using System.IO;
using RayCarrot.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The binary serializer to use for standard serializable data
    /// </summary>
    /// <typeparam name="T">The type of data to serialize to. Either a supported value type of a class implementing <see cref="IBinarySerializable"/></typeparam>
    public abstract class StandardBinaryDataSerializer<T> : BinaryDataSerializer<T>
    {
        #region Public Methods

        /// <summary>
        /// Deserializes the data from the serialized file as a stream
        /// </summary>
        /// <param name="stream">The file stream to deserialize</param>
        /// <returns>The deserialized object</returns>
        public override T Deserialize(Stream stream)
        {
            // Get the reader
            using var binaryReader = GetBinaryReader(stream);

            // Create the wrapper reader
            using var reader = new BinaryDataReader(binaryReader, false);

            // Deserialize the data
            return reader.Read<T>();
        }

        #endregion
    }
}