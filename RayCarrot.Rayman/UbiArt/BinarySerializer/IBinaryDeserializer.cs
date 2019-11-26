using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Used for a binary deserializer
    /// </summary>
    public interface IBinaryDeserializer
    {
        /// <summary>
        /// Deserializes the type
        /// </summary>
        /// <typeparam name="T">The type of data to deserialize</typeparam>
        /// <param name="stream">The stream to deserialize from</param>
        /// <returns>The deserialized data</returns>
        T Deserialize<T>(FileStream stream);
    }
}