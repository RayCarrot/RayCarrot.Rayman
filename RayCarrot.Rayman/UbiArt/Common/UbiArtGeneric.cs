namespace RayCarrot.Rayman.UbiArt
{
    public class UbiArtGeneric<T> : IBinarySerializable<UbiArtSettings>
        where T : IBinarySerializable<UbiArtSettings>
    {
        public UbiArtStringID Name { get; set; }

        public T Object { get; set; }

        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            Name = reader.Read<UbiArtStringID>();
            Object = reader.Read<T>();
        }

        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            writer.Write(Name);
            writer.Write(Object);
        }
    }
}