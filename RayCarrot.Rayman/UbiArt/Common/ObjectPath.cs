namespace RayCarrot.Rayman.UbiArt
{
    public class ObjectPath : IBinarySerializable<UbiArtSettings>
    {
        public SerializableList<Level> Levels { get; set; }

        public string Id { get; set; }

        public bool Absolute { get; set; }

        public class Level : IBinarySerializable<UbiArtSettings>
        {
            public string Name { get; set; }

            public bool Parent { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Name = reader.Read<string>();
                Parent = reader.Read<bool>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Name);
                writer.Write(Parent);
            }
        }

        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            if (reader.SerializerSettings.Game == UbiArtGame.RaymanOrigins)
            {
                Id = reader.Read<string>();
            }
            else
            {
                Levels = reader.Read<SerializableList<Level>>();
                Id = reader.Read<string>();
                Absolute = reader.Read<bool>();
            }
        }

        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            if (writer.SerializerSettings.Game == UbiArtGame.RaymanOrigins)
            {
                writer.Write(Id);
            }
            else
            {
                writer.Write(Levels);
                writer.Write(Id);
                writer.Write(Absolute);
            }
        }
    }
}