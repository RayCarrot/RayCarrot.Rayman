using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Base interface for a binary data writer
    /// </summary>
    public interface IBinaryDataWriter
    {
        /// <summary>
        /// The underlying stream
        /// </summary>
        Stream BaseStream { get; }

        /// <summary>
        /// Writes the specified type
        /// </summary>
        /// <typeparam name="T">The type of the value to write. This is either a supported value type or implements <see cref="IBinarySerializable{Settings}"/></typeparam>
        /// <param name="value">The value to write</param>
        void Write<T>(T value);
    }

    /// <summary>
    /// Base generic interface for a binary data writer
    /// </summary>
    /// <typeparam name="Settings">The type of serializer settings</typeparam>
    public interface IBinaryDataWriter<out Settings> : IBinaryDataWriter
        where Settings : BinarySerializerSettings
    {
        /// <summary>
        /// The serializer settings
        /// </summary>
        Settings SerializerSettings { get; }
    }
}