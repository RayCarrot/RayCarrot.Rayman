using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// Data entry in a Rayman 3 PC save file
    /// </summary>
    public class Rayman3PCSaveDataEntry : IBinarySerializable
    {
        public byte KeyLength { get; set; }

        public string Key { get; set; }

        public byte ValueType { get; set; }

        public EntryValueTypes GetValueType => (EntryValueTypes)(ValueType & 0xF);

        public byte ValuesCount { get; set; }

        public object[] Values { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            KeyLength = s.Serialize<byte>(KeyLength, name: nameof(KeyLength));

            if (KeyLength == 0x00)
                return;

            Key = s.SerializeString(Key, KeyLength, name: nameof(Key));
            ValueType = s.Serialize<byte>(ValueType, name: nameof(ValueType));
            ValuesCount = s.Serialize<byte>(ValuesCount, name: nameof(ValuesCount));

            if (Values == null || s.IsReading)
                Values = new object[ValuesCount];

            for (int i = 0; i < Values.Length; i++)
            {
                switch (GetValueType)
                {
                    case EntryValueTypes.Byte:
                        Values[i] = s.Serialize<byte>((byte)(Values[i] ?? (byte)0), name: $"{nameof(Values)}[{i}]");
                        break;

                    case EntryValueTypes.UShort:
                        Values[i] = s.Serialize<ushort>((ushort)(Values[i] ?? (ushort)0), name: $"{nameof(Values)}[{i}]");
                        break;

                    case EntryValueTypes.UInt:
                    case EntryValueTypes.Unk_04:
                        Values[i] = s.Serialize<uint>((uint)(Values[i] ?? (uint)0), name: $"{nameof(Values)}[{i}]");
                        break;

                    case EntryValueTypes.Unk_0C:
                        Values[i] = s.SerializeArray<byte>((byte[])(Values[i] ?? (byte)0), 0xC, name: $"{nameof(Values)}[{i}]");
                        break;

                    case EntryValueTypes.String:
                        var length = s.Serialize<byte>((byte)(Values[i] == null ? 0 : ((string)Values[i]).Length), name: $"ValueLength");
                        Values[i] = s.SerializeString((string)Values[i], length, name: $"{nameof(Values)}[{i}]");
                        break;
                }
            }
        }

        public enum EntryValueTypes
        {
            Byte = 1,
            UShort = 2,
            UInt = 3,
            Unk_04 = 4,
            Unk_0C = 5,
            String = 6
        }
    }
}