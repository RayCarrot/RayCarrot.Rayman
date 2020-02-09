using System;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Defines the available ways to serialize a binary encoded string
    /// </summary>
    public enum BinaryStringEncoding
    {
        /// <summary>
        /// The standard encoding user by a <see cref="BinaryReader"/>
        /// </summary>
        BinaryReaderStandard,

        /// <summary>
        /// The string end is indicated by a null byte 0x00
        /// </summary>
        NullTerminated,

        /// <summary>
        /// The string length is indicated by a leading <see cref="Int32"/>
        /// </summary>
        LengthPrefixed
    }
}