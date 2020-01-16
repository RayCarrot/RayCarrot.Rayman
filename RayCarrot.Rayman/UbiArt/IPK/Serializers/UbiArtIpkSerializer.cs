namespace RayCarrot.Rayman
{
    /// <summary>
    /// The serializer for the .ipk files in UbiArt games
    /// </summary>
    public class UbiArtIpkSerializer : ConfigurableBinaryDataSerializer<UbiArtIpkData, UbiArtSettings>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        public UbiArtIpkSerializer(UbiArtSettings settings) : base(settings, settings.TextEncoding)
        { }
    }
}