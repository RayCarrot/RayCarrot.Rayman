﻿using RayCarrot.Binary;

namespace RayCarrot.Rayman.Ray1
{
    /// <summary>
    /// The save data for Rayman 1 on PC
    /// </summary>
    public class Rayman1PCSaveData : IBinarySerializable
    {
        /// <summary>
        /// The save file name (maximum of 3 characters)
        /// </summary>
        public string SaveName { get; set; }

        /// <summary>
        /// The number of remaining continues
        /// </summary>
        public byte ContinuesCount { get; set; }

        // World map data for every level (last 6 are the save points)
        public Rayman1PCSaveDataLevel[] Wi_Save_Zone { get; set; }

        // Flags for the powers you get from Betilla
        public byte[] RayEvts { get; set; }

        // Fist upgrades?
        public byte[] Poing { get; set; }

        // First byte is number of lives
        public byte[] StatusBar { get; set; }

        public byte Unk { get; set; }

        // 32 bytes per map (not counting Breakout)
        // Includes flags for cages and lives in each map
        public byte[][] SaveZone { get; set; }

        // Probably 2 bytes for every bonus level, with the first 2 always being 0x00
        public byte[] BonusPerfect { get; set; }

        /// <summary>
        /// The placement on the world map to start
        /// </summary>
        public ushort WorldIndex { get; set; }

        // First byte is a flag for having beaten specific bosses
        public ushort FinBossLevel { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            SaveName = s.SerializeString(SaveName, 4, name: nameof(SaveName));
            ContinuesCount = s.Serialize<byte>(ContinuesCount, name: nameof(ContinuesCount));
            Wi_Save_Zone = s.SerializeObjectArray<Rayman1PCSaveDataLevel>(Wi_Save_Zone, 24, name: nameof(Wi_Save_Zone));
            RayEvts = s.SerializeArray<byte>(RayEvts, 2, name: nameof(RayEvts));
            Poing = s.SerializeArray<byte>(Poing, 20, name: nameof(Poing));
            StatusBar = s.SerializeArray<byte>(StatusBar, 10, name: nameof(StatusBar));
            Unk = s.Serialize<byte>(Unk, name: nameof(Unk));

            if (SaveZone == null)
                SaveZone = new byte[81][];

            for (int i = 0; i < SaveZone.Length; i++)
                SaveZone[i] = s.SerializeArray<byte>(SaveZone[i], 32, name: $"{nameof(SaveZone)}[{i}]");

            BonusPerfect = s.SerializeArray<byte>(BonusPerfect, 24, name: nameof(BonusPerfect));
            WorldIndex = s.Serialize<ushort>(WorldIndex, name: nameof(WorldIndex));
            FinBossLevel = s.Serialize<ushort>(FinBossLevel, name: nameof(FinBossLevel));
        }
    }
}