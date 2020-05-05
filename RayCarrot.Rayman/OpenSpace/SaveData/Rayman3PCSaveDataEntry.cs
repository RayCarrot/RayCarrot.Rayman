using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// Data entry in a Rayman 3 PC save file
    /// </summary>
    public class Rayman3PCSaveDataEntry : IBinarySerializable
    {
        /// <summary>
        /// The length of the key
        /// </summary>
        public byte KeyLength { get; set; }

        /// <summary>
        /// The key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The data
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            KeyLength = s.Serialize<byte>(KeyLength, name: nameof(KeyLength));
            Key = s.SerializeString(Key, KeyLength, name: nameof(Key));
            Data = s.SerializeArray<byte>(Data, 6, name: nameof(Data));
        }
    }
}