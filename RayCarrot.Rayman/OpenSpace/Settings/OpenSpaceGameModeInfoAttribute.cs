using System;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// Attribute to use on <see cref="OpenSpaceGameMode"/> fields, specifying the settings and data
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class OpenSpaceGameModeInfoAttribute : GameModeBaseAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="displayName">The game mode display name</param>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        public OpenSpaceGameModeInfoAttribute(string displayName, OpenSpaceGame game, OpenSpacePlatform platform) : base(displayName)
        {
            Game = game;
            Platform = platform;
        }

        /// <summary>
        /// The game
        /// </summary>
        public OpenSpaceGame Game { get; }

        /// <summary>
        /// The platform
        /// </summary>
        public OpenSpacePlatform Platform { get; }
    }
}