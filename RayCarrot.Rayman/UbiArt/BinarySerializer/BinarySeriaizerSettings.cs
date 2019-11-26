using System;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The settings to use for a binary serializer
    /// </summary>
    public class BinarySeriaizerSettings
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="invertBytes">Indicates if the bytes should be inverted</param>
        public BinarySeriaizerSettings(bool invertBytes = true)
        {
            InvertBytes = invertBytes;
        }

        /// <summary>
        /// The string encoding to use
        /// </summary>
        public Encoding StringEncoding { get; set; }

        /// <summary>
        /// The size of a character in bytes with the current encoding
        /// </summary>
        public int CharSize => StringEncoding.GetByteCount(Char.MinValue.ToString());

        /// <summary>
        /// Indicates if the bytes should be inverted
        /// </summary>
        public bool InvertBytes { get; set; }
    }
}