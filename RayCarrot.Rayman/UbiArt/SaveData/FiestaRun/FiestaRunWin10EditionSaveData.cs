using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The save file data used for Rayman Fiesta Run Windows 10 Edition in the .dat file
    /// </summary>
    public class FiestaRunWin10EditionSaveData : IBinarySerializable
    {
        /// <summary>
        /// The file begins with two unknown bytes, presumably a 16-bit integer. The value is always 2.
        /// </summary>
        public ushort Unknown { get; set; }

        /// <summary>
        /// The data for the levels. The count is always 72.
        /// </summary>
        public FiestaRunPCSaveDataLevel[] Levels { get; set; }

        /// <summary>
        /// Unknown bytes
        /// </summary>
        public byte[] Unknown1 { get; set; }

        /// <summary>
        /// The number of available Lums
        /// </summary>
        public uint Lums { get; set; }

        /// <summary>
        /// The remaining unknown bytes in the file
        /// </summary>
        public byte[] Unknown2 { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            // Read the unknown value
            Unknown = s.Serialize<ushort>(Unknown, name: nameof(Unknown));

            // Create the level collection
            Levels = s.SerializeObjectArray<FiestaRunPCSaveDataLevel>(Levels, 72, name: nameof(Levels));

            // Read unknown bytes
            Unknown1 = s.SerializeArray<byte>(Unknown1, 128, name: nameof(Unknown1));

            // Read Lums
            Lums = s.Serialize<uint>(Lums, name: nameof(Lums));

            // Read remaining bytes
            Unknown2 = s.SerializeArray<byte>(Unknown2, (int)(s.Stream.Length - s.Stream.Position), name: nameof(Unknown2));
        }
    }
}