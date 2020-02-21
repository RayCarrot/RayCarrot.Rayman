using System;
using RayCarrot.IO;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The binary serializer for binary serialized files
    /// </summary>
    /// <typeparam name="T">The type of data to serialize to</typeparam>
    /// <typeparam name="Settings">The type of settings</typeparam>
    public class BinaryDataSerializer<T, Settings>
        where T : IBinarySerializable<Settings>
        where Settings : BinarySerializerSettings
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="serializerSettings">The serializer settings</param>
        public BinaryDataSerializer(Settings serializerSettings)
        {
            SerializerSettings = serializerSettings;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The serializer settings
        /// </summary>
        public Settings SerializerSettings { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a new binary reader to use for the specified stream
        /// </summary>
        /// <param name="stream">The stream to use</param>
        public virtual BinaryReader GetBinaryReader(Stream stream)
        {
            return new StandardBinaryReader(stream, SerializerSettings, true);
        }

        /// <summary>
        /// Gets a new binary writer to use for the specified stream
        /// </summary>
        /// <param name="stream">The stream to use</param>
        public virtual BinaryWriter GetBinaryWriter(Stream stream)
        {
            return new StandardBinaryWriter(stream, SerializerSettings, true);
        }

        /// <summary>
        /// Deserializes the data from the serialized file
        /// </summary>
        /// <param name="filePath">The path of the file to deserialize</param>
        /// <returns>The deserialized object</returns>
        public virtual T Deserialize(FileSystemPath filePath)
        {
            // Create the file stream
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            // Deserialize the data from the stream
            return Deserialize(stream);
        }

        /// <summary>
        /// Deserializes the data from the serialized file as a stream to an existing instance
        /// </summary>
        /// <param name="stream">The file stream to deserialize</param>
        /// <param name="instance">The object instance to deserialize to</param>
        /// <returns>The deserialized object</returns>
        public virtual T Deserialize(Stream stream, T instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            // Get the reader
            using var reader = GetBinaryReader(stream);

            // Create the wrapper reader
            var dataReader = new BinaryDataReader<Settings>(reader, SerializerSettings);

            // Deserialize the data
            instance.Deserialize(dataReader);

            // Return the instance
            return instance;
        }

        /// <summary>
        /// Deserializes the data from the serialized file as a stream
        /// </summary>
        /// <param name="stream">The file stream to deserialize</param>
        /// <returns>The deserialized object</returns>
        public virtual T Deserialize(Stream stream)
        {
            // Get the reader
            using var binaryReader = GetBinaryReader(stream);

            // Create the wrapper reader
            var reader = new BinaryDataReader<Settings>(binaryReader, SerializerSettings);

            // Deserialize the data
            return reader.Read<T>();
        }

        /// <summary>
        /// Serializes the data to the serialized file
        /// </summary>
        /// <param name="filePath">The path of the file to serialize to</param>
        /// <param name="obj">The object to serialize</param>
        public virtual void Serialize(FileSystemPath filePath, T obj)
        {
            // Create the file stream
            using var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            // Deserialize the data
            Serialize(stream, obj);
        }

        /// <summary>
        /// Serializes the data to the stream
        /// </summary>
        /// <param name="stream">The stream to serialize to</param>
        /// <param name="obj">The object to serialize</param>
        public virtual void Serialize(Stream stream, T obj)
        {
            // Create the writer
            using var writer = GetBinaryWriter(stream);

            // Create the wrapper data writer
            var dataWriter = new BinaryDataWriter<Settings>(writer, SerializerSettings);

            // Deserialize the data
            dataWriter.Write(obj);
        }

        #endregion
    }
}