using System;
using System.Collections.Generic;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// A generator to use for getting archive file contents when serializing an archive
    /// </summary>
    /// <typeparam name="FileEntry">The type of file entry</typeparam>
    public class ArchiveFileGenerator<FileEntry> : Dictionary<FileEntry, Func<byte[]>>, IArchiveFileGenerator<FileEntry>
    {
        /// <summary>
        /// Gets the bytes for the specified key
        /// </summary>
        /// <param name="fileEntry">The file entry to get the bytes for</param>
        /// <returns>The bytes</returns>
        public byte[] GetBytes(FileEntry fileEntry)
        {
            // Get the bytes
            var bytes = this[fileEntry].Invoke();
            
            // Return the bytes
            return bytes;
        }

        /// <summary>
        /// Disposes the generator
        /// </summary>
        public void Dispose()
        { }
    }
}