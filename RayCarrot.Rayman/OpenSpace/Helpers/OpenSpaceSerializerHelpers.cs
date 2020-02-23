using System.Text;

namespace RayCarrot.Rayman.OpenSpace
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
        /// <param name="isXORUsed">Indicates if the XOR key is used</param>
        /// <returns>The string value</returns>
        public static string ReadEncryptedString<S>(IBinaryDataReader<S> reader, byte XORKey, bool isXORUsed)
            where S : BinarySerializerSettings
        {
            // Get the length of the string
            var strLength = reader.Read<int>();

            // Read the bytes
            var bytes = reader.ReadBytes(strLength);

            // Decrypt the bytes if set to do so
            if (isXORUsed && XORKey != 0)
                bytes = new XORDataEncoder(XORKey).Decode(bytes);

            // NOTE: Windows 1252 is used rather than UTF-8 here
            // Return the string
            return Encoding.GetEncoding(1252).GetString(bytes);
        }

        /// <summary>
        /// Writes an encrypted string based on the provided XOR key
        /// </summary>
        /// <param name="writer">The writer to use to write the string</param>
        /// <param name="value">The string value to write</param>
        /// <param name="XORKey">The XOR key</param>
        /// <param name="isXORUsed">Indicates if the XOR key is used</param>
        public static void WriteEncryptedString<S>(IBinaryDataWriter<S> writer, string value, byte XORKey, bool isXORUsed)
            where S : BinarySerializerSettings
        {
            // Write the length of the string
            writer.Write(value.Length);

            // Get the bytes
            var bytes = Encoding.GetEncoding(1252).GetBytes(value);

            // Encrypt the bytes if set to do so
            if (isXORUsed && XORKey != 0)
                bytes = new XORDataEncoder(XORKey).Encode(bytes);

            // NOTE: Windows 1252 is used rather than UTF-8 here
            // Write the bytes
            writer.Write(bytes);
        }
    }
}