using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    public class UbiArtTEXFileData : IBinarySerializable
    {
        /// <summary>
        /// The UbiArt texture header
        /// </summary>
        public UbiArtTEXData TexHeader { get; set; }

        /// <summary>
        /// The image data
        /// </summary>
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            TexHeader = s.SerializeObject<UbiArtTEXData>(TexHeader, name: nameof(TexHeader));
            ImageData = s.SerializeArray<byte>(ImageData, (int)(s.Stream.Length - s.Stream.Position), name: nameof(ImageData));
        }
    }
}