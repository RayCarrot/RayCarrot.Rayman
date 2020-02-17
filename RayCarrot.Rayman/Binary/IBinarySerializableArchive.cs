using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Used for objects which can be serialized using a <see cref="BinaryDataReader{Settings}"/> and <see cref="BinaryDataWriter{Settings}"/> which contain an archive
    /// </summary>
    /// <typeparam name="Settings">The type of serializer settings</typeparam>
    /// <typeparam name="FileEntry">The type of file entry</typeparam>
    public interface IBinarySerializableArchive<in Settings, FileEntry> : IBinarySerializable<Settings>
        where Settings : BinarySerializerSettings
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