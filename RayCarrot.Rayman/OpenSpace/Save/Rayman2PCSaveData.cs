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

        /// <summary>
        /// The global bit array. This contains 1440 bit flags which determine things like which Lums/cages are collected, which cutscenes have been viewed etc. Some values are parsed as integers.
        /// </summary>
        public int[] GlobalArray { get; set; }
        // 12 is walk of life time in centiseconds

        public byte[] Unk3 { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Unk1 = s.SerializeArray<byte>(Unk1, 3, name: nameof(Unk1));
            LastLevel = s.Serialize<byte>(LastLevel, name: nameof(LastLevel));
            Unk2 = s.SerializeArray<byte>(Unk2, 7, name: nameof(Unk2));
            GlobalArray = s.SerializeArray<int>(GlobalArray, 45, name: nameof(GlobalArray));
            Unk3 = s.SerializeArray<byte>(Unk3, 5, name: nameof(Unk3));
        }

        /// <summary>
        /// Gets the global array as bit flags
        /// </summary>
        /// <returns>The booleans, representing every bit flag</returns>
        public bool[] GlobalArrayAsBitFlags()
        {
            // Get the size of each value in bits
            const int valueSize = sizeof(int) * 8;

            // Create the array if it doesn't exist
            var output = new bool[GlobalArray.Length * valueSize];

            var index = 0;

            // Get the bits from every value
            for (int i = output.Length / valueSize - 1; i >= 0; i--)
            {
                // Get the value
                var v = GlobalArray[index];

                for (int j = valueSize - 1; j >= 0; j--)
                    output[i * valueSize + j] = BitHelpers.ExtractBits(v, 1, j) == 1;

                index++;
            }

            // Return the value
            return output;
        }

        /// <summary>
        /// Serializes a bit array made as a <see cref="System.UInt32"/> array
        /// </summary>
        /// <param name="s">The serializer</param>
        /// <param name="value">The value</param>
        /// <param name="length">The length, in <see cref="System.UInt32"/></param>
        /// <param name="name">The value name, for logging</param>
        /// <returns>The value</returns>
        public static bool[] SerializeUintBitArray(IBinarySerializer s, bool[] value, int length, string name = null)
        {
            // Get the size of each value in bits
            const int valueSize = sizeof(int) * 8;

            // Create the array if it doesn't exist
            value ??= new bool[length * valueSize];

            // Serialize every value
            for (int i = value.Length / valueSize - 1; i >= 0; i--)
            {
                int v = 0;

                for (int j = valueSize - 1; j >= 0; j--)
                    BitHelpers.SetBits(v, value[i * valueSize + j] ? 1 : 0, 1, j);

                // Serialize the value
                v = s.Serialize<int>(v, name: $"{name}[{i}]");

                for (int j = valueSize - 1; j >= 0; j--)
                    value[i * valueSize + j] = BitHelpers.ExtractBits(v, 1, j) == 1;
            }

            // Return the value
            return value;
        }
    }
}