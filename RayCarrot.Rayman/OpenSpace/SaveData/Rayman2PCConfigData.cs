using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// Rayman 2 config data for .cfg files on PC
    /// </summary>
    public class Rayman2PCConfigData : IBinarySerializable
    {
        public int VersionLength { get; set; }
        public string Version { get; set; }

        /// <summary>
        /// The amount of save slots
        /// </summary>
        public int SlotCount { get; set; }

        /// <summary>
        /// The available save slots
        /// </summary>
        public Rayman2PCConfigSlotData[] Slots { get; set; }

        // Always 0?
        public int UnkDword1 { get; set; }
        public int UnkDword2 { get; set; }

        // Max is 100
        public int Lumonosity { get; set; }

        // 127 is max
        public int SoundEffectVolume { get; set; }
        public int MusicVolume { get; set; }

        // Also some volume?
        public int UnkDword6 { get; set; }

        // Joystick related?
        public byte[] Unk2 { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            VersionLength = s.Serialize<int>(VersionLength, name: nameof(VersionLength));
            Version = s.SerializeString(Version, VersionLength + 1, name: nameof(Version));
            SlotCount = s.Serialize<int>(SlotCount, name: nameof(SlotCount));
            Slots = s.SerializeObjectArray<Rayman2PCConfigSlotData>(Slots, SlotCount, name: nameof(Slots));
            UnkDword1 = s.Serialize<int>(UnkDword1, name: nameof(UnkDword1));
            UnkDword2 = s.Serialize<int>(UnkDword2, name: nameof(UnkDword2));
            Lumonosity = s.Serialize<int>(Lumonosity, name: nameof(Lumonosity));
            SoundEffectVolume = s.Serialize<int>(SoundEffectVolume, name: nameof(SoundEffectVolume));
            MusicVolume = s.Serialize<int>(MusicVolume, name: nameof(MusicVolume));
            UnkDword6 = s.Serialize<int>(UnkDword6, name: nameof(UnkDword6));
            Unk2 = s.SerializeArray<byte>(Unk2, 7, name: nameof(Unk2));
        }
    }
}