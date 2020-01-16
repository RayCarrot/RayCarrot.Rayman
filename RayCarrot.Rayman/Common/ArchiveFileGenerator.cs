using System;
using System.Collections.Generic;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// A generator to use for getting archive file contents when serializing an archive
    /// </summary>
    public class ArchiveFileGenerator : Dictionary<string, Func<byte[]>>, IDisposable
    {
        /// <summary>
        /// Gets the bytes for the specified key and removes the item from the generator
        /// </summary>
        /// <param name="key">The key for the bytes</param>
        /// <returns>The bytes</returns>
        public byte[] GetBytes(string key)
        {
            // Get the bytes
            var bytes = this[key].Invoke();
            
            // Remove the key
            Remove(key);

            // Return the bytes
            return bytes;
        }

        /// <summary>
        /// Clears the collection of generator items
        /// </summary>
        public void Dispose()
        {
            Clear();
        }
    }
}