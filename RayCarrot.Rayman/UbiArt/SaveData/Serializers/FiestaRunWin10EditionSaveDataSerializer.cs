namespace RayCarrot.Rayman
{
    /// <summary>
    /// The progression data serializer for Rayman Fiesta Run Windows 10 Edition
    /// </summary>
    public class FiestaRunWin10EditionSaveDataSerializer : StandardBinaryDataSerializer<FiestaRunWin10EditionSaveData>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public FiestaRunWin10EditionSaveDataSerializer() : base(ByteOrder.LittleEndian)
        { }
    }
}