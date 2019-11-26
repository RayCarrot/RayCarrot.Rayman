using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The localization serializer for Rayman Fiesta Run
    /// </summary>
    public class FiestaRunLocalizationSerializer : BinaryDataSerializer<FiestaRunUbiArtLocalizationData>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public FiestaRunLocalizationSerializer() : base(new BinarySeriaizerSettings()
        {
            // Set the encoding to Unicode
            StringEncoding = Encoding.BigEndianUnicode
        })
        {

        }
    }
}