namespace RayCarrot.Rayman
{
    /// <summary>
    /// The serializer for texture files in UbiArt games
    /// </summary>
    public class UbiArtTextureSerializer : ConfigurableBinaryDataSerializer<UbiArtTEXFile, UbiArtSettings>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        public UbiArtTextureSerializer(UbiArtSettings settings) : base(settings, settings.TextEncoding)
        { }
    }
}