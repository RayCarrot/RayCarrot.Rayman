using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// A pair of two values, where the key is a serializable object and the value a value
    /// </summary>
    /// <typeparam name="TKey">The key serializable object type</typeparam>
    /// <typeparam name="TValue">The serializable object type</typeparam>
    public class UbiArtObjKeyValuePair<TKey, TValue> : IBinarySerializable
        where TKey : IBinarySerializable, new()
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
            Key = s.SerializeObject<TKey>(Key, name: nameof(Key));
            Value = s.SerializeUbiArtGenericValue<TValue>(Value, name: nameof(Value));
        }
    }
}