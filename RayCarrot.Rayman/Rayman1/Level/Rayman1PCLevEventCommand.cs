namespace RayCarrot.Rayman
{
    /// <summary>
    /// The data for an event command for a Rayman 1 .lev file on PC
    /// </summary>
    public class Rayman1PCLevEventCommand : IBinarySerializable<Rayman1Settings>
    {
        public ushort CodeCount { get; set; }

        public ushort LabelOffsetCount { get; set; }

        public BinarySerializableList<byte> EventCode { get; set; }

        public BinarySerializableList<ushort> LabelOffsetTable { get; set; }

        public void Deserialize(IBinaryDataReader<Rayman1Settings> reader)
        {
            CodeCount = reader.Read<ushort>();
            LabelOffsetCount = reader.Read<ushort>();

            EventCode = new BinarySerializableList<byte>(CodeCount);
            EventCode.Deserialize(reader);

            LabelOffsetTable = new BinarySerializableList<ushort>(LabelOffsetCount);
            LabelOffsetTable.Deserialize(reader);
        }

        public void Serialize(IBinaryDataWriter<Rayman1Settings> writer)
        {
            writer.Write(CodeCount);
            writer.Write(LabelOffsetCount);
            writer.Write(EventCode);
            writer.Write(LabelOffsetTable);
        }
    }
}