namespace RayCarrot.Rayman
{
    /// <summary>
    /// Defines settings for serializing binary data
    /// </summary>
    public interface IBinarySerializableSettings
    {
        /// <summary>
        /// The byte order to use
        /// </summary>
        ByteOrder ByteOrder { get; }
    }
}