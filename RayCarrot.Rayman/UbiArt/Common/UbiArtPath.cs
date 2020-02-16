using System.Collections.Generic;
using System.Linq;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// A path for UbiArt games. The directory separator character is '/'
    /// </summary>
    public class UbiArtPath : IBinarySerializable<UbiArtSettings>
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public UbiArtPath()
        {
            
        }

        /// <summary>
        /// Constructor for a full path
        /// </summary>
        /// <param name="fullPath">The full path</param>
        public UbiArtPath(string fullPath)
        {
            var separatorIndex = fullPath.LastIndexOf('/') + 1;

            DirectoryPath = fullPath.Substring(0, separatorIndex);
            FileName = fullPath.Substring(separatorIndex);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The full directory path, ending with the separator character
        /// </summary>
        public string DirectoryPath { get; set; }

        /// <summary>
        /// The file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The String ID
        /// </summary>
        public UbiArtStringID StringID { get; set; }

        /// <summary>
        /// The flags
        /// </summary>
        public uint Flags { get; set; }

        /// <summary>
        /// The full path including the directory path and file name
        /// </summary>
        public string FullPath => DirectoryPath + FileName;

        /// <summary>
        /// Gets the file extensions used for the file
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetFileExtensions() => FileName.Split('.').Skip(1).Select(x => $".{x}");

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            // Just Dance reads the values in reverse
            if (reader.SerializerSettings.Game == UbiArtGame.JustDance2017)
            {
                // Read the path
                FileName = reader.Read<string>();
                DirectoryPath = reader.Read<string>();
            }
            else
            {
                // Read the path
                DirectoryPath = reader.Read<string>();
                FileName = reader.Read<string>();
            }

            StringID = reader.Read<UbiArtStringID>();

            if (reader.SerializerSettings.Game != UbiArtGame.RaymanOrigins)
                Flags = reader.Read<uint>();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            // Just Dance reads the values in reverse
            if (writer.SerializerSettings.Game == UbiArtGame.JustDance2017)
            {
                // Write the path
                writer.Write(FileName);
                writer.Write(DirectoryPath);
            }
            else
            {
                // Write the path
                writer.Write(DirectoryPath);
                writer.Write(FileName);
            }

            writer.Write(StringID);

            if (writer.SerializerSettings.Game != UbiArtGame.RaymanOrigins)
                writer.Write(Flags);
        }

        #endregion
    }
}