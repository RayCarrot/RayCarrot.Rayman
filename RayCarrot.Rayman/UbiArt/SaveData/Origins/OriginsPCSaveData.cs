using System.Text;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The save file data used for Rayman Origins on PC
    /// </summary>
    public class OriginsPCSaveData : IBinarySerializable<UbiArtSettings>
    {
        #region Public Static Properties

        /// <summary>
        /// Gets the default serializer
        /// </summary>
        public static BinaryDataSerializer<OriginsPCSaveData, UbiArtSettings> GetSerializer()
        {
            var settings = UbiArtGameMode.RaymanOriginsPC.GetSettings();

            settings.Encoding = Encoding.UTF8;

            return new BinaryDataSerializer<OriginsPCSaveData, UbiArtSettings>(settings);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Unknown initial bytes
        /// </summary>
        public byte[] Unknown1 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public bool Unknown2 { get; set; }

        /// <summary>
        /// The available save data
        /// </summary>
        public PersistentGameData_Universe SaveData { get; set; }

        public byte[] Unknown3 { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            Unknown1 = reader.ReadBytes(528);
            Unknown2 = reader.Read<bool>();
            SaveData = reader.Read<PersistentGameData_Universe>();
            Unknown3 = reader.ReadRemainingBytes();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            writer.Write(Unknown1);
            writer.Write(Unknown2);
            writer.Write(SaveData);
            writer.Write(Unknown3);
        }

        #endregion

        #region Save Data Classes

        /// <summary>
        /// The main save data for a Rayman Origins save slot
        /// </summary>
        public class PersistentGameData_Universe : IBinarySerializable<UbiArtSettings>
        {
            /// <summary>
            /// The save data for each level
            /// </summary>
            public SerializableDictionary<UbiArtStringID, UbiArtGeneric<PersistentGameData_Level>> Levels { get; set; }

            public SaveSession Rewards { get; set; }

            public Ray_PersistentGameData_Score Score { get; set; }

            public Ray_PersistentGameData_WorldMap WorldMapData { get; set; }

            public Ray_PersistentGameData_UniverseTracking TrackingData { get; set; }

            public SerializableList<AbsoluteObjectPath> DiscoveredCageMapList { get; set; }

            public uint TeethReturned { get; set; }

            public UbiArtStringID UsedPlayerIDInfo { get; set; }

            public int SprintTutorialDisabled { get; set; }

            public uint CostumeLastPrice { get; set; }

            public SerializableList<UbiArtStringID> CostumesUsed { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Levels = reader.Read<SerializableDictionary<UbiArtStringID, UbiArtGeneric<PersistentGameData_Level>>>();
                Rewards = reader.Read<SaveSession>();
                Score = reader.Read<Ray_PersistentGameData_Score>();
                WorldMapData = reader.Read<Ray_PersistentGameData_WorldMap>();
                TrackingData = reader.Read<Ray_PersistentGameData_UniverseTracking>();
                DiscoveredCageMapList = reader.Read<SerializableList<AbsoluteObjectPath>>();
                TeethReturned = reader.Read<uint>();
                UsedPlayerIDInfo = reader.Read<UbiArtStringID>();
                SprintTutorialDisabled = reader.Read<int>();
                CostumeLastPrice = reader.Read<uint>();
                CostumesUsed = reader.Read<SerializableList<UbiArtStringID>>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Levels);
                writer.Write(Rewards);
                writer.Write(Score);
                writer.Write(WorldMapData);
                writer.Write(TrackingData);
                writer.Write(DiscoveredCageMapList);
                writer.Write(TeethReturned);
                writer.Write(UsedPlayerIDInfo);
                writer.Write(SprintTutorialDisabled);
                writer.Write(CostumeLastPrice);
                writer.Write(CostumesUsed);
            }
        }

        public class PersistentGameData_Level : IBinarySerializable<UbiArtSettings>
        {
            public SerializableDictionary<UbiArtStringID, UbiArtGeneric<Ray_PersistentGameData_ISD>> ISDs { get; set; }

            public SerializableList<PackedObjectPath> CageMapPassedDoors { get; set; }

            public uint WonChallenges { get; set; }

            public SPOT_STATE LevelState { get; set; }

            public uint BestTimeAttack { get; set; }

            public uint BestLumAttack { get; set; }

            public bool HasWarning { get; set; }

            public bool IsSkipped { get; set; }

            public Ray_PersistentGameData_LevelTracking Trackingdata { get; set; }

            public class Ray_PersistentGameData_ISD : IBinarySerializable<UbiArtSettings>
            {
                public SerializableList<PackedObjectPath> PickedUpLums { get; set; }

                public SerializableList<PackedObjectPath> TakenTooth { get; set; }

                public SerializableList<PackedObjectPath> AlreadySeenCutScenes { get; set; }

                public uint FoundRelicMask { get; set; }

                public uint FoundCageMask { get; set; }

                public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
                {
                    PickedUpLums = reader.Read<SerializableList<PackedObjectPath>>();
                    TakenTooth = reader.Read<SerializableList<PackedObjectPath>>();
                    AlreadySeenCutScenes = reader.Read<SerializableList<PackedObjectPath>>();
                    FoundRelicMask = reader.Read<uint>();
                    FoundCageMask = reader.Read<uint>();
                }

                public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
                {
                    writer.Write(PickedUpLums);
                    writer.Write(TakenTooth);
                    writer.Write(AlreadySeenCutScenes);
                    writer.Write(FoundRelicMask);
                    writer.Write(FoundCageMask);
                }
            }

            public class Ray_PersistentGameData_LevelTracking : IBinarySerializable<UbiArtSettings>
            {
                public uint RunCount { get; set; }

                public uint ChallengeTimeAttackCount { get; set; }

                public uint ChallengeHidden1 { get; set; }

                public uint ChallengeHidden2 { get; set; }

                public uint ChallengeCage { get; set; }

                public float FirstTimeLevelCompleted { get; set; }

                public uint ChallengeLumsStage1 { get; set; }

                public uint ChallengeLumsStage2 { get; set; }

                public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
                {
                    RunCount = reader.Read<uint>();
                    ChallengeTimeAttackCount = reader.Read<uint>();
                    ChallengeHidden1 = reader.Read<uint>();
                    ChallengeHidden2 = reader.Read<uint>();
                    ChallengeCage = reader.Read<uint>();
                    FirstTimeLevelCompleted = reader.Read<float>();
                    ChallengeLumsStage1 = reader.Read<uint>();
                    ChallengeLumsStage2 = reader.Read<uint>();
                }

                public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
                {
                    writer.Write(RunCount);
                    writer.Write(ChallengeTimeAttackCount);
                    writer.Write(ChallengeHidden1);
                    writer.Write(ChallengeHidden2);
                    writer.Write(ChallengeCage);
                    writer.Write(FirstTimeLevelCompleted);
                    writer.Write(ChallengeLumsStage1);
                    writer.Write(ChallengeLumsStage2);
                }
            }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                ISDs = reader.Read<SerializableDictionary<UbiArtStringID, UbiArtGeneric<Ray_PersistentGameData_ISD>>>();
                CageMapPassedDoors = reader.Read<SerializableList<PackedObjectPath>>();
                WonChallenges = reader.Read<uint>();
                LevelState = (SPOT_STATE)reader.Read<int>();
                BestTimeAttack = reader.Read<uint>();
                BestLumAttack = reader.Read<uint>();
                HasWarning = reader.Read<bool>();
                IsSkipped = reader.Read<bool>();
                Trackingdata = reader.Read<Ray_PersistentGameData_LevelTracking>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(ISDs);
                writer.Write(CageMapPassedDoors);
                writer.Write(WonChallenges);
                writer.Write((int)LevelState);
                writer.Write(BestTimeAttack);
                writer.Write(BestLumAttack);
                writer.Write(HasWarning);
                writer.Write(IsSkipped);
                writer.Write(Trackingdata);
            }
        }

        public class SaveSession : IBinarySerializable<UbiArtSettings>
        {
            public SerializableList<float> Tags { get; set; }

            public SerializableList<float> Timers { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Tags = reader.Read<SerializableList<float>>();
                Timers = reader.Read<SerializableList<float>>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Tags);
                writer.Write(Timers);
            }
        }

        public class Ray_PersistentGameData_Score : IBinarySerializable<UbiArtSettings>
        {
            public SerializableList<uint> LumCount { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                LumCount = reader.Read<SerializableList<uint>>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(LumCount);
            }
        }

        public class Ray_PersistentGameData_WorldMap : IBinarySerializable<UbiArtSettings>
        {
            public SerializableDictionary<UbiArtStringID, WorldInfo> WorldsInfo { get; set; }

            public ObjectPath CurrentWorld { get; set; }

            public UbiArtStringID CurrentWorldTag { get; set; }

            public ObjectPath CurrentLevel { get; set; }

            public UbiArtStringID CurrentLevelTag { get; set; }

            public class WorldInfo : IBinarySerializable<UbiArtSettings>
            {
                public SPOT_STATE State { get; set; }

                public bool HasWarning { get; set; }

                public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
                {
                    State = (SPOT_STATE)reader.Read<int>();
                    HasWarning = reader.Read<bool>();
                }

                public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
                {
                    writer.Write((int)State);
                    writer.Write(HasWarning);
                }
            }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                WorldsInfo = reader.Read<SerializableDictionary<UbiArtStringID, WorldInfo>>();
                CurrentWorld = reader.Read<ObjectPath>();
                CurrentWorldTag = reader.Read<UbiArtStringID>();
                CurrentLevel = reader.Read<ObjectPath>();
                CurrentLevelTag = reader.Read<UbiArtStringID>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(WorldsInfo);
                writer.Write(CurrentWorld);
                writer.Write(CurrentWorldTag);
                writer.Write(CurrentLevel);
                writer.Write(CurrentLevelTag);
            }
        }

        public enum SPOT_STATE
        {
            CLOSED = 0,
            NEW = 1,
            CANNOT_ENTER = 2,
            OPEN = 3,
            COMPLETED = 4,
        }

        public class Ray_PersistentGameData_UniverseTracking : IBinarySerializable<UbiArtSettings>
        {
            public SerializableList<float> Timers { get; set; }

            public SerializableList<uint> PafCounter { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Timers = reader.Read<SerializableList<float>>();
                PafCounter = reader.Read<SerializableList<uint>>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Timers);
                writer.Write(PafCounter);
            }
        }

        public class PackedObjectPath : IBinarySerializable<UbiArtSettings>
        {
            public long Id { get; set; }

            public uint PathCode { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Id = reader.Read<long>();
                PathCode = reader.Read<uint>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Id);
                writer.Write(PathCode);
            }
        }

        public class AbsoluteObjectPath : IBinarySerializable<UbiArtSettings>
        {
            public PackedObjectPath PackedObjectPath { get; set; }

            public uint LevelCRC { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                PackedObjectPath = reader.Read<PackedObjectPath>();
                LevelCRC = reader.Read<uint>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(PackedObjectPath);
                writer.Write(LevelCRC);
            }
        }

        #endregion
    }
}