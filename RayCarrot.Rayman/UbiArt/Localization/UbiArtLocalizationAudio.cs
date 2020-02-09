namespace RayCarrot.Rayman
{
    /// <summary>
    /// UbiArt localization audio data
    /// </summary>
    public class UbiArtLocalizationAudio : IBinarySerializable<BinarySerializerSettings>
    {
        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown0 { get; set; }

        /// <summary>
        /// The audio file
        /// </summary>
        public string AudioFile { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown1 { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<BinarySerializerSettings> reader)
        {
            Unknown0 = reader.Read<uint>();
            AudioFile = reader.Read<string>();
            Unknown1 = reader.Read<uint>();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<BinarySerializerSettings> writer)
        {
            writer.Write(Unknown0);
            writer.Write(AudioFile);
            writer.Write(Unknown1);
        }
    }
}