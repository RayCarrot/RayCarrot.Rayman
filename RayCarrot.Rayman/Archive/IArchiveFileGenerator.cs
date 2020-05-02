using System;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Defines an archive file generator used to get the file bytes from a file entry
    /// </summary>
    /// <typeparam name="FileEntry">The type of file entry</typeparam>
    public interface IArchiveFileGenerator<in FileEntry> : IDisposable
    {
        /// <summary>
        /// Gets the number of files which can be retrieved from the generator
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Gets the bytes for the specified key
        /// </summary>
        /// <param name="fileEntry">The file entry to get the bytes for</param>
        /// <returns>The bytes</returns>
        byte[] GetBytes(FileEntry fileEntry);
    }
}