using System;
using System.Drawing;
using System.Linq;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman
{
    // TODO: Serialize event info

    /// <summary>
    /// The data for a Rayman 1 .lev file
    /// </summary>
    public class Rayman1LevData : IBinarySerializable
    {
        public uint EventBlockPointer { get; set; }

        /// <summary>
        /// The pointer to <see cref="TexturesOffsetTable"/>
        /// </summary>
        public uint TextureOffsetTablePointer { get; set; }

        /// <summary>
        /// The width of the map, in cells
        /// </summary>
        public ushort MapWidth { get; set; }

        /// <summary>
        /// The height of the map, in cells
        /// </summary>
        public ushort MapHeight { get; set; }

        /// <summary>
        /// The size of the map in pixels
        /// </summary>
        public Size MapPixelSize => new Size(MapWidth * Rayman1LevTexture.Size, MapHeight * Rayman1LevTexture.Size);

        /// <summary>
        /// The color palettes
        /// </summary>
        public Color[][] ColorPalettes { get; set; }

        /// <summary>
        /// Unknown byte, always set to 2
        /// </summary>
        public byte Unknown1 { get; set; }

        /// <summary>
        /// The cells for the map
        /// </summary>
        public Rayman1LevMapCell[,] MapCells { get; set; }
        
        /// <summary>
        /// Unknown byte, different for each level
        /// </summary>
        public byte Unknown2 { get; set; }

        /// <summary>
        /// The index of the background image
        /// </summary>
        public byte BackgroundIndex { get; set; }

        /// <summary>
        /// The DES for the background sprites
        /// </summary>
        public uint BackgroundSpritesDES { get; set; }

        /// <summary>
        /// The length of <see cref="RoughTextures"/>
        /// </summary>
        public uint RoughTextureCount { get; set; }

        /// <summary>
        /// The length of <see cref="Unknown3"/>
        /// </summary>
        public uint Unknown3Count { get; set; }

        // TODO: Instead of int, each item is a texture with ONLY the ColorIndexes property
        /// <summary>
        /// The color indexes for the rough textures
        /// </summary>
        public int[][,] RoughTextures { get; set; }

        /// <summary>
        /// The checksum for the <see cref="RoughTextures"/>
        /// </summary>
        public byte RoughTexturesChecksum { get; set; }

        /// <summary>
        /// The index table for the <see cref="RoughTextures"/>
        /// </summary>
        public uint[] RoughTexturesIndexTable { get; set; }

        /// <summary>
        /// Unknown array of bytes
        /// </summary>
        public byte[] Unknown3 { get; set; }

        /// <summary>
        /// The checksum for <see cref="Unknown3"/>
        /// </summary>
        public byte Unknown3Checksum { get; set; }
        
        /// <summary>
        /// Offset table for <see cref="Unknown3"/>
        /// </summary>
        public uint[] Unknown3OffsetTable { get; set; }
        
        /// <summary>
        /// The offset table for the <see cref="NonTransparentTextures"/> and <see cref="TransparentTextures"/>
        /// </summary>
        public uint[] TexturesOffsetTable { get; set; }

        /// <summary>
        /// The total amount of textures for <see cref="NonTransparentTextures"/> and <see cref="TransparentTextures"/>
        /// </summary>
        public uint TexturesCount { get; set; }

        /// <summary>
        /// The amount of <see cref="NonTransparentTextures"/>
        /// </summary>
        public uint NonTransparentTexturesCount { get; set; }

        /// <summary>
        /// The byte size of <see cref="NonTransparentTextures"/>, <see cref="TransparentTextures"/> and <see cref="Unknown4"/>
        /// </summary>
        public uint TexturesDataTableCount { get; set; }

        /// <summary>
        /// The textures which are not transparent
        /// </summary>
        public Rayman1LevTexture[] NonTransparentTextures { get; set; }

        /// <summary>
        /// The textures which have transparency
        /// </summary>
        public Rayman1LevTransparentTexture[] TransparentTextures { get; set; }

        /// <summary>
        /// Unknown array of bytes, always 32 in length
        /// </summary>
        public byte[] Unknown4 { get; set; }

        /// <summary>
        /// The checksum for <see cref="NonTransparentTextures"/>, <see cref="TransparentTextures"/> and <see cref="Unknown4"/>
        /// </summary>
        public byte TexturesChecksum { get; set; }

        /// <summary>
        /// Gets a bitmap for the map cell textures
        /// </summary>
        /// <returns>The bitmap for the map</returns>
        public Bitmap GetBitmap()
        {
            // Create the bitmap
            Bitmap bmp = new Bitmap(Rayman1LevTexture.Size * MapWidth, Rayman1LevTexture.Size * MapHeight);

            // Enumerate each cell
            for (int cellY = 0; cellY < MapHeight; cellY++)
            {
                for (int cellX = 0; cellX < MapWidth; cellX++)
                {
                    // Get the cell
                    var cell = MapCells[cellX, cellY];

                    // Ignore if fully transparent
                    if (cell.TransparencyMode == Rayman1LevMapCellTransparencyMode.FullyTransparent)
                        continue;

                    // Get the offset for the texture
                    var texOffset = TexturesOffsetTable[cell.TextureIndex];

                    // Get the texture
                    var texture = cell.TransparencyMode == Rayman1LevMapCellTransparencyMode.NoTransparency ? NonTransparentTextures.FindItem(x => x.Offset == texOffset) : TransparentTextures.FindItem(x => x.Offset == texOffset);

                    // Make sure we got a texture
                    if (texture == null)
                        throw new Exception($"No texture found for cell ({cellX}, {cellY})");

                    // Write each pixel for the texture
                    for (int x = 0; x < Rayman1LevTexture.Size; x++)
                    {
                        for (int y = 0; y < Rayman1LevTexture.Size; y++)
                        {
                            // TODO: The color palette isn't always correct
                            // Get the color
                            var c = ColorPalettes[0][texture.ColorIndexes[x, y]];

                            // If the texture is transparent, replace the color with one with the alpha channel
                            if (texture is Rayman1LevTransparentTexture tt)
                                c = Color.FromArgb(tt.Alpha[x, y], c.R, c.G, c.B);

                            // Set the pixel
                            bmp.SetPixel(Rayman1LevTexture.Size * cellX + x, Rayman1LevTexture.Size * cellY + y, c);
                        }
                    }
                }
            }

            // Return the bitmap
            return bmp;
        }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            EventBlockPointer = reader.Read<uint>();
            TextureOffsetTablePointer = reader.Read<uint>();

            MapWidth = reader.Read<ushort>();
            MapHeight = reader.Read<ushort>();

            ColorPalettes = new Color[][]
            {
                new Color[256], 
                new Color[256], 
                new Color[256], 
            };

            foreach (var colorCollection in ColorPalettes)
            {
                for (int i = 0; i < colorCollection.Length; i++)
                {
                    colorCollection[i] = Color.FromArgb(reader.Read<byte>() * 4, reader.Read<byte>() * 4, reader.Read<byte>() * 4);
                }
            }

            ColorPalettes[0] = ColorPalettes[1].Reverse().ToArray();

            Unknown1 = reader.Read<byte>();

            if (reader.BaseStream.Position != 2317)
                throw new Exception("Header block length is incorrect");

            MapCells = new Rayman1LevMapCell[MapWidth, MapHeight];

            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    MapCells[x, y] = reader.Read<Rayman1LevMapCell>();
                }
            }
            
            Unknown2 = reader.Read<byte>();
            BackgroundIndex = reader.Read<byte>();
            BackgroundSpritesDES = reader.Read<uint>();

            RoughTextureCount = reader.Read<uint>();
            Unknown3Count = reader.Read<uint>();

            RoughTextures = new int[RoughTextureCount][,];

            for (int i = 0; i < RoughTextureCount; i++)
            {
                RoughTextures[i] = new int[16, 16];

                for (int x = 0; x < 16; x++)
                {
                    for (int y = 0; y < 16; y++)
                    {
                        RoughTextures[i][x, y] = reader.Read<byte>();
                    }
                }
            }

            RoughTexturesChecksum = reader.Read<byte>();
            RoughTexturesIndexTable = new uint[1200];

            for (int i = 0; i < RoughTexturesIndexTable.Length; i++)
                RoughTexturesIndexTable[i] = reader.Read<uint>();

            Unknown3 = new byte[Unknown3Count];

            for (int i = 0; i < Unknown3.Length; i++)
                Unknown3[i] = reader.Read<byte>();

            Unknown3Checksum = reader.Read<byte>();

            Unknown3OffsetTable = new uint[1200];

            for (int i = 0; i < Unknown3OffsetTable.Length; i++)
                Unknown3OffsetTable[i] = reader.Read<uint>();

            // NOTE: At this point the stream position should match the texture block offset

            if (reader.BaseStream.Position != TextureOffsetTablePointer)
                throw new Exception("Texture block offset is incorrect");

            TexturesOffsetTable = new uint[1200];

            for (int i = 0; i < TexturesOffsetTable.Length; i++)
                TexturesOffsetTable[i] = reader.Read<uint>();

            TexturesCount = reader.Read<uint>();
            NonTransparentTexturesCount = reader.Read<uint>();
            TexturesDataTableCount = reader.Read<uint>();

            var textureOffset = reader.BaseStream.Position;

            NonTransparentTextures = new Rayman1LevTexture[NonTransparentTexturesCount];

            for (int i = 0; i < NonTransparentTextures.Length; i++)
            {
                var t = new Rayman1LevTexture()
                {
                    Offset = (uint)(reader.BaseStream.Position - textureOffset)
                };

                t.Deserialize(reader);

                NonTransparentTextures[i] = t;
            }

            TransparentTextures = new Rayman1LevTransparentTexture[TexturesCount - NonTransparentTexturesCount];

            for (int i = 0; i < TransparentTextures.Length; i++)
            {
                var t = new Rayman1LevTransparentTexture()
                {
                    Offset = (uint)(reader.BaseStream.Position - textureOffset)
                };

                t.Deserialize(reader);

                TransparentTextures[i] = t;
            }

            Unknown4 = reader.ReadBytes(32);
            TexturesChecksum = reader.Read<byte>();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}