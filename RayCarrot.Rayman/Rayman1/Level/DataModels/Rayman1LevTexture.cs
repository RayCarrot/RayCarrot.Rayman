using System;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The data for a 16x16 texture for a Rayman 1 .lev file
    /// </summary>
    public class Rayman1LevTexture : IBinarySerializable
    {
        /// <summary>
        /// The size of the texture
        /// </summary>
        public const int Size = 16;

        /// <summary>
        /// The offset for this texture, as defines in the textures offset table. This value is not a part of the texture and has to be set manually.
        /// </summary>
        public uint Offset { get; set; }

        /// <summary>
        /// The color indexes for this texture
        /// </summary>
        public byte[,] ColorIndexes { get; set; }

        /// <summary>
        /// Unknown array of bytes, always 32 in length
        /// </summary>
        public byte[] Unknown1 { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public virtual void Deserialize(BinaryDataReader reader)
        {
            // Set the color array
            ColorIndexes = new byte[Size, Size];

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    ColorIndexes[x, y] = reader.Read<byte>();
                }
            }

            Unknown1 = reader.ReadBytes(32);
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public virtual void Serialize(BinaryDataWriter writer)
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    writer.Write(ColorIndexes[x, y]);
                }
            }

            writer.Write(Unknown1);
        }
    }
}