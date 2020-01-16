namespace RayCarrot.Rayman
{
    /// <summary>
    /// The serializer for the .lev files in Rayman 1
    /// </summary>
    public class Rayman1LevSerializer : ConfigurableBinaryDataSerializer<Rayman1LevData, Rayman1Settings>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        public Rayman1LevSerializer(Rayman1Settings settings) : base(settings)
        { }
    }
}