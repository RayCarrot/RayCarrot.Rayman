using System;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// Attribute to use on <see cref="UbiArtGameMode"/> fields, specifying the settings and data
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class UbiArtGameModeInfoAttribute : GameModeBaseAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="displayName">The game mode display name</param>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        /// <param name="byteOrder">The byte order to use</param>
        /// <param name="textEncoding">The text encoding to use</param>
        public UbiArtGameModeInfoAttribute(string displayName, UbiArtGame game, UbiArtPlatform platform, ByteOrder byteOrder, TextEncoding textEncoding) : base(displayName)
        {
            Game = game;
            Platform = platform;
            ByteOrder = byteOrder;
            TextEncoding = textEncoding;
        }

        /// <summary>
        /// The game
        /// </summary>
        public UbiArtGame Game { get; }

        /// <summary>
        /// The platform
        /// </summary>
        public UbiArtPlatform Platform { get; }

        /// <summary>
        /// The byte order to use
        /// </summary>
        public ByteOrder ByteOrder { get; }

        /// <summary>
        /// The text encoding to use
        /// </summary>
        public TextEncoding TextEncoding { get; }
    }
}