namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The localization data for a UbiArt game
    /// </summary>
    public class UbiArtLocalizationData : BaseUbiArtLocalizationData<string>
    {
        /// <summary>
        /// Gets the default serializer
        /// </summary>
        public static BinaryDataSerializer<UbiArtLocalizationData, UbiArtSettings> GetSerializer(UbiArtSettings settings) => new BinaryDataSerializer<UbiArtLocalizationData, UbiArtSettings>(settings);
    }
}