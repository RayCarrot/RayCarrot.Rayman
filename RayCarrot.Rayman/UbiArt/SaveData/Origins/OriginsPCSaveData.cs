using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The save file data used for Rayman Origins on PC
    /// </summary>
    public class OriginsPCSaveData : IBinarySerializable
    {
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

        public void Serialize(IBinarySerializer s)
        {
            Unknown1 = s.SerializeArray<byte>(Unknown1, 528, name: nameof(Unknown1));
            Unknown2 = s.SerializeBool<uint>(Unknown2, name: nameof(Unknown2));
            SaveData = s.SerializeObject<PersistentGameData_Universe>(SaveData, name: nameof(SaveData));
            Unknown3 = s.SerializeArray<byte>(Unknown3, (int)(s.Stream.Length - s.Stream.Position), name: nameof(Unknown3));
        }

        #endregion

        #region Save Data Classes

        /// <summary>
        /// The main save data for a Rayman Origins save slot
        /// </summary>
        public class PersistentGameData_Universe : IBinarySerializable
        {
            /// <summary>
            /// The save data for each level
            /// </summary>
            public UbiArtObjKeyObjValuePair<UbiArtStringID, UbiArtGeneric<PersistentGameData_Level>>[] Levels { get; set; }

            public SaveSession Rewards { get; set; }

            public Ray_PersistentGameData_Score Score { get; set; }

            public Ray_PersistentGameData_WorldMap WorldMapData { get; set; }

            public Ray_PersistentGameData_UniverseTracking TrackingData { get; set; }

            public AbsoluteObjectPath[] DiscoveredCageMapList { get; set; }

            public uint TeethReturned { get; set; }

            public UbiArtStringID UsedPlayerIDInfo { get; set; }

            public int SprintTutorialDisabled { get; set; }

            public uint CostumeLastPrice { get; set; }

            public UbiArtStringID[] CostumesUsed { get; set; }

            public void Serialize(IBinarySerializer s)
            {
                Levels = s.SerializeUbiArtObjectArray<UbiArtObjKeyObjValuePair<UbiArtStringID, UbiArtGeneric<PersistentGameData_Level>>>(Levels, name: nameof(Levels));
                Rewards = s.SerializeObject<SaveSession>(Rewards, name: nameof(Rewards));
                Score = s.SerializeObject<Ray_PersistentGameData_Score>(Score, name: nameof(Score));
                WorldMapData = s.SerializeObject<Ray_PersistentGameData_WorldMap>(WorldMapData, name: nameof(WorldMapData));
                TrackingData = s.SerializeObject<Ray_PersistentGameData_UniverseTracking>(TrackingData, name: nameof(TrackingData));
                DiscoveredCageMapList = s.SerializeUbiArtObjectArray<AbsoluteObjectPath>(DiscoveredCageMapList, name: nameof(DiscoveredCageMapList));
                TeethReturned = s.Serialize<uint>(TeethReturned, name: nameof(TeethReturned));
                UsedPlayerIDInfo = s.SerializeObject<UbiArtStringID>(UsedPlayerIDInfo, name: nameof(UsedPlayerIDInfo));
                SprintTutorialDisabled = s.Serialize<int>(SprintTutorialDisabled, name: nameof(SprintTutorialDisabled));
                CostumeLastPrice = s.Serialize<uint>(CostumeLastPrice, name: nameof(CostumeLastPrice));
                CostumesUsed = s.SerializeUbiArtObjectArray<UbiArtStringID>(CostumesUsed, name: nameof(CostumesUsed));
            }
        }

        public class PersistentGameData_Level : IBinarySerializable
        {
            public UbiArtObjKeyObjValuePair<UbiArtStringID, UbiArtGeneric<Ray_PersistentGameData_ISD>>[] ISDs { get; set; }

            public PackedObjectPath[] CageMapPassedDoors { get; set; }

            public uint WonChallenges { get; set; }

            public SPOT_STATE LevelState { get; set; }

            public uint BestTimeAttack { get; set; }

            public uint BestLumAttack { get; set; }

            public bool HasWarning { get; set; }

            public bool IsSkipped { get; set; }

            public Ray_PersistentGameData_LevelTracking Trackingdata { get; set; }

            public class Ray_PersistentGameData_ISD : IBinarySerializable
            {
                public PackedObjectPath[] PickedUpLums { get; set; }

                public PackedObjectPath[] TakenTooth { get; set; }

                public PackedObjectPath[] AlreadySeenCutScenes { get; set; }

                public uint FoundRelicMask { get; set; }

                public uint FoundCageMask { get; set; }

                public void Serialize(IBinarySerializer s)
                {
                    PickedUpLums = s.SerializeUbiArtObjectArray<PackedObjectPath>(PickedUpLums, name: nameof(PickedUpLums));
                    TakenTooth = s.SerializeUbiArtObjectArray<PackedObjectPath>(TakenTooth, name: nameof(TakenTooth));
                    AlreadySeenCutScenes = s.SerializeUbiArtObjectArray<PackedObjectPath>(AlreadySeenCutScenes, name: nameof(AlreadySeenCutScenes));
                    FoundRelicMask = s.Serialize<uint>(FoundRelicMask, name: nameof(FoundRelicMask));
                    FoundCageMask = s.Serialize<uint>(FoundCageMask, name: nameof(FoundCageMask));
                }
            }

            public class Ray_PersistentGameData_LevelTracking : IBinarySerializable
            {
                public uint RunCount { get; set; }

                public uint ChallengeTimeAttackCount { get; set; }

                public uint ChallengeHidden1 { get; set; }

                public uint ChallengeHidden2 { get; set; }

                public uint ChallengeCage { get; set; }

                public float FirstTimeLevelCompleted { get; set; }

                public uint ChallengeLumsStage1 { get; set; }

                public uint ChallengeLumsStage2 { get; set; }

                public void Serialize(IBinarySerializer s)
                {
                    RunCount = s.Serialize<uint>(RunCount, name: nameof(RunCount));
                    ChallengeTimeAttackCount = s.Serialize<uint>(ChallengeTimeAttackCount, name: nameof(ChallengeTimeAttackCount));
                    ChallengeHidden1 = s.Serialize<uint>(ChallengeHidden1, name: nameof(ChallengeHidden1));
                    ChallengeHidden2 = s.Serialize<uint>(ChallengeHidden2, name: nameof(ChallengeHidden2));
                    ChallengeCage = s.Serialize<uint>(ChallengeCage, name: nameof(ChallengeCage));
                    FirstTimeLevelCompleted = s.Serialize<float>(FirstTimeLevelCompleted, name: nameof(FirstTimeLevelCompleted));
                    ChallengeLumsStage1 = s.Serialize<uint>(ChallengeLumsStage1, name: nameof(ChallengeLumsStage1));
                    ChallengeLumsStage2 = s.Serialize<uint>(ChallengeLumsStage2, name: nameof(ChallengeLumsStage2));
                }
            }

            public void Serialize(IBinarySerializer s)
            {
                ISDs = s.SerializeUbiArtObjectArray<UbiArtObjKeyObjValuePair<UbiArtStringID, UbiArtGeneric<Ray_PersistentGameData_ISD>>>(ISDs, name: nameof(ISDs));
                CageMapPassedDoors = s.SerializeUbiArtObjectArray<PackedObjectPath>(CageMapPassedDoors, name: nameof(CageMapPassedDoors));
                WonChallenges = s.Serialize<uint>(WonChallenges, name: nameof(WonChallenges));
                LevelState = s.Serialize<SPOT_STATE>(LevelState, name: nameof(LevelState));
                BestTimeAttack = s.Serialize<uint>(BestTimeAttack, name: nameof(BestTimeAttack));
                BestLumAttack = s.Serialize<uint>(BestLumAttack, name: nameof(BestLumAttack));
                HasWarning = s.SerializeBool<uint>(HasWarning, name: nameof(HasWarning));
                IsSkipped = s.SerializeBool<uint>(IsSkipped, name: nameof(IsSkipped));
                Trackingdata = s.SerializeObject<Ray_PersistentGameData_LevelTracking>(Trackingdata, name: nameof(Ray_PersistentGameData_LevelTracking));
            }
        }

        public class SaveSession : IBinarySerializable
        {
            public float[] Tags { get; set; }

            public float[] Timers { get; set; }

            public void Serialize(IBinarySerializer s)
            {
                Tags = s.SerializeUbiArtArray<float>(Tags, name: nameof(Tags));
                Timers = s.SerializeUbiArtArray<float>(Timers, name: nameof(Timers));
            }
        }

        public class Ray_PersistentGameData_Score : IBinarySerializable
        {
            public uint[] LumCount { get; set; }

            public void Serialize(IBinarySerializer s)
            {
                LumCount = s.SerializeUbiArtArray<uint>(LumCount, name: nameof(LumCount));
            }
        }

        public class Ray_PersistentGameData_WorldMap : IBinarySerializable
        {
            public UbiArtObjKeyObjValuePair<UbiArtStringID, WorldInfo>[] WorldsInfo { get; set; }

            public ObjectPath CurrentWorld { get; set; }

            public UbiArtStringID CurrentWorldTag { get; set; }

            public ObjectPath CurrentLevel { get; set; }

            public UbiArtStringID CurrentLevelTag { get; set; }

            public class WorldInfo : IBinarySerializable
            {
                public SPOT_STATE State { get; set; }

                public bool HasWarning { get; set; }

                public void Serialize(IBinarySerializer s)
                {
                    State = s.Serialize<SPOT_STATE>(State, name: nameof(State));
                    HasWarning = s.SerializeBool<uint>(HasWarning, name: nameof(HasWarning));
                }
            }

            public void Serialize(IBinarySerializer s)
            {
                WorldsInfo = s.SerializeUbiArtObjectArray<UbiArtObjKeyObjValuePair<UbiArtStringID, WorldInfo>>(WorldsInfo, name: nameof(WorldsInfo));
                CurrentWorld = s.SerializeObject<ObjectPath>(CurrentWorld, name: nameof(CurrentWorld));
                CurrentWorldTag = s.SerializeObject<UbiArtStringID>(CurrentWorldTag, name: nameof(CurrentWorldTag));
                CurrentLevel = s.SerializeObject<ObjectPath>(CurrentLevel, name: nameof(CurrentLevel));
                CurrentLevelTag = s.SerializeObject<UbiArtStringID>(CurrentLevelTag, name: nameof(CurrentLevelTag));
            }
        }

        public enum SPOT_STATE : uint
        {
            CLOSED = 0,
            NEW = 1,
            CANNOT_ENTER = 2,
            OPEN = 3,
            COMPLETED = 4,
        }

        public class Ray_PersistentGameData_UniverseTracking : IBinarySerializable
        {
            public float[] Timers { get; set; }

            public uint[] PafCounter { get; set; }

            public void Serialize(IBinarySerializer s)
            {
                Timers = s.SerializeUbiArtArray<float>(Timers, name: nameof(Timers));
                PafCounter = s.SerializeUbiArtArray<uint>(PafCounter, name: nameof(PafCounter));
            }
        }

        public class PackedObjectPath : IBinarySerializable
        {
            public long Id { get; set; }

            public uint PathCode { get; set; }

            public void Serialize(IBinarySerializer s)
            {
                Id = s.Serialize<long>(Id, name: nameof(Id));
                PathCode = s.Serialize<uint>(PathCode, name: nameof(PathCode));
            }
        }

        public class AbsoluteObjectPath : IBinarySerializable
        {
            public PackedObjectPath PackedObjectPath { get; set; }

            public uint LevelCRC { get; set; }

            public void Serialize(IBinarySerializer s)
            {
                PackedObjectPath = s.SerializeObject<PackedObjectPath>(PackedObjectPath, name: nameof(PackedObjectPath));
                LevelCRC = s.Serialize<uint>(LevelCRC, name: nameof(LevelCRC));
            }
        }

        #endregion
    }
}