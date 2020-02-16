using System.Text;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The localization data for Rayman Fiesta Run
    /// </summary>
    public class FiestaRunLocalizationData : BaseUbiArtLocalizationData<SerializablePair<string, string>>
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