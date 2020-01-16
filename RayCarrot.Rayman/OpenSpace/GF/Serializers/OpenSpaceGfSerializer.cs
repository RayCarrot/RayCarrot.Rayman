namespace RayCarrot.Rayman
{
    /// <summary>
    /// The serializer for .gf files in OpenSpace games
    /// </summary>
    public class OpenSpaceGfSerializer : ConfigurableBinaryDataSerializer<OpenSpaceGFFile, OpenSpaceSettings>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        public OpenSpaceGfSerializer(OpenSpaceSettings settings) : base(settings, TextEncoding.UTF8)
        { }
    }
}