using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The localization data for Rayman Fiesta Run
    /// </summary>
    public class FiestaRunLocalizationData : BaseUbiArtLocalizationData<UbiArtSerializablePair<string, string>>
    {
        /// <summary>
        /// Gets the default serializer
        /// </summary>
        public static BinaryDataSerializer<FiestaRunLocalizationData, BinarySerializerSettings> GetSerializer() => new BinaryDataSerializer<FiestaRunLocalizationData, BinarySerializerSettings>(new BinarySerializerSettings()
        {
            ByteOrder = ByteOrder.BigEndian,
            Encoding = Encoding.BigEndianUnicode
        });
    }
}