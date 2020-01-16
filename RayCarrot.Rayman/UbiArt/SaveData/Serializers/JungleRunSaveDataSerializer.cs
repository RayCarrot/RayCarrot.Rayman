namespace RayCarrot.Rayman
{
    /// <summary>
    /// The progression data serializer for Rayman Jungle Run
    /// </summary>
    public class JungleRunSaveDataSerializer : StandardBinaryDataSerializer<JungleRunSaveData>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public JungleRunSaveDataSerializer() : base(ByteOrder.LittleEndian)
        { }
    }
}