namespace RayCarrot.Rayman.Rayman1
{
    /// <summary>
    /// The data for a map cell for a Rayman 1 .lev file on PC
    /// </summary>
    public class Rayman1PCLevMapCell : IBinarySerializable<Rayman1Settings>
    {
        /// <summary>
        /// The index for the texture for this cell
        /// </summary>
        public ushort TextureIndex { get; set; }

        /// <summary>
        /// The cell type
        /// </summary>
        public Rayman1PCLevMapCellType CellType { get; set; }

        /// <summary>
        /// An unknown byte
        /// </summary>
        public byte Unknown1 { get; set; }

        /// <summary>
        /// The transparency mode for this cell
        /// </summary>
        public Rayman1PCLevMapCellTransparencyMode TransparencyMode { get; set; }

        /// <summary>
        /// An unknown byte
        /// </summary>
        public byte Unknown2 { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<Rayman1Settings> reader)
        {
            TextureIndex = reader.Read<ushort>();
            CellType = (Rayman1PCLevMapCellType)reader.Read<byte>();
            Unknown1 = reader.Read<byte>();
            TransparencyMode = (Rayman1PCLevMapCellTransparencyMode)reader.Read<byte>();
            Unknown2 = reader.Read<byte>();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<Rayman1Settings> writer)
        {
            writer.Write(TextureIndex);
            writer.Write((byte)CellType);
            writer.Write(Unknown1);
            writer.Write((byte)TransparencyMode);
            writer.Write(Unknown2);
        }
    }
}