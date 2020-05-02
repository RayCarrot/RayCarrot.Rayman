using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    // NOTE: This does currently only work for the Rayman games. Just Dance and Child of Light use a different format where each language has its own file.
    /// <summary>
    /// The localization data for a UbiArt game
    /// </summary>
    public class UbiArtLocalizationData : IBinarySerializable
    {
        /// <summary>
        /// The localized strings, categorized by the language index and the localization ID
        /// </summary>
        public UbiArtLocStringValuePair[] Strings { get; set; }

        /// <summary>
        /// The audio to use for each localized string
        /// </summary>
        public UbiArtKeyObjValuePair<int, UbiArtLocalizationAudio>[] Audio { get; set; }

        /// <summary>
        /// Unknown list of paths
        /// </summary>
        public string[] Paths { get; set; }

        /// <summary>
        /// Unknown values, used in Legends and later
        /// </summary>
        public uint[] Unknown { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Strings = s.SerializeUbiArtObjectArray<UbiArtLocStringValuePair>(Strings, name: nameof(Strings));
            Audio = s.SerializeUbiArtObjectArray<UbiArtKeyObjValuePair<int, UbiArtLocalizationAudio>>(Audio, name: nameof(Audio));
            Paths = s.SerializeUbiArtArray<string>(Paths, name: nameof(Paths));

            Unknown = s.SerializeArray<uint>(Unknown, (int)(s.Stream.Length - s.Stream.Position) / sizeof(uint), name: nameof(Unknown));
        }
    }
}