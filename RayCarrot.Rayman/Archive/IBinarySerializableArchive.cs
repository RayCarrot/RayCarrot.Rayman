using System.IO;
using RayCarrot.Binary;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Defines a serializable binary archive file
    /// </summary>
    /// <typeparam name="FileEntry">The file entry type</typeparam>
    public interface IBinarySerializableArchive<FileEntry> : IBinarySerializable
    {
        /// <summary>
        /// Writes every listed file entry based on its offset to the file, getting the contents from the generator
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="fileGenerator">The file generator</param>
        void WriteArchiveContent(Stream stream, IArchiveFileGenerator<FileEntry> fileGenerator);

        /// <summary>
        /// Gets a generator for the archive content
        /// </summary>
        /// <param name="stream">The archive stream</param>
        /// <returns>The generator</returns>
        IArchiveFileGenerator<FileEntry> GetArchiveContent(Stream stream);
    }
}