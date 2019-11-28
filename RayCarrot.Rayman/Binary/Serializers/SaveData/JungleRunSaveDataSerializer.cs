namespace RayCarrot.Rayman
{
    /// <summary>
    /// The progression data serializer for Rayman Jungle Run
    /// </summary>
    public class JungleRunSaveDataSerializer : BinaryDataSerializer<JungleRunSaveData>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public JungleRunSaveDataSerializer() : base(new BinarySeriaizerSettings(false))
        {

        }
    }
}