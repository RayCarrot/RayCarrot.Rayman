using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// Rayman 2 config data for .cfg files on PC
    /// </summary>
    public class Rayman2PCConfigData : IBinarySerializable
    {
        public uint UnkDword0 { get; set; }

        public byte[] Unk1 { get; set; }

        /// <summary>
        /// The amount of save slots
        /// </summary>
        public int SlotCount { get; set; }

        /// <summary>
        /// The available save slots
        /// </summary>
        public Rayman2PCConfigSlotData[] Slots { get; set; }

        // Always 0?
        public uint UnkDword1 { get; set; }
        public uint UnkDword2 { get; set; }

        // Max is 100
        public uint Lumonosity { get; set; }

        // 127 is max
        public uint SoundEffectVolume { get; set; }
        public uint MusicVolume { get; set; }

        // Also some volume?
        public uint UnkDword6 { get; set; }

        public byte[] Unk2 { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            UnkDword0 = s.Serialize<uint>(UnkDword0, name: nameof(UnkDword0));
            Unk1 = s.SerializeArray<byte>(Unk1, 3, name: nameof(Unk1));
            SlotCount = s.Serialize<int>(SlotCount, name: nameof(SlotCount));
            Slots = s.SerializeObjectArray<Rayman2PCConfigSlotData>(Slots, SlotCount, name: nameof(Slots));
            UnkDword1 = s.Serialize<uint>(UnkDword1, name: nameof(UnkDword1));
            UnkDword2 = s.Serialize<uint>(UnkDword2, name: nameof(UnkDword2));
            Lumonosity = s.Serialize<uint>(Lumonosity, name: nameof(Lumonosity));
            SoundEffectVolume = s.Serialize<uint>(SoundEffectVolume, name: nameof(SoundEffectVolume));
            MusicVolume = s.Serialize<uint>(MusicVolume, name: nameof(MusicVolume));
            UnkDword6 = s.Serialize<uint>(UnkDword6, name: nameof(UnkDword6));
            Unk2 = s.SerializeArray<byte>(Unk2, 7, name: nameof(Unk2));
        }
    }
}