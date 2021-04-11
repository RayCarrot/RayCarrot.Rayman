using RayCarrot.Binary;

namespace RayCarrot.Rayman.Ray1
{
    /// <summary>
    /// The data used for a file entry within a Rayman 1 PC archive file
    /// </summary>
    public class Rayman1PCArchiveEntry : IBinarySerializable
    {
        /// <summary>
        /// The XOR key to use when decoding the file
        /// </summary>
        public byte XORKey { get; set; }

        /// <summary>
        /// The encoded file checksum
        /// </summary>
        public byte Checksum { get; set; }

        /// <summary>
        /// The file offset
        /// </summary>
        public uint FileOffset { get; set; }

        /// <summary>
        /// The file size
        /// </summary>
        public uint FileSize { get; set; }

        /// <summary>
        /// The file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            // Get the settings
            var settings = s.GetSettings<Ray1Settings>();

            if (settings.Game == Ray1Game.Rayman1)
            {
                FileOffset = s.Serialize<uint>(FileOffset, name: nameof(FileOffset));
                FileSize = s.Serialize<uint>(FileSize, name: nameof(FileSize));
                XORKey = s.Serialize<byte>(XORKey, name: nameof(XORKey));
                Checksum = s.Serialize<byte>(Checksum, name: nameof(Checksum));

                s.SerializeArray<byte>(new byte[2], 2, name: "Padding");
            }
            else
            {
                XORKey = s.Serialize<byte>(XORKey, name: nameof(XORKey));
                Checksum = s.Serialize<byte>(Checksum, name: nameof(Checksum));
                FileOffset = s.Serialize<uint>(FileOffset, name: nameof(FileOffset));
                FileSize = s.Serialize<uint>(FileSize, name: nameof(FileSize));

                s.DoXOR(XORKey, () => FileName = s.SerializeString(FileName, 9, name: nameof(FileName)));
            }
        }
    }
}