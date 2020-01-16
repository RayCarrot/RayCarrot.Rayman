namespace RayCarrot.Rayman
{
    /// <summary>
    /// The available text encodings to use
    /// </summary>
    public enum TextEncoding
    {
        /// <summary>
        /// UTF-7
        /// </summary>
        UTF7,

        /// <summary>
        /// UTF-16 format using the big endian byte order
        /// </summary>
        BigEndianUnicode,

        /// <summary>
        /// UTF-16 format using the little endian byte order
        /// </summary>
        Unicode,

        /// <summary>
        /// ASCII (7-bit)
        /// </summary>
        ASCII,

        /// <summary>
        /// UTF-8
        /// </summary>
        UTF8,

        /// <summary>
        /// UTF-32
        /// </summary>
        UTF32
    }
}