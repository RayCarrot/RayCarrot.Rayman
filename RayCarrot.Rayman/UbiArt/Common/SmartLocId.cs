namespace RayCarrot.Rayman.UbiArt
{
    public class SmartLocId : IBinarySerializable<UbiArtSettings>
    {
        public string DefaultText { get; set; }

        public LocalisationId LocId { get; set; }

        public bool UseText { get; set; }

        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            DefaultText = reader.Read<string>();
            LocId = reader.Read<LocalisationId>();
            UseText = reader.Read<bool>();
        }

        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            writer.Write(DefaultText);
            writer.Write(LocId);
            writer.Write(UseText);
        }
    }
}