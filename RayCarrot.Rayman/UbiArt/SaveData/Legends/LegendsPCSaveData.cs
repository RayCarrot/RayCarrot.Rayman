using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// The save file data used for Rayman Legends on PC
    /// </summary>
    public class LegendsPCSaveData : IBinarySerializable
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

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
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
        /// The main save data for a Rayman Legends save slot
        /// </summary>
        public class PersistentGameData_Universe : IBinarySerializable
        {
            /// <summary>
            /// The save data for each level
            /// </summary>
            public UbiArtObjKeyObjValuePair<UbiArtStringID, UbiArtGeneric<PersistentGameData_Level>>[] Levels { get; set; }

            public SaveSession Rewards { get; set; }

            public PersistentGameData_Score Score { get; set; }

            public ProfileData Profile { get; set; }

            public PersistentGameData_BubbleDreamerData BubbleDreamer { get; set; }

            public int[] UnlockedPets { get; set; }

            public PetRewardData[] PetsDailyReward { get; set; }

            public St_petCups[] UnlockedCupsForPets { get; set; }

            public uint GivenPetCount { get; set; }

            public bool NewPetsUnlocked { get; set; }

            public bool FirstPetShown { get; set; }

            public bool HasShownMessageAllPet { get; set; }

            public Message[] Messages { get; set; }

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

            public UbiArtStringID[] MrDarkUnlockCount { get; set; }

            public uint CatchEmAllIndex { get; set; }

            public UbiArtStringID[] NewCostumes { get; set; }

            public UbiArtStringID[] CostumeUnlockSeen { get; set; }

            public UbiArtStringID[] RetroUnlocks { get; set; }

            public UnlockedDoor[] NewUnlockedDoor { get; set; }

            public RO2_LuckyTicketReward[] LuckyTicketRewardList { get; set; }

            public NodeDataStruct[] NodeData { get; set; }

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

            public UbiArtStringID[] PlayedDiamondCupSequence { get; set; }

            public UbiArtStringID[] Costumes { get; set; }

            public uint[] PlayedChallenge { get; set; }

            public UbiArtStringID[] PlayedInvasion { get; set; }

            public uint TvOffOptionEnabledNb { get; set; }

            public uint TvOffOptionActivatedTime { get; set; }

            public bool BarbaraCostumeUnlockSeen { get; set; }

            public UbiArtStringID[] WorldUnlockMessagesSeen { get; set; }

            public bool RetroWorldUnlockMessageSeen { get; set; }

            public bool FreedAllTeensiesMessageSeen { get; set; }

            public bool MisterDarkCompletionMessageSeen { get; set; }

            public bool FirstInvasionMessageSeen { get; set; }

            public bool InvitationTutoSeen { get; set; }

            public bool MessageSeen8Bit { get; set; }

            public bool ChallengeWorldUnlockMessageSeen { get; set; }

            public UbiArtStringID[] DoorUnlockMessageSeen { get; set; }

            public UbiArtStringID[] DoorUnlockDRCMessageRequired { get; set; }

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

            public void Serialize(IBinarySerializer s)
            {
                Levels = s.SerializeUbiArtObjectArray<UbiArtObjKeyObjValuePair<UbiArtStringID, UbiArtGeneric<PersistentGameData_Level>>>(Levels, name: nameof(Levels));
                Rewards = s.SerializeObject<SaveSession>(Rewards, name: nameof(Rewards));
                Score = s.SerializeObject<PersistentGameData_Score>(Score, name: nameof(Score));
                Profile = s.SerializeObject<ProfileData>(Profile, name: nameof(Profile));
                BubbleDreamer = s.SerializeObject<PersistentGameData_BubbleDreamerData>(BubbleDreamer, name: nameof(BubbleDreamer));
                UnlockedPets = s.SerializeUbiArtArray<int>(UnlockedPets, name: nameof(UnlockedPets));
                PetsDailyReward = s.SerializeUbiArtObjectArray<PetRewardData>(PetsDailyReward, name: nameof(PetsDailyReward));
                UnlockedCupsForPets = s.SerializeUbiArtObjectArray<St_petCups>(UnlockedCupsForPets, name: nameof(UnlockedCupsForPets));
                GivenPetCount = s.Serialize<uint>(GivenPetCount, name: nameof(GivenPetCount));
                NewPetsUnlocked = s.SerializeBool<uint>(NewPetsUnlocked, name: nameof(NewPetsUnlocked));
                FirstPetShown = s.SerializeBool<uint>(FirstPetShown, name: nameof(FirstPetShown));
                HasShownMessageAllPet = s.SerializeBool<uint>(HasShownMessageAllPet, name: nameof(HasShownMessageAllPet));
                Messages = s.SerializeUbiArtObjectArray<Message>(Messages, name: nameof(Messages));
                MessagesTotalCount = s.Serialize<uint>(MessagesTotalCount, name: nameof(MessagesTotalCount));
                Messages_onlineDate = s.SerializeObject<UbiArtDateTime>(Messages_onlineDate, name: nameof(Messages_onlineDate));
                Messages_localDate = s.SerializeObject<UbiArtDateTime>(Messages_localDate, name: nameof(Messages_localDate));
                Messages_readDrcCount = s.Serialize<uint>(Messages_readDrcCount, name: nameof(Messages_readDrcCount));
                Messages_interactDrcCount = s.Serialize<uint>(Messages_interactDrcCount, name: nameof(Messages_interactDrcCount));
                Messages_lastSeenMessageHandle = s.Serialize<uint>(Messages_lastSeenMessageHandle, name: nameof(Messages_lastSeenMessageHandle));
                Messages_tutoCount = s.Serialize<uint>(Messages_tutoCount, name: nameof(Messages_tutoCount));
                Messages_drcCountSinceLastInteract = s.Serialize<uint>(Messages_drcCountSinceLastInteract, name: nameof(Messages_drcCountSinceLastInteract));
                PlayerCard_displayedCount = s.Serialize<uint>(PlayerCard_displayedCount, name: nameof(PlayerCard_displayedCount));
                PlayerCard_tutoSeen = s.SerializeBool<uint>(PlayerCard_tutoSeen, name: nameof(PlayerCard_tutoSeen));
                GameCompleted = s.SerializeBool<uint>(GameCompleted, name: nameof(GameCompleted));
                TimeToCompleteGameInSec = s.Serialize<uint>(TimeToCompleteGameInSec, name: nameof(TimeToCompleteGameInSec));
                TimeSpendInGameInSec = s.Serialize<uint>(TimeSpendInGameInSec, name: nameof(TimeSpendInGameInSec));
                TeensiesBonusCounter = s.Serialize<uint>(TeensiesBonusCounter, name: nameof(TeensiesBonusCounter));
                LuckyTicketsCounter = s.Serialize<uint>(LuckyTicketsCounter, name: nameof(LuckyTicketsCounter));
                LuckyTicketLevelCount = s.Serialize<uint>(LuckyTicketLevelCount, name: nameof(LuckyTicketLevelCount));
                RetroMapUnlockedCounter = s.Serialize<uint>(RetroMapUnlockedCounter, name: nameof(RetroMapUnlockedCounter));
                MrDarkUnlockCount = s.SerializeUbiArtObjectArray<UbiArtStringID>(MrDarkUnlockCount, name: nameof(MrDarkUnlockCount));
                CatchEmAllIndex = s.Serialize<uint>(CatchEmAllIndex, name: nameof(CatchEmAllIndex));
                NewCostumes = s.SerializeUbiArtObjectArray<UbiArtStringID>(NewCostumes, name: nameof(NewCostumes));
                CostumeUnlockSeen = s.SerializeUbiArtObjectArray<UbiArtStringID>(CostumeUnlockSeen, name: nameof(CostumeUnlockSeen));
                RetroUnlocks = s.SerializeUbiArtObjectArray<UbiArtStringID>(RetroUnlocks, name: nameof(RetroUnlocks));
                NewUnlockedDoor = s.SerializeUbiArtObjectArray<UnlockedDoor>(NewUnlockedDoor, name: nameof(NewUnlockedDoor));
                LuckyTicketRewardList = s.SerializeUbiArtObjectArray<RO2_LuckyTicketReward>(LuckyTicketRewardList, name: nameof(LuckyTicketRewardList));
                NodeData = s.SerializeUbiArtObjectArray<NodeDataStruct>(NodeData, name: nameof(NodeData));
                LuckyTicketsRewardGivenCounter = s.Serialize<uint>(LuckyTicketsRewardGivenCounter, name: nameof(LuckyTicketsRewardGivenCounter));
                ConsecutiveLuckyTicketCount = s.Serialize<uint>(ConsecutiveLuckyTicketCount, name: nameof(ConsecutiveLuckyTicketCount));
                TicketReminderMessageCount = s.Serialize<uint>(TicketReminderMessageCount, name: nameof(TicketReminderMessageCount));
                DisplayGhosts = s.Serialize<uint>(DisplayGhosts, name: nameof(DisplayGhosts));
                UplayDoneAction0 = s.SerializeBool<uint>(UplayDoneAction0, name: nameof(UplayDoneAction0));
                UplayDoneAction1 = s.SerializeBool<uint>(UplayDoneAction1, name: nameof(UplayDoneAction1));
                UplayDoneAction2 = s.SerializeBool<uint>(UplayDoneAction2, name: nameof(UplayDoneAction2));
                UplayDoneAction3 = s.SerializeBool<uint>(UplayDoneAction3, name: nameof(UplayDoneAction3));
                UplayDoneReward0 = s.SerializeBool<uint>(UplayDoneReward0, name: nameof(UplayDoneReward0));
                UplayDoneReward1 = s.SerializeBool<uint>(UplayDoneReward1, name: nameof(UplayDoneReward1));
                UplayDoneReward2 = s.SerializeBool<uint>(UplayDoneReward2, name: nameof(UplayDoneReward2));
                UplayDoneReward3 = s.SerializeBool<uint>(UplayDoneReward3, name: nameof(UplayDoneReward3));
                PlayedDiamondCupSequence = s.SerializeUbiArtObjectArray<UbiArtStringID>(PlayedDiamondCupSequence, name: nameof(PlayedDiamondCupSequence));
                Costumes = s.SerializeUbiArtObjectArray<UbiArtStringID>(Costumes, name: nameof(Costumes));
                PlayedChallenge = s.SerializeUbiArtArray<uint>(PlayedChallenge, name: nameof(PlayedChallenge));
                PlayedInvasion = s.SerializeUbiArtObjectArray<UbiArtStringID>(PlayedInvasion, name: nameof(PlayedInvasion));
                TvOffOptionEnabledNb = s.Serialize<uint>(TvOffOptionEnabledNb, name: nameof(TvOffOptionEnabledNb));
                TvOffOptionActivatedTime = s.Serialize<uint>(TvOffOptionActivatedTime, name: nameof(TvOffOptionActivatedTime));
                BarbaraCostumeUnlockSeen = s.SerializeBool<uint>(BarbaraCostumeUnlockSeen, name: nameof(BarbaraCostumeUnlockSeen));
                WorldUnlockMessagesSeen = s.SerializeUbiArtObjectArray<UbiArtStringID>(WorldUnlockMessagesSeen, name: nameof(WorldUnlockMessagesSeen));
                RetroWorldUnlockMessageSeen = s.SerializeBool<uint>(RetroWorldUnlockMessageSeen, name: nameof(RetroWorldUnlockMessageSeen));
                FreedAllTeensiesMessageSeen = s.SerializeBool<uint>(FreedAllTeensiesMessageSeen, name: nameof(FreedAllTeensiesMessageSeen));
                MisterDarkCompletionMessageSeen = s.SerializeBool<uint>(MisterDarkCompletionMessageSeen, name: nameof(MisterDarkCompletionMessageSeen));
                FirstInvasionMessageSeen = s.SerializeBool<uint>(FirstInvasionMessageSeen, name: nameof(FirstInvasionMessageSeen));
                InvitationTutoSeen = s.SerializeBool<uint>(InvitationTutoSeen, name: nameof(InvitationTutoSeen));
                MessageSeen8Bit = s.SerializeBool<uint>(MessageSeen8Bit, name: nameof(MessageSeen8Bit));
                ChallengeWorldUnlockMessageSeen = s.SerializeBool<uint>(ChallengeWorldUnlockMessageSeen, name: nameof(ChallengeWorldUnlockMessageSeen));
                DoorUnlockMessageSeen = s.SerializeUbiArtObjectArray<UbiArtStringID>(DoorUnlockMessageSeen, name: nameof(DoorUnlockMessageSeen));
                DoorUnlockDRCMessageRequired = s.SerializeUbiArtObjectArray<UbiArtStringID>(DoorUnlockDRCMessageRequired, name: nameof(DoorUnlockDRCMessageRequired));
                LuckyTicketRewardWorldName = s.SerializeObject<UbiArtStringID>(LuckyTicketRewardWorldName, name: nameof(LuckyTicketRewardWorldName));
                IsUGCMiiverseWarningSet = s.SerializeBool<uint>(IsUGCMiiverseWarningSet, name: nameof(IsUGCMiiverseWarningSet));
                Reward39Failed = s.Serialize<int>(Reward39Failed, name: nameof(Reward39Failed));
                UnlockPrivilegesData = s.SerializeLengthPrefixedString(UnlockPrivilegesData, name: nameof(UnlockPrivilegesData));
                IsDemoRewardChecked = s.Serialize<int>(IsDemoRewardChecked, name: nameof(IsDemoRewardChecked));
                PrisonerDataDummy = s.SerializeObject<PrisonerData>(PrisonerDataDummy, name: nameof(PrisonerDataDummy));
                PersistentGameDataLevelDummy = s.SerializeObject<PersistentGameData_Level>(PersistentGameDataLevelDummy, name: nameof(PersistentGameDataLevelDummy));
                MessageDummy = s.SerializeObject<Message>(MessageDummy, name: nameof(MessageDummy));
                UnlockedDoorDummy = s.SerializeObject<UnlockedDoor>(UnlockedDoorDummy, name: nameof(UnlockedDoorDummy));
                BubbleDreamerDataDummy = s.SerializeObject<PersistentGameData_BubbleDreamerData>(BubbleDreamerDataDummy, name: nameof(BubbleDreamerDataDummy));
                DummmyNodeData = s.SerializeObject<NodeDataStruct>(DummmyNodeData, name: nameof(DummmyNodeData));
            }
        }

        public class Message : IBinarySerializable
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

            public SmartLocId[] Buttons { get; set; }

            public Attribute[] Attributes { get; set; }

            public Marker[] Markers { get; set; }

            public class Marker : IBinarySerializable
            {
                public SmartLocId LocId { get; set; }

                public uint Color { get; set; }

                public float FontSize { get; set; }

                public void Serialize(IBinarySerializer s)
                {
                    LocId = s.SerializeObject<SmartLocId>(LocId, name: nameof(LocId));
                    Color = s.Serialize<uint>(Color, name: nameof(Color));
                    FontSize = s.Serialize<float>(FontSize, name: nameof(FontSize));
                }
            }

            public class Attribute : IBinarySerializable
            {
                public uint Type { get; set; }

                public uint Value { get; set; }

                public void Serialize(IBinarySerializer s)
                {
                    Type = s.Serialize<uint>(Type, name: nameof(Type));
                    Value = s.Serialize<uint>(Value, name: nameof(Value));
                }
            }

            public void Serialize(IBinarySerializer s)
            {
                Message_handle = s.Serialize<uint>(Message_handle, name: nameof(Message_handle));
                Type = s.Serialize<uint>(Type, name: nameof(Type));
                Onlinedate = s.SerializeObject<UbiArtDateTime>(Onlinedate, name: nameof(Onlinedate));
                LocalDate = s.SerializeObject<UbiArtDateTime>(LocalDate, name: nameof(LocalDate));
                PersistentSeconds = s.Serialize<uint>(PersistentSeconds, name: nameof(PersistentSeconds));
                Title = s.SerializeObject<SmartLocId>(Title, name: nameof(Title));
                Body = s.SerializeObject<SmartLocId>(Body, name: nameof(Body));
                IsPrompt = s.SerializeBool<uint>(IsPrompt, name: nameof(IsPrompt));
                IsDrc = s.SerializeBool<uint>(IsDrc, name: nameof(IsDrc));
                HasBeenRead = s.SerializeBool<uint>(HasBeenRead, name: nameof(HasBeenRead));
                IsOnline = s.SerializeBool<uint>(IsOnline, name: nameof(IsOnline));
                RemoveAfterRead = s.SerializeBool<uint>(RemoveAfterRead, name: nameof(RemoveAfterRead));
                HasBeenInteract = s.SerializeBool<uint>(HasBeenInteract, name: nameof(HasBeenInteract));
                RemoveAfterInteract = s.SerializeBool<uint>(RemoveAfterInteract, name: nameof(RemoveAfterInteract));
                LockedAfterInteract = s.SerializeBool<uint>(LockedAfterInteract, name: nameof(LockedAfterInteract));
                Buttons = s.SerializeUbiArtObjectArray<SmartLocId>(Buttons, name: nameof(Buttons));
                Attributes = s.SerializeUbiArtObjectArray<Attribute>(Attributes, name: nameof(Attributes));
                Markers = s.SerializeUbiArtObjectArray<Marker>(Markers, name: nameof(Markers));
            }
        }

        public class PersistentGameData_Level : IBinarySerializable
        {
            #region Public Properties

            public UbiArtStringID Id { get; set; }

            public uint BestLumsTaken { get; set; }

            public float BestDistance { get; set; }

            public float BestTime { get; set; }

            public PrisonerData[] FreedPrisoners { get; set; }

            public uint Cups { get; set; }

            public uint Medals { get; set; }

            public bool Completed { get; set; }

            public bool IsVisited { get; set; }

            public bool BestTimeSent { get; set; }

            public uint Type { get; set; }

            public uint LuckyTicketsLeft { get; set; }

            public ObjectPath[] SequenceAlreadySeen { get; set; }

            public int OnlineSynced { get; set; }

            #endregion

            #region Public Methods

            /// <summary>
            /// Handles the serialization using the specified serializer
            /// </summary>
            /// <param name="s">The serializer</param>
            public void Serialize(IBinarySerializer s)
            {
                Id = s.SerializeObject<UbiArtStringID>(Id, name: nameof(Id));
                BestLumsTaken = s.Serialize<uint>(BestLumsTaken, name: nameof(BestLumsTaken));
                BestDistance = s.Serialize<float>(BestDistance, name: nameof(BestDistance));
                BestTime = s.Serialize<float>(BestTime, name: nameof(BestTime));
                FreedPrisoners = s.SerializeUbiArtObjectArray<PrisonerData>(FreedPrisoners, name: nameof(FreedPrisoners));
                Cups = s.Serialize<uint>(Cups, name: nameof(Cups));
                Medals = s.Serialize<uint>(Medals, name: nameof(Medals));
                Completed = s.SerializeBool<uint>(Completed, name: nameof(Completed));
                IsVisited = s.SerializeBool<uint>(IsVisited, name: nameof(IsVisited));
                BestTimeSent = s.SerializeBool<uint>(BestTimeSent, name: nameof(BestTimeSent));
                Type = s.Serialize<uint>(Type, name: nameof(Type));
                LuckyTicketsLeft = s.Serialize<uint>(LuckyTicketsLeft, name: nameof(LuckyTicketsLeft));
                SequenceAlreadySeen = s.SerializeUbiArtObjectArray<ObjectPath>(SequenceAlreadySeen, name: nameof(SequenceAlreadySeen));
                OnlineSynced = s.Serialize<int>(OnlineSynced, name: nameof(OnlineSynced));
            }

            #endregion
        }

        public class SaveSession : IBinarySerializable
        {
            public float[] Tags { get; set; }

            public float[] Timers { get; set; }

            public UbiArtObjKeyValuePair<UbiArtStringID, bool>[] RewardsState { get; set; }

            public void Serialize(IBinarySerializer s)
            {
                Tags = s.SerializeUbiArtArray<float>(Tags, name: nameof(Tags));
                Timers = s.SerializeUbiArtArray<float>(Timers, name: nameof(Timers));
                RewardsState = s.SerializeUbiArtObjectArray<UbiArtObjKeyValuePair<UbiArtStringID, bool>>(RewardsState, name: nameof(RewardsState));
            }
        }

        public class PersistentGameData_Score : IBinarySerializable
        {
            #region Public Properties

            public uint[] PlayersLumCount { get; set; }

            public uint[] TreasuresLumCount { get; set; }

            public int LocalLumsCount { get; set; }

            public int PendingLumsCount { get; set; }

            public int TempLumsCount { get; set; }

            #endregion

            #region Public Methods

            /// <summary>
            /// Handles the serialization using the specified serializer
            /// </summary>
            /// <param name="s">The serializer</param>
            public void Serialize(IBinarySerializer s)
            {
                PlayersLumCount = s.SerializeUbiArtArray<uint>(PlayersLumCount, name: nameof(PlayersLumCount));
                TreasuresLumCount = s.SerializeUbiArtArray<uint>(TreasuresLumCount, name: nameof(TreasuresLumCount));
                LocalLumsCount = s.Serialize<int>(LocalLumsCount, name: nameof(LocalLumsCount));
                PendingLumsCount = s.Serialize<int>(PendingLumsCount, name: nameof(PendingLumsCount));
                TempLumsCount = s.Serialize<int>(TempLumsCount, name: nameof(TempLumsCount));
            }

            #endregion
        }

        public class ProfileData : IBinarySerializable
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

            /// <summary>
            /// Handles the serialization using the specified serializer
            /// </summary>
            /// <param name="s">The serializer</param>
            public void Serialize(IBinarySerializer s)
            {
                Pid = s.Serialize<int>(Pid, name: nameof(Pid));
                Name = s.SerializeLengthPrefixedString(Name, name: nameof(Name));
                StatusIcon = s.Serialize<uint>(StatusIcon, name: nameof(StatusIcon));
                Country = s.Serialize<int>(Country, name: nameof(Country));
                GlobalMedalsRank = s.Serialize<uint>(GlobalMedalsRank, name: nameof(GlobalMedalsRank));
                GlobalMedalsMaxRank = s.Serialize<uint>(GlobalMedalsMaxRank, name: nameof(GlobalMedalsMaxRank));
                DiamondMedals = s.Serialize<uint>(DiamondMedals, name: nameof(DiamondMedals));
                GoldMedals = s.Serialize<uint>(GoldMedals, name: nameof(GoldMedals));
                SilverMedals = s.Serialize<uint>(SilverMedals, name: nameof(SilverMedals));
                BronzeMedals = s.Serialize<uint>(BronzeMedals, name: nameof(BronzeMedals));
                PlayerStats = s.SerializeObject<PlayerStatsData>(PlayerStats, name: nameof(PlayerStats));
                Costume = s.Serialize<uint>(Costume, name: nameof(Costume));
                TotalChallengePlayed = s.Serialize<uint>(TotalChallengePlayed, name: nameof(TotalChallengePlayed));
            }
        }

        public class PlayerStatsData : IBinarySerializable
        {
            public PlayerStatsDataItem Lums { get; set; }

            public PlayerStatsDataItem Distance { get; set; }

            public PlayerStatsDataItem Kills { get; set; }

            public PlayerStatsDataItem Jumps { get; set; }

            public PlayerStatsDataItem Deaths { get; set; }

            public class PlayerStatsDataItem : IBinarySerializable
            {
                public float Value { get; set; }

                public uint Rank { get; set; }

                public void Serialize(IBinarySerializer s)
                {
                    Value = s.Serialize<float>(Value, name: nameof(Value));
                    Rank = s.Serialize<uint>(Rank, name: nameof(Rank));
                }
            }

            public void Serialize(IBinarySerializer s)
            {
                Lums = s.SerializeObject<PlayerStatsDataItem>(Lums, name: nameof(Lums));
                Distance = s.SerializeObject<PlayerStatsDataItem>(Distance, name: nameof(Distance));
                Kills = s.SerializeObject<PlayerStatsDataItem>(Kills, name: nameof(Kills));
                Jumps = s.SerializeObject<PlayerStatsDataItem>(Jumps, name: nameof(Jumps));
                Deaths = s.SerializeObject<PlayerStatsDataItem>(Deaths, name: nameof(Deaths));
            }
        }

        public class PersistentGameData_BubbleDreamerData : IBinarySerializable
        {
            #region Public Properties

            public bool HasMet { get; set; }

            public bool UpdateRequested { get; set; }

            public bool HasWonPetCup { get; set; }

            public uint TeensyLocksOpened { get; set; }

            public uint ChallengeLocksOpened { get; set; }

            public uint TutoCount { get; set; }

            public bool[] DisplayQuoteStates { get; set; }

            #endregion

            #region Public Methods

            public void Serialize(IBinarySerializer s)
            {
                HasMet = s.SerializeBool<uint>(HasMet, name: nameof(HasMet));
                UpdateRequested = s.SerializeBool<uint>(UpdateRequested, name: nameof(UpdateRequested));
                HasWonPetCup = s.SerializeBool<uint>(HasWonPetCup, name: nameof(HasWonPetCup));
                TeensyLocksOpened = s.Serialize<uint>(TeensyLocksOpened, name: nameof(TeensyLocksOpened));
                ChallengeLocksOpened = s.Serialize<uint>(ChallengeLocksOpened, name: nameof(ChallengeLocksOpened));
                TutoCount = s.Serialize<uint>(TutoCount, name: nameof(TutoCount));
                DisplayQuoteStates = s.SerializeUbiArtArray<bool>(DisplayQuoteStates, name: nameof(DisplayQuoteStates));
            }

            #endregion
        }

        public class PetRewardData : IBinarySerializable
        {
            public uint LastSpawnDay { get; set; }

            public uint MaxRewardNb { get; set; }

            public uint RemainingRewards { get; set; }

            public uint RewardType { get; set; }

            public void Serialize(IBinarySerializer s)
            {
                LastSpawnDay = s.Serialize<uint>(LastSpawnDay, name: nameof(LastSpawnDay));
                MaxRewardNb = s.Serialize<uint>(MaxRewardNb, name: nameof(MaxRewardNb));
                RemainingRewards = s.Serialize<uint>(RemainingRewards, name: nameof(RemainingRewards));
                RewardType = s.Serialize<uint>(RewardType, name: nameof(RewardType));
            }
        }

        public class St_petCups : IBinarySerializable
        {
            public int Family { get; set; }

            public uint Cups { get; set; }

            public void Serialize(IBinarySerializer s)
            {
                Family = s.Serialize<int>(Family, name: nameof(Family));
                Cups = s.Serialize<uint>(Cups, name: nameof(Cups));
            }
        }

        public class UnlockedDoor : IBinarySerializable
        {
            #region Public Properties

            public UbiArtStringID WorldTag { get; set; }

            public uint Type { get; set; }

            public bool IsNew { get; set; }

            #endregion

            #region Public Methods

            public void Serialize(IBinarySerializer s)
            {
                WorldTag = s.SerializeObject<UbiArtStringID>(WorldTag, name: nameof(WorldTag));
                Type = s.Serialize<uint>(Type, name: nameof(Type));
                IsNew = s.SerializeBool<uint>(IsNew, name: nameof(IsNew));
            }

            #endregion
        }

        public class RO2_LuckyTicketReward : IBinarySerializable
        {
            #region Public Properties

            public uint ID { get; set; }

            public uint Type { get; set; }

            #endregion

            #region Public Methods

            public void Serialize(IBinarySerializer s)
            {
                ID = s.Serialize<uint>(ID, name: nameof(ID));
                Type = s.Serialize<uint>(Type, name: nameof(Type));
            }

            #endregion
        }

        public class NodeDataStruct : IBinarySerializable
        {
            #region Public Properties

            public UbiArtStringID Tag { get; set; }

            public bool UnteaseSeen { get; set; }

            public bool UnlockSeend { get; set; }

            public bool SentUnlockMessage { get; set; }

            #endregion

            #region Public Methods

            public void Serialize(IBinarySerializer s)
            {
                Tag = s.SerializeObject<UbiArtStringID>(Tag, name: nameof(Tag));
                UnteaseSeen = s.SerializeBool<uint>(UnteaseSeen, name: nameof(UnteaseSeen));
                UnlockSeend = s.SerializeBool<uint>(UnlockSeend, name: nameof(UnlockSeend));
                SentUnlockMessage = s.SerializeBool<uint>(SentUnlockMessage, name: nameof(SentUnlockMessage));
            }

            #endregion
        }

        public class PrisonerData : IBinarySerializable
        {
            #region Public Properties

            public UbiArtPath Path { get; set; }

            public bool IsFree { get; set; }

            public Index IndexType { get; set; }

            public Prisoner VisualType { get; set; }

            #endregion

            #region Enums

            public enum Index : uint
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

            public enum Prisoner : uint
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

            public void Serialize(IBinarySerializer s)
            {
                Path = s.SerializeObject<UbiArtPath>(Path, name: nameof(Path));
                IsFree = s.SerializeBool<uint>(IsFree, name: nameof(IsFree));
                IndexType = s.Serialize<Index>(IndexType, name: nameof(IndexType));
                VisualType = s.Serialize<Prisoner>(VisualType, name: nameof(VisualType));
            }

            #endregion
        }

        #endregion
    }
}