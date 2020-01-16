namespace RayCarrot.Rayman
{
    /// <summary>
    /// The localization serializer for UbiArt games
    /// </summary>
    public class UbiArtLocalizationSerializer : StandardBinaryDataSerializer<UbiArtLocalizationData>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings to use</param>
        public UbiArtLocalizationSerializer(UbiArtSettings settings) : base(settings.ByteOrder, settings.TextEncoding)
        { }
    }
}