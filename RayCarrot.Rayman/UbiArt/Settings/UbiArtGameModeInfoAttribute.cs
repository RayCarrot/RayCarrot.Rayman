using System;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// Attribute to use on <see cref="GameMode"/> fields, specifying the settings and data
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
        public UbiArtGameModeInfoAttribute(string displayName, UbiArtGame game, Platform platform) : base(displayName)
        {
            Game = game;
            Platform = platform;
        }

        /// <summary>
        /// The game
        /// </summary>
        public UbiArtGame Game { get; }

        /// <summary>
        /// The platform
        /// </summary>
        public Platform Platform { get; }
    }
}