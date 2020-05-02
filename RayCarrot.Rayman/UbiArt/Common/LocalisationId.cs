using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    public class LocalisationId : IBinarySerializable
    {
        public uint ID { get; set; }

        public void Serialize(IBinarySerializer s)
        {
            ID = s.Serialize<uint>(ID, name: nameof(ID));
        }
    }
}