using System;
using System.Diagnostics;
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

        // WIP: Instead of int, each item is a texture with ONLY the ColorIndexes property
        /// <summary>
        /// The color indexes for the rough textures
        /// </summary>
        public byte[][,] RoughTextures { get; set; }

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

        // WIP: Deserialize into properties
        /// <summary>
        /// Data for the events
        /// </summary>
        public byte[] EventData { get; set; }

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
        /// Gets a bitmap for the map cell types
        /// </summary>
        /// <returns>The bitmap for the map cell types</returns>
        public Bitmap GetTypeBitmap()
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

                    // Get the type bitmap
                    using var typeBmp = Rayman1LevIcons.ResourceManager.GetObject(cell.CellType.ToString()).CastTo<Bitmap>();

                    // Write each pixel for the texture
                    for (int x = 0; x < Rayman1LevTexture.Size; x++)
                    {
                        for (int y = 0; y < Rayman1LevTexture.Size; y++)
                        {
                            if (typeBmp == null)
                                Console.WriteLine((int)cell.CellType);

                            // Get the pixel
                            var c = typeBmp?.GetPixel(x, y) ?? Color.Red;

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
            // HEADER BLOCK

            // Read block pointer
            EventBlockPointer = reader.Read<uint>();
            TextureOffsetTablePointer = reader.Read<uint>();

            // Read map size
            MapWidth = reader.Read<ushort>();
            MapHeight = reader.Read<ushort>();

            // Create the palettes
            ColorPalettes = new Color[][]
            {
                new Color[256], 
                new Color[256], 
                new Color[256], 
            };

            // Read each palette color
            for (var paletteIndex = 0; paletteIndex < ColorPalettes.Length; paletteIndex++)
            {
                // Get the palette
                var palette = ColorPalettes[paletteIndex];

                // Read each color
                for (int i = 0; i < palette.Length; i++)
                {
                    // Read the palette color as RGB and multiply by 4 (as the values are between 0-64)
                    palette[i] = Color.FromArgb(reader.Read<byte>() * 4, reader.Read<byte>() * 4,
                        reader.Read<byte>() * 4);
                }

                // Reverse the palette
                ColorPalettes[paletteIndex] = palette.Reverse().ToArray();
            }

            // Read unknown byte
            Unknown1 = reader.Read<byte>();

            // MAP BLOCK

            // Create the collection of map cells
            MapCells = new Rayman1LevMapCell[MapWidth, MapHeight];

            // Read each map cell
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    MapCells[x, y] = reader.Read<Rayman1LevMapCell>();
                }
            }
            
            // Read unknown byte
            Unknown2 = reader.Read<byte>();
            
            // Read the background data
            BackgroundIndex = reader.Read<byte>();
            BackgroundSpritesDES = reader.Read<uint>();

            // Read the rough textures count
            RoughTextureCount = reader.Read<uint>();
            
            // Read the length of the third unknown value
            Unknown3Count = reader.Read<uint>();

            // Create the collection of rough textures
            RoughTextures = new byte[RoughTextureCount][,];

            // Read each rough texture
            for (int i = 0; i < RoughTextureCount; i++)
            {
                RoughTextures[i] = new byte[16, 16];

                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        RoughTextures[i][x, y] = reader.Read<byte>();
                    }
                }
            }

            // Read the checksum for the rough textures
            RoughTexturesChecksum = reader.Read<byte>();

            // Create the index table for the rough textures
            RoughTexturesIndexTable = new uint[1200];

            // Read the index table for the rough textures
            for (int i = 0; i < RoughTexturesIndexTable.Length; i++)
                RoughTexturesIndexTable[i] = reader.Read<uint>();

            // Create the collection for the third unknown value
            Unknown3 = new byte[Unknown3Count];

            // Read the items for the third unknown value
            for (int i = 0; i < Unknown3.Length; i++)
                Unknown3[i] = reader.Read<byte>();

            // Read the checksum for the third unknown value
            Unknown3Checksum = reader.Read<byte>();

            // Create the offset table for the third unknown value
            Unknown3OffsetTable = new uint[1200];

            // Read the offset table for the third unknown value
            for (int i = 0; i < Unknown3OffsetTable.Length; i++)
                Unknown3OffsetTable[i] = reader.Read<uint>();

            // TEXTURE BLOCK

            // At this point the stream position should match the texture block offset
            if (reader.BaseStream.Position != TextureOffsetTablePointer)
                throw new Exception("Texture block offset is incorrect");

            // Create the offset table for the textures
            TexturesOffsetTable = new uint[1200];

            // Read the offset table for the textures
            for (int i = 0; i < TexturesOffsetTable.Length; i++)
                TexturesOffsetTable[i] = reader.Read<uint>();

            // Read the textures count
            TexturesCount = reader.Read<uint>();
            NonTransparentTexturesCount = reader.Read<uint>();
            TexturesDataTableCount = reader.Read<uint>();

            // Get the current offset to use for the texture offsets
            var textureBaseOffset = reader.BaseStream.Position;

            // Create the collection of non-transparent textures
            NonTransparentTextures = new Rayman1LevTexture[NonTransparentTexturesCount];

            // Read the non-transparent textures
            for (int i = 0; i < NonTransparentTextures.Length; i++)
            {
                // Create the texture
                var t = new Rayman1LevTexture()
                {
                    // Set the offset
                    Offset = (uint)(reader.BaseStream.Position - textureBaseOffset)
                };

                // Deserialize the texture
                t.Deserialize(reader);

                // Add the texture to the collection
                NonTransparentTextures[i] = t;
            }

            // Create the collection of transparent textures
            TransparentTextures = new Rayman1LevTransparentTexture[TexturesCount - NonTransparentTexturesCount];

            // Read the transparent textures
            for (int i = 0; i < TransparentTextures.Length; i++)
            {
                // Create the texture
                var t = new Rayman1LevTransparentTexture()
                {
                    // Set the offset
                    Offset = (uint)(reader.BaseStream.Position - textureBaseOffset)
                };

                // Deserialize the texture
                t.Deserialize(reader);

                // Add the texture to the collection
                TransparentTextures[i] = t;
            }

            // Read the fourth unknown value
            Unknown4 = reader.ReadBytes(32);

            // Read the checksum for the textures
            TexturesChecksum = reader.Read<byte>();

            // EVENT BLOCK

            // Read the event data
            EventData = reader.ReadRemainingBytes();
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            // HEADER BLOCK

            // Write block pointer
            writer.Write(EventBlockPointer);
            writer.Write(TextureOffsetTablePointer);

            // Write map size
            writer.Write(MapWidth);
            writer.Write(MapHeight);

            // Write each palette
            foreach (var palette in ColorPalettes)
            {
                foreach (var color in palette.Reverse())
                {
                    // Write the palette color as RGB and divide by 4 (as the values are between 0-64)
                    writer.Write((byte)(color.R / 4));
                    writer.Write((byte)(color.G / 4));
                    writer.Write((byte)(color.B / 4));
                }
            }

            // Write unknown byte
            writer.Write(Unknown1);

            // MAP BLOCK

            // Write each map cell
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    writer.Write(MapCells[x, y]);
                }
            }

            // Write unknown byte
            writer.Write(Unknown2);

            // Write the background data
            writer.Write(BackgroundIndex);
            writer.Write(BackgroundSpritesDES);

            // Write the rough textures count
            writer.Write(RoughTextureCount);

            // Write the length of the third unknown value
            writer.Write(Unknown3Count);

            // Write each rough texture
            for (int i = 0; i < RoughTextureCount; i++)
            {
                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        writer.Write(RoughTextures[i][x, y]);
                    }
                }
            }

            // Write the checksum for the rough textures
            writer.Write(RoughTexturesChecksum);

            // Write the index table for the rough textures
            foreach (var t in RoughTexturesIndexTable)
                writer.Write(t);

            // Write the items for the third unknown value
            foreach (var item in Unknown3)
                writer.Write(item);

            // Write the checksum for the third unknown value
            writer.Write(Unknown3Checksum);

            // Write the offset table for the third unknown value
            foreach (var offset in Unknown3OffsetTable)
                writer.Write(offset);

            // TEXTURE BLOCK

            // Write the offset table for the textures
            foreach (var offset in TexturesOffsetTable)
                writer.Write(offset);

            // Write the textures count
            writer.Write(TexturesCount);
            writer.Write(NonTransparentTexturesCount);
            writer.Write(TexturesDataTableCount);

            // Write the non-transparent textures
            foreach (var texture in NonTransparentTextures)
                // Write the texture
                writer.Write(texture);

            // Write the transparent textures
            foreach (var texture in TransparentTextures)
                // Write the texture
                writer.Write(texture);

            // Write the fourth unknown value
            writer.Write(Unknown4);

            // Write the checksum for the textures
            writer.Write(TexturesChecksum);

            // EVENT BLOCK

            // Write the event data
            writer.Write(EventData);
        }
    }
}