using System.Text;
using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
    /*
        Textures:

        Origins (PC):                 (.dds)
        Origins (Wii):                Reverse endianness (.dds)
        Origins (3DS):                (.???)
        Origins (Vita):               (.gxt)
        Just Dance 2017 (Wii U)       (.png, .jpg, .gtx)

        Legends (PS3):                Missing DDS header (.dds)

        Legends demo (Wii U):         44 byte TEX header, version 8 (.gtx) (GFX2)
        Legends (Vita):               44 byte TEX header, version 9 (.gxt)

        Child of Light (PC):          52 byte TEX header, version 12 (.dds)
        Child of Light (Vita):        52 byte TEX header, version 12 (.gxt)
        Legends (PC):                 52 byte TEX header, version 13 (.dds)
        Legends (Wii U):              52 byte TEX header, version 13 (.gtx) (GFX2)

        Legends (PS4):                52 byte TEX header, version 23 (.gnf)
        Legends (Switch):             52 byte TEX header, version 23 (.xtx) (DFvN)
        Gravity Falls (3DS):          52 byte TEX header, version 23 (.???)

        Valiant Heart (Android):      56 byte TEX header, version 16 (.dds)
        Valiant Heart (Switch):       56 byte TEX header, version 16 (.xtx)
        Rayman Adventures (Android):  56 byte TEX header, version 17 (.dds)
        Rayman Adventures (iOS):      56 byte TEX header, version 17 (.dds, .pvr)
        Rayman Mini (Mac):            56 byte TEX header, version 17 (.dds)

    */

    /*
        IPK:

        Rayman Origins (PC, Wii, PS3, PS Vita):      3 
        Rayman Origins (3DS):                        4
        Rayman Legends (PC, Wii U, PS Vita, Switch): 5
        Just Dance 2017 (Wii U):                     5
        Valiant Hearts (Android):                    7
        Child of Light (PC, PS Vita):                7
        Rayman Legends (PS4):                        7
        Gravity Falls (3DS):                         7
        Rayman Adventures (Android, iOS):            8
        Rayman Mini (Mac):                           8

    */

    /// <summary>
    /// Settings for serializing UbiArt game formats
    /// </summary>
    public class UbiArtSettings : BinarySerializerSettings
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="endian">The endianness</param>
        /// <param name="textEncoding">The text encoding to use</param>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        public UbiArtSettings(Endian endian, Encoding textEncoding, UbiArtGame game, Platform platform) : base(endian, textEncoding)
        {
            Game = game;
            Platform = platform;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// The game
        /// </summary>
        public UbiArtGame Game { get; }

        /// <summary>
        /// The platform
        /// </summary>
        public Platform Platform { get; }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Gets the default settings based on the game and platform
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        /// <returns>The settings</returns>
        public static UbiArtSettings GetDefaultSettings(UbiArtGame game, Platform platform)
        {
            var isLittleEndian = game == UbiArtGame.RaymanOrigins && platform == Platform.Nintendo3DS;

            Encoding getEncoding()
            {
                if (game != UbiArtGame.RaymanOrigins && game != UbiArtGame.RaymanJungleRun && game != UbiArtGame.RaymanFiestaRun)
                    return Encoding.UTF8;

                return isLittleEndian ? Encoding.Unicode : Encoding.BigEndianUnicode;
            }

            return new UbiArtSettings(isLittleEndian ? Endian.Little : Endian.Big, getEncoding(), game, platform);
        }

        /// <summary>
        /// Gets the default settings for save files based on the game and platform
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        /// <returns>The settings</returns>
        public static UbiArtSettings GetSaveSettings(UbiArtGame game, Platform platform)
        {
            var isLittleEndian = game == UbiArtGame.RaymanJungleRun || game == UbiArtGame.RaymanFiestaRun || (game == UbiArtGame.RaymanOrigins && platform == Platform.Nintendo3DS);

            return new UbiArtSettings(isLittleEndian ? Endian.Little : Endian.Big, Encoding.UTF8, game, platform);
        }

        #endregion
    }
}