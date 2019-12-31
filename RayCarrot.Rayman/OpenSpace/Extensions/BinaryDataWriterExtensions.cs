namespace RayCarrot.Rayman
{
    /// <summary>
    /// Extension methods for <see cref="BinaryDataWriter"/>
    /// </summary>
    public static class BinaryDataWriterExtensions
    {
        /// <summary>
        /// Writes an encrypted string based on the provided XOR key
        /// </summary>
        /// <param name="writer">The writer to use to write the string</param>
        /// <param name="value">The string value to write</param>
        /// <param name="XORKey">The XOR key</param>
        public static void WriteEncryptedString(this BinaryDataWriter writer, string value, int XORKey)
        {
            // Write the length of the string
            writer.Write(value.Length);

            // Write every character
            foreach (var c in value)
                writer.Write((byte)(c ^ XORKey));
        }
    }
}