using RayCarrot.Binary;

namespace RayCarrot.Rayman.Ray1
{
    /// <summary>
    /// Level save data for Rayman 1 on PC
    /// </summary>
    public class Rayman1PCSaveDataLevel : IBinarySerializable
    {
        /// <summary>
        /// Indicates if the level has been unlocked on the world map
        /// </summary>
        public bool IsUnlocked { get; set; }

        /// <summary>
        /// Indicates if the level is currently unlocking
        /// </summary>
        public byte IsUnlocking { get; set; }
        
        /// <summary>
        /// The amount of cages in the level (0-6)
        /// </summary>
        public byte Cages { get; set; }

        public byte Padding { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            byte value = 0;

            value = (byte)BitHelpers.SetBits(value, IsUnlocked ? 1 : 0, 1, 0);
            value = (byte)BitHelpers.SetBits(value, IsUnlocking, 1, 1);
            value = (byte)BitHelpers.SetBits(value, Cages, 3, 2);
            value = (byte)BitHelpers.SetBits(value, Padding, 3, 5);

            value = s.Serialize<byte>(value, name: nameof(value));

            IsUnlocked = BitHelpers.ExtractBits(value, 1, 0) == 1;
            IsUnlocking = (byte)BitHelpers.ExtractBits(value, 1, 1);
            Cages = (byte)BitHelpers.ExtractBits(value, 3, 2);
            Padding = (byte)BitHelpers.ExtractBits(value, 3, 5);
        }
    }
}