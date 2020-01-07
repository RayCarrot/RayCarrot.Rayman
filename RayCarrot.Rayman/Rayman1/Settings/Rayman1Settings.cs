﻿namespace RayCarrot.Rayman
{
    /// <summary>
    /// Settings for serializing Rayman 1 game formats
    /// </summary>
    public class Rayman1Settings : IBinarySerializableSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="gameMode">The game mode</param>
        public Rayman1Settings(Rayman1GameMode gameMode)
        {
            GameMode = gameMode;
        }

        /// <summary>
        /// The game mode
        /// </summary>
        public Rayman1GameMode GameMode { get; }

        // WIP: Set based on platform?
        /// <summary>
        /// The byte order to use
        /// </summary>
        public ByteOrder ByteOrder => ByteOrder.LittleEndian;
    }
}