using RayCarrot.Binary;

namespace RayCarrot.Rayman.Ray1
{
    /// <summary>
    /// Base file data for Rayman 1 PC versions
    /// </summary>
    public abstract class Rayman1PCBaseFile : IBinarySerializable
    {
        /// <summary>
        /// The primary kit header, always 5 bytes starting with KIT and then NULL padding
        /// </summary>
        public string PrimaryKitHeader { get; set; }

        /// <summary>
        /// The secondary kit header, always 5 bytes starting with KIT or the language tag and then NULL padding
        /// </summary>
        public string SecondaryKitHeader { get; set; }

        /// <summary>
        /// Unknown value
        /// </summary>
        public ushort Unknown1 { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public virtual void Serialize(IBinarySerializer s)
        {
            // Get the settings
            var settings = s.GetSettings<Ray1Settings>();

            if (settings.Game != Ray1Game.Rayman1 && settings.Platform == Platform.PC)
            {
                PrimaryKitHeader = s.SerializeString(PrimaryKitHeader, 5, name: nameof(PrimaryKitHeader));
                SecondaryKitHeader = s.SerializeString(SecondaryKitHeader, 5, name: nameof(SecondaryKitHeader));
                Unknown1 = s.Serialize<ushort>(Unknown1, name: nameof(Unknown1));
            }
        }
    }
}