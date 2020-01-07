namespace RayCarrot.Rayman
{
    /// <summary>
    /// The data for a map cell for a Rayman 1 .lev file
    /// </summary>
    public class Rayman1LevMapCell : IBinarySerializable
    {
        /// <summary>
        /// The index for the texture for this cell
        /// </summary>
        public ushort TextureIndex { get; set; }

        /// <summary>
        /// The cell type
        /// </summary>
        public Rayman1LevMapCellType CellType { get; set; }

        /// <summary>
        /// An unknown byte
        /// </summary>
        public byte Unknown1 { get; set; }

        /// <summary>
        /// The transparency mode for this cell
        /// </summary>
        public Rayman1LevMapCellTransparencyMode TransparencyMode { get; set; }

        /// <summary>
        /// An unknown byte
        /// </summary>
        public byte Unknown2 { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            TextureIndex = reader.Read<ushort>();
            CellType = (Rayman1LevMapCellType)reader.Read<byte>();
            Unknown1 = reader.Read<byte>();
            TransparencyMode = (Rayman1LevMapCellTransparencyMode)reader.Read<byte>();
            Unknown2 = reader.Read<byte>();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            writer.Write(TextureIndex);
            writer.Write((byte)CellType);
            writer.Write(Unknown1);
            writer.Write((byte)TransparencyMode);
            writer.Write(Unknown2);
        }
    }
}