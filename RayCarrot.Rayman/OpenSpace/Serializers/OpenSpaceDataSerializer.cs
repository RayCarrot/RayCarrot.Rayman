namespace RayCarrot.Rayman
{
    /// <summary>
    /// Base serializer class for OpenSpace data
    /// </summary>
    /// <typeparam name="T">The type of data to serialize to. Either a supported value type of a class implementing <see cref="IBinarySerializable"/></typeparam>
    public abstract class OpenSpaceDataSerializer<T> : BinaryDataSerializer<T>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        protected OpenSpaceDataSerializer(OpenSpaceSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// The settings when serializing the data
        /// </summary>
        public OpenSpaceSettings Settings { get; }
    }
}