using System.IO;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// The archive data used for the .cnt files from OpenSpace games
    /// </summary>
    public class OpenSpaceCntData : IBinarySerializableArchive<OpenSpaceSettings>
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
        public OpenSpaceCntFileEntry[] Files { get; set; }

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
            Files = new OpenSpaceCntFileEntry[fileCount];

            // Read file info
            for (int i = 0; i < fileCount; i++)
            {
                // Create the new file data and pass in the XOR key
                var file = new OpenSpaceCntFileEntry(XORKey);

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

        /// <summary>
        /// Writes every listed file entry based on its offset to the file, getting the contents from the generator
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="fileGenerator">The file generator</param>
        public void WriteArchiveContent(Stream stream, ArchiveFileGenerator fileGenerator)
        {
            // Make sure we have a generator for each file
            if (fileGenerator.Count != Files.Length)
                throw new BinarySerializableException("The .cnt file can't be serialized without a file generator for each file");

            // Write the file contents
            foreach (var file in Files)
            {
                // Set the position to the pointer
                stream.Position = file.Pointer;

                // Get the full path
                var fullPath = file.GetFullPath(Directories);

                // Write the contents from the generator
                stream.Write(fileGenerator.GetBytes(fullPath));
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
            GetSerializer(settings).Serialize(stream, this);

            // Get the position, which will be the size of the header
            return (int)stream.Position;
        }

        #endregion
    }
}