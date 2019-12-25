using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using RayCarrot.CarrotFramework.Abstractions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The data used for a .gf file for OpenSpace games
    /// </summary>
    public class OpenSpaceGFFile : IBinarySerializable
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="settings">The settings when serializing the data</param>
        public OpenSpaceGFFile(OpenSpaceSettings settings)
        {
            // Set properties
            Settings = settings;
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The settings when serializing the data
        /// </summary>
        protected OpenSpaceSettings Settings { get; }

        #endregion

        #region Public Properties

        /// <summary>
        /// The version (only used for <see cref="OpenSpaceEngineVersion.Montreal"/>)
        /// </summary>
        public byte Version { get; set; }

        /// <summary>
        /// The number of available channels to read
        /// </summary>
        public byte Channels { get; set; }

        /// <summary>
        /// The enlarge value (only used for <see cref="OpenSpaceEngineVersion.Rayman3"/> for certain games)
        /// </summary>
        public byte EnlargeValue { get; set; }

        /// <summary>
        /// The repeat byte to use
        /// </summary>
        public byte RepeatByte { get; set; }

        /// <summary>
        /// The palette bytes per color (only used for <see cref="OpenSpaceEngineVersion.Montreal"/>)
        /// </summary>
        public byte PaletteBytesPerColor { get; set; }

        /// <summary>
        /// The pallet 
        /// </summary>
        public byte[] Palette { get; set; }

        /// <summary>
        /// The number of available palette colors (only used for <see cref="OpenSpaceEngineVersion.Montreal"/>)
        /// </summary>
        public ushort PaletteNumColors { get; set; }

        /// <summary>
        /// The format signature
        /// </summary>
        public uint Format { get; set; }

        /// <summary>
        /// The image width in pixels
        /// </summary>
        public uint Width { get; set; }

        /// <summary>
        /// The image height in pixels
        /// </summary>
        public uint Height { get; set; }

        /// <summary>
        /// Indicates if the image is transparent
        /// </summary>
        public bool IsTransparent { get; set; }

        /// <summary>
        /// The color for each pixel of the image
        /// </summary>
        public Color[,] Pixels { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a bitmap from the image as a thumbnail
        /// </summary>
        /// <param name="width">The image width</param>
        /// <returns>The bitmap</returns>
        public Bitmap GetBitmapThumbnail(int width)
        {
            return GetBitmapThumbnail(width, (int)(Height / ((double)Width / width)));
        }

        /// <summary>
        /// Gets a bitmap from the image as a thumbnail
        /// </summary>
        /// <param name="width">The image width</param>
        /// <param name="height">The image height</param>
        /// <returns>The bitmap</returns>
        public Bitmap GetBitmapThumbnail(int width, int height)
        {
            // Create the bitmap
            Bitmap bmp = new Bitmap(width, height, IsTransparent ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);

            // Get the scale factors
            var widthScale = Width / (double)width;
            var heightScale = Height / (double)height;

            // Set each pixel
            for (int y = 0; y < height; y++)
            for (int x = 0; x < width; x++)
                bmp.SetPixel(x, y, Pixels[(int)(x * widthScale), (int)(y * heightScale)]);

            // Return the bitmap
            return bmp;
        }

        /// <summary>
        /// Gets a bitmap from the image
        /// </summary>
        /// <returns>The bitmap</returns>
        public Bitmap GetBitmap()
        {
            // Create the bitmap
            Bitmap bmp = new Bitmap((int)Width, (int)Height, IsTransparent ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);

            // Set each pixel
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                bmp.SetPixel(x, y, Pixels[x, y]);

            // Return the bitmap
            return bmp;
        }

        /// <summary>
        /// Imports a bitmap image into the file, keeping the structure based on the properties
        /// </summary>
        /// <param name="bmp">The bitmap to import</param>
        public void ImportFromBitmap(Bitmap bmp)
        {
            // Set size
            Pixels = new Color[bmp.Width, bmp.Height];
            Width = (uint)bmp.Width;
            Height = (uint)bmp.Height;

            var isTransparent = false;

            // Set each pixel
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    // Get the pixel data
                    var pixel = bmp.GetPixel(x, y);
                    
                    // Set the pixel
                    Pixels[x, y] = pixel;

                    // Check if it's transparent
                    if (pixel.A != 0)
                        isTransparent = true;
                }
            }

            // Check if the transparency value should be updated
            if (IsTransparent != isTransparent)
            {
                // Update channels if they are 3 or more
                if (Channels == 3)
                    Channels = 4;
                else if (Channels == 4)
                    Channels = 3;

                // Update transparency
                IsTransparent = isTransparent;
            }
        }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            // Read the format
            if (Settings.EngineVersion == OpenSpaceEngineVersion.Montreal)
            {
                Version = reader.Read<byte>();
                Format = 1555;
            }
            else if (Settings.Platform == OpenSpacePlatform.iOS || Settings.Game == OpenSpaceGame.TonicTroubleSpecialEdition)
            {
                Format = 8888;
            }
            else
            {
                Format = reader.Read<uint>();
            }

            // Read the size
            Width = reader.Read<uint>();
            Height = reader.Read<uint>();

            // Get the pixel count
            var channelPixels = Width * Height;

            // Create the pixels array
            Pixels = new Color[Width, Height];

            // Read the channels
            Channels = reader.Read<byte>();

            EnlargeValue = 0;

            if (Settings.EngineVersion == OpenSpaceEngineVersion.Rayman3 && Settings.Game != OpenSpaceGame.Dinosaur && Settings.Game != OpenSpaceGame.LargoWinch)
                EnlargeValue = reader.Read<byte>();

            if (EnlargeValue > 0)
            {
                var width = Width;
                var height = Height;

                channelPixels = 0;

                for (int i = 0; i < EnlargeValue; i++)
                {
                    channelPixels += (width * height);
                    width /= 2;
                    height /= 2;
                }
            }

            RepeatByte = reader.Read<byte>();

            if (Settings.EngineVersion == OpenSpaceEngineVersion.Montreal)
            {
                PaletteNumColors = reader.Read<ushort>();
                PaletteBytesPerColor = reader.Read<byte>();
                var byte1 = reader.Read<byte>();
                var byte2 = reader.Read<byte>();
                var byte3 = reader.Read<byte>();
                var num4 = reader.Read<uint>();
                channelPixels = reader.Read<uint>(); // Hype has mipmaps
                var montrealType = reader.Read<byte>();

                if (PaletteNumColors != 0 && PaletteBytesPerColor != 0)
                    Palette = reader.ReadBytes(PaletteBytesPerColor * PaletteNumColors);

                Format = montrealType switch
                {
                    5 => 0u,
                    10 => 565u,
                    11 => 1555u,
                    12 => 4444u,
                    _ => throw new Exception($"Unknown Montreal GF format {montrealType}")
                };
            }

            // Helper method for extracting bits
            static uint extractBits(int number, int count, int offset) => (uint)(((1 << count) - 1) & (number >> (offset)));

            // Helper method for reading a channel
            byte[] ReadChannel()
            {
                // Read the channel data
                byte[] channel = new byte[channelPixels];

                int pixel = 0;

                while (pixel < channelPixels)
                {
                    byte b1 = reader.Read<byte>();

                    if (b1 == RepeatByte)
                    {
                        byte value = reader.Read<byte>();

                        byte channelCount = reader.Read<byte>();

                        for (int i = 0; i < channelCount; ++i)
                        {
                            if (pixel < channelPixels)
                                channel[pixel] = value;

                            pixel++;
                        }
                    }
                    else
                    {
                        channel[pixel] = b1;
                        pixel++;
                    }
                }

                return channel;
            }

            // Read the channels
            byte[] blueChannel;
            byte[] greenChannel;
            byte[] redChannel;
            byte[] alphaChannel = null;

            if (Channels >= 3)
            {
                // Read RGB channels
                blueChannel = ReadChannel();
                greenChannel = ReadChannel();
                redChannel = ReadChannel();

                // Get if the image is transparent
                IsTransparent = Channels == 4;

                // Read the alpha channel if transparent
                if (IsTransparent)
                    alphaChannel = ReadChannel();
            }
            else if (Channels == 2)
            {
                byte[] channel_1 = ReadChannel();
                byte[] channel_2 = ReadChannel();

                redChannel = new byte[channelPixels];
                greenChannel = new byte[channelPixels];
                blueChannel = new byte[channelPixels];
                alphaChannel = new byte[channelPixels];
                
                if (Format == 1555 || Format == 4444) 
                    IsTransparent = true;
                
                for (int i = 0; i < channelPixels; i++)
                {
                    ushort pixel = BitConverter.ToUInt16(new byte[] { channel_1[i], channel_2[i] }, 0); // RRRRR, GGGGGG, BBBBB (565)
                    
                    uint red;
                    uint green;
                    uint blue;
                    uint alpha;

                    switch (Format)
                    {
                        case 88:
                            alphaChannel[i] = channel_2[i];
                            redChannel[i] = channel_1[i];
                            blueChannel[i] = channel_1[i];
                            greenChannel[i] = channel_1[i];
                            break;

                        case 4444:
                            alpha = extractBits(pixel, 4, 12);
                            red = extractBits(pixel, 4, 8);
                            green = extractBits(pixel, 4, 4);
                            blue = extractBits(pixel, 4, 0);

                            redChannel[i] = (byte)((red / 15.0f) * 255.0f);
                            greenChannel[i] = (byte)((green / 15.0f) * 255.0f);
                            blueChannel[i] = (byte)((blue / 15.0f) * 255.0f);
                            alphaChannel[i] = (byte)((alpha / 15.0f) * 255.0f);
                            break;

                        case 1555:
                            alpha = extractBits(pixel, 1, 15);
                            red = extractBits(pixel, 5, 10);
                            green = extractBits(pixel, 5, 5);
                            blue = extractBits(pixel, 5, 0);

                            redChannel[i] = (byte)((red / 31.0f) * 255.0f);
                            greenChannel[i] = (byte)((green / 31.0f) * 255.0f);
                            blueChannel[i] = (byte)((blue / 31.0f) * 255.0f);
                            alphaChannel[i] = (byte)(alpha * 255);
                            break;

                        case 565:
                        default: // 565
                            red = extractBits(pixel, 5, 11);
                            green = extractBits(pixel, 6, 5);
                            blue = extractBits(pixel, 5, 0);

                            redChannel[i] = (byte)((red / 31.0f) * 255.0f);
                            greenChannel[i] = (byte)((green / 63.0f) * 255.0f);
                            blueChannel[i] = (byte)((blue / 31.0f) * 255.0f);
                            break;
                    }
                }
            }
            else if (Channels == 1)
            {
                byte[] channel_1 = ReadChannel();

                redChannel = new byte[channelPixels];
                greenChannel = new byte[channelPixels];
                blueChannel = new byte[channelPixels];
                
                for (int i = 0; i < channelPixels; i++)
                {
                    if (Palette != null)
                    {
                        redChannel[i] = Palette[channel_1[i] * PaletteBytesPerColor + 2];
                        greenChannel[i] = Palette[channel_1[i] * PaletteBytesPerColor + 1];
                        blueChannel[i] = Palette[channel_1[i] * PaletteBytesPerColor + 0];
                    }
                    else
                    {
                        redChannel[i] = channel_1[i];
                        blueChannel[i] = channel_1[i];
                        greenChannel[i] = channel_1[i];
                    }
                }
            }
            else
            {
                throw new Exception("The number of channels is not valid");
            }

            // Set each pixel
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                if (IsTransparent)
                    Pixels[x, Height - y - 1] = Color.FromArgb(alphaChannel[Width * y + x], redChannel[Width * y + x],
                        greenChannel[Width * y + x], blueChannel[Width * y + x]);
                else
                    Pixels[x, Height - y - 1] = Color.FromArgb(redChannel[Width * y + x], greenChannel[Width * y + x],
                        blueChannel[Width * y + x]);
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            // Write the format or version
            if (Settings.EngineVersion == OpenSpaceEngineVersion.Montreal)
                writer.Write(Version);

            else if (Settings.Platform != OpenSpacePlatform.iOS && Settings.Game != OpenSpaceGame.TonicTroubleSpecialEdition)
                writer.Write(Format);

            // Write the size
            writer.Write(Width);
            writer.Write(Height);

            // Get the pixel count
            var channelPixels = Width * Height;

            // Write the channels
            writer.Write(Channels);

            if (Settings.EngineVersion == OpenSpaceEngineVersion.Rayman3 && Settings.Game != OpenSpaceGame.Dinosaur && Settings.Game != OpenSpaceGame.LargoWinch)
                writer.Write(EnlargeValue);
            
            writer.Write(RepeatByte);

            if (Settings.EngineVersion == OpenSpaceEngineVersion.Montreal)
            {
                // TODO: Implement
                //writer.Write(PaletteNumColors);
                //writer.Write(PaletteBytesPerColor);

                //var byte1 = reader.Read<byte>();
                //var byte2 = reader.Read<byte>();
                //var byte3 = reader.Read<byte>();
                //var num4 = reader.Read<uint>();
                
                //channelPixels = reader.Read<uint>(); // Hype has mipmaps
                
                //var montrealType = reader.Read<byte>();

                //if (paletteNumColors != 0 && paletteBytesPerColor != 0)
                //    palette = reader.ReadBytes(paletteBytesPerColor * paletteNumColors);

                //Format = montrealType switch
                //{
                //    5 => 0u,
                //    10 => 565u,
                //    11 => 1555u,
                //    12 => 4444u,
                //    _ => throw new Exception($"Unknown Montreal GF format {montrealType}")
                //};
            }

            if (Channels >= 3)
            {
                // Helper method for writing a channel
                void WriteChannel(IReadOnlyList<byte> channelData)
                {
                    int pixel = 0;

                    while (pixel < channelPixels)
                    {
                        var pixelData = channelData[pixel];

                        writer.Write(pixelData);

                        // TODO: Change this to take less space - only use repeat byte when same byte is used more than twice & set repeat byte to most common value

                        if (pixelData == RepeatByte)
                        {
                            writer.Write(pixelData);

                            byte count = 1;

                            pixel++;

                            while (pixel < channelPixels)
                            {
                                pixelData = channelData[pixel];

                                if (pixelData != RepeatByte)
                                    break;

                                pixel++;
                                count++;
                            }

                            writer.Write(count);
                        }
                        else
                        {
                            pixel++;
                        }
                    }
                }

                // Get the channels
                byte[] blueChannel = new byte[Pixels.Length];
                byte[] greenChannel = new byte[Pixels.Length];
                byte[] redChannel = new byte[Pixels.Length];
                byte[] alphaChannel = new byte[Pixels.Length];

                // Get RGB channels
                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        // NOTE: We for some reason have to reverse the y-axis    
                        blueChannel[Width * y + x] = Pixels[x, Height - y - 1].B;
                        greenChannel[Width * y + x] = Pixels[x, Height - y - 1].G;
                        redChannel[Width * y + x] = Pixels[x, Height - y - 1].R;

                        if (IsTransparent)
                            alphaChannel[Width * y + x] = Pixels[x, Height - y - 1].A;
                    }
                }

                // Write RGB channels
                WriteChannel(blueChannel);
                WriteChannel(greenChannel);
                WriteChannel(redChannel);

                if (IsTransparent)
                    WriteChannel(alphaChannel);
            }
            else if (Channels == 2)
            {
                // TODO: Implement
                //byte[] channel_1 = ReadChannel();
                //byte[] channel_2 = ReadChannel();

                //redChannel = new byte[channelPixels];
                //greenChannel = new byte[channelPixels];
                //blueChannel = new byte[channelPixels];
                //alphaChannel = new byte[channelPixels];

                //if (Format == 1555 || Format == 4444)
                //    IsTransparent = true;

                //for (int i = 0; i < channelPixels; i++)
                //{
                //    ushort pixel = BitConverter.ToUInt16(new byte[] { channel_1[i], channel_2[i] }, 0); // RRRRR, GGGGGG, BBBBB (565)

                //    uint red;
                //    uint green;
                //    uint blue;
                //    uint alpha;

                //    switch (Format)
                //    {
                //        case 88:
                //            alphaChannel[i] = channel_2[i];
                //            redChannel[i] = channel_1[i];
                //            blueChannel[i] = channel_1[i];
                //            greenChannel[i] = channel_1[i];
                //            break;

                //        case 4444:
                //            alpha = extractBits(pixel, 4, 12);
                //            red = extractBits(pixel, 4, 8);
                //            green = extractBits(pixel, 4, 4);
                //            blue = extractBits(pixel, 4, 0);

                //            redChannel[i] = (byte)((red / 15.0f) * 255.0f);
                //            greenChannel[i] = (byte)((green / 15.0f) * 255.0f);
                //            blueChannel[i] = (byte)((blue / 15.0f) * 255.0f);
                //            alphaChannel[i] = (byte)((alpha / 15.0f) * 255.0f);
                //            break;

                //        case 1555:
                //            alpha = extractBits(pixel, 1, 15);
                //            red = extractBits(pixel, 5, 10);
                //            green = extractBits(pixel, 5, 5);
                //            blue = extractBits(pixel, 5, 0);

                //            redChannel[i] = (byte)((red / 31.0f) * 255.0f);
                //            greenChannel[i] = (byte)((green / 31.0f) * 255.0f);
                //            blueChannel[i] = (byte)((blue / 31.0f) * 255.0f);
                //            alphaChannel[i] = (byte)(alpha * 255);
                //            break;

                //        case 565:
                //        default: // 565
                //            red = extractBits(pixel, 5, 11);
                //            green = extractBits(pixel, 6, 5);
                //            blue = extractBits(pixel, 5, 0);

                //            redChannel[i] = (byte)((red / 31.0f) * 255.0f);
                //            greenChannel[i] = (byte)((green / 63.0f) * 255.0f);
                //            blueChannel[i] = (byte)((blue / 31.0f) * 255.0f);
                //            break;
                //    }
                //}
            }
            else if (Channels == 1)
            {
                // TODO: Implement

                //byte[] channel_1 = ReadChannel();

                //redChannel = new byte[channelPixels];
                //greenChannel = new byte[channelPixels];
                //blueChannel = new byte[channelPixels];
                    
                //for (int i = 0; i < channelPixels; i++)
                //{
                //    if (palette != null)
                //    {
                //        redChannel[i] = palette[channel_1[i] * paletteBytesPerColor + 2];
                //        greenChannel[i] = palette[channel_1[i] * paletteBytesPerColor + 1];
                //        blueChannel[i] = palette[channel_1[i] * paletteBytesPerColor + 0];
                //    }
                //    else
                //    {
                //        redChannel[i] = channel_1[i];
                //        blueChannel[i] = channel_1[i];
                //        greenChannel[i] = channel_1[i];
                //    }
                //}
            }
            else
            {
                throw new Exception("The number of channels is not valid");
            }
        }

        #endregion
    }
}