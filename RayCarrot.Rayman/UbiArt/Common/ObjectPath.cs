using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    public class ObjectPath : IBinarySerializable
    {
        public Level[] Levels { get; set; }

        public string Id { get; set; }

        public bool Absolute { get; set; }

        public class Level : IBinarySerializable
        {
            public string Name { get; set; }

            public bool Parent { get; set; }

            public void Serialize(IBinarySerializer s)
            {
                Name = s.SerializeLengthPrefixedString(Name, name: nameof(Name));
                Parent = s.SerializeBool<uint>(Parent);
            }
        }

        public void Serialize(IBinarySerializer s)
        {
            if (s.GetSettings<UbiArtSettings>().Game == UbiArtGame.RaymanOrigins)
            {
                Id = s.SerializeLengthPrefixedString(Id, name: nameof(Id));
            }
            else
            {
                Levels = s.SerializeUbiArtObjectArray<Level>(Levels, name: nameof(Levels));
                Id = s.SerializeLengthPrefixedString(Id, name: nameof(Id));
                Absolute = s.SerializeBool<uint>(Absolute, name: nameof(Absolute));
            }
        }
    }
}