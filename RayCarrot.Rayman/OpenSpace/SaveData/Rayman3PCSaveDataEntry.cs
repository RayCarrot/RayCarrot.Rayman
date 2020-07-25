using System;
using System.Linq;
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

        public int[] Values { get; set; }
        public string[] StringValues { get; set; }

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

            if (GetValueType == EntryValueTypes.String)
            {
                if (StringValues == null || s.IsReading)
                    StringValues = new string[ValuesCount];
            }
            else
            {
                if (Values == null || s.IsReading)
                    Values = new int[ValuesCount];
            }

            for (int i = 0; i < ValuesCount; i++)
            {
                switch (GetValueType)
                {
                    case EntryValueTypes.Byte:
                        Values[i] = s.Serialize<byte>((byte)Values[i], name: $"{nameof(Values)}[{i}]");
                        break;

                    case EntryValueTypes.Short:
                        Values[i] = s.Serialize<short>((short)Values[i], name: $"{nameof(Values)}[{i}]");
                        break;

                    case EntryValueTypes.Int:
                    case EntryValueTypes.Unk_04:
                        Values[i] = s.Serialize<int>((int)Values[i], name: $"{nameof(Values)}[{i}]");
                        break;

                    case EntryValueTypes.String:
                        var length = s.Serialize<byte>((byte)(StringValues[i]?.Length ?? 0), name: $"ValueLength");
                        StringValues[i] = s.SerializeString(StringValues[i], length, name: $"{nameof(StringValues)}[{i}]");
                        break;

                    default:
                    case EntryValueTypes.Unk_0C:
                        // An array of bytes of size 0x0C
                        throw new NotImplementedException($"The specified save value type {GetValueType} is currently not supported");
                }
            }
        }

        public enum EntryValueTypes
        {
            Byte = 1,
            Short = 2,
            Int = 3,
            Unk_04 = 4,
            Unk_0C = 5,
            String = 6
        }
    }
}