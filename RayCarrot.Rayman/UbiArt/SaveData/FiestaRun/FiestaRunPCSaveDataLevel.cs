using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The level progression data for Rayman Fiesta Run on PC
    /// </summary>
    public class FiestaRunPCSaveDataLevel : IBinarySerializable
    {
        /// <summary>
        /// The last amount of Lums earned in the level. Max is 100.
        /// </summary>
        public byte Lums { get; set; }

        /// <summary>
        /// The number of Teensies freed in the level. Max is 4.
        /// </summary>
        public byte TeensiesFreed { get; set; }

        /// <summary>
        /// Unknown byte. Is set to 6 when the level has a crown.
        /// </summary>
        public byte Unknown1 { get; set; }

        /// <summary>
        /// 11 unknown bytes
        /// </summary>
        public byte[] Unknown2 { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Lums = s.Serialize<byte>(Lums, name: nameof(Lums));
            TeensiesFreed = s.Serialize<byte>(TeensiesFreed, name: nameof(TeensiesFreed));
            Unknown1 = s.Serialize<byte>(Unknown1, name: nameof(Unknown1));
            Unknown2 = s.SerializeArray<byte>(Unknown2, 11, name: nameof(Unknown2));
        }

        public override string ToString()
        {
            return $"Teensies = {TeensiesFreed}, Unk1 = {Unknown1}";
        }
    }
}