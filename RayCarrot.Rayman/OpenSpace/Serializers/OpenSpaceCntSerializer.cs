using System.IO;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The serializer for the .cnt files in OpenSpace games
    /// </summary>
    public class OpenSpaceCntSerializer : OpenSpaceDataSerializer<OpenSpaceCntData>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        public OpenSpaceCntSerializer(OpenSpaceSettings settings) : base(settings)
        { }

        /// <summary>
        /// Gets a new binary reader to use for the specified stream
        /// </summary>
        protected override BinaryReader GetBinaryReader(Stream stream)
        {
            return new StandardBinaryReader(stream, Settings.ByteOrder, Encoding.UTF8, true);
        }

        /// <summary>
        /// Gets a new binary writer to use for the specified stream
        /// </summary>
        protected override BinaryWriter GetBinaryWriter(Stream stream)
        {
            return new StandardBinaryWriter(stream, Settings.ByteOrder, Encoding.UTF8, true);
        }
    }
}