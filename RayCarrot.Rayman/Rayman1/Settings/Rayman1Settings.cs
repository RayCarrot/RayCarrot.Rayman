namespace RayCarrot.Rayman.Rayman1
{
    /// <summary>
    /// Settings for serializing Rayman 1 game formats
    /// </summary>
    public class Rayman1Settings : BinarySerializerSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        public Rayman1Settings(Rayman1Game game, Rayman1Platform platform)
        {
            ByteOrder = ByteOrder.LittleEndian;
            BoolEncoding = BinaryBoolEncoding.Byte;

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