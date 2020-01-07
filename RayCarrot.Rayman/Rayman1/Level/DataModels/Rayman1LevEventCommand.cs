namespace RayCarrot.Rayman
{
    /// <summary>
    /// The data for an event command for a Rayman 1 .lev file
    /// </summary>
    public class Rayman1LevEventCommand : IBinarySerializable
    {
        public ushort CodeCount { get; set; }

        public ushort LabelOffsetCount { get; set; }

        public BinarySerializableFixedList<byte> EventCode { get; set; }

        public BinarySerializableFixedList<ushort> LabelOffsetTable { get; set; }

        public void Deserialize(BinaryDataReader reader)
        {
            CodeCount = reader.Read<ushort>();
            LabelOffsetCount = reader.Read<ushort>();

            EventCode = new BinarySerializableFixedList<byte>(CodeCount);
            EventCode.Deserialize(reader);

            LabelOffsetTable = new BinarySerializableFixedList<ushort>(LabelOffsetCount);
            LabelOffsetTable.Deserialize(reader);
        }

        public void Serialize(BinaryDataWriter writer)
        {
            writer.Write(CodeCount);
            writer.Write(LabelOffsetCount);
            writer.Write(EventCode);
            writer.Write(LabelOffsetTable);
        }
    }
}