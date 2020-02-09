using System;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The save file data used for Rayman Legends on PC
    /// </summary>
    public class LegendsPCSaveData : IBinarySerializable<UbiArtSettings>
    {
        #region Public Static Properties

        /// <summary>
        /// Gets the default serializer
        /// </summary>
        public static BinaryDataSerializer<LegendsPCSaveData, UbiArtSettings> GetSerializer() => new BinaryDataSerializer<LegendsPCSaveData, UbiArtSettings>(UbiArtGameMode.RaymanLegendsPC.GetSettings());

        #endregion

        #region Public Properties

        /// <summary>
        /// Unknown initial bytes
        /// </summary>
        public byte[] Unknown1 { get; set; }

        /// <summary>
        /// The available save data
        /// </summary>
        public UbiArtSerializableList<PersistentGameData_Universe> SaveData { get; set; }

        public byte[] Unknown2 { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            Unknown1 = reader.ReadBytes(528);
            SaveData = reader.Read<UbiArtSerializableList<PersistentGameData_Universe>>();
            Unknown2 = reader.ReadRemainingBytes();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            writer.Write(Unknown1);
            writer.Write(SaveData);
            writer.Write(Unknown2);
        }

        #endregion

        #region Save Data Classes

        public class PersistentGameData_Universe : IBinarySerializable<UbiArtSettings>
        {
            public UbiArtSerializableDictionary<UbiArtStringID, UbiArtGeneric<PersistentGameData_Level>> Levels;

            public SaveSession Rewards { get; set; }

            public PersistentGameData_Score Score { get; set; }

            public ProfileData Profile { get; set; }

            public PersistentGameData_BubbleDreamerData BubbleDreamer { get; set; }

            public UbiArtSerializableList<int> UnlockedPets { get; set; }

            public UbiArtSerializableList<PetRewardData> PetsDailyReward { get; set; }

            public UbiArtSerializableList<St_petCups> UnlockedCupsForPets { get; set; }

            public uint GivenPetCount { get; set; }

            public bool NewPetsUnlocked { get; set; }

            public bool FirstPetShown { get; set; }

            public bool HasShownMessageAllPet { get; set; }

            public UbiArtSerializableList<Message> Messages { get; set; }

            public uint MessagesTotalCount { get; set; }

            public UbiArtDateTime Messages_onlineDate { get; set; }

            public UbiArtDateTime Messages_localDate { get; set; }

            public uint Messages_readDrcCount { get; set; }

            public uint Messages_interactDrcCount { get; set; }

            public uint Messages_lastSeenMessageHandle { get; set; }

            public uint Messages_tutoCount { get; set; }

            public uint Messages_drcCountSinceLastInteract { get; set; }

            public uint PlayerCard_displayedCount { get; set; }

            public bool PlayerCard_tutoSeen { get; set; }

            public bool GameCompleted { get; set; }

            public uint TimeToCompleteGameInSec { get; set; }

            public uint TimeSpendInGameInSec { get; set; }

            public uint TeensiesBonusCounter { get; set; }

            public uint LuckyTicketsCounter { get; set; }

            public uint LuckyTicketLevelCount { get; set; }

            public uint RetroMapUnlockedCounter { get; set; }

            public UbiArtSerializableList<UbiArtStringID> MrDarkUnlockCount { get; set; }

            public uint CatchEmAllIndex { get; set; }

            public UbiArtSerializableList<UbiArtStringID> NewCostumes { get; set; }

            public UbiArtSerializableList<UbiArtStringID> CostumeUnlockSeen { get; set; }

            public UbiArtSerializableList<UbiArtStringID> RetroUnlocks { get; set; }

            public UbiArtSerializableList<UnlockedDoor> NewUnlockedDoor { get; set; }

            public UbiArtSerializableList<RO2_LuckyTicketReward> LuckyTicketRewardList { get; set; }

            public UbiArtSerializableList<NodeDataStruct> NodeData { get; set; }

            public uint LuckyTicketsRewardGivenCounter { get; set; }

            public uint ConsecutiveLuckyTicketCount { get; set; }

            public uint TicketReminderMessageCount { get; set; }

            public uint DisplayGhosts { get; set; }

            public bool UplayDoneAction0 { get; set; }

            public bool UplayDoneAction1 { get; set; }

            public bool UplayDoneAction2 { get; set; }

            public bool UplayDoneAction3 { get; set; }

            public bool UplayDoneReward0 { get; set; }

            public bool UplayDoneReward1 { get; set; }

            public bool UplayDoneReward2 { get; set; }

            public bool UplayDoneReward3 { get; set; }

            public UbiArtSerializableList<UbiArtStringID> PlayedDiamondCupSequence { get; set; }

            public UbiArtSerializableList<UbiArtStringID> Costumes { get; set; }

            public UbiArtSerializableList<uint> PlayedChallenge { get; set; }

            public UbiArtSerializableList<UbiArtStringID> PlayedInvasion { get; set; }

            public uint TvOffOptionEnabledNb { get; set; }

            public uint TvOffOptionActivatedTime { get; set; }

            public bool BarbaraCostumeUnlockSeen { get; set; }

            public UbiArtSerializableList<UbiArtStringID> WorldUnlockMessagesSeen { get; set; }

            public bool RetroWorldUnlockMessageSeen { get; set; }

            public bool FreedAllTeensiesMessageSeen { get; set; }

            public bool MisterDarkCompletionMessageSeen { get; set; }

            public bool FirstInvasionMessageSeen { get; set; }

            public bool InvitationTutoSeen { get; set; }

            public bool MessageSeen8Bit { get; set; }

            public bool ChallengeWorldUnlockMessageSeen { get; set; }

            public UbiArtSerializableList<UbiArtStringID> DoorUnlockMessageSeen { get; set; }

            public UbiArtSerializableList<UbiArtStringID> DoorUnlockDRCMessageRequired { get; set; }

            public UbiArtStringID LuckyTicketRewardWorldName { get; set; }

            public bool IsUGCMiiverseWarningSet { get; set; }

            public int Reward39Failed { get; set; }

            public string UnlockPrivilegesData { get; set; }

            public int IsDemoRewardChecked { get; set; }

            public PrisonerData PrisonerDataDummy { get; set; }

            public PersistentGameData_Level PersistentGameDataLevelDummy { get; set; }

            public Message MessageDummy { get; set; }

            public UnlockedDoor UnlockedDoorDummy { get; set; }

            public PersistentGameData_BubbleDreamerData BubbleDreamerDataDummy { get; set; }

            public NodeDataStruct DummmyNodeData { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Levels = reader.Read<UbiArtSerializableDictionary<UbiArtStringID, UbiArtGeneric<PersistentGameData_Level>>>();
                Rewards = reader.Read<SaveSession>();
                Score = reader.Read<PersistentGameData_Score>();

                throw new NotImplementedException();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                throw new NotImplementedException();
            }
        }

        public class Message : IBinarySerializable<UbiArtSettings>
        {
            public uint Message_handle { get; set; }

            public uint Type { get; set; }

            public UbiArtDateTime Onlinedate { get; set; }

            public UbiArtDateTime LocalDate { get; set; }

            public uint PersistentSeconds { get; set; }

            public SmartLocId Title { get; set; }

            public SmartLocId Body { get; set; }

            public bool IsPrompt { get; set; }

            public bool IsDrc { get; set; }

            public bool HasBeenRead { get; set; }

            public bool IsOnline { get; set; }

            public bool RemoveAfterRead { get; set; }

            public bool HasBeenInteract { get; set; }

            public bool RemoveAfterInteract { get; set; }

            public bool LockedAfterInteract { get; set; }

            public UbiArtSerializableList<SmartLocId> Buttons { get; set; }

            public UbiArtSerializableList<Attribute> Attributes { get; set; }

            public UbiArtSerializableList<Marker> Markers { get; set; }

            public class Marker : IBinarySerializable<UbiArtSettings>
            {
                public SmartLocId LocId { get; set; }

                public uint Color { get; set; }

                public float FontSize { get; set; }

                public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
                {
                    throw new NotImplementedException();
                }

                public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
                {
                    throw new NotImplementedException();
                }
            }

            public class Attribute : IBinarySerializable<UbiArtSettings>
            {
                public uint Type { get; set; }

                public uint Value { get; set; }

                public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
                {
                    throw new NotImplementedException();
                }

                public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
                {
                    throw new NotImplementedException();
                }
            }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                throw new NotImplementedException();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                throw new NotImplementedException();
            }
        }

        public class PersistentGameData_Level : IBinarySerializable<UbiArtSettings>
        {
            #region Public Properties

            public UbiArtStringID Id { get; set; }

            public uint BestLumsTaken { get; set; }

            public float BestDistance { get; set; }

            public float BestTime { get; set; }

            public UbiArtSerializableList<PrisonerData> FreedPrisoners { get; set; }

            public uint Cups { get; set; }

            public uint Medals { get; set; }

            public bool Completed { get; set; }

            public bool IsVisited { get; set; }

            public bool BestTimeSent { get; set; }

            public uint Type { get; set; }

            public uint LuckyTicketsLeft { get; set; }

            public UbiArtSerializableList<ObjectPath> SequenceAlreadySeen { get; set; }

            public int OnlineSynced { get; set; }

            #endregion

            #region Public Methods

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Id = reader.Read<UbiArtStringID>();
                BestLumsTaken = reader.Read<uint>();
                BestDistance = reader.Read<float>();
                BestTime = reader.Read<float>();
                FreedPrisoners = reader.Read<UbiArtSerializableList<PrisonerData>>();
                Cups = reader.Read<uint>();
                Medals = reader.Read<uint>();
                Completed = reader.Read<bool>();
                IsVisited = reader.Read<bool>();
                BestTimeSent = reader.Read<bool>();
                Type = reader.Read<uint>();
                LuckyTicketsLeft = reader.Read<uint>();
                SequenceAlreadySeen = reader.Read<UbiArtSerializableList<ObjectPath>>();
                OnlineSynced = reader.Read<int>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Id);
                writer.Write(BestLumsTaken);
                writer.Write(BestDistance);
                writer.Write(BestTime);
                writer.Write(FreedPrisoners);
                writer.Write(Cups);
                writer.Write(Medals);
                writer.Write(Completed);
                writer.Write(IsVisited);
                writer.Write(BestTimeSent);
                writer.Write(Type);
                writer.Write(LuckyTicketsLeft);
                writer.Write(SequenceAlreadySeen);
                writer.Write(OnlineSynced);
            }

            #endregion
        }

        public class SaveSession : IBinarySerializable<UbiArtSettings>
        {
            public UbiArtSerializableList<float> Tags { get; set; }

            public UbiArtSerializableList<float> Timers { get; set; }

            public UbiArtSerializableDictionary<UbiArtStringID, bool> RewardsState { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Tags = reader.Read<UbiArtSerializableList<float>>();
                Timers = reader.Read<UbiArtSerializableList<float>>();
                RewardsState = reader.Read<UbiArtSerializableDictionary<UbiArtStringID, bool>>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Tags);
                writer.Write(Timers);
                writer.Write(RewardsState);
            }
        }

        public class ObjectPath : IBinarySerializable<UbiArtSettings>
        {
            public UbiArtSerializableList<Level> Levels { get; set; }

            public string Id { get; set; }

            public bool Absolute { get; set; }

            public class Level : IBinarySerializable<UbiArtSettings>
            {
                public string Name { get; set; }

                public bool Parent { get; set; }

                public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
                {
                    Name = reader.Read<string>();
                    Parent = reader.Read<bool>();
                }

                public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
                {
                    writer.Write(Name);
                    writer.Write(Parent);
                }
            }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Levels = reader.Read<UbiArtSerializableList<Level>>();
                Id = reader.Read<string>();
                Absolute = reader.Read<bool>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Levels);
                writer.Write(Id);
                writer.Write(Absolute);
            }
        }

        public class PersistentGameData_Score : IBinarySerializable<UbiArtSettings>
        {
            #region Public Properties

            public UbiArtSerializableList<uint> PlayersLumCount { get; set; }

            public UbiArtSerializableList<uint> TreasuresLumCount { get; set; }

            public int LocalLumsCount { get; set; }

            public int PendingLumsCount { get; set; }

            public int TempLumsCount { get; set; }

            #endregion

            #region Public Methods

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                PlayersLumCount = reader.Read<UbiArtSerializableList<uint>>();
                TreasuresLumCount = reader.Read<UbiArtSerializableList<uint>>();
                LocalLumsCount = reader.Read<int>();
                PendingLumsCount = reader.Read<int>();
                TempLumsCount = reader.Read<int>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(PlayersLumCount);
                writer.Write(TreasuresLumCount);
                writer.Write(LocalLumsCount);
                writer.Write(PendingLumsCount);
                writer.Write(TempLumsCount);
            }

            #endregion
        }

        public class ProfileData : IBinarySerializable<UbiArtSettings>
        {
            public int Pid { get; set; }

            public string Name { get; set; }

            public uint StatusIcon { get; set; }

            public int Country { get; set; }

            public uint GlobalMedalsRank { get; set; }

            public uint GlobalMedalsMaxRank { get; set; }

            public uint DiamondMedals { get; set; }

            public uint GoldMedals { get; set; }

            public uint SilverMedals { get; set; }

            public uint BronzeMedals { get; set; }

            public PlayerStatsData PlayerStats { get; set; }

            public uint Costume { get; set; }

            public uint TotalChallengePlayed { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Pid = reader.Read<int>();
                Name = reader.Read<string>();
                StatusIcon = reader.Read<uint>();
                Country = reader.Read<int>();
                GlobalMedalsRank = reader.Read<uint>();
                DiamondMedals = reader.Read<uint>();
                GoldMedals = reader.Read<uint>();
                SilverMedals = reader.Read<uint>();
                BronzeMedals = reader.Read<uint>();
                PlayerStats = reader.Read<PlayerStatsData>();
                Costume = reader.Read<uint>();
                TotalChallengePlayed = reader.Read<uint>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Pid);
                writer.Write(Name);
                writer.Write(StatusIcon);
                writer.Write(Country);
                writer.Write(GlobalMedalsRank);
                writer.Write(DiamondMedals);
                writer.Write(GoldMedals);
                writer.Write(SilverMedals);
                writer.Write(BronzeMedals);
                writer.Write(PlayerStats);
                writer.Write(Costume);
                writer.Write(TotalChallengePlayed);
            }
        }

        public class PlayerStatsData : IBinarySerializable<UbiArtSettings>
        {
            public PlayerStatsDataItem Lums { get; set; }

            public PlayerStatsDataItem Distance { get; set; }

            public PlayerStatsDataItem Kills { get; set; }

            public PlayerStatsDataItem Jumps { get; set; }

            public PlayerStatsDataItem Deaths { get; set; }

            public class PlayerStatsDataItem : IBinarySerializable<UbiArtSettings>
            {
                public float Value { get; set; }

                public uint Rank { get; set; }

                public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
                {
                    Value = reader.Read<float>();
                    Rank = reader.Read<uint>();
                }

                public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
                {
                    writer.Write(Value);
                    writer.Write(Rank);
                }
            }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Lums = reader.Read<PlayerStatsDataItem>();
                Distance = reader.Read<PlayerStatsDataItem>();
                Kills = reader.Read<PlayerStatsDataItem>();
                Jumps = reader.Read<PlayerStatsDataItem>();
                Deaths = reader.Read<PlayerStatsDataItem>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Lums);
                writer.Write(Distance);
                writer.Write(Kills);
                writer.Write(Jumps);
                writer.Write(Deaths);
            }
        }

        public class PersistentGameData_BubbleDreamerData : IBinarySerializable<UbiArtSettings>
        {
            #region Public Properties

            public bool HasMet { get; set; }

            public bool UpdateRequested { get; set; }

            public bool HasWonPetCup { get; set; }

            public uint TeensyLocksOpened { get; set; }

            public uint ChallengeLocksOpened { get; set; }

            public uint TutoCount { get; set; }

            public UbiArtSerializableList<bool> DisplayQuoteStates { get; set; }

            #endregion

            #region Public Methods

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                HasMet = reader.Read<bool>();
                UpdateRequested = reader.Read<bool>();
                HasWonPetCup = reader.Read<bool>();
                TeensyLocksOpened = reader.Read<uint>();
                ChallengeLocksOpened = reader.Read<uint>();
                TutoCount = reader.Read<uint>();
                DisplayQuoteStates = reader.Read<UbiArtSerializableList<bool>>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(HasMet);
                writer.Write(UpdateRequested);
                writer.Write(HasWonPetCup);
                writer.Write(TeensyLocksOpened);
                writer.Write(ChallengeLocksOpened);
                writer.Write(TutoCount);
                writer.Write(DisplayQuoteStates);
            }

            #endregion
        }

        public class PetRewardData : IBinarySerializable<UbiArtSettings>
        {
            public uint LastSpawnDay { get; set; }

            public uint MaxRewardNb { get; set; }

            public uint RemainingRewards { get; set; }

            public uint RewardType { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                LastSpawnDay = reader.Read<uint>();
                MaxRewardNb = reader.Read<uint>();
                RemainingRewards = reader.Read<uint>();
                RewardType = reader.Read<uint>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(LastSpawnDay);
                writer.Write(MaxRewardNb);
                writer.Write(RemainingRewards);
                writer.Write(RewardType);
            }
        }

        public class St_petCups : IBinarySerializable<UbiArtSettings>
        {
            public int Family { get; set; }

            public uint Cups { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Family = reader.Read<int>();
                Cups = reader.Read<uint>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Family);
                writer.Write(Cups);
            }
        }

        public class UnlockedDoor : IBinarySerializable<UbiArtSettings>
        {
            #region Public Properties

            public UbiArtStringID WorldTag { get; set; }

            public uint Type { get; set; }

            public bool IsNew { get; set; }

            #endregion

            #region Public Methods

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                WorldTag = reader.Read<UbiArtStringID>();
                Type = reader.Read<uint>();
                IsNew = reader.Read<bool>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(WorldTag);
                writer.Write(Type);
                writer.Write(IsNew);
            }

            #endregion
        }

        public class RO2_LuckyTicketReward : IBinarySerializable<UbiArtSettings>
        {
            #region Public Properties

            public uint ID { get; set; }

            public uint Type { get; set; }

            #endregion

            #region Public Methods

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                ID = reader.Read<uint>();
                Type = reader.Read<uint>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(ID);
                writer.Write(Type);
            }

            #endregion
        }

        public class NodeDataStruct : IBinarySerializable<UbiArtSettings>
        {
            #region Public Properties

            public UbiArtStringID Tag { get; set; }

            public bool UnteaseSeen { get; set; }

            public bool UnlockSeend { get; set; }

            public bool SentUnlockMessage { get; set; }

            #endregion

            #region Public Methods

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Tag = reader.Read<UbiArtStringID>();
                UnteaseSeen = reader.Read<bool>();
                UnlockSeend = reader.Read<bool>();
                SentUnlockMessage = reader.Read<bool>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Tag);
                writer.Write(UnteaseSeen);
                writer.Write(UnlockSeend);
                writer.Write(SentUnlockMessage);
            }

            #endregion
        }

        public class PrisonerData : IBinarySerializable<UbiArtSettings>
        {
            #region Public Properties

            public UbiArtPath Path { get; set; }

            public bool IsFree { get; set; }

            public Index IndexType { get; set; }

            public Prisoner VisualType { get; set; }

            #endregion

            #region Enums

            public enum Index
            {
                Map1 = 0,
                Map2 = 1,
                Map3 = 2,
                Map4 = 3,
                Map5 = 4,
                Map6 = 5,
                Map7 = 6,
                Map8 = 7,
            }
            public enum Prisoner
            {
                Soldier = 0,
                Baby = 1,
                Fool = 2,
                Princess = 3,
                Prince = 4,
                Queen = 5,
                King = 6,
            }

            #endregion

            #region Public Methods

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Path = reader.Read<UbiArtPath>();
                IsFree = reader.Read<bool>();
                IndexType = (Index)reader.Read<int>();
                VisualType = (Prisoner)reader.Read<int>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Path);
                writer.Write(IsFree);
                writer.Write(IndexType);
                writer.Write(VisualType);
            }

            #endregion
        }

        #endregion

        // IDEA: Move elsewhere
        #region UbiArt Classes

        public class SmartLocId : IBinarySerializable<UbiArtSettings>
        {
            public string DefaultText { get; set; }

            public LocalisationId LocId { get; set; }

            public bool UseText { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                DefaultText = reader.Read<string>();
                LocId = reader.Read<LocalisationId>();
                UseText = reader.Read<bool>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(DefaultText);
                writer.Write(LocId);
                writer.Write(UseText);
            }
        }

        public class LocalisationId : IBinarySerializable<UbiArtSettings>
        {
            public uint ID { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                ID = reader.Read<uint>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(ID);
            }
        }

        public class UbiArtPath : IBinarySerializable<UbiArtSettings>
        {
            #region Public Properties

            public string Folder { get; set; }

            public string Filename { get; set; }

            public UbiArtStringID StringID { get; set; }

            // Only available on games after Origins
            public uint Flags { get; set; }

            #endregion

            #region Public Methods

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Folder = reader.Read<string>();
                Filename = reader.Read<string>();
                StringID = reader.Read<UbiArtStringID>();
                Flags = reader.Read<uint>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Folder);
                writer.Write(Filename);
                writer.Write(StringID);
                writer.Write(Flags);
            }

            #endregion
        }

        public class UbiArtStringID : IBinarySerializable<UbiArtSettings>
        {
            #region Public Properties

            public uint ID { get; set; }

            #endregion

            #region Public Methods

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                ID = reader.Read<uint>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(ID);
            }

            #endregion
        }

        /// <summary>
        /// Date time data for a UbiArt game
        /// </summary>
        public class UbiArtDateTime : IBinarySerializable<UbiArtSettings>
        {
            public uint Year { get; set; }

            public uint Month { get; set; }

            public uint Day { get; set; }

            public uint Hour { get; set; }

            public uint Minute { get; set; }

            public uint Second { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Year = reader.Read<uint>();
                Month = reader.Read<uint>();
                Day = reader.Read<uint>();
                Hour = reader.Read<uint>();
                Minute = reader.Read<uint>();
                Second = reader.Read<uint>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Year);
                writer.Write(Month);
                writer.Write(Day);
                writer.Write(Hour);
                writer.Write(Minute);
                writer.Write(Second);
            }
        }

        public class UbiArtGeneric<T> : IBinarySerializable<UbiArtSettings>
            where T : IBinarySerializable<UbiArtSettings>
        {
            public UbiArtStringID Name { get; set; }

            public T Object { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Name = reader.Read<UbiArtStringID>();
                Object = reader.Read<T>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Name);
                writer.Write(Object);
            }
        }

        #endregion
    }
}