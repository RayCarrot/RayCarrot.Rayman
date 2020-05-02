using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    // WIP: There are more values in the save file than this (button mapping, current level score etc.) - figure out what they are for
    // WIP: Support other platforms - the format seems very similar

    /// <summary>
    /// The data for a Rayman 3 save file on PC
    /// </summary>
    public class Rayman3PCSaveData : IBinarySerializable
    {
        /// <summary>
        /// The total amount of cages
        /// </summary>
        public int TotalCages { get; set; }

        /// <summary>
        /// The total score
        /// </summary>
        public int TotalScore { get; set; }

        /// <summary>
        /// The data for each of the available levels. Count is always 9.
        /// </summary>
        public Rayman3PCSaveDataLevel[] Levels { get; set; }

        /// <summary>
        /// Unknown values
        /// </summary>
        public byte[] Unknown1 { get; set; }

        /// <summary>
        /// Indicates if controller vibration is enabled
        /// </summary>
        public bool IsVibrationEnabled { get; set; }

        /// <summary>
        /// Indicates if the horizontal axis is inverted
        /// </summary>
        public bool IsHorizontalInversionEnabled { get; set; }

        /// <summary>
        /// Indicates if the vertical axis is inverted
        /// </summary>
        public bool IsVerticalInversionEnabled { get; set; }

        /// <summary>
        /// Unknown values
        /// </summary>
        public byte[] Unknown2 { get; set; }

        /// <summary>
        /// Same as <see cref="TotalScore"/>. This value is not always set.
        /// </summary>
        public int TotalScore2 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public int Unknown4 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public int Unknown5 { get; set; }

        /// <summary>
        /// The current level to load, or "endgame" if the game has been finished
        /// </summary>
        public string CurrentLevel { get; set; }

        //// Offset 212
        ///// <summary>
        ///// The last unlocked level, only set if it differs from <see cref="CurrentLevel"/>
        ///// </summary>
        //public string LastLevel { get; set; }

        /// <summary>
        /// Unknown values
        /// </summary>
        public byte[] Unknown3 { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            TotalCages = s.Serialize<int>(TotalCages, name: nameof(TotalCages));
            TotalScore = s.Serialize<int>(TotalScore, name: nameof(TotalScore));

            Levels = s.SerializeObjectArray<Rayman3PCSaveDataLevel>(Levels, 9, name: nameof(Levels));

            Unknown1 = s.SerializeArray<byte>(Unknown1, 33, name: nameof(Unknown1));

            // TODO: As they formatted as ints?
            IsVibrationEnabled = s.Serialize<bool>(IsVibrationEnabled, name: nameof(IsVibrationEnabled));
            IsHorizontalInversionEnabled = s.Serialize<bool>(IsHorizontalInversionEnabled, name: nameof(IsHorizontalInversionEnabled));
            IsVerticalInversionEnabled = s.Serialize<bool>(IsVerticalInversionEnabled, name: nameof(IsVerticalInversionEnabled));

            Unknown2 = s.SerializeArray<byte>(Unknown2, 64, name: nameof(Unknown2));

            TotalScore2 = s.Serialize<int>(TotalScore2, name: nameof(TotalScore2));

            Unknown4 = s.Serialize<int>(Unknown4, name: nameof(Unknown4));
            Unknown5 = s.Serialize<int>(Unknown5, name: nameof(Unknown5));

            // TODO: The length is probably fixed?
            CurrentLevel = s.Serialize<string>(CurrentLevel, name: nameof(CurrentLevel));

            Unknown3 = s.SerializeArray<byte>(Unknown3, (int)(s.Stream.Length - s.Stream.Position), name: nameof(Unknown3));
        }
    }
}