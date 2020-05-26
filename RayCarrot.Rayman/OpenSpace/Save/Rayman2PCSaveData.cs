using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// Rayman 2 save data for .sav files on PC
    /// </summary>
    public class Rayman2PCSaveData : IBinarySerializable
    {
        public byte[] Unk1 { get; set; }
        
        // Level to spawn on in the Hall of Doors
        public byte LastLevel { get; set; }
        
        public byte[] Unk2 { get; set; }

        // TODO: This doesn't fully match - why?
        public bool[] GlobalBitArray { get; set; }

        public byte[] Unk3 { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Unk1 = s.SerializeArray<byte>(Unk1, 3, name: nameof(Unk1));
            LastLevel = s.Serialize<byte>(LastLevel, name: nameof(LastLevel));
            Unk2 = s.SerializeArray<byte>(Unk2, 12, name: nameof(Unk2));
            GlobalBitArray = SerializeBitArray(s, GlobalBitArray, 175, name: nameof(GlobalBitArray));
            Unk3 = s.SerializeArray<byte>(Unk3, 5, name: nameof(Unk3));
        }

        // TODO: Move to RayCarrot.Logging as an extension method
        public static bool[] SerializeBitArray(IBinarySerializer s, bool[] value, int length, string name = null)
        {
            value ??= new bool[length * 8];

            for (int i = value.Length / 8 - 1; i >= 0; i--)
            {
                byte v = 0;

                for (int j = 8 - 1; j >= 0; j--)
                    BitHelpers.SetBits(v, value[i * 8 + j] ? 1 : 0, 1, j);

                v = s.Serialize<byte>(v, name: $"{name}[{i}]");

                for (int j = 8 - 1; j >= 0; j--)
                    value[i * 8 + j] = BitHelpers.ExtractBits(v, 1, j) == 1;
            }

            return value;
        }
    }
}