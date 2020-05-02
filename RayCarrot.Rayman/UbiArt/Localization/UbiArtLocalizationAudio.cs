using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// UbiArt localization audio data
    /// </summary>
    public class UbiArtLocalizationAudio : IBinarySerializable
    {
        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown0 { get; set; }

        /// <summary>
        /// The audio file
        /// </summary>
        public string AudioFile { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown1 { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Unknown0 = s.Serialize<uint>(Unknown0, name: nameof(Unknown0));
            AudioFile = s.SerializeLengthPrefixedString(AudioFile, name: nameof(AudioFile));
            Unknown1 = s.Serialize<uint>(Unknown1, name: nameof(Unknown1));
        }
    }
}