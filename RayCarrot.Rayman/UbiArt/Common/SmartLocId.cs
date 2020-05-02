using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    public class SmartLocId : IBinarySerializable
    {
        public string DefaultText { get; set; }

        public LocalisationId LocId { get; set; }

        public bool UseText { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            DefaultText = s.SerializeLengthPrefixedString(DefaultText, name: nameof(DefaultText));
            LocId = s.SerializeObject<LocalisationId>(LocId, name: nameof(LocId));
            UseText = s.SerializeBool<uint>(UseText, name: nameof(UseText));
        }
    }
}