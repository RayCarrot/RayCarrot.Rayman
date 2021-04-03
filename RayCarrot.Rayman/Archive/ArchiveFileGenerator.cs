﻿using System;
using System.Collections.Generic;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// A generator to use for getting archive file contents when serializing an archive
    /// </summary>
    /// <typeparam name="FileEntry">The type of file entry</typeparam>
    public class ArchiveFileGenerator<FileEntry> : Dictionary<FileEntry, Func<Stream>>, IArchiveFileGenerator<FileEntry>
    {
        /// <summary>
        /// Gets the file stream for the specified key
        /// </summary>
        /// <param name="fileEntry">The file entry to get the stream for</param>
        /// <returns>The stream</returns>
        public Stream GetFileStream(FileEntry fileEntry)
        {
            // Get the stream
            var bytes = this[fileEntry].Invoke();

            // Return the stream
            return bytes;
        }

        /// <summary>
        /// Disposes the generator
        /// </summary>
        public void Dispose()
        { }
    }
}