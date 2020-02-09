using System;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Helpers methods for <see cref="IBinaryDataReader{Settings}"/> and <see cref="IBinaryDataWriter{Settings}"/>
    /// </summary>
    public static class OpenSpaceSerializerHelpers
    {
        /// <summary>
        /// Reads an encrypted string based on the provided XOR key
        /// </summary>
        /// <param name="reader">The reader to use to read the string</param>
        /// <param name="XORKey">The XOR key</param>
        /// <returns>The string value</returns>
        public static string ReadEncryptedString<S>(IBinaryDataReader<S> reader, int XORKey)
            where S : BinarySerializerSettings
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

        /// <summary>
        /// Writes an encrypted string based on the provided XOR key
        /// </summary>
        /// <param name="writer">The writer to use to write the string</param>
        /// <param name="value">The string value to write</param>
        /// <param name="XORKey">The XOR key</param>
        public static void WriteEncryptedString<S>(IBinaryDataWriter<S> writer, string value, int XORKey)
            where S : BinarySerializerSettings
        {
            // Write the length of the string
            writer.Write(value.Length);

            // Write every character
            foreach (var c in value)
                writer.Write((byte)(c ^ XORKey));
        }
    }
}