using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The localization data for Rayman Fiesta Run
    /// </summary>
    public class FiestaRunLocalizationData : IBinarySerializable
    {
        /// <summary>
        /// The localized strings, categorized by the language index and the localization ID
        /// </summary>
        public UbiArtFiestaRunLocStringValuePair[] Strings { get; set; }

        /// <summary>
        /// The audio to use for each localized string
        /// </summary>
        public UbiArtKeyObjValuePair<int, UbiArtLocalizationAudio>[] Audio { get; set; }

        /// <summary>
        /// Unknown list of paths
        /// </summary>
        public string[] Paths { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Strings = s.SerializeUbiArtObjectArray<UbiArtFiestaRunLocStringValuePair>(Strings, name: nameof(Strings));
            Audio = s.SerializeUbiArtObjectArray<UbiArtKeyObjValuePair<int, UbiArtLocalizationAudio>>(Audio, name: nameof(Audio));
            Paths = s.SerializeUbiArtArray<string>(Paths, name: nameof(Paths));
        }
    }
}