using System.Collections.Generic;
using System.Linq;
using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// The data for a Rayman M/Arena save file on PC
    /// </summary>
    public class RaymanMPCSaveData : IBinarySerializable
    {
        public string Header { get; set; }

        // Day + (31 * (Month + (12 * Year)))
        public uint Time1 { get; set; }

        // Milliseconds + (1000 * (Second + (60 * (Minute + (60 * Hour)))))
        public uint Time2 { get; set; }

        public uint Dword_14 { get; set; }
        public uint Dword_18 { get; set; }
        public uint Dword_1C { get; set; }
        public uint Dword_20 { get; set; }
        public uint Dword_24 { get; set; }
        public uint Dword_28 { get; set; }
        public uint Dword_2C { get; set; }
        public uint Dword_30 { get; set; }
        public uint Dword_34 { get; set; }

        public Rayman3PCSaveDataEntry[] Items { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            // Serialize header
            Header = s.SerializeString(Header, 12, name: nameof(Header));
            Time1 = s.Serialize<uint>(Time1, name: nameof(Time1));
            Time2 = s.Serialize<uint>(Time2, name: nameof(Time2));

            // Serialize unknown data
            Dword_14 = s.Serialize<uint>(Dword_14, name: nameof(Dword_14));
            Dword_18 = s.Serialize<uint>(Dword_18, name: nameof(Dword_18));
            Dword_1C = s.Serialize<uint>(Dword_1C, name: nameof(Dword_1C));
            Dword_20 = s.Serialize<uint>(Dword_20, name: nameof(Dword_20));
            Dword_24 = s.Serialize<uint>(Dword_24, name: nameof(Dword_24));
            Dword_28 = s.Serialize<uint>(Dword_28, name: nameof(Dword_28));
            Dword_2C = s.Serialize<uint>(Dword_2C, name: nameof(Dword_2C));
            Dword_30 = s.Serialize<uint>(Dword_30, name: nameof(Dword_30));
            Dword_34 = s.Serialize<uint>(Dword_34, name: nameof(Dword_34));

            // Serialize data entries
            if (s.IsReading)
            {
                var temp = new List<Rayman3PCSaveDataEntry>();

                var index = 0;

                while (temp.LastOrDefault()?.KeyLength != 0)
                {
                    temp.Add(s.SerializeObject<Rayman3PCSaveDataEntry>(default, name: $"{Items}[{index}]"));
                    index++;
                }

                Items = temp.ToArray();
            }
            else
            {
                s.SerializeObjectArray<Rayman3PCSaveDataEntry>(Items, Items.Length, name: nameof(Items));
            }
        }
    }
}