using System;
using System.Linq;
using RayCarrot.Binary;

namespace RayCarrot.Rayman.Ray1
{
    /// <summary>
    /// The save data for Rayman Kit on PC
    /// </summary>
    public class RayKitSaveData : IBinarySerializable
    {
        /// <summary>
        /// The file data
        /// </summary>
        public byte[] Data { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Data = s.SerializeArray<byte>(Data, 17, name: nameof(Data));
        }

        public int GetDecodedValue(int world, int level, SaveRevision revision)
        {
            var tempArray = Data.ToArray();

            var xorIndex = world - 1 + 6 * (level - 1);
            var xorTable1 = revision == SaveRevision.KIT ? XORTable1_KIT : XORTable1_FAN;
            var xorTable2 = revision == SaveRevision.KIT ? XORTable2_KIT : XORTable2_FAN;

            tempArray[7] ^= xorTable2[xorIndex];
            tempArray[4] ^= xorTable1[xorIndex];
            tempArray[16] ^= xorTable1[xorIndex];
            tempArray[1] ^= xorTable2[xorIndex];
            tempArray[15] ^= xorTable2[xorIndex];
            var v6 = (byte)(tempArray[10] ^ 0x4F);
            tempArray[10] = (byte)(v6 >> 1);
            tempArray[15] -= 11;
            tempArray[9] ^= 0x7C;
            tempArray[13] ^= 0x1A;
            tempArray[3] ^= 0x63;
            tempArray[5] ^= 0x6F;

            if (tempArray[15] == tempArray[10] && 
                tempArray[7] == tempArray[13] && 
                tempArray[4] == tempArray[9] && 
                tempArray[16] == tempArray[3] && 
                tempArray[1] == tempArray[5])
                return (tempArray[5] << 24) + (tempArray[3] << 16) + tempArray[13] + (tempArray[9] << 8);
            else
                return -1;
        }

        public void SetEncodedValue(int v) => throw new NotImplementedException();

        public enum SaveRevision
        {
            KIT,
            FAN_60N
        }

        #region XOR Tables

        public byte[] XORTable1_KIT = new byte[]
        {
            0x41, 0x51, 0x57, 0x5A, 0x53, 0x58, 0x45, 0x44, 0x43, 0x52, 0x46, 0x56,
            0x54, 0x47, 0x42, 0x59, 0x48, 0x4E, 0x55, 0x4A, 0x49, 0x4B, 0x4F, 0x4C
        };

        public byte[] XORTable2_KIT = new byte[]
        {
            0x6D, 0x70, 0x6C, 0x6F, 0x6B, 0x69, 0x6A, 0x6E, 0x75, 0x68, 0x62, 0x79,
            0x67, 0x76, 0x74, 0x66, 0x63, 0x72, 0x64, 0x78, 0x65, 0x73, 0x77, 0x7A
        };

        // NOTE: Although each table is 24 bytes like in KIT these games have 40-60 levels, thus the game will read more bytes than are available. The additional bytes might be different depending on the release of the game. If so the validation check on the save should fail and it'll return -1.
        public byte[] XORTable1_FAN = new byte[]
        {
            0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46,
            0x41, 0x42, 0x43, 0x44, 0x45, 0x46, 0x41, 0x42, 0x43, 0x44, 0x45, 0x46,
            0x00, 0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x32, 0x32, 0x32, 0x32, 0x32,
            0x32, 0x33, 0x33, 0x33, 0x33, 0x33, 0x33, 0x34, 0x34, 0x34, 0x34, 0x34,
            0x34, 0x00, 0x00, 0x00, 0x20, 0x07, 0x3F, 0x1F, 0x07, 0x3D, 0x20, 0x07
        };

        public byte[] XORTable2_FAN = new byte[]
        {
            0x31, 0x31, 0x31, 0x31, 0x31, 0x31, 0x32, 0x32, 0x32, 0x32, 0x32, 0x32,
            0x33, 0x33, 0x33, 0x33, 0x33, 0x33, 0x34, 0x34, 0x34, 0x34, 0x34, 0x34,
            0x00, 0x00, 0x00, 0x20, 0x07, 0x3F, 0x1F, 0x07, 0x3D, 0x20, 0x07, 0x3A,
            0x21, 0x07, 0x38, 0x24, 0x0A, 0x35, 0x25, 0x0C, 0x33, 0x2D, 0x0F, 0x30,
            0x34, 0x11, 0x2E, 0x2D, 0x0F, 0x30, 0x28, 0x0F, 0x33, 0x23, 0x0C, 0x35
        };

        #endregion
    }
}