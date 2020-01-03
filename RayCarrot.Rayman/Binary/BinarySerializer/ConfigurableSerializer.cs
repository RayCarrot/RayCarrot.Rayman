namespace RayCarrot.Rayman
{
    /// <summary>
    /// Base serializer class for a configurable serializer with settings
    /// </summary>
    /// <typeparam name="T">The type of data to serialize to. Either a supported value type of a class implementing <see cref="IBinarySerializable"/></typeparam>
    /// <typeparam name="S">The type of settings to use</typeparam>
    public abstract class ConfigurableSerializer<T, S> : BinaryDataSerializer<T>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        protected ConfigurableSerializer(S settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// The settings when serializing the data
        /// </summary>
        public S Settings { get; }
    }
}