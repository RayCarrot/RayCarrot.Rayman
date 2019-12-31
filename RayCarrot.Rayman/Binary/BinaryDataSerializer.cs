using System;
using RayCarrot.IO;
using System.IO;
using RayCarrot.CarrotFramework.Abstractions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The binary serializer for binary serialized files
    /// </summary>
    /// <typeparam name="T">The type of data to serialize to. Either a supported value type of a class implementing <see cref="IBinarySerializable"/></typeparam>
    public abstract class BinaryDataSerializer<T>
    {
        #region Protected Abstract Methods

        /// <summary>
        /// Gets a new binary reader to use for the specified stream
        /// </summary>
        protected abstract BinaryReader GetBinaryReader(Stream stream);

        /// <summary>
        /// Gets a new binary writer to use for the specified stream
        /// </summary>
        protected abstract BinaryWriter GetBinaryWriter(Stream stream);

        #endregion

        #region Public Methods

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
        /// Deserializes the data from the serialized file as a stream
        /// </summary>
        /// <param name="stream">The file stream to deserialize</param>
        /// <returns>The deserialized object</returns>
        public virtual T Deserialize(Stream stream)
        {
            // Get the reader
            using var reader = GetBinaryReader(stream);

            // Create the wrapper reader
            using var dataReader = new BinaryDataReader(reader, false);

            // Deserialize the data
            return dataReader.Read<T>();
        }

        /// <summary>
        /// Deserializes the data from the serialized file as a stream to an existing instance
        /// </summary>
        /// <param name="stream">The file stream to deserialize</param>
        /// <param name="instance">The object instance to deserialize to</param>
        /// <returns>The deserialized object</returns>
        public virtual T Deserialize<I>(Stream stream, I instance)
            where I : T, IBinarySerializable
        {
            if (instance == null) 
                throw new ArgumentNullException(nameof(instance));

            // Get the reader
            using var reader = GetBinaryReader(stream);

            // Create the wrapper reader
            using var dataReader = new BinaryDataReader(reader, false);

            // Deserialize the data
            instance.Deserialize(dataReader);

            // Return the instance
            return instance;
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
            using var dataWriter = new BinaryDataWriter(writer);

            // Deserialize the data
            dataWriter.Write(obj);
        }

        #endregion
    }
}