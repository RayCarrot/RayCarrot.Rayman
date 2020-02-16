﻿namespace RayCarrot.Rayman.Rayman1
{
    /// <summary>
    /// The data for a transparent 16x16 texture for a Rayman 1 .lev file
    /// </summary>
    public class Rayman1PCLevTransparentTexture : Rayman1PCLevTexture
    {
        /// <summary>
        /// The alpha channel values for each texture pixel
        /// </summary>
        public byte[,] Alpha { get; set; }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public override void Deserialize(IBinaryDataReader<Rayman1Settings> reader)
        {
            ColorIndexes = new byte[Size, Size];

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    ColorIndexes[x, y] = reader.Read<byte>();
                }
            }

            Alpha = new byte[Size, Size];

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    Alpha[x, y] = reader.Read<byte>();
                }
            }

            Unknown1 = reader.ReadBytes(32);
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public override void Serialize(IBinaryDataWriter<Rayman1Settings> writer)
        {
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    writer.Write(ColorIndexes[x, y]);
                }
            }

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    writer.Write(Alpha[x, y]);
                }
            }

            writer.Write(Unknown1);
        }
    }
}