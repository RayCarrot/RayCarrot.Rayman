using System;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Base attribute to use on game mode enum fields, specifying the data
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class GameModeBaseAttribute : Attribute
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="displayName">The game mode display name</param>
        public GameModeBaseAttribute(string displayName)
        {
            DisplayName = displayName;
        }

        /// <summary>
        /// The game mode display name
        /// </summary>
        public string DisplayName { get; }
    }
}