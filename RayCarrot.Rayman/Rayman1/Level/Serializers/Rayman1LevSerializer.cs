using System.IO;
using System.Text;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The serializer for the .lev files in Rayman 1
    /// </summary>
    public class Rayman1LevSerializer : ConfigurableBinaryDataSerializer<Rayman1LevData, Rayman1Settings>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        public Rayman1LevSerializer(Rayman1Settings settings) : base(settings)
        { }

        /// <summary>
        /// Gets a new binary reader to use for the specified stream
        /// </summary>
        protected override BinaryReader GetBinaryReader(Stream stream)
        {
            return new StandardBinaryReader(stream, Settings.ByteOrder, Encoding.Unicode);
        }

        /// <summary>
        /// Gets a new binary writer to use for the specified stream
        /// </summary>
        protected override BinaryWriter GetBinaryWriter(Stream stream)
        {
            return new StandardBinaryWriter(stream, Settings.ByteOrder, Encoding.Unicode);
        }
    }
}