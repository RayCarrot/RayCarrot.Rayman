using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The localization serializer for Rayman Origins
    /// </summary>
    public class OriginsLocalizationSerializer : BinaryDataSerializer<StandardUbiArtLocalizationData>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public OriginsLocalizationSerializer() : base(new BinarySeriaizerSettings()
        {
            // Set the encoding to Unicode
            StringEncoding = Encoding.BigEndianUnicode
        })
        {

        }
    }
}