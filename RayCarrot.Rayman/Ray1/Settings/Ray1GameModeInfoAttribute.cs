using System;

namespace RayCarrot.Rayman.Ray1
{
    /// <summary>
    /// Attribute to use on <see cref="GameMode"/> fields, specifying the settings and data
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class Ray1GameModeInfoAttribute : GameModeBaseAttribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="displayName">The game mode display name</param>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        public Ray1GameModeInfoAttribute(string displayName, Ray1Game game, Platform platform) : base(displayName)
        {
            Game = game;
            Platform = platform;
        }

        /// <summary>
        /// The game
        /// </summary>
        public Ray1Game Game { get; }

        /// <summary>
        /// The platform
        /// </summary>
        public Platform Platform { get; }
    }
}