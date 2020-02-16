namespace RayCarrot.Rayman.UbiArt
{
    public class LocalisationId : IBinarySerializable<UbiArtSettings>
    {
        public uint ID { get; set; }

        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            ID = reader.Read<uint>();
        }

        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            writer.Write(ID);
        }
    }
}