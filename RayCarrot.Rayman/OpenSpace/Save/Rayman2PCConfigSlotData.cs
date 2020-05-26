using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// Rayman 2 config slot data
    /// </summary>
    public class Rayman2PCConfigSlotData : IBinarySerializable
    {
        /// <summary>
        /// The display name for the slot, including the percentage
        /// </summary>
        public string SlotDisplayName { get; set; }

        // Fourth byte is slot index
        public byte[] Unk1 { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            SlotDisplayName = s.SerializeString(SlotDisplayName, 8, name: nameof(SlotDisplayName));
            Unk1 = s.SerializeArray<byte>(Unk1, 7, name: nameof(Unk1));
        }
    }
}