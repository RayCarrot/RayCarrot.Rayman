using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The localization serializer for Rayman Legends
    /// </summary>
    public class LegendsLocalizationSerializer : BinaryDataSerializer<StandardUbiArtLocalizationData>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public LegendsLocalizationSerializer() : base(new BinarySeriaizerSettings()
        {
            // Set the encoding to UTF-8
            StringEncoding = Encoding.UTF8
        })
        {

        }
    }
}