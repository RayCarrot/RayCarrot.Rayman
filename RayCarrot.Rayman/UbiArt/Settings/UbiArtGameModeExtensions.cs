using RayCarrot.Extensions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Extension methods for <see cref="UbiArtGameMode"/>
    /// </summary>
    public static class UbiArtGameModeExtensions
    {
        /// <summary>
        /// Gets the display name for the game mode
        /// </summary>
        /// <param name="gameMode">The game mode</param>
        /// <returns>The display name</returns>
        public static string GetDisplayName(this UbiArtGameMode gameMode)
        {
            var attribute = gameMode.GetAttribute<UbiArtGameModeInfoAttribute>();

            return attribute.DisplayName;
        }

        /// <summary>
        /// Gets the settings for the game mode
        /// </summary>
        /// <param name="gameMode">The game mode</param>
        /// <returns>The OpenSpace settings</returns>
        public static UbiArtSettings GetSettings(this UbiArtGameMode gameMode)
        {
            var attribute = gameMode.GetAttribute<UbiArtGameModeInfoAttribute>();

            return new UbiArtSettings(attribute.Game, attribute.Platform, attribute.ByteOrder, attribute.TextEncoding);
        }
    }
}