using RayCarrot.Extensions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Extension methods for <see cref="OpenSpaceGameMode"/>
    /// </summary>
    public static class OpenSpaceGameModeExtensions
    {
        /// <summary>
        /// Gets the display name for the game mode
        /// </summary>
        /// <param name="gameMode">The game mode</param>
        /// <returns>The display name</returns>
        public static string GetDisplayName(this OpenSpaceGameMode gameMode)
        {
            var attribute = gameMode.GetAttribute<OpenSpaceGameModeInfoAttribute>();

            return attribute.DisplayName;
        }

        /// <summary>
        /// Gets the settings for the game mode
        /// </summary>
        /// <param name="gameMode">The game mode</param>
        /// <returns>The OpenSpace settings</returns>
        public static OpenSpaceSettings GetSettings(this OpenSpaceGameMode gameMode)
        {
            var attribute = gameMode.GetAttribute<OpenSpaceGameModeInfoAttribute>();

            return new OpenSpaceSettings(attribute.Game, attribute.Platform);
        }
    }
}