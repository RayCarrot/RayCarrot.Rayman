using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// Extension methods for <see cref="IBinarySerializable"/>
    /// </summary>
    public static class BinarySerializerExtensions
    {
        /// <summary>
        /// Serializes an unknown generic value for UbiArt games
        /// </summary>
        /// <typeparam name="T">The type of value to serialize</typeparam>
        /// <param name="s">The serializer</param>
        /// <param name="value">The value</param>
        /// <param name="name">The object value name, for logging</param>
        /// <returns>The value</returns>
        public static T SerializeUbiArtGenericValue<T>(this IBinarySerializer s, T value, string name = null)
        {
            // Get the type
            var t = typeof(T);

            // Check if the value is a boolean or string, which we handle differently
            if (t == typeof(string))
            {
                return (T)(object)s.SerializeLengthPrefixedString((string)(object)value, name: name);
            }
            else if (t == typeof(bool))
            {
                return (T)(object)s.SerializeBool<uint>((bool)(object)value, name: name);
            }
            else
            {
                return s.Serialize<T>(value, name: name);
            }
        }

        public static T[] SerializeUbiArtObjectArray<T>(this IBinarySerializer s, T[] array, string name = null)
            where T : IBinarySerializable, new()
        {
            // Serialize the size
            array = s.SerializeArraySize<T, uint>(array, name: name);

            // Serialize the array
            array = s.SerializeObjectArray<T>(array, array.Length, name: name);

            // Return the array
            return array;
        }

        public static T[] SerializeUbiArtArray<T>(this IBinarySerializer s, T[] array, string name = null)
        {
            // Serialize the size
            array = s.SerializeArraySize<T, uint>(array, name: name);

            // Serialize the array values
            for (int i = 0; i < array.Length; i++)
                array[i] = s.SerializeUbiArtGenericValue<T>(array[i], name: $"{nameof(array)}[{i}]");

            // Return the array
            return array;
        }
    }
}