using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The archive data used for the .cnt files from OpenSpace games
    /// </summary>
    public class OpenSpaceCntData : IBinarySerializable<OpenSpaceSettings>
    {
        #region Public Static Properties

        /// <summary>
        /// Gets the default serializer
        /// </summary>
        public static BinaryDataSerializer<OpenSpaceCntData, OpenSpaceSettings> GetSerializer(OpenSpaceSettings settings) => new BinaryDataSerializer<OpenSpaceCntData, OpenSpaceSettings>(settings);

        #endregion

        #region Public Properties

        /// <summary>
        /// The file signature
        /// </summary>
        public short Signature { get; set; }

        /// <summary>
        /// The XOR key used to get the directory and file names
        /// </summary>
        public byte XORKey { get; set; }

        /// <summary>
        /// The available directories
        /// </summary>
        public string[] Directories { get; set; }

        /// <summary>
        /// The version ID
        /// </summary>
        public byte VersionID { get; set; }

        /// <summary>
        /// The available files
        /// </summary>
        public OpenSpaceCntFile[] Files { get; set; }

        /// <summary>
        /// The generator to use for retrieving the file contents when serializing
        /// </summary>
        public ArchiveFileGenerator FileGenerator { get; set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Serializes the header data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        protected void SerializeHeader(IBinaryDataWriter<OpenSpaceSettings> writer)
        {
            // Write the directory and file count
            writer.Write(Directories.Length);
            writer.Write(Files.Length);

            // Read header info
            writer.Write(Signature);
            writer.Write(XORKey);

            // Write directory paths
            foreach (var dir in Directories)
                OpenSpaceSerializerHelpers.WriteEncryptedString(writer, dir, XORKey);

            // Write version ID
            writer.Write(VersionID);

            // Write files
            foreach (var file in Files)
                writer.Write(file);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<OpenSpaceSettings> reader)
        {
            // Read the directory and file count
            var dirCount = reader.Read<int>();
            var fileCount = reader.Read<int>();

            // Read header info
            Signature = reader.Read<short>();
            XORKey = reader.Read<byte>();

            // Create the list
            Directories = new string[dirCount];

            // Read directory paths
            for (int i = 0; i < dirCount; i++)
                Directories[i] = OpenSpaceSerializerHelpers.ReadEncryptedString(reader, XORKey);

            // Read version ID
            VersionID = reader.Read<byte>();

            // Create the array
            Files = new OpenSpaceCntFile[fileCount];

            // Read file info
            for (int i = 0; i < fileCount; i++)
            {
                // Create the new file data and pass in the XOR key
                var file = new OpenSpaceCntFile(XORKey);

                // Deserialize the file data
                file.Deserialize(reader);

                // Add the file info
                Files[i] = file;
            }
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<OpenSpaceSettings> writer)
        {
            // Make sure we have a generator
            if (FileGenerator == null)
                throw new BinarySerializableException("The .cnt file can't be serialized without a file generator");

            // Make sure we have a generator for each file
            if (FileGenerator.Count != Files.Length)
                throw new BinarySerializableException("The .cnt file can't be serialized without a file generator for each file");

            // Serialize the header
            SerializeHeader(writer);

            // Serialize the file contents
            foreach (var file in Files)
            {
                // Set the position to the pointer
                writer.BaseStream.Position = file.Pointer;

                // Get the full path
                var fullPath = file.GetFullPath(Directories);

                // Write the contents from the generator
                writer.Write(FileGenerator.GetBytes(fullPath));
            }
        }

        /// <summary>
        /// Gets the size of the header information in the archive
        /// </summary>
        /// <param name="settings">The serializer settings</param>
        /// <returns>The size in bytes</returns>
        public int GetHeaderSize(OpenSpaceSettings settings)
        {
            // Create a temporary memory stream to determine the size
            using var stream = new MemoryStream();

            // Serialize the header only
            SerializeHeader(new BinaryDataWriter<OpenSpaceSettings>(GetSerializer(settings).GetBinaryWriter(stream), settings));

            // Get the position, which will be the size of the header
            return (int)stream.Position;
        }

        #endregion
    }
}