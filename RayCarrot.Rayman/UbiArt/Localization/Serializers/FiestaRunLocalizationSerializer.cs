namespace RayCarrot.Rayman
{
    /// <summary>
    /// The localization serializer for Rayman Fiesta Run
    /// </summary>
    public class FiestaRunLocalizationSerializer : StandardBinaryDataSerializer<FiestaRunLocalizationData>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public FiestaRunLocalizationSerializer() : base(ByteOrder.BigEndian, TextEncoding.BigEndianUnicode)
        { }
    }
}