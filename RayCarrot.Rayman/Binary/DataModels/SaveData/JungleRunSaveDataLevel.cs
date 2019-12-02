using System;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The level progression data for Rayman Jungle Run
    /// </summary>
    public class JungleRunSaveDataLevel : IBinarySerializable
    {
        /// <summary>
        /// Indicates if the level is locked
        /// </summary>
        public bool IsLocked { get; set; }

        /// <summary>
        /// The highest amount of Lums earned in the level. Max is 100.
        /// </summary>
        public short LumsRecord { get; set; }

        /// <summary>
        /// The level record time. This value is only used for the Livid Dead levels.
        /// </summary>
        public TimeSpan RecordTime { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            IsLocked = reader.Read<bool>();
            LumsRecord = reader.Read<short>();
            RecordTime = new TimeSpan(0, 0, 0, 0, (int)reader.Read<uint>());
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            writer.Write(IsLocked);
            writer.Write(LumsRecord);
            writer.Write((uint)RecordTime.TotalMilliseconds);
        }
    }
}