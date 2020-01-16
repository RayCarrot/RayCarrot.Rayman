namespace RayCarrot.Rayman
{
    /// <summary>
    /// The serializer for the .cnt files in OpenSpace games
    /// </summary>
    public class OpenSpaceCntSerializer : ConfigurableBinaryDataSerializer<OpenSpaceCntData, OpenSpaceSettings>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        public OpenSpaceCntSerializer(OpenSpaceSettings settings) : base(settings, TextEncoding.UTF8)
        { }
    }
}