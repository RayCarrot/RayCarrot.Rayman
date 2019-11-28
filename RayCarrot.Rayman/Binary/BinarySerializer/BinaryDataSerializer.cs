using System;
using System.IO;
using RayCarrot.Extensions;
using RayCarrot.IO;

namespace RayCarrot.Rayman
{
    // TODO: Log

    /// <summary>
    /// The binary serializer for binary serialized files
    /// </summary>
    /// <typeparam name="T">The type of data to serialize to. Either a supported value type of a class implementing <see cref="IBinarySerializable"/></typeparam>
    public class BinaryDataSerializer<T> : IBinarySerializer, IBinaryDeserializer
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The serializer settings</param>
        public BinaryDataSerializer(BinarySeriaizerSettings settings)
        {
            // Set the settings
            Settings = settings;
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The serializer settings
        /// </summary>
        protected BinarySeriaizerSettings Settings { get; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Reads an <see cref="Int32"/> from the stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <returns>The value</returns>
        protected virtual int ReadInt(FileStream stream)
        {
            // Create the buffer with the size of a 32-bit integer (4 bytes)
            byte[] buffer = new byte[sizeof(int)];

            // Read the bytes into the buffer
            stream.Read(buffer, 0, sizeof(int));

            if (Settings.InvertBytes)
                // Reverse the bytes due to being little endian
                Array.Reverse(buffer);

            // Convert to a 32-bit integer and return
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Writes an <see cref="Int32"/> to the stream
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="value">The value to write</param>
        protected virtual void WriteInt(FileStream stream, int value)
        {
            // Get the bytes to write
            byte[] bytes = BitConverter.GetBytes(value);

            if (Settings.InvertBytes)
                // Reverse the bytes due to being little endian
                Array.Reverse(bytes);

            // Write the bytes to the stream
            stream.Write(bytes, 0, sizeof(int));
        }

        /// <summary>
        /// Reads an <see cref="Int16"/> from the stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <returns>The value</returns>
        protected virtual short ReadShort(FileStream stream)
        {
            // Create the buffer with the size of a 16-bit integer (2 bytes)
            byte[] buffer = new byte[sizeof(short)];

            // Read the bytes into the buffer
            stream.Read(buffer, 0, sizeof(short));

            if (Settings.InvertBytes)
                // Reverse the bytes due to being little endian
                Array.Reverse(buffer);

            // Convert to a 16-bit integer and return
            return BitConverter.ToInt16(buffer, 0);
        }

        /// <summary>
        /// Writes an <see cref="Int16"/> to the stream
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="value">The value to write</param>
        protected virtual void WriteShort(FileStream stream, short value)
        {
            // Get the bytes to write
            byte[] bytes = BitConverter.GetBytes(value);

            if (Settings.InvertBytes)
                // Reverse the bytes due to being little endian
                Array.Reverse(bytes);

            // Write the bytes to the stream
            stream.Write(bytes, 0, sizeof(short));
        }

        /// <summary>
        /// Reads a <see cref="Boolean"/> from the stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <returns>The value</returns>
        protected virtual bool ReadBool(FileStream stream)
        {
            // Return true if the byte is 1, otherwise false
            return stream.ReadByte() == 1;
        }

        /// <summary>
        /// Writes a <see cref="Boolean"/> to the stream
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="value">The value to write</param>
        protected virtual void WriteBool(FileStream stream, bool value)
        {
            // Write the byte
            stream.WriteByte((byte)(value ? 1 : 0));
        }

        /// <summary>
        /// Reads a <see cref="String"/> from the stream
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <returns>The value</returns>
        protected virtual string ReadString(FileStream stream)
        {
            // Get the item size
            int size = ReadInt(stream) * Settings.CharSize;

            // Make sure we have bytes to read
            if (size == 0)
                return String.Empty;

            // Create the buffer with the specified size
            byte[] buffer = new byte[size];

            // Read the bytes into the buffer
            stream.Read(buffer, 0, size);

            // Convert to a string
            var str = Settings.StringEncoding.GetString(buffer);

            // Remove null termination
            str = str.TrimEnd('\0');

            // Return the string
            return str;
        }

        /// <summary>
        /// Writes a <see cref="String"/> to the stream
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="value">The value to write</param>
        protected virtual void WriteString(FileStream stream, string value)
        {
            // Get the string bytes
            var bytes = Settings.StringEncoding.GetBytes(value);

            // Write the item size
            WriteInt(stream, bytes.Length / Settings.CharSize);

            // Write the bytes to the stream
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Deserializes the specified type
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <param name="type">The type to deserialize</param>
        /// <returns>The deserialized object</returns>
        protected virtual object DeserializeType(FileStream stream, Type type)
        {
            if (type == typeof(int))
                return ReadInt(stream);
            else if (type == typeof(short))
                return ReadShort(stream);
            else if (type == typeof(bool))
                return ReadBool(stream);
            else if (type == typeof(string))
                return ReadString(stream);

            var instance = type.CreateInstance<IBinarySerializable>();

            instance.Deserialize(stream, this);

            return instance;
        }

        /// <summary>
        /// Serializes the specified object
        /// </summary>
        /// <param name="stream">The stream to write to</param>
        /// <param name="obj">The object to serialize</param>
        protected virtual void SerializeObject(FileStream stream, object obj)
        {
            if (obj is int i)
                WriteInt(stream, i);
            else if (obj is short sh)
                WriteShort(stream, sh);
            else if (obj is bool b)
                WriteBool(stream, b);
            else if (obj is string s)
                WriteString(stream, s);
            else
                obj.CastTo<IBinarySerializable>().Serialize(stream, this);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the type
        /// </summary>
        /// <typeparam name="DataType">The type of data to deserialize</typeparam>
        /// <param name="stream">The stream to deserialize from</param>
        /// <returns>The deserialized data</returns>
        public DataType Deserialize<DataType>(FileStream stream)
        {
            return DeserializeType(stream, typeof(DataType)).CastTo<DataType>();
        }

        /// <summary>
        /// Serializes the value of the specified type
        /// </summary>
        /// <typeparam name="DataType">The type of data to serialize</typeparam>
        /// <param name="stream">The stream to serialize to</param>
        /// <param name="value">The value to serialize</param>
        public void Serialize<DataType>(FileStream stream, DataType value)
        {
            SerializeObject(stream, value);
        }

        /// <summary>
        /// Deserializes the data from the serialized file
        /// </summary>
        /// <param name="filePath">The path of the file to deserialize</param>
        /// <returns>The deserialized object</returns>
        public T Deserialize(FileSystemPath filePath)
        {
            // Create the file stream
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            // Deserialize the data
            return Deserialize<T>(stream);
        }

        /// <summary>
        /// Serializes the data to the serialized file
        /// </summary>
        /// <param name="filePath">The path of the file to serialize to</param>
        /// <param name="obj">The object to serialize</param>
        public void Serialize(FileSystemPath filePath, T obj)
        {
            // Create the file stream
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            // Deserialize the data
            Serialize(stream, obj);
        }

        #endregion
    }
}