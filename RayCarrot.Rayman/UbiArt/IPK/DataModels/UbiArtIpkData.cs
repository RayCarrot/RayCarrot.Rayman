using RayCarrot.CarrotFramework.Abstractions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The archive data used for the .ipk files from UbiArt games
    /// </summary>
    public class UbiArtIpkData : IBinarySerializable
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        public UbiArtIpkData(UbiArtSettings settings)
        {
            // Set properties
            Settings = settings;
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The settings when serializing the data
        /// </summary>
        protected UbiArtSettings Settings { get; }

        #endregion

        /// <summary>
        /// The generator to use for retrieving the file contents when serializing
        /// </summary>
        public ArchiveFileGenerator FileGenerator { get; set; }

        /// <summary>
        /// The ID header of the file
        /// </summary>
        public uint ID { get; set; }

        /// <summary>
        /// The IPK archive version
        /// </summary>
        public uint Version { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown1 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown2 { get; set; }

        /// <summary>
        /// The base offset to use when reading the files
        /// </summary>
        public uint BaseOffset { get; set; }

        /// <summary>
        /// The amount of files
        /// </summary>
        public uint FilesCount { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown3 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown4 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown5 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown6 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown7 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown8 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown9 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown10 { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public uint Unknown11 { get; set; }

        /// <summary>
        /// The files
        /// </summary>
        public UbiArtIPKFile[] Files { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            // Read ID and version
            ID = reader.Read<uint>();
            Version = reader.Read<uint>();

            // Set the version
            Settings.IPKVersion = Version;

            // Read first unknown value
            Unknown1 = reader.Read<uint>();

            // Read second unknown value if version is above or equal to 8
            if (Version >= 8)
                Unknown2 = reader.Read<uint>();

            // Read offset and file count
            BaseOffset = reader.Read<uint>();
            FilesCount = reader.Read<uint>();

            // Read unknown values
            Unknown3 = reader.Read<uint>();
            Unknown4 = reader.Read<uint>();
            Unknown5 = reader.Read<uint>();
            Unknown6 = reader.Read<uint>();
            Unknown7 = reader.Read<uint>();
            Unknown8 = reader.Read<uint>();

            if (Version >= 6)
            {
                Unknown9 = reader.Read<uint>();
                Unknown10 = reader.Read<uint>();

                // TODO: Check based on previous values rather than game is possible
                if (Version < 8 && Settings.Game == UbiArtGame.ValiantHearts)
                    Unknown11 = reader.Read<uint>();
            }

            // Get the file count (for the file array)
            var fileCount = reader.Read<uint>();

            // NOTE: So far this only appears to be the case for the bundle_pc32.ipk file used in Child of Light
            if (fileCount != FilesCount)
                RCFCore.Logger?.LogWarningSource($"The initial file count {FilesCount} does not match the file array size {fileCount}");

            // Read the files
            Files = new UbiArtIPKFile[fileCount];

            for (int i = 0; i < fileCount; i++)
            {
                Files[i] = new UbiArtIPKFile(Settings);
                Files[i].Deserialize(reader);
            }

            if (reader.BaseStream.Position != BaseOffset)
                throw new BinarySerializableException("Offset value is incorrect.");
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            // Make sure we have a generator
            if (FileGenerator == null)
                throw new BinarySerializableException("The .ipk file can't be serialized without a file generator");

            // Make sure we have a generator for each file
            if (FileGenerator.Count != Files.Length)
                throw new BinarySerializableException("The .ipk file can't be serialized without a file generator for each file");

            // Write ID and version
            writer.Write(ID);
            writer.Write(Version);

            // Write first unknown value
            writer.Write(Unknown1);

            // Write second unknown value if version is above or equal to 8
            if (Version >= 8)
                writer.Write(Unknown2);

            // Write offset and file count
            writer.Write(BaseOffset);
            writer.Write(FilesCount);

            // TODO: Update this
            // Write third unknown value
            writer.Write(Unknown3);

            // Write the file count (for the file array)
            writer.Write(Files.Length);

            // Write the files
            foreach (var file in Files)
                writer.Write(file);

            // Serialize the file contents
            foreach (var file in Files)
            {
                // Set the position to the pointer
                writer.BaseStream.Position = BaseOffset + file.Offset;

                // Get the bytes from the generator
                var bytes = FileGenerator.GetBytes(file.FullPath);

                // Make sure the size matches
                if (bytes.Length != file.ArchiveSize)
                    throw new BinarySerializableException("The archived file size does not match the bytes retrieved from the generator");

                // Write the bytes
                writer.Write(bytes);
            }
        }
    }
}