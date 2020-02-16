using System;

namespace RayCarrot.Rayman.Rayman1
{
    /// <summary>
    /// Attribute to use on <see cref="Rayman1GameMode"/> fields, specifying the settings and data
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class Rayman1GameModeInfoAttribute : GameModeBaseAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="displayName">The game mode display name</param>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        public Rayman1GameModeInfoAttribute(string displayName, Rayman1Game game, Rayman1Platform platform) : base(displayName)
        {
            Game = game;
            Platform = platform;
        }

        /// <summary>
        /// The game
        /// </summary>
        public Rayman1Game Game { get; }

        /// <summary>
        /// The platform
        /// </summary>
        public Rayman1Platform Platform { get; }
    }
}