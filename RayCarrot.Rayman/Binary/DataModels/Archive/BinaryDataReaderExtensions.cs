using System;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Extension methods for <see cref="BinaryDataReader"/>
    /// </summary>
    public static class BinaryDataReaderExtensions
    {
        /// <summary>
        /// Reads an encrypted string based on the provided XOR key
        /// </summary>
        /// <param name="reader">The reader to use to read the string</param>
        /// <param name="XORKey">The XOR key</param>
        /// <returns>The string value</returns>
        public static string ReadyEncryptedString(this BinaryDataReader reader, int XORKey)
        {
            // Create the output
            string output = String.Empty;

            // Get the length of the string
            var strLength = reader.Read<int>();

            // Read every character
            for (int j = 0; j < strLength; j++)
                // Decrypt the character and add it to the output
                output += (char)(XORKey ^ reader.Read<byte>());

            // Return the output
            return output;
        }
    }
}