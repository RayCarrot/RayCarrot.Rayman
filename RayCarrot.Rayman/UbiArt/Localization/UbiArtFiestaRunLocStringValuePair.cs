using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// Localization string value pair for Rayman Fiesta Run
    /// </summary>
    public class UbiArtFiestaRunLocStringValuePair : IBinarySerializable
    {
        /// <summary>
        /// The localization key
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// The localization value
        /// </summary>
        public UbiArtKeyObjValuePair<int, UbiArtKeyValuePair<string, string>>[] Value { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Key = s.Serialize<int>(Key, name: nameof(Key));
            Value = s.SerializeUbiArtObjectArray<UbiArtKeyObjValuePair<int, UbiArtKeyValuePair<string, string>>>(Value, name: nameof(Value));
        }
    }
}