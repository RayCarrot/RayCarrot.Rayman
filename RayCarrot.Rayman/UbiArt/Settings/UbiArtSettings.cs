using System.Text;
using RayCarrot.Binary;

namespace RayCarrot.Rayman.UbiArt
{
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
                if (game != UbiArtGame.RaymanOrigins)
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