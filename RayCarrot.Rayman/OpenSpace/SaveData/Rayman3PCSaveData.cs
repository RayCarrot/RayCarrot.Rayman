using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    // WIP: Support other platforms - the format seems very similar

    /// <summary>
    /// The data for a Rayman 3 save file on PC
    /// </summary>
    public class Rayman3PCSaveData : IBinarySerializable
    {
        /// <summary>
        /// The total amount of cages
        /// </summary>
        public int TotalCages { get; set; }

        /// <summary>
        /// The total score
        /// </summary>
        public int TotalScore { get; set; }

        /// <summary>
        /// The data for each of the available levels. Count is always 9.
        /// </summary>
        public Rayman3PCSaveDataLevel[] Levels { get; set; }

        public uint ForKeyAlgo { get; set; }
        public uint Dword_7FE504 { get; set; }
        public uint Dword_7FE508 { get; set; }
        public uint Dword_7FE50C { get; set; }
        public bool EndOfGameIsDone { get; set; }
        public uint MaxIndCurrentEnv { get; set; }

        // Always 0 and gets set to 0 when saving
        public uint UnusedDword1 { get; set; }
        public uint UnusedDword2 { get; set; }

        /// <summary>
        /// Dummy byte for aligning the bytes, always 0
        /// </summary>
        public byte DummyByte { get; set; }

        /// <summary>
        /// Indicates if controller vibration is enabled
        /// </summary>
        public bool IsVibrationEnabled { get; set; }

        /// <summary>
        /// Indicates if the horizontal axis is inverted
        /// </summary>
        public bool IsHorizontalInversionEnabled { get; set; }

        /// <summary>
        /// Indicates if the vertical axis is inverted
        /// </summary>
        public bool IsVerticalInversionEnabled { get; set; }

        public DirectXKey[] KeyboardMapping { get; set; }

        public ushort[] ControllerMapping { get; set; }

        // These are probably booleans (0 or 1)
        public int UserWarnedAboutAutoSave { get; set; }

        public int RevisitMode { get; set; }
        
        public int RevisitEnvScoreAtBeginingOfLevel { get; set; }

        public int RevisitGlobalScoreAtBeginingOfLevel { get; set; }

        public int RevisitIndCurrentEnv { get; set; }

        public int RevisitIndEnvironementFromWhereWeCome { get; set; }

        /// <summary>
        /// The current level to load, or "endgame" if the game has been finished
        /// </summary>
        public string LevelNameToSave { get; set; }

        public string LevelNameAfterReinitToSave { get; set; }

        public byte[] Unk1 { get; set; }

        public int SetVolumeSound { get; set; }

        public int SetVolumeVoice { get; set; }

        public int SetVolumeMusic { get; set; }
        
        public int SetVolumeAmbiance { get; set; }
        
        public int SetVolumeMenu { get; set; }

        // Contains data like the current player health etc.
        public Rayman3PCSaveDataEntry[] DataEntries { get; set; }

        public byte[] Unk2 { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            TotalCages = s.Serialize<int>(TotalCages, name: nameof(TotalCages));
            TotalScore = s.Serialize<int>(TotalScore, name: nameof(TotalScore));

            Levels = s.SerializeObjectArray<Rayman3PCSaveDataLevel>(Levels, 9, name: nameof(Levels));

            ForKeyAlgo = s.Serialize<uint>(ForKeyAlgo, name: nameof(ForKeyAlgo));
            Dword_7FE504 = s.Serialize<uint>(Dword_7FE504, name: nameof(Dword_7FE504));
            Dword_7FE508 = s.Serialize<uint>(Dword_7FE508, name: nameof(Dword_7FE508));
            Dword_7FE50C = s.Serialize<uint>(Dword_7FE50C, name: nameof(Dword_7FE50C));
            EndOfGameIsDone = s.SerializeBool<uint>(EndOfGameIsDone, name: nameof(EndOfGameIsDone));
            MaxIndCurrentEnv = s.Serialize<uint>(MaxIndCurrentEnv, name: nameof(MaxIndCurrentEnv));
            UnusedDword1 = s.Serialize<uint>(UnusedDword1, name: nameof(UnusedDword1));
            UnusedDword2 = s.Serialize<uint>(UnusedDword2, name: nameof(UnusedDword2));

            DummyByte = s.Serialize<byte>(DummyByte, name: nameof(DummyByte));
            IsVibrationEnabled = s.Serialize<bool>(IsVibrationEnabled, name: nameof(IsVibrationEnabled));
            IsHorizontalInversionEnabled = s.Serialize<bool>(IsHorizontalInversionEnabled, name: nameof(IsHorizontalInversionEnabled));
            IsVerticalInversionEnabled = s.Serialize<bool>(IsVerticalInversionEnabled, name: nameof(IsVerticalInversionEnabled));

            KeyboardMapping = s.SerializeArray<DirectXKey>(KeyboardMapping, 13, name: nameof(KeyboardMapping));
            ControllerMapping = s.SerializeArray<ushort>(ControllerMapping, 13, name: nameof(ControllerMapping));

            UserWarnedAboutAutoSave = s.Serialize<int>(UserWarnedAboutAutoSave, name: nameof(UserWarnedAboutAutoSave));
            RevisitMode = s.Serialize<int>(RevisitMode, name: nameof(RevisitMode));
            RevisitEnvScoreAtBeginingOfLevel = s.Serialize<int>(RevisitEnvScoreAtBeginingOfLevel, name: nameof(RevisitEnvScoreAtBeginingOfLevel));
            RevisitGlobalScoreAtBeginingOfLevel = s.Serialize<int>(RevisitGlobalScoreAtBeginingOfLevel, name: nameof(RevisitGlobalScoreAtBeginingOfLevel));
            RevisitIndCurrentEnv = s.Serialize<int>(RevisitIndCurrentEnv, name: nameof(RevisitIndCurrentEnv));
            RevisitIndEnvironementFromWhereWeCome = s.Serialize<int>(RevisitIndEnvironementFromWhereWeCome, name: nameof(RevisitIndEnvironementFromWhereWeCome));

            LevelNameToSave = s.SerializeString(LevelNameToSave, 20, LevelNameToSave);
            LevelNameAfterReinitToSave = s.SerializeString(LevelNameAfterReinitToSave, 20, LevelNameAfterReinitToSave);

            Unk1 = s.SerializeArray<byte>(Unk1, 16, name: nameof(Unk1));

            SetVolumeSound = s.Serialize<int>(SetVolumeSound, name: nameof(SetVolumeSound));
            SetVolumeVoice = s.Serialize<int>(SetVolumeVoice, name: nameof(SetVolumeVoice));
            SetVolumeMusic = s.Serialize<int>(SetVolumeMusic, name: nameof(SetVolumeMusic));
            SetVolumeAmbiance = s.Serialize<int>(SetVolumeAmbiance, name: nameof(SetVolumeAmbiance));
            SetVolumeMenu = s.Serialize<int>(SetVolumeMenu, name: nameof(SetVolumeMenu));

            DataEntries = s.SerializeObjectArray<Rayman3PCSaveDataEntry>(DataEntries, 3, name: nameof(DataEntries));

            Unk2 = s.SerializeArray<byte>(Unk2, (int)(s.Stream.Length - s.Stream.Position), name: nameof(Unk2));
        }
    }
}