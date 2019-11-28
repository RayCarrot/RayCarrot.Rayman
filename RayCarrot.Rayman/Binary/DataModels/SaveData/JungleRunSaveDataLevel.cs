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
        /// <param name="stream">The stream to deserialize from</param>
        /// <param name="deserializer">The deserializer</param>
        public void Deserialize(FileStream stream, IBinaryDeserializer deserializer)
        {
            IsLocked = deserializer.Deserialize<bool>(stream);
            LumsRecord = deserializer.Deserialize<short>(stream);

            // NOTE: The actual type is an unsigned integer, but time span does not support that
            RecordTime = new TimeSpan(0, 0, 0, 0, deserializer.Deserialize<int>(stream));

        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="stream">The stream to serialize to</param>
        /// <param name="serializer">The serializer</param>
        public void Serialize(FileStream stream, IBinarySerializer serializer)
        {
            serializer.Serialize(stream, IsLocked);
            serializer.Serialize(stream, LumsRecord);
            serializer.Serialize(stream, (int)RecordTime.TotalMilliseconds);
        }
    }
}