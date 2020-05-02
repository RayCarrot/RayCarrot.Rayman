using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The save file data used for Rayman Jungle Run in the three .dat files on PC
    /// </summary>
    public class JungleRunPCSaveData : IBinarySerializable
    {
        /// <summary>
        /// The file begins with two unknown bytes, presumably a 16-bit integer. The value is always 3.
        /// </summary>
        public ushort Unknown { get; set; }

        /// <summary>
        /// The data for the levels. The count is always 70.
        /// </summary>
        public JungleRunPCSaveDataLevel[] Levels { get; set; }

        /// <summary>
        /// The remaining bytes in the file. Currently there is 1 known remaining byte which is always 0.
        /// </summary>
        public byte[] RemainingBytes { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            // Serialize the unknown value
            Unknown = s.Serialize<ushort>(Unknown, name: nameof(Unknown));

            // Serialize the level collection
            Levels = s.SerializeObjectArray<JungleRunPCSaveDataLevel>(Levels, 70, name: nameof(Levels));

            // Serialize remaining bytes
            RemainingBytes = s.SerializeArray<byte>(RemainingBytes, (int)(s.Stream.Length - s.Stream.Position), name: nameof(RemainingBytes));
        }
    }
}