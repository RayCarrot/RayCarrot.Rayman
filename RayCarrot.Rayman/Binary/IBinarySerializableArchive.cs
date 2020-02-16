using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Used for objects which can be serialized using a <see cref="BinaryDataReader{Settings}"/> and <see cref="BinaryDataWriter{Settings}"/> which contain an archive
    /// </summary>
    /// <typeparam name="Settings">The type of serializer settings</typeparam>
    public interface IBinarySerializableArchive<in Settings> : IBinarySerializable<Settings>
        where Settings : BinarySerializerSettings
    {
        /// <summary>
        /// Writes every listed file entry based on its offset to the file, getting the contents from the generator
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="fileGenerator">The file generator</param>
        void WriteArchiveContent(Stream stream, ArchiveFileGenerator fileGenerator);
    }
}