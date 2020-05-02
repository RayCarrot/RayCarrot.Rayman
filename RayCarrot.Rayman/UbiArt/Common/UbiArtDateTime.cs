using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// Date time data for a UbiArt game
    /// </summary>
    public class UbiArtDateTime : IBinarySerializable
    {
        public ulong Value { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Value = s.Serialize<ulong>(Value, name: nameof(Value));
        }
    }
}