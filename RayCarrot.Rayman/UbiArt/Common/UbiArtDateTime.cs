namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// Date time data for a UbiArt game
    /// </summary>
    public class UbiArtDateTime : IBinarySerializable<UbiArtSettings>
    {
        public ulong Value { get; set; }

        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            Value = reader.Read<ulong>();
        }

        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            writer.Write(Value);
        }
    }
}