using System.Collections.Generic;

namespace RayCarrot.Rayman
{
    // NOTE: This does currently only work for the Rayman games. Just Dance and Child of Light use a different format where each language has its own file.
    /// <summary>
    /// The base for UbiArt localization data
    /// </summary>
    /// <typeparam name="LocStringType">The type of localization string item</typeparam>
    public class BaseUbiArtLocalizationData<LocStringType> : IBinarySerializable
    {
        /// <summary>
        /// The localized strings, categorized by the language index and the localization ID
        /// </summary>
        public BinarySerializableDictionary<int, BinarySerializableDictionary<int, LocStringType>> Strings { get; set; }

        /// <summary>
        /// The audio to use for each localized string
        /// </summary>
        public BinarySerializableDictionary<int, UbiArtLocalizationAudio> Audio { get; set; }

        /// <summary>
        /// Unknown list of paths
        /// </summary>
        public BinarySerializableFixedList<string> Paths { get; set; }

        /// <summary>
        /// Unknown values, used in Legends and later
        /// </summary>
        public List<int> Unknown { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            Strings = reader.Read<BinarySerializableDictionary<int, BinarySerializableDictionary<int, LocStringType>>>();
            Audio = reader.Read<BinarySerializableDictionary<int, UbiArtLocalizationAudio>>();
            Paths = reader.Read<BinarySerializableFixedList<string>>();

            Unknown = new List<int>();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
                Unknown.Add(reader.Read<int>());
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            writer.Write(Strings);
            writer.Write(Audio);
            writer.Write(Paths);

            foreach (var v in Unknown)
                writer.Write(v);
        }
    }
}