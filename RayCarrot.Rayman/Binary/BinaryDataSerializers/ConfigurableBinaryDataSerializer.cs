using System.IO;
using RayCarrot.Extensions;
using RayCarrot.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The binary serializer to use for serializable data which requires settings to be passed in their constructor
    /// </summary>
    /// <typeparam name="T">The type of data to serialize to. Either a supported value type of a class implementing <see cref="IBinarySerializable"/></typeparam>
    /// <typeparam name="S">The type of settings to pass in</typeparam>
    public abstract class ConfigurableBinaryDataSerializer<T, S> : BinaryDataSerializer<T>
        where T : IBinarySerializable
        where S : IBinarySerializableSettings
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        protected ConfigurableBinaryDataSerializer(S settings)
        {
            Settings = settings;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// The settings when serializing the data
        /// </summary>
        public S Settings { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Deserializes the data from the serialized file as a stream
        /// </summary>
        /// <param name="stream">The file stream to deserialize</param>
        /// <param name="settings">The serializer settings to use</param>
        /// <returns>The deserialized object</returns>
        public override T Deserialize(Stream stream)
        {
            // Get the reader
            using var binaryReader = GetBinaryReader(stream);

            // Create the wrapper reader
            using var reader = new BinaryDataReader(binaryReader, false);

            // Create a new instance with the settings
            var instance = typeof(T).CreateInstance(Settings).CastTo<T>();

            // Deserialize the data
            instance.Deserialize(reader);

            // Return the instance
            return instance;
        }

        #endregion
    }
}