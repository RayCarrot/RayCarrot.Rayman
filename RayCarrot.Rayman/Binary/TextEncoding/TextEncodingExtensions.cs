using System;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Extension methods for <see cref="TextEncoding"/>
    /// </summary>
    public static class TextEncodingExtensions
    {
        /// <summary>
        /// Gets the <see cref="Encoding"/> from the <see cref="TextEncoding"/>
        /// </summary>
        /// <param name="encoding">The text encoding value to get the encoding from</param>
        /// <returns>The encoding</returns>
        public static Encoding GetEncoding(this TextEncoding encoding)
        {
            return encoding switch
            {
                TextEncoding.UTF7 => Encoding.UTF7,
                TextEncoding.BigEndianUnicode => Encoding.BigEndianUnicode,
                TextEncoding.Unicode => Encoding.Unicode,
                TextEncoding.ASCII => Encoding.ASCII,
                TextEncoding.UTF8 => Encoding.UTF8,
                TextEncoding.UTF32 => Encoding.UTF32,
                _ => throw new ArgumentOutOfRangeException(nameof(encoding), encoding, null)
            };
        }
    }
}