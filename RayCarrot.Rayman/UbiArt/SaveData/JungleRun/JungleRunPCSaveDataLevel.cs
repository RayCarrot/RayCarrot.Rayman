using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The level progression data for Rayman Jungle Run on PC
    /// </summary>
    public class JungleRunPCSaveDataLevel : IBinarySerializable
    {
        /// <summary>
        /// Indicates if the level is locked
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// The highest amount of Lums earned in the level. Max is 100.
        /// </summary>
        public ushort LumsRecord { get; set; }

        /// <summary>
        /// The level record time in milliseconds. This value is only used for the Livid Dead levels.
        /// </summary>
        public uint RecordTime { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            IsLocked = s.SerializeBool<byte>(IsLocked, name: nameof(IsLocked));
            LumsRecord = s.Serialize<ushort>(LumsRecord, name: nameof(LumsRecord));
            RecordTime = s.Serialize<uint>(RecordTime, name: nameof(RecordTime));
        }
    }
}