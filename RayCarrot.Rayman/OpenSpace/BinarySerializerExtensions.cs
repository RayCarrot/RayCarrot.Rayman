using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// Extension methods for <see cref="IBinarySerializable"/>
    /// </summary>
    public static class BinarySerializerExtensions
    {
        /// <summary>
        /// Serializes an encrypted string for OpenSpace games
        /// </summary>
        /// <param name="s">The serializer</param>
        /// <param name="value">The string value</param>
        /// <param name="xorKey">The xor key to use</param>
        /// <param name="name">The string value name, for logging</param>
        /// <returns>The string value</returns>
        public static string SerializeOpenSpaceEncryptedString(this IBinarySerializer s, string value, byte xorKey, string name = null)
        {
            // Serialize the length
            var length = s.Serialize<int>(value?.Length ?? 0, name: $"{nameof(value)}.Length");

            // Serialize the string value using the xor key
            s.DoXOR(xorKey, () => value = s.SerializeString(value, length, name: $"{nameof(value)}"));

            // Return the value
            return value;
        }
    }
}