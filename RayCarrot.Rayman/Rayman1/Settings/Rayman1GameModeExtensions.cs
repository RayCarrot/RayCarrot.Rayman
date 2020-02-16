using RayCarrot.Extensions;

namespace RayCarrot.Rayman.Rayman1
{
    /// <summary>
    /// Extension methods for <see cref="Rayman1GameMode"/>
    /// </summary>
    public static class Rayman1GameModeExtensions
    {
        /// <summary>
        /// Gets the display name for the game mode
        /// </summary>
        /// <param name="gameMode">The game mode</param>
        /// <returns>The display name</returns>
        public static string GetDisplayName(this Rayman1GameMode gameMode)
        {
            var attribute = gameMode.GetAttribute<Rayman1GameModeInfoAttribute>();

            return attribute.DisplayName;
        }

        /// <summary>
        /// Gets the settings for the game mode
        /// </summary>
        /// <param name="gameMode">The game mode</param>
        /// <returns>The OpenSpace settings</returns>
        public static Rayman1Settings GetSettings(this Rayman1GameMode gameMode)
        {
            var attribute = gameMode.GetAttribute<Rayman1GameModeInfoAttribute>();

            return new Rayman1Settings(attribute.Game, attribute.Platform);
        }
    }
}