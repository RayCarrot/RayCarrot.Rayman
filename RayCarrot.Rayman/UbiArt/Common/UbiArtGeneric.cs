using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    public class UbiArtGeneric<T> : IBinarySerializable
        where T : IBinarySerializable, new()
    {
        public UbiArtStringID Name { get; set; }

        public T Object { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Name = s.SerializeObject<UbiArtStringID>(Name, name: nameof(Name));
            Object = s.SerializeObject<T>(Object, name: nameof(Object));
        }
    }
}