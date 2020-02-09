namespace RayCarrot.Rayman
{
    /// <summary>
    /// Defines the available ways to serialize a boolean
    /// </summary>
    public enum BinaryBoolEncoding
    {
        /// <summary>
        /// As a byte
        /// </summary>
        Byte,

        /// <summary>
        /// As a 16-bit integer
        /// </summary>
        Int16,

        /// <summary>
        /// As a 32-bit integer
        /// </summary>
        Int32,

        /// <summary>
        /// As a 64-bit integer
        /// </summary>
        Int64
    }
}