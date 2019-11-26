using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Used for a binary serializer
    /// </summary>
    public interface IBinarySerializer
    {
        /// <summary>
        /// Serializes the value of the specified type
        /// </summary>
        /// <typeparam name="T">The type of data to serialize</typeparam>
        /// <param name="stream">The stream to serialize to</param>
        /// <param name="value">The value to serialize</param>
        void Serialize<T>(FileStream stream, T value);
    }
}