﻿using System;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Settings for serializing OpenSpace game formats
    /// </summary>
    public class OpenSpaceSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="platform">The platform</param>
        public OpenSpaceSettings(OpenSpaceGame game, OpenSpacePlatform platform)
        {
            // Set the properties
            Game = game;
            Platform = platform;

            // Set the byte order based on platform
            switch (platform)
            {
                case OpenSpacePlatform.PC:
                case OpenSpacePlatform.iOS:
                case OpenSpacePlatform.DreamCast:
                case OpenSpacePlatform.PlayStation1:
                case OpenSpacePlatform.PlayStation2:
                case OpenSpacePlatform.NintendoDS:
                case OpenSpacePlatform.Nintendo3DS:
                    ByteOrder = ByteOrder.LittleEndian;
                    break;

                case OpenSpacePlatform.GameCube:
                case OpenSpacePlatform.Nintendo64:
                    ByteOrder = ByteOrder.BigEndian;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, null);
            }

            // Get the engine version
            foreach (var version in OpenSpaceEngineVersion.TonicTrouble.GetValues())
            {
                if ((int) game > (int) version) 
                    continue;

                EngineVersion = version;
                break;
            }
        }

        /// <summary>
        /// The byte order to use
        /// </summary>
        public ByteOrder ByteOrder { get; }

        /// <summary>
        /// The game
        /// </summary>
        public OpenSpaceGame Game { get; }

        /// <summary>
        /// The engine version
        /// </summary>
        public OpenSpaceEngineVersion EngineVersion { get; }

        /// <summary>
        /// The platform
        /// </summary>
        public OpenSpacePlatform Platform { get; }
    }
}