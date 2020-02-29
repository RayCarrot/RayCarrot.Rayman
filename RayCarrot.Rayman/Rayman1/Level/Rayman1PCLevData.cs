using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using RayCarrot.CarrotFramework.Abstractions;
using RayCarrot.Extensions;
using RayCarrot.IO;

namespace RayCarrot.Rayman.Rayman1
{
    /// <summary>
    /// The data for a Rayman 1 .lev file on PC
    /// </summary>
    public class Rayman1PCLevData : IBinarySerializable<Rayman1Settings>
    {
        #region Public Static Properties

        /// <summary>
        /// Gets the default serializer
        /// </summary>
        public static BinaryDataSerializer<Rayman1PCLevData, Rayman1Settings> GetSerializer() => new BinaryDataSerializer<Rayman1PCLevData, Rayman1Settings>(Rayman1GameMode.Rayman1PC.GetSettings());

        #endregion

        #region Public Properties

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
        public Size MapPixelSize => new Size(MapWidth * Rayman1PCLevTexture.Size, MapHeight * Rayman1PCLevTexture.Size);

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
        public Rayman1PCLevMapCell[,] MapCells { get; set; }

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
        public BinarySerializableList<uint> RoughTexturesIndexTable { get; set; }

        /// <summary>
        /// Unknown array of bytes
        /// </summary>
        public BinarySerializableList<byte> Unknown3 { get; set; }

        /// <summary>
        /// The checksum for <see cref="Unknown3"/>
        /// </summary>
        public byte Unknown3Checksum { get; set; }

        /// <summary>
        /// Offset table for <see cref="Unknown3"/>
        /// </summary>
        public BinarySerializableList<uint> Unknown3OffsetTable { get; set; }

        /// <summary>
        /// The offset table for the <see cref="NonTransparentTextures"/> and <see cref="TransparentTextures"/>
        /// </summary>
        public BinarySerializableList<uint> TexturesOffsetTable { get; set; }

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
        public Rayman1PCLevTexture[] NonTransparentTextures { get; set; }

        /// <summary>
        /// The textures which have transparency
        /// </summary>
        public Rayman1PCLevTransparentTexture[] TransparentTextures { get; set; }

        /// <summary>
        /// Unknown array of bytes, always 32 in length
        /// </summary>
        public byte[] Unknown4 { get; set; }

        /// <summary>
        /// The checksum for <see cref="NonTransparentTextures"/>, <see cref="TransparentTextures"/> and <see cref="Unknown4"/>
        /// </summary>
        public byte TexturesChecksum { get; set; }  

        /// <summary>
        /// The number of available events in the map
        /// </summary>
        public ushort EventCount { get; set; }

        /// <summary>
        /// Data table for event linking
        /// </summary>
        public BinarySerializableList<ushort> EventLinkingTable { get; set; }

        /// <summary>
        /// The events in the map
        /// </summary>
        public BinarySerializableList<Rayman1PCLevEvent> Events { get; set; }

        /// <summary>
        /// The event commands in the map
        /// </summary>
        public BinarySerializableList<Rayman1PCLevEventCommand> EventCommands { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a bitmap for the map cell textures
        /// </summary>
        /// <returns>The bitmap for the map</returns>
        public Bitmap GetBitmap()
        {
            // Create the bitmap
            var bmp = new Bitmap(Rayman1PCLevTexture.Size * MapWidth, Rayman1PCLevTexture.Size * MapHeight);

            // Lock the bitmap for faster reading/writing
            using (var lockedBmp = new BitmapLock(bmp))
            {
                // Get the palette changers
                var paletteXChangers = Events.Where(x => x.Type == 158 && x.SubEtat < 6).ToDictionary(x => x.XPosition, x => (PaletteChangerMode)x.SubEtat);
                var paletteYChangers = Events.Where(x => x.Type == 158 && x.SubEtat >= 6).ToDictionary(x => x.YPosition, x => (PaletteChangerMode)x.SubEtat);

                if (paletteXChangers.Any() && paletteYChangers.Any())
                    throw new Exception("Horizontal and vertical palette changers can't both appear in the same level");

                bool isPaletteHorizontal = paletteXChangers.Any();
                int currentPalette = 0;

                // Enumerate each cell
                for (int cellY = 0; cellY < MapHeight; cellY++)
                {
                    if (isPaletteHorizontal)
                        currentPalette = 0;
                    else
                    {
                        for (int y = 0; y < Rayman1PCLevTexture.Size; y++)
                        {
                            var py = paletteYChangers.TryGetValue((uint)(Rayman1PCLevTexture.Size * cellY + y), out PaletteChangerMode pm) ? (PaletteChangerMode?)pm : null;

                            if (py != null)
                            {
                                currentPalette = py switch
                                {
                                    PaletteChangerMode.Top2tobottom1 => 0,
                                    PaletteChangerMode.Top3tobottom1 => 0,

                                    PaletteChangerMode.Top1tobottom2 => 1,
                                    PaletteChangerMode.Top3tobottom2 => 1,

                                    PaletteChangerMode.Top1tobottom3 => 2,
                                    PaletteChangerMode.Top2tobottom3 => 2,
                                    _ => currentPalette
                                };
                            }
                        }
                    }

                    for (int cellX = 0; cellX < MapWidth; cellX++)
                    {
                        // Get the cell
                        var cell = MapCells[cellX, cellY];

                        if (isPaletteHorizontal)
                        {
                            for (int x = 0; x < Rayman1PCLevTexture.Size; x++)
                            {
                                var px = paletteXChangers.TryGetValue((uint)(Rayman1PCLevTexture.Size * cellX + x), out PaletteChangerMode pm) ? (PaletteChangerMode?)pm : null;

                                if (px != null)
                                {
                                    currentPalette = px switch
                                    {
                                        PaletteChangerMode.Left3toRight1 => 0,
                                        PaletteChangerMode.Left2toRight1 => 0,

                                        PaletteChangerMode.Left1toRight2 => 1,
                                        PaletteChangerMode.Left3toRight2 => 1,

                                        PaletteChangerMode.Left1toRight3 => 2,
                                        PaletteChangerMode.Left2toRight3 => 2,
                                        _ => currentPalette
                                    };
                                }
                            }
                        }

                        // Ignore if fully transparent
                        if (cell.TransparencyMode == Rayman1PCLevMapCellTransparencyMode.FullyTransparent)
                            continue;

                        // Get the offset for the texture
                        var texOffset = TexturesOffsetTable[cell.TextureIndex];

                        // Get the texture
                        var texture = cell.TransparencyMode == Rayman1PCLevMapCellTransparencyMode.NoTransparency ? NonTransparentTextures.FindItem(x => x.Offset == texOffset) : TransparentTextures.FindItem(x => x.Offset == texOffset);

                        // Make sure we got a texture
                        if (texture == null)
                            throw new Exception($"No texture found for cell ({cellX}, {cellY})");

                        // Write each pixel for the texture
                        for (int x = 0; x < Rayman1PCLevTexture.Size; x++)
                        {
                            var currentX = Rayman1PCLevTexture.Size * cellX + x;

                            for (int y = 0; y < Rayman1PCLevTexture.Size; y++)
                            {
                                var currentY = Rayman1PCLevTexture.Size * cellY + y;

                                // Get the color
                                var c = ColorPalettes[currentPalette][texture.ColorIndexes[x, y]];

                                // If the texture is transparent, replace the color with one with the alpha channel
                                if (texture is Rayman1PCLevTransparentTexture tt)
                                    c = Color.FromArgb(tt.Alpha[x, y], c.R, c.G, c.B);

                                // Set the pixel
                                lockedBmp.SetPixel(currentX, currentY, c);
                            }
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
            var bmp = new Bitmap(Rayman1PCLevTexture.Size * MapWidth, Rayman1PCLevTexture.Size * MapHeight);

            // Load every bitmap
            Dictionary<Rayman1PCLevMapCellType, Bitmap> typeBitmaps = new Dictionary<Rayman1PCLevMapCellType, Bitmap>();
            Dictionary<Bitmap, BitmapLock> bitmapLocks = new Dictionary<Bitmap, BitmapLock>();

            try
            {
                // Load the bitmaps and lock them
                foreach (var type in EnumHelpers.GetValues<Rayman1PCLevMapCellType>())
                {
                    // Get the bitmap
                    var typeBmp = Rayman1LevIcons.ResourceManager.GetObject(type.ToString()).CastTo<Bitmap>();

                    typeBitmaps.Add(type, typeBmp);
                    bitmapLocks.Add(typeBmp, new BitmapLock(typeBmp));
                }

                // Lock the bitmap for faster reading/writing
                using var lockedBmp = new BitmapLock(bmp);

                // Enumerate each cell
                for (int cellY = 0; cellY < MapHeight; cellY++)
                {
                    for (int cellX = 0; cellX < MapWidth; cellX++)
                    {
                        // Get the cell
                        var cell = MapCells[cellX, cellY];

                        // Get the bitmap lock
                        var bmpLock = bitmapLocks[typeBitmaps.TryGetValue(cell.CellType)];

                        // Write each pixel for the texture
                        for (int x = 0; x < Rayman1PCLevTexture.Size; x++)
                        {
                            for (int y = 0; y < Rayman1PCLevTexture.Size; y++)
                            {
                                // Get the pixel
                                var c = bmpLock.GetPixel(x, y);

                                // Set the pixel
                                lockedBmp.SetPixel(Rayman1PCLevTexture.Size * cellX + x, Rayman1PCLevTexture.Size * cellY + y, c);
                            }
                        }
                    }
                }
            }
            finally
            {
                bitmapLocks?.Values.DisposeAll();
                typeBitmaps?.Values.DisposeAll();
            }

            // Return the bitmap
            return bmp;
        }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<Rayman1Settings> reader)
        {
            if (reader.SerializerSettings.Game != Rayman1Game.Rayman1)
                throw new NotImplementedException("Currently only Rayman 1 level files can be read");

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
            MapCells = new Rayman1PCLevMapCell[MapWidth, MapHeight];

            // Read each map cell
            for (int y = 0; y < MapHeight; y++)
            {
                for (int x = 0; x < MapWidth; x++)
                {
                    MapCells[x, y] = reader.Read<Rayman1PCLevMapCell>();
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

            // Read the index table for the rough textures
            RoughTexturesIndexTable = new BinarySerializableList<uint>(1200);
            RoughTexturesIndexTable.Deserialize(reader);

            // Read the items for the third unknown value
            Unknown3 = new BinarySerializableList<byte>((int)Unknown3Count);
            Unknown3.Deserialize(reader);

            // Read the checksum for the third unknown value
            Unknown3Checksum = reader.Read<byte>();

            // Read the offset table for the third unknown value
            Unknown3OffsetTable = new BinarySerializableList<uint>(1200);
            Unknown3OffsetTable.Deserialize(reader);

            // TEXTURE BLOCK

            // At this point the stream position should match the texture block offset
            if (reader.BaseStream.Position != TextureOffsetTablePointer)
                throw new BinarySerializableException("Texture block offset is incorrect");

            // Read the offset table for the textures
            TexturesOffsetTable = new BinarySerializableList<uint>(1200);
            TexturesOffsetTable.Deserialize(reader);

            // Read the textures count
            TexturesCount = reader.Read<uint>();
            NonTransparentTexturesCount = reader.Read<uint>();
            TexturesDataTableCount = reader.Read<uint>();

            // Get the current offset to use for the texture offsets
            var textureBaseOffset = reader.BaseStream.Position;

            // Create the collection of non-transparent textures
            NonTransparentTextures = new Rayman1PCLevTexture[NonTransparentTexturesCount];

            // Read the non-transparent textures
            for (int i = 0; i < NonTransparentTextures.Length; i++)
            {
                // Create the texture
                var t = new Rayman1PCLevTexture()
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
            TransparentTextures = new Rayman1PCLevTransparentTexture[TexturesCount - NonTransparentTexturesCount];

            // Read the transparent textures
            for (int i = 0; i < TransparentTextures.Length; i++)
            {
                // Create the texture
                var t = new Rayman1PCLevTransparentTexture()
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

            // Read the event count
            EventCount = reader.Read<ushort>();

            // Read the event linking table
            EventLinkingTable = new BinarySerializableList<ushort>(EventCount);
            EventLinkingTable.Deserialize(reader);

            // Read the events
            Events = new BinarySerializableList<Rayman1PCLevEvent>(EventCount);
            Events.Deserialize(reader);

            // Read the event commands
            EventCommands = new BinarySerializableList<Rayman1PCLevEventCommand>(EventCount);
            EventCommands.Deserialize(reader);
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<Rayman1Settings> writer)
        {
            if (writer.SerializerSettings.Game != Rayman1Game.Rayman1)
                throw new NotImplementedException("Currently only Rayman 1 level files can be written");

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
            writer.Write(RoughTexturesIndexTable);

            // Write the items for the third unknown value
            writer.Write(Unknown3);

            // Write the checksum for the third unknown value
            writer.Write(Unknown3Checksum);

            // Write the offset table for the third unknown value
            writer.Write(Unknown3OffsetTable);

            // TEXTURE BLOCK

            // Write the offset table for the textures
            writer.Write(TexturesOffsetTable);

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

            // Write the event count
            writer.Write(EventCount);

            // Write the event linking table
            writer.Write(EventLinkingTable);

            // Write the events
            writer.Write(Events);

            // Write the event commands
            writer.Write(EventCommands);
        }

        #endregion
    }

    public enum PaletteChangerMode
    {
        Left1toRight2 = 0,
        Left1toRight3 = 1,

        Left2toRight3 = 2,
        Left2toRight1 = 3,

        Left3toRight1 = 4,
        Left3toRight2 = 5,
                      
        Top1tobottom2 = 6,
        Top1tobottom3 = 7,

        Top2tobottom3 = 8,
        Top2tobottom1 = 9,

        Top3tobottom1 = 10,
        Top3tobottom2 = 11,
    }

    public static class DesignerEventParser
    {
        public static IEnumerable<DesignerEvent> ParseEvents(FileSystemPath file)
        {
            using var stream = File.OpenRead(file);
            using var reader = new StreamReader(stream);

            bool isInComment = false;

            bool crashed = false;

            while (!reader.EndOfStream && !crashed)
            {
                string getNewLine()
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (line == null || line.IsNullOrWhiteSpace())
                            continue;

                        string l = String.Empty;

                        foreach (var c in line)
                        {
                            if (c == '/')
                                isInComment ^= true;
                            else if (!isInComment)
                                l += c;
                        }

                        if (!l.IsNullOrWhiteSpace())
                            return l;
                    }

                    return null;
                }

                List<string> lineBuffer = new List<string>();

                string nextValue()
                {
                    if (!lineBuffer.Any())
                        lineBuffer = getNewLine().Split(',').Select(x => x.Trim()).Where(x => !x.IsNullOrWhiteSpace()).ToList();

                    var v = lineBuffer.First();

                    lineBuffer.RemoveAt(0);

                    return v;
                }

                var e = new DesignerEvent();

                nextValue();
                
                e.Name = nextValue();
                e.DESFile = nextValue();
                e.UnkGroup = UInt32.Parse(nextValue());

                e.ETAFile = nextValue();

                var cmds = new List<int>();

                bool is33 = false;

                while (true)
                {
                    var v = nextValue();

                    if (is33 && v == "255")
                    {
                        cmds.RemoveAt(cmds.Count - 1);
                        break;
                    }

                    if (v == "33")
                    {
                        is33 = true;
                    }
                    else
                    {
                        is33 = false;
                    }

                    cmds.Add(Int32.Parse(v));
                }

                try
                {                
                    e.EventCommands = cmds.ToArray();

                    e.XPosition = Int32.Parse(nextValue());
                    e.YPosition = Int32.Parse(nextValue());

                    e.Etat = UInt32.Parse(nextValue());
                    e.SubEtat = UInt32.Parse(nextValue());

                    e.Offset_BX = UInt32.Parse(nextValue());
                    e.Offset_BY = UInt32.Parse(nextValue());
                    e.Offset_HY = UInt32.Parse(nextValue());

                    e.Follow_enabled = nextValue() == "1";
                    e.Follow_sprite = nextValue() == "1";
                    e.Hitpoints = UInt32.Parse(nextValue());

                    e.Obj_type = UInt32.Parse(nextValue());
                    e.Hit_sprite = UInt32.Parse(nextValue());
                    e.DesignerGroup = UInt32.Parse(nextValue());
                }
                catch (Exception ex)
                {
                    crashed = true;
                }

                yield return e;
            }
        }

        public static IEnumerable<DesignerEventLoc> ParseLoc(FileSystemPath file, int offset)
        {
            using var stream = File.OpenRead(file);

            stream.Position = offset;

            string readString()
            {
                string str = "";

                char[] ch;

                while ((ch = Encoding.ASCII.GetChars(new byte[]
                {
                    (byte)stream.ReadByte()
                })).First() != 0)
                    str += new string(ch);

                return str;
            }

            while (stream.Position < stream.Length)
            {
                yield return new DesignerEventLoc()
                {
                    LocKey = readString(),
                    Name = readString(),
                    Description = readString(),
                };
            }
        }

        public class DesignerEvent
        {
            public string Name { get; set; }

            public string DESFile { get; set; }

            public uint UnkGroup { get; set; }

            public string ETAFile { get; set; }

            public int[] EventCommands { get; set; }

            public int XPosition { get; set; }

            public int YPosition { get; set; }

            public uint Etat { get; set; }

            public uint SubEtat { get; set; }
            
            public uint Offset_BX { get; set; }

            public uint Offset_BY { get; set; }

            public uint Offset_HY { get; set; }

            public bool Follow_enabled { get; set; }

            public bool Follow_sprite { get; set; }

            public uint Hitpoints { get; set; }

            public uint Obj_type { get; set; }

            public uint Hit_sprite { get; set; }

            public uint DesignerGroup { get; set; }
        }

        public class DesignerEventLoc
        {
            public string LocKey { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }
        }
    }
}