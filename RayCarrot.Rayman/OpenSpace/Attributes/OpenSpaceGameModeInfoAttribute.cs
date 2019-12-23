using System;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Attribute to use on <see cref="OpenSpaceGameMode"/> fields, specifying the settings and data
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class OpenSpaceGameModeInfoAttribute : Attribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="displayName">The game mode display name</param>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        public OpenSpaceGameModeInfoAttribute(string displayName, OpenSpaceGame game, OpenSpacePlatform platform)
        {
            DisplayName = displayName;
            Game = game;
            Platform = platform;
        }

        /// <summary>
        /// The game mode display name
        /// </summary>
        public string DisplayName { get; }

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