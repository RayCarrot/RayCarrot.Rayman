using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// A pair of two values, where the key and the value are values
    /// </summary>
    /// <typeparam name="TKey">The key value type</typeparam>
    /// <typeparam name="TValue">The value type</typeparam>
    public class UbiArtKeyValuePair<TKey, TValue> : IBinarySerializable
    {
        /// <summary>
        /// The key
        /// </summary>
        public TKey Key { get; set; }

        /// <summary>
        /// The value
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Key = s.SerializeUbiArtGenericValue<TKey>(Key, name: nameof(Key));
            Value = s.SerializeUbiArtGenericValue<TValue>(Value, name: nameof(Value));
        }
    }
}