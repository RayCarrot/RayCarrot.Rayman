using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace RayCarrot.Rayman.Ray1
{
    public class Rayman1PS1Password
    {
        /*

        A PS1 password always consist of 10 bytes. These bytes can be in several forms:

        Indexed    -> Each byte is an index 0-31
        Encrypted  -> Use PasswordIndexTranslateTable[] on each indexed byte to get the encrypted data
        Decrypted  -> Use VerifyAndDecrypt() on an encrypted password, this is the data we need to get the data from the password
        
        Display    -> Use PasswordDisplayTable[] on each encrypted byte, this will give the character representation of each byte to display

        */

        #region Constructors

        /// <summary>
        /// Creates a new password from the encrypted password data
        /// </summary>
        /// <param name="encryptedPassword">The encrypted password</param>
        /// <param name="mode">The password mode</param>
        public Rayman1PS1Password(byte[] encryptedPassword, PasswordMode mode)
        {
            if (mode == PasswordMode.PAL)
                throw new NotImplementedException();

            Password = encryptedPassword;
            Mode = mode;
        }

        /// <summary>
        /// Creates a new password from the display version of a password
        /// </summary>
        /// <param name="password">The password display version</param>
        /// <param name="mode">The password mode</param>
        public Rayman1PS1Password(string password, PasswordMode mode)
        {
            if (mode == PasswordMode.PAL)
                throw new NotImplementedException();

            Password = password.ToLower().Select(b => (byte)Array.FindIndex(PasswordDisplayTable[mode], x => (char)x == b)).ToArray();
            Mode = mode;
        }

        /// <summary>
        /// Creates a new password from save data
        /// </summary>
        /// <param name="save">The save</param>
        /// <param name="mode">The password mode</param>
        public Rayman1PS1Password(SaveData save, PasswordMode mode)
        {
            if (mode == PasswordMode.PAL)
                throw new NotImplementedException();

            // Set the mode
            Mode = mode;

            // Create a decrypted password
            Password = new byte[10];

            // Encode the password
            save.Process(Password, false);

            // Encrypt the password
            EncryptPassword(Password);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The encrypted password
        /// </summary>
        public byte[] Password { get; }

        /// <summary>
        /// The password mode
        /// </summary>
        public PasswordMode Mode { get; }

        #endregion

        #region Protected Static Methods

        /// <summary>
        /// XORs the password and returns the password
        /// </summary>
        /// <param name="password">The password</param>
        /// <param name="isDecrypting">Indicates if the password is being decrypted</param>
        /// <returns>The hash</returns>
        protected int XORAndCalculateHash(byte[] password, bool isDecrypting)
        {
            int hash = 0;

            for (int i = 0; i < password.Length; i++)
            {
                var value = password[i];

                // XOR the byte
                password[i] = (byte)(password[i] ^ PasswordXORTable[Mode][i]);

                if (!isDecrypting)
                    value = password[i];

                // Calculate the hash
                hash += (value >> 1) * PasswordHashTable[Mode][i];
            }

            return hash;
        }

        /// <summary>
        /// Verifies and decrypts the password
        /// </summary>
        /// <param name="password">The password</param>
        /// <returns>True if the password was successfully verified, otherwise false</returns>
        protected bool VerifyAndDecrypt(byte[] password)
        {
            // Decrypt the password and get the hash
            int hash = XORAndCalculateHash(password, true);

            var encodedHash = 0;

            // Get the encoded hash
            encodedHash = BitHelpers.SetBits(encodedHash, BitHelpers.ExtractBits(password[0], 1, 0), 1, 5);
            encodedHash = BitHelpers.SetBits(encodedHash, BitHelpers.ExtractBits(password[1], 1, 0), 1, 6);
            encodedHash = BitHelpers.SetBits(encodedHash, BitHelpers.ExtractBits(password[2], 1, 0), 1, 8);
            encodedHash = BitHelpers.SetBits(encodedHash, BitHelpers.ExtractBits(password[3], 1, 0), 1, 7);
            encodedHash = BitHelpers.SetBits(encodedHash, BitHelpers.ExtractBits(password[4], 1, 0), 1, 9);
            encodedHash = BitHelpers.SetBits(encodedHash, BitHelpers.ExtractBits(password[5], 1, 0), 1, 4);
            encodedHash = BitHelpers.SetBits(encodedHash, BitHelpers.ExtractBits(password[6], 1, 0), 1, 2);
            encodedHash = BitHelpers.SetBits(encodedHash, BitHelpers.ExtractBits(password[7], 1, 0), 1, 1);
            encodedHash = BitHelpers.SetBits(encodedHash, BitHelpers.ExtractBits(password[8], 1, 0), 1, 3);
            encodedHash = BitHelpers.SetBits(encodedHash, BitHelpers.ExtractBits(password[9], 1, 0), 1, 0);

            // Compare the hashes
            return hash == encodedHash;
        }

        /// <summary>
        /// Encrypts the password
        /// </summary>
        /// <param name="password">The password</param>
        protected void EncryptPassword(byte[] password)
        {
            // Encrypt the password and get the hash
            int hash = XORAndCalculateHash(password, false);

            // Encode the hash
            password[0] = (byte)BitHelpers.SetBits(password[0], BitHelpers.ExtractBits(hash, 1, 5), 1, 0);
            password[1] = (byte)BitHelpers.SetBits(password[1], BitHelpers.ExtractBits(hash, 1, 6), 1, 0);
            password[2] = (byte)BitHelpers.SetBits(password[2], BitHelpers.ExtractBits(hash, 1, 8), 1, 0);
            password[3] = (byte)BitHelpers.SetBits(password[3], BitHelpers.ExtractBits(hash, 1, 7), 1, 0);
            password[4] = (byte)BitHelpers.SetBits(password[4], BitHelpers.ExtractBits(hash, 1, 9), 1, 0);
            password[5] = (byte)BitHelpers.SetBits(password[5], BitHelpers.ExtractBits(hash, 1, 4), 1, 0);
            password[6] = (byte)BitHelpers.SetBits(password[6], BitHelpers.ExtractBits(hash, 1, 2), 1, 0);
            password[7] = (byte)BitHelpers.SetBits(password[7], BitHelpers.ExtractBits(hash, 1, 1), 1, 0);
            password[8] = (byte)BitHelpers.SetBits(password[8], BitHelpers.ExtractBits(hash, 1, 3), 1, 0);
            password[9] = (byte)BitHelpers.SetBits(password[9], BitHelpers.ExtractBits(hash, 1, 0), 1, 0);
        }

        #endregion

        #region Public Static Methods

        public static char[] GetSupportedCharacters() => new char[]
        {
            '?', 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 
            'T', 'V', 'W', 'X', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '!',
        };

        #endregion

        #region Public Methods

        /// <summary>
        /// Decodes the password and gets the save data
        /// </summary>
        /// <returns>The save or null if invalid</returns>
        public SaveData Decode()
        {
            // Create a new save
            var save = new SaveData();

            // Get the password and create a new array
            var password = Password.ToArray();

            // Verify and decrypt the password
            if (!VerifyAndDecrypt(password))
                return null;

            // Decode the password
            save.Process(password, true);

            return save;
        }

        public override string ToString() => Password.Aggregate(String.Empty, (current, p) => current + (char)PasswordDisplayTable[Mode][p]);

        #endregion

        #region Tables

        protected Dictionary<PasswordMode, byte[]> PasswordIndexTranslateTable { get; } = new Dictionary<PasswordMode, byte[]>()
        {
            [PasswordMode.NTSC] = new byte[]
            {
                0x19, 0x02, 0x1B, 0x04, 0x1D, 0x06, 0x1F, 0x10,
                0x09, 0x12, 0x0B, 0x14, 0x0D, 0x16, 0x0F, 0x18,
                0x11, 0x0A, 0x13, 0x15, 0x0E, 0x17, 0x08, 0x01,
                0x1A, 0x03, 0x0C, 0x05, 0x1E, 0x07, 0x00, 0x1C
            },
        };
        protected Dictionary<PasswordMode, byte[]> PasswordDisplayTable { get; } = new Dictionary<PasswordMode, byte[]>()
        {
            [PasswordMode.NTSC] = new byte[]
            {
                0x21, 0x33, 0x63, 0x35, 0x66, 0x37, 0x68, 0x39,
                0x32, 0x6C, 0x77, 0x6E, 0x36, 0x71, 0x30, 0x73,
                0x6B, 0x76, 0x6D, 0x78, 0x70, 0x7A, 0x72, 0x31,
                0x74, 0x62, 0x34, 0x64, 0x3F, 0x67, 0x38, 0x6A
            },
        };
        protected Dictionary<PasswordMode, byte[]> PasswordXORTable { get; } = new Dictionary<PasswordMode, byte[]>()
        {
            [PasswordMode.NTSC] = new byte[]
            {
                0x18, 0x08, 0x12, 0x0A, 0x06, 0x16, 0x04, 0x1C, 0x0C, 0x14
            },
        };
        protected Dictionary<PasswordMode, byte[]> PasswordHashTable { get; } = new Dictionary<PasswordMode, byte[]>()
        {
            [PasswordMode.NTSC] = new byte[]
            {
                0x02, 0x03, 0x05, 0x07, 0x11, 0x02, 0x03, 0x05, 0x07, 0x11
            },
        };

        #endregion

        #region Enums

        public enum PasswordMode
        {
            NTSC,
            PAL,
        }

        #endregion

        #region Classes

        public class SaveData
        {
            public SaveData()
            {
                WorldInfo = new WorldMapInfo[24];

                for (int i = 0; i < WorldInfo.Length; i++)
                    WorldInfo[i] = new WorldMapInfo();

                WorldInfo[0].IsUnlocked = true; // First level should always be unlocked
            }

            public WorldMapInfo[] WorldInfo { get; }
            public Rayman1FinBossLevelFlags FinBossLevel { get; set; }
            public byte LivesCount { get; set; }
            public byte Continues { get; set; }
            public byte LevelIndex
            {
                get
                {
                    byte lvlIndex;
                    byte lvl = 0x11;
                    do
                    {
                        // Branched levels are handled separately
                        if (lvl == 3 || lvl == 7)
                            lvl -= 2;

                        lvlIndex = lvl;
                        lvl--;
                    } while (!WorldInfo[lvlIndex].IsUnlocked);
                    return lvlIndex;
                }
                set
                {
                    for (int i = 0; i <= value; i++)
                        // Branched levels are handled separately
                        if (i != 3 && i != 7)
                            WorldInfo[i].IsUnlocked = true;
                }
            }

            public void Process(byte[] password, bool isDecoding)
            {
                // Get data from save
                var t_world_info = WorldInfo;
                var finBossLevel_0 = (byte)BitHelpers.ExtractBits((ushort)FinBossLevel, 8, 0);
                var finBossLevel_1 = (byte)BitHelpers.ExtractBits((ushort)FinBossLevel, 8, 8);
                var lives = LivesCount;
                var nb_continue = Continues;

                // Process world info
                t_world_info[0].ProcessHasAllCages(ref password[0], 1, isDecoding);
                t_world_info[1].ProcessHasAllCages(ref password[2], 1, isDecoding);
                t_world_info[2].ProcessHasAllCages(ref password[4], 1, isDecoding);
                t_world_info[3].ProcessHasAllCages(ref password[1], 1, isDecoding);
                t_world_info[4].ProcessHasAllCages(ref password[3], 1, isDecoding);
                t_world_info[5].ProcessHasAllCages(ref password[5], 1, isDecoding);
                t_world_info[6].ProcessHasAllCages(ref password[7], 1, isDecoding);
                t_world_info[7].ProcessHasAllCages(ref password[6], 1, isDecoding);
                t_world_info[8].ProcessHasAllCages(ref password[8], 1, isDecoding);
                t_world_info[9].ProcessHasAllCages(ref password[9], 1, isDecoding);
                t_world_info[10].ProcessHasAllCages(ref password[4], 4, isDecoding);
                t_world_info[11].ProcessHasAllCages(ref password[0], 4, isDecoding);
                t_world_info[12].ProcessHasAllCages(ref password[2], 4, isDecoding);
                t_world_info[13].ProcessHasAllCages(ref password[1], 4, isDecoding);
                t_world_info[14].ProcessHasAllCages(ref password[5], 4, isDecoding);
                t_world_info[15].ProcessHasAllCages(ref password[3], 4, isDecoding);
                t_world_info[16].ProcessHasAllCages(ref password[7], 4, isDecoding);
                t_world_info[3].ProcessIsUnlocked(ref password[8], 2, isDecoding); // Jungle branch
                t_world_info[7].ProcessIsUnlocked(ref password[9], 2, isDecoding); // Music branch

                // Get the level based on unlocked levels if we are encoding
                byte lvlIndex = LevelIndex;

                // Process level
                BitHelpers.CopyBits(ref lvlIndex, ref password[4], 1, 0, 2, isDecoding);
                BitHelpers.CopyBits(ref lvlIndex, ref password[2], 1, 1, 2, isDecoding);
                BitHelpers.CopyBits(ref lvlIndex, ref password[3], 1, 2, 2, isDecoding);
                BitHelpers.CopyBits(ref lvlIndex, ref password[1], 1, 3, 2, isDecoding);
                BitHelpers.CopyBits(ref lvlIndex, ref password[0], 1, 4, 2, isDecoding);

                // Unlock the levels based on the level index
                LevelIndex = lvlIndex;

                // Process finish boss level flags
                BitHelpers.CopyBits(ref finBossLevel_0, ref password[9], 1, 1, 3, isDecoding); // Moskito
                BitHelpers.CopyBits(ref finBossLevel_0, ref password[6], 1, 6, 3, isDecoding); // Skops
                BitHelpers.CopyBits(ref finBossLevel_0, ref password[8], 1, 2, 3, isDecoding); // Mr Sax
                BitHelpers.CopyBits(ref finBossLevel_0, ref password[6], 1, 7, 4, isDecoding); // Mr Dark
                BitHelpers.CopyBits(ref finBossLevel_1, ref password[8], 1, 3, 4, isDecoding); // Helped The Musician

                // Some flags are automatically determined based on the level index
                finBossLevel_0 = (byte)BitHelpers.SetBits(finBossLevel_0, lvlIndex >= 4 ? 1 : 0, 1, 0); // Bzzit
                finBossLevel_0 = (byte)BitHelpers.SetBits(finBossLevel_0, lvlIndex >= 11 ? 1 : 0, 1, 3); // Mr Stone
                finBossLevel_0 = (byte)BitHelpers.SetBits(finBossLevel_0, lvlIndex >= 12 ? 1 : 0, 1, 4); // Viking Mama
                finBossLevel_0 = (byte)BitHelpers.SetBits(finBossLevel_0, lvlIndex >= 14 ? 1 : 0, 1, 5); // Space Mama

                // Process lives count
                BitHelpers.CopyBits(ref lives, ref password[3], 1, 0, 3, isDecoding);
                BitHelpers.CopyBits(ref lives, ref password[2], 1, 1, 3, isDecoding);
                BitHelpers.CopyBits(ref lives, ref password[5], 1, 2, 3, isDecoding);
                BitHelpers.CopyBits(ref lives, ref password[1], 1, 3, 3, isDecoding);
                BitHelpers.CopyBits(ref lives, ref password[4], 1, 4, 3, isDecoding);
                BitHelpers.CopyBits(ref lives, ref password[0], 1, 5, 3, isDecoding);
                BitHelpers.CopyBits(ref lives, ref password[7], 1, 6, 3, isDecoding);

                // Process continues
                BitHelpers.CopyBits(ref nb_continue, ref password[5], 1, 0, 2, isDecoding);
                BitHelpers.CopyBits(ref nb_continue, ref password[6], 1, 2, 2, isDecoding);
                BitHelpers.CopyBits(ref nb_continue, ref password[7], 1, 1, 2, isDecoding);
                BitHelpers.CopyBits(ref nb_continue, ref password[9], 1, 3, 4, isDecoding);

                // Set data in save
                FinBossLevel = (Rayman1FinBossLevelFlags)BitHelpers.SetBits((ushort)FinBossLevel, finBossLevel_0, 8, 0);
                FinBossLevel = (Rayman1FinBossLevelFlags)BitHelpers.SetBits((ushort)FinBossLevel, finBossLevel_1, 8, 8);
                LivesCount = lives;
                Continues = nb_continue;
            }

            // The game calls this when generating and loading a password to make sure it's valid (0x801a1b14). We re-implement it here to make sure generated passwords will be accepted by the game.
            public string Validate()
            {
                // Get the level index
                byte level = LevelIndex;

                // Make sure the level is not above the allowed range or set to any of the branched levels
                if (level >= 18 || 1 >= (byte)(level - 2) || 1 >= (byte)(level - 6))
                    return $"Invalid level index of {level}";

                // Lives have a range of 0-99
                if (LivesCount >= 100)
                    return $"Invalid lives count of {LivesCount}";

                // Continues have a range of 0-9
                if (Continues >= 10)
                    return $"Invalid continues count of {Continues}";

                // Make sure bosses haven't been marked as complete if the level hasn't been reached
                if (FinBossLevel.HasFlag(Rayman1FinBossLevelFlags.Moskito))
                {
                    if (!WorldInfo[3].IsUnlocked || level <= 3)
                        return $"Moskito has been beaten under invalid circumstances";
                }
                else
                {
                    if (level >= 9)
                        return $"Moskito has to be beaten before Twilight Gulch";
                }

                if (FinBossLevel.HasFlag(Rayman1FinBossLevelFlags.MrSax) && (!WorldInfo[7].IsUnlocked || level <= 7))
                    return $"Mr Sax has been beaten under invalid circumstances";

                if (FinBossLevel.HasFlag(Rayman1FinBossLevelFlags.HelpedMusician))
                {
                    if (level <= 9)
                        return $"Helped The Musician under invalid circumstances";
                }
                else
                {
                    if (level >= 11 || WorldInfo[11].HasAllCages)
                        return $"Hasn't helped The Musician";
                }

                if (FinBossLevel.HasFlag(Rayman1FinBossLevelFlags.MrSkops))
                {
                    if (level <= 15 || !FinBossLevel.HasFlag(Rayman1FinBossLevelFlags.HelpedMusician) || !FinBossLevel.HasFlag(Rayman1FinBossLevelFlags.Moskito))
                        return $"Mr Skops has been beaten under invalid circumstances";
                }
                else
                {
                    if (level >= 17 || FinBossLevel.HasFlag(Rayman1FinBossLevelFlags.MrDark))
                        return $"Mr Skops has to be beaten before Mr Dark";
                }

                // Jungle branch
                if (WorldInfo[3].IsUnlocked)
                {
                    if (level <= 3)
                        return $"Jungle branch should not be unlocked";
                }
                else
                {
                    if (WorldInfo[3].HasAllCages || FinBossLevel.HasFlag(Rayman1FinBossLevelFlags.Moskito))
                        return $"Jungle branch should be unlocked";
                }

                // Music branch
                if (WorldInfo[7].IsUnlocked)
                {
                    if (level <= 7)
                        return $"Music branch should not be unlocked";
                }
                else
                {
                    if (WorldInfo[7].HasAllCages || FinBossLevel.HasFlag(Rayman1FinBossLevelFlags.MrSax))
                        return $"Music branch should be unlocked";
                }

                for (int i = level + 1; i <= 17; i++)
                {
                    if (WorldInfo[i].HasAllCages)
                        return $"Cages found in locked level {i}";
                }

                if (level > 8 && (!FinBossLevel.HasFlag(Rayman1FinBossLevelFlags.Moskito) || !WorldInfo[3].IsUnlocked))
                    return $"Moskito has to be beaten and unlocked before Twilight Gulch";

                if ((level == 17) || WorldInfo[17].HasAllCages)
                {
                    var cages = WorldInfo.Take(17).Count(x => x.HasAllCages);

                    if (cages != 17 || !FinBossLevel.HasFlag(Rayman1FinBossLevelFlags.MrSkops) || !WorldInfo[3].IsUnlocked || !WorldInfo[7].IsUnlocked)
                        return $"Last level has been unlocked under invalid circumstances";
                }

                return null;
            }

            [DebuggerDisplay("IsUnlocked = {IsUnlocked}, HasAllCages = {HasAllCages}")]
            public class WorldMapInfo
            {
                // Corresponds to (R1_WorldMapInfo.Runtime_State & 5) != 0
                public bool IsUnlocked { get; set; }

                // Corresponds to R1_WorldMapInfo.Runtime_Cages == 6
                public bool HasAllCages { get; set; }

                public void ProcessIsUnlocked(ref byte b, int offset, bool setValue)
                {
                    var isUnlocked = (byte)(IsUnlocked ? 1 : 0);
                    BitHelpers.CopyBits(ref isUnlocked, ref b, 1, 0, offset, setValue);
                    IsUnlocked = isUnlocked == 1;
                }
                public void ProcessHasAllCages(ref byte b, int offset, bool setValue)
                {
                    var hasAllCages = (byte)(HasAllCages ? 1 : 0);
                    BitHelpers.CopyBits(ref hasAllCages, ref b, 1, 0, offset, setValue);
                    HasAllCages = hasAllCages == 1;
                }
            }
        }

        #endregion
    }
}