using System.Dynamic;

namespace RayCarrot.Rayman
{
    // WIP: There are more values in the save file than this (button mapping, current level score etc.) - figure out what they are for
    /// <summary>
    /// The data for a Rayman 3 save file
    /// </summary>
    public class Rayman3SaveData : IBinarySerializable
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
        public BinarySerializableList<Rayman3SaveDataLevel> Levels { get; set; }

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
        /// Same as <see cref="TotalScore"/>
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
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            TotalCages = reader.Read<int>();
            TotalScore = reader.Read<int>();

            Levels = new BinarySerializableList<Rayman3SaveDataLevel>(9);
            Levels.Deserialize(reader);

            Unknown1 = reader.ReadBytes(33);

            IsVibrationEnabled = reader.Read<bool>();
            IsHorizontalInversionEnabled = reader.Read<bool>();
            IsVerticalInversionEnabled = reader.Read<bool>();

            Unknown2 = reader.ReadBytes(64);

            TotalScore2 = reader.Read<int>();

            Unknown4 = reader.Read<int>();
            Unknown5 = reader.Read<int>();

            CurrentLevel = reader.Read<string>();

            Unknown3 = reader.ReadRemainingBytes();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            writer.Write(TotalCages);
            writer.Write(TotalScore);
            writer.Write(Levels);

            writer.Write(Unknown1);

            writer.Write(IsVibrationEnabled);
            writer.Write(IsHorizontalInversionEnabled);
            writer.Write(IsVerticalInversionEnabled);

            writer.Write(Unknown2);

            writer.Write(TotalScore2);

            writer.Write(Unknown4);
            writer.Write(Unknown5);

            writer.Write(CurrentLevel);

            writer.Write(Unknown3);
        }
    }
}