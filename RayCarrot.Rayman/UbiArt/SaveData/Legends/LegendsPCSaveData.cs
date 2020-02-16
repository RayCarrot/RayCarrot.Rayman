namespace RayCarrot.Rayman.UbiArt
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
        /// The main save data for a Rayman Legends save slot
        /// </summary>
        public class PersistentGameData_Universe : IBinarySerializable<UbiArtSettings>
        {
            /// <summary>
            /// The save data for each level
            /// </summary>
            public SerializableDictionary<UbiArtStringID, UbiArtGeneric<PersistentGameData_Level>> Levels { get; set; }

            public SaveSession Rewards { get; set; }

            public PersistentGameData_Score Score { get; set; }

            public ProfileData Profile { get; set; }

            public PersistentGameData_BubbleDreamerData BubbleDreamer { get; set; }

            public SerializableList<int> UnlockedPets { get; set; }

            public SerializableList<PetRewardData> PetsDailyReward { get; set; }

            public SerializableList<St_petCups> UnlockedCupsForPets { get; set; }

            public uint GivenPetCount { get; set; }

            public bool NewPetsUnlocked { get; set; }

            public bool FirstPetShown { get; set; }

            public bool HasShownMessageAllPet { get; set; }

            public SerializableList<Message> Messages { get; set; }

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

            public SerializableList<UbiArtStringID> MrDarkUnlockCount { get; set; }

            public uint CatchEmAllIndex { get; set; }

            public SerializableList<UbiArtStringID> NewCostumes { get; set; }

            public SerializableList<UbiArtStringID> CostumeUnlockSeen { get; set; }

            public SerializableList<UbiArtStringID> RetroUnlocks { get; set; }

            public SerializableList<UnlockedDoor> NewUnlockedDoor { get; set; }

            public SerializableList<RO2_LuckyTicketReward> LuckyTicketRewardList { get; set; }

            public SerializableList<NodeDataStruct> NodeData { get; set; }

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

            public SerializableList<UbiArtStringID> PlayedDiamondCupSequence { get; set; }

            public SerializableList<UbiArtStringID> Costumes { get; set; }

            public SerializableList<uint> PlayedChallenge { get; set; }

            public SerializableList<UbiArtStringID> PlayedInvasion { get; set; }

            public uint TvOffOptionEnabledNb { get; set; }

            public uint TvOffOptionActivatedTime { get; set; }

            public bool BarbaraCostumeUnlockSeen { get; set; }

            public SerializableList<UbiArtStringID> WorldUnlockMessagesSeen { get; set; }

            public bool RetroWorldUnlockMessageSeen { get; set; }

            public bool FreedAllTeensiesMessageSeen { get; set; }

            public bool MisterDarkCompletionMessageSeen { get; set; }

            public bool FirstInvasionMessageSeen { get; set; }

            public bool InvitationTutoSeen { get; set; }

            public bool MessageSeen8Bit { get; set; }

            public bool ChallengeWorldUnlockMessageSeen { get; set; }

            public SerializableList<UbiArtStringID> DoorUnlockMessageSeen { get; set; }

            public SerializableList<UbiArtStringID> DoorUnlockDRCMessageRequired { get; set; }

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
                Levels = reader.Read<SerializableDictionary<UbiArtStringID, UbiArtGeneric<PersistentGameData_Level>>>();
                Rewards = reader.Read<SaveSession>();
                Score = reader.Read<PersistentGameData_Score>();
                Profile = reader.Read<ProfileData>();
                BubbleDreamer = reader.Read<PersistentGameData_BubbleDreamerData>();
                UnlockedPets = reader.Read<SerializableList<int>>();
                PetsDailyReward = reader.Read<SerializableList<PetRewardData>>();
                UnlockedCupsForPets = reader.Read<SerializableList<St_petCups>>();
                GivenPetCount = reader.Read<uint>();
                NewPetsUnlocked = reader.Read<bool>();
                FirstPetShown = reader.Read<bool>();
                HasShownMessageAllPet = reader.Read<bool>();
                Messages = reader.Read<SerializableList<Message>>();
                MessagesTotalCount = reader.Read<uint>();
                Messages_onlineDate = reader.Read<UbiArtDateTime>();
                Messages_localDate = reader.Read<UbiArtDateTime>();
                Messages_readDrcCount = reader.Read<uint>();
                Messages_interactDrcCount = reader.Read<uint>();
                Messages_lastSeenMessageHandle = reader.Read<uint>();
                Messages_tutoCount = reader.Read<uint>();
                Messages_drcCountSinceLastInteract = reader.Read<uint>();
                PlayerCard_displayedCount = reader.Read<uint>();
                PlayerCard_tutoSeen = reader.Read<bool>();
                GameCompleted = reader.Read<bool>();
                TimeToCompleteGameInSec = reader.Read<uint>();
                TimeSpendInGameInSec = reader.Read<uint>();
                TeensiesBonusCounter = reader.Read<uint>();
                LuckyTicketsCounter = reader.Read<uint>();
                LuckyTicketLevelCount = reader.Read<uint>();
                RetroMapUnlockedCounter = reader.Read<uint>();
                MrDarkUnlockCount = reader.Read<SerializableList<UbiArtStringID>>();
                CatchEmAllIndex = reader.Read<uint>();
                NewCostumes = reader.Read<SerializableList<UbiArtStringID>>();
                CostumeUnlockSeen = reader.Read<SerializableList<UbiArtStringID>>();
                RetroUnlocks = reader.Read<SerializableList<UbiArtStringID>>();
                NewUnlockedDoor = reader.Read<SerializableList<UnlockedDoor>>();
                LuckyTicketRewardList = reader.Read<SerializableList<RO2_LuckyTicketReward>>();
                NodeData = reader.Read<SerializableList<NodeDataStruct>>();
                LuckyTicketsRewardGivenCounter = reader.Read<uint>();
                ConsecutiveLuckyTicketCount = reader.Read<uint>();
                TicketReminderMessageCount = reader.Read<uint>();
                DisplayGhosts = reader.Read<uint>();
                UplayDoneAction0 = reader.Read<bool>();
                UplayDoneAction1 = reader.Read<bool>();
                UplayDoneAction2 = reader.Read<bool>();
                UplayDoneAction3 = reader.Read<bool>();
                UplayDoneReward0 = reader.Read<bool>();
                UplayDoneReward1 = reader.Read<bool>();
                UplayDoneReward2 = reader.Read<bool>();
                UplayDoneReward3 = reader.Read<bool>();
                PlayedDiamondCupSequence = reader.Read<SerializableList<UbiArtStringID>>();
                Costumes = reader.Read<SerializableList<UbiArtStringID>>();
                PlayedChallenge = reader.Read<SerializableList<uint>>();
                PlayedInvasion = reader.Read<SerializableList<UbiArtStringID>>();
                TvOffOptionEnabledNb = reader.Read<uint>();
                TvOffOptionActivatedTime = reader.Read<uint>();
                BarbaraCostumeUnlockSeen = reader.Read<bool>();
                WorldUnlockMessagesSeen = reader.Read<SerializableList<UbiArtStringID>>();
                RetroWorldUnlockMessageSeen = reader.Read<bool>();
                FreedAllTeensiesMessageSeen = reader.Read<bool>();
                MisterDarkCompletionMessageSeen = reader.Read<bool>();
                FirstInvasionMessageSeen = reader.Read<bool>();
                InvitationTutoSeen = reader.Read<bool>();
                MessageSeen8Bit = reader.Read<bool>();
                ChallengeWorldUnlockMessageSeen = reader.Read<bool>();
                DoorUnlockMessageSeen = reader.Read<SerializableList<UbiArtStringID>>();
                DoorUnlockDRCMessageRequired = reader.Read<SerializableList<UbiArtStringID>>();
                LuckyTicketRewardWorldName = reader.Read<UbiArtStringID>();
                IsUGCMiiverseWarningSet = reader.Read<bool>();
                Reward39Failed = reader.Read<int>();
                UnlockPrivilegesData = reader.Read<string>();
                IsDemoRewardChecked = reader.Read<int>();
                PrisonerDataDummy = reader.Read<PrisonerData>();
                PersistentGameDataLevelDummy = reader.Read<PersistentGameData_Level>();
                MessageDummy = reader.Read<Message>();
                UnlockedDoorDummy = reader.Read<UnlockedDoor>();
                BubbleDreamerDataDummy = reader.Read<PersistentGameData_BubbleDreamerData>();
                DummmyNodeData = reader.Read<NodeDataStruct>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            { 
                writer.Write(Levels);
                writer.Write(Rewards);
                writer.Write(Score);
                writer.Write(Profile);
                writer.Write(BubbleDreamer);
                writer.Write(UnlockedPets);
                writer.Write(PetsDailyReward);
                writer.Write(UnlockedCupsForPets);
                writer.Write(GivenPetCount);
                writer.Write(NewPetsUnlocked);
                writer.Write(FirstPetShown);
                writer.Write(HasShownMessageAllPet);
                writer.Write(Messages);
                writer.Write(MessagesTotalCount);
                writer.Write(Messages_onlineDate);
                writer.Write(Messages_localDate);
                writer.Write(Messages_readDrcCount);
                writer.Write(Messages_interactDrcCount);
                writer.Write(Messages_lastSeenMessageHandle);
                writer.Write(Messages_tutoCount);
                writer.Write(Messages_drcCountSinceLastInteract);
                writer.Write(PlayerCard_displayedCount);
                writer.Write(PlayerCard_tutoSeen);
                writer.Write(GameCompleted);
                writer.Write(TimeToCompleteGameInSec);
                writer.Write(TimeSpendInGameInSec);
                writer.Write(TeensiesBonusCounter);
                writer.Write(LuckyTicketsCounter);
                writer.Write(LuckyTicketLevelCount);
                writer.Write(RetroMapUnlockedCounter);
                writer.Write(MrDarkUnlockCount);
                writer.Write(CatchEmAllIndex);
                writer.Write(NewCostumes);
                writer.Write(CostumeUnlockSeen);
                writer.Write(RetroUnlocks);
                writer.Write(NewUnlockedDoor);
                writer.Write(LuckyTicketRewardList);
                writer.Write(NodeData);
                writer.Write(LuckyTicketsRewardGivenCounter);
                writer.Write(ConsecutiveLuckyTicketCount);
                writer.Write(TicketReminderMessageCount);
                writer.Write(DisplayGhosts);
                writer.Write(UplayDoneAction0);
                writer.Write(UplayDoneAction1);
                writer.Write(UplayDoneAction2);
                writer.Write(UplayDoneAction3);
                writer.Write(UplayDoneReward0);
                writer.Write(UplayDoneReward1);
                writer.Write(UplayDoneReward2);
                writer.Write(UplayDoneReward3);
                writer.Write(PlayedDiamondCupSequence);
                writer.Write(Costumes);
                writer.Write(PlayedChallenge);
                writer.Write(PlayedInvasion);
                writer.Write(TvOffOptionEnabledNb);
                writer.Write(TvOffOptionActivatedTime );
                writer.Write(BarbaraCostumeUnlockSeen);
                writer.Write(WorldUnlockMessagesSeen);
                writer.Write(RetroWorldUnlockMessageSeen );
                writer.Write(FreedAllTeensiesMessageSeen);
                writer.Write(MisterDarkCompletionMessageSeen);
                writer.Write(FirstInvasionMessageSeen);
                writer.Write(InvitationTutoSeen);
                writer.Write(MessageSeen8Bit);
                writer.Write(ChallengeWorldUnlockMessageSeen);
                writer.Write(DoorUnlockMessageSeen);
                writer.Write(DoorUnlockDRCMessageRequired);
                writer.Write(LuckyTicketRewardWorldName);
                writer.Write(IsUGCMiiverseWarningSet);
                writer.Write(Reward39Failed);
                writer.Write(UnlockPrivilegesData);
                writer.Write(IsDemoRewardChecked);
                writer.Write(PrisonerDataDummy);
                writer.Write(PersistentGameDataLevelDummy);
                writer.Write(MessageDummy);
                writer.Write(UnlockedDoorDummy);
                writer.Write(BubbleDreamerDataDummy);
                writer.Write(DummmyNodeData);
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

            public SerializableList<SmartLocId> Buttons { get; set; }

            public SerializableList<Attribute> Attributes { get; set; }

            public SerializableList<Marker> Markers { get; set; }

            public class Marker : IBinarySerializable<UbiArtSettings>
            {
                public SmartLocId LocId { get; set; }

                public uint Color { get; set; }

                public float FontSize { get; set; }

                public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
                {
                    LocId = reader.Read<SmartLocId>();
                    Color = reader.Read<uint>();
                    FontSize = reader.Read<float>();
                }

                public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
                {
                    writer.Write(LocId);
                    writer.Write(Color);
                    writer.Write(FontSize);
                }
            }

            public class Attribute : IBinarySerializable<UbiArtSettings>
            {
                public uint Type { get; set; }

                public uint Value { get; set; }

                public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
                {
                    Type = reader.Read<uint>();
                    Value = reader.Read<uint>();
                }

                public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
                {
                    writer.Write(Type);
                    writer.Write(Value);
                }
            }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Message_handle = reader.Read<uint>();
                Type = reader.Read<uint>();
                Onlinedate = reader.Read<UbiArtDateTime>();
                LocalDate = reader.Read<UbiArtDateTime>();
                PersistentSeconds = reader.Read<uint>();
                Title = reader.Read<SmartLocId>();
                Body = reader.Read<SmartLocId>();
                IsPrompt = reader.Read<bool>();
                IsDrc = reader.Read<bool>();
                HasBeenRead = reader.Read<bool>();
                IsOnline = reader.Read<bool>();
                RemoveAfterRead = reader.Read<bool>();
                HasBeenInteract = reader.Read<bool>();
                RemoveAfterInteract = reader.Read<bool>();
                LockedAfterInteract = reader.Read<bool>();
                Buttons = reader.Read<SerializableList<SmartLocId>>();
                Attributes = reader.Read<SerializableList<Attribute>>();
                Markers = reader.Read<SerializableList<Marker>>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Message_handle);
                writer.Write(Type);
                writer.Write(Onlinedate);
                writer.Write(LocalDate);
                writer.Write(PersistentSeconds);
                writer.Write(Title);
                writer.Write(Body);
                writer.Write(IsPrompt);
                writer.Write(IsDrc);
                writer.Write(HasBeenRead);
                writer.Write(IsOnline);
                writer.Write(RemoveAfterRead);
                writer.Write(HasBeenInteract);
                writer.Write(RemoveAfterInteract);
                writer.Write(LockedAfterInteract);
                writer.Write(Buttons);
                writer.Write(Attributes);
                writer.Write(Markers);
            }
        }

        public class PersistentGameData_Level : IBinarySerializable<UbiArtSettings>
        {
            #region Public Properties

            public UbiArtStringID Id { get; set; }

            public uint BestLumsTaken { get; set; }

            public float BestDistance { get; set; }

            public float BestTime { get; set; }

            public SerializableList<PrisonerData> FreedPrisoners { get; set; }

            public uint Cups { get; set; }

            public uint Medals { get; set; }

            public bool Completed { get; set; }

            public bool IsVisited { get; set; }

            public bool BestTimeSent { get; set; }

            public uint Type { get; set; }

            public uint LuckyTicketsLeft { get; set; }

            public SerializableList<ObjectPath> SequenceAlreadySeen { get; set; }

            public int OnlineSynced { get; set; }

            #endregion

            #region Public Methods

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Id = reader.Read<UbiArtStringID>();
                BestLumsTaken = reader.Read<uint>();
                BestDistance = reader.Read<float>();
                BestTime = reader.Read<float>();
                FreedPrisoners = reader.Read<SerializableList<PrisonerData>>();
                Cups = reader.Read<uint>();
                Medals = reader.Read<uint>();
                Completed = reader.Read<bool>();
                IsVisited = reader.Read<bool>();
                BestTimeSent = reader.Read<bool>();
                Type = reader.Read<uint>();
                LuckyTicketsLeft = reader.Read<uint>();
                SequenceAlreadySeen = reader.Read<SerializableList<ObjectPath>>();
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
            public SerializableList<float> Tags { get; set; }

            public SerializableList<float> Timers { get; set; }

            public SerializableDictionary<UbiArtStringID, bool> RewardsState { get; set; }

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                Tags = reader.Read<SerializableList<float>>();
                Timers = reader.Read<SerializableList<float>>();
                RewardsState = reader.Read<SerializableDictionary<UbiArtStringID, bool>>();
            }

            public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
            {
                writer.Write(Tags);
                writer.Write(Timers);
                writer.Write(RewardsState);
            }
        }

        public class PersistentGameData_Score : IBinarySerializable<UbiArtSettings>
        {
            #region Public Properties

            public SerializableList<uint> PlayersLumCount { get; set; }

            public SerializableList<uint> TreasuresLumCount { get; set; }

            public int LocalLumsCount { get; set; }

            public int PendingLumsCount { get; set; }

            public int TempLumsCount { get; set; }

            #endregion

            #region Public Methods

            public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
            {
                PlayersLumCount = reader.Read<SerializableList<uint>>();
                TreasuresLumCount = reader.Read<SerializableList<uint>>();
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
                GlobalMedalsMaxRank = reader.Read<uint>();
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
                writer.Write(GlobalMedalsMaxRank);
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

            public SerializableList<bool> DisplayQuoteStates { get; set; }

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
                DisplayQuoteStates = reader.Read<SerializableList<bool>>();
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
                writer.Write((int)IndexType);
                writer.Write((int)VisualType);
            }

            #endregion
        }

        #endregion
    }
}