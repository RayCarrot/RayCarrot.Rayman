using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Base interface for a binary data reader
    /// </summary>
    public interface IBinaryDataReader
    {
        /// <summary>
        /// The underlying stream
        /// </summary>
        Stream BaseStream { get; }

        /// <summary>
        /// Reads the specified type
        /// </summary>
        /// <typeparam name="T">The type to read. This is either a supported value type or implements <see cref="IBinarySerializable{Settings}"/></typeparam>
        /// <returns>The value</returns>
        T Read<T>();

        /// <summary>
        /// Reads the specified number of bytes
        /// </summary>
        /// <returns>The number of bytes to read</returns>
        byte[] ReadBytes(int count);

        /// <summary>
        /// Reads the remaining bytes
        /// </summary>
        /// <returns>The remaining bytes</returns>
        byte[] ReadRemainingBytes();
    }

    /// <summary>
    /// Base generic interface for a binary data reader
    /// </summary>
    /// <typeparam name="Settings">The type of serializer settings</typeparam>
    public interface IBinaryDataReader<out Settings> : IBinaryDataReader
        where Settings : BinarySerializerSettings
    {
        /// <summary>
        /// The serializer settings
        /// </summary>
        Settings SerializerSettings { get; }
    }
}
