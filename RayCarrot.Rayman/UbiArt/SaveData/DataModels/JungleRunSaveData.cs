using System;
using System.IO;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The save file data used for Rayman Jungle Run in the three .dat files
    /// </summary>
    public class JungleRunSaveData : IBinarySerializable
    {
        /// <summary>
        /// The file begins with two unknown bytes, presumably a 16-bit integer. The value is always 3.
        /// </summary>
        public short Unknown { get; set; }

        /// <summary>
        /// The data for the levels. The count is always 70.
        /// </summary>
        public JungleRunSaveDataLevelCollection Levels { get; set; }

        /// <summary>
        /// The remaining bytes in the file. Currently there is 1 known remaining byte which is always 0.
        /// </summary>
        public byte[] RemainingBytes { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="stream">The stream to deserialize from</param>
        /// <param name="deserializer">The deserializer</param>
        public void Deserialize(FileStream stream, IBinaryDeserializer deserializer)
        {
            Unknown = deserializer.Deserialize<short>(stream);
            Levels = new JungleRunSaveDataLevelCollection(70);
            Levels.Deserialize(stream, deserializer);

            // Read remaining bytes
            RemainingBytes = stream.ReadRemainingBytes();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="stream">The stream to serialize to</param>
        /// <param name="serializer">The serializer</param>
        public void Serialize(FileStream stream, IBinarySerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}