using FastBitmapLib;
using RayCarrot.CarrotFramework.Abstractions;
using RayCarrot.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;

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
        /// The number of available mipmaps, including the main image (only used for <see cref="OpenSpaceEngineVersion.Rayman3"/> for certain games)
        /// </summary>
        public byte MipmapCount { get; set; }

        /// <summary>
        /// The actual number of available mipmaps, not counting the main image
        /// </summary>
        public int RealMipmapCount => MipmapCount == 0 ? 0 : MipmapCount - 1;

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

        /// <summary>
        /// The color for each pixel of each available mipmap, not including the main image
        /// </summary>
        public Color[][,] MipmapPixels { get; set; }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets all pixel arrays, including the mipmaps
        /// </summary>
        /// <returns>The pixel arrays</returns>
        protected IEnumerable<Color[,]> GetAllPixels()
        {
            // Return main image pixels
            yield return Pixels;

            if (MipmapPixels != null)
            {
                // Return mipmap pixels
                foreach (var m in MipmapPixels)
                    yield return m;
            }
        }

        /// <summary>
        /// Gets the channels for the current colors. This returns either 3 or 4 channels depending on if the image is transparent.
        /// </summary>
        /// <returns>The channels for blue, green, red and alpha (if transparent)</returns>
        protected byte[][] GetBGRAChannels()
        {
            return GetBGRAChannels(Pixels, IsTransparent);
        }

        /// <summary>
        /// Gets the sizes for all mipmap images, starting from the largest
        /// </summary>
        /// <returns>The sizes of all images</returns>
        protected IEnumerable<Size> GetMipmapSizes()
        {
            // Get the largest size
            var width = Width;
            var height = Height;

            // Enumerate each mipmap
            for (int i = 0; i < RealMipmapCount; i++)
            {
                // Get the next mipmap size
                if (width != 1) 
                    width >>= 1;

                if (height != 1) 
                    height >>= 1;

                // Return the current size
                yield return new Size((int)width, (int)height);
            }
        }

        #endregion

        #region Protected Static Methods

        /// <summary>
        /// Gets the channels for the current colors. This returns either 3 or 4 channels depending on if the image is transparent.
        /// </summary>
        /// <param name="pixels">The pixels to get the channels from</param>
        /// <param name="isTransparent">Indicates if the image is transparent and the alpha channel should be returned</param>
        /// <returns>The channels for blue, green, red and alpha (if transparent)</returns>
        protected static byte[][] GetBGRAChannels(Color[,] pixels, bool isTransparent)
        {
            // Get the size
            int width = pixels.GetLength(0);
            int height = pixels.GetLength(1);

            // Create the output array
            var output = new byte[isTransparent ? 4 : 3][];

            // Create sub-arrays
            for (int i = 0; i < output.Length; i++)
                output[i] = new byte[width * height];

            // Get channels
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    // The index of the array to add to
                    var index = 0;

                    // Helper method for adding a value to the array
                    void AddValue(byte value)
                    {
                        output[index][width * y + x] = value;
                        index++;
                    }

                    // Add RGB values
                    AddValue(pixels[x, height - y - 1].B);
                    AddValue(pixels[x, height - y - 1].G);
                    AddValue(pixels[x, height - y - 1].R);

                    // Only add the alpha channel if transparent
                    if (isTransparent)
                        AddValue(pixels[x, height - y - 1].A);
                }
            }

            return output;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Reads the available channels
        /// </summary>
        /// <param name="reader">The reader</param>
        /// <param name="pixelCount">The pixel count for each channel</param>
        /// <returns>The channels data</returns>
        protected byte[] ReadChannels(BinaryDataReader reader, uint pixelCount)
        {
            // Create the data array
            byte[] data = new byte[Channels * pixelCount];

            // Keep track of the current channel
            int channel = 0;

            // Enumerate each channel
            while (channel < Channels)
            {
                int pixel = 0;

                // Enumerate through each pixel
                while (pixel < pixelCount)
                {
                    // Read the pixel
                    byte b1 = reader.Read<byte>();

                    // If it's the repeat byte...
                    if (b1 == RepeatByte)
                    {
                        // Get the value to repeat
                        byte value = reader.Read<byte>();

                        // Get the number of times to repeat
                        byte count = reader.Read<byte>();

                        // Repeat the value the specified number of times
                        for (int i = 0; i < count; ++i)
                        {
                            // Set the value
                            data[channel + pixel * Channels] = value;

                            pixel++;
                        }
                    }
                    else
                    {
                        // Set the value
                        data[channel + pixel * Channels] = b1;
                        pixel++;
                    }
                }

                channel++;
            }

            return data;
        }

        /// <summary>
        /// Gets the pixel color from the pixel data
        /// </summary>
        /// <param name="pixelData">The pixel data to get the color from</param>
        /// <param name="offset">The offset for the specific pixel in the data array</param>
        /// <returns>The color for the pixel</returns>
        protected Color GetColor(byte[] pixelData, int offset)
        {
            if (Channels >= 3)
            {
                // Get the BGR color values
                byte b = pixelData[offset + 0];
                byte g = pixelData[offset + 1];
                byte r = pixelData[offset + 2];

                // If transparent, get the alpha value
                if (IsTransparent)
                {
                    byte a = pixelData[offset + 3];

                    return Color.FromArgb(a, r, g, b);
                }
                else
                {
                    return Color.FromArgb(r, g, b);
                }
            }
            else if (Channels == 2)
            {
                // Helper method for extracting bits
                static int extractBits(int number, int count, int offset) => (((1 << count) - 1) & (number >> (offset)));

                ushort pixel = BitConverter.ToUInt16(new byte[]
                {
                    pixelData[offset],
                    pixelData[offset + 1]
                }, 0); // RRRRR, GGGGGG, BBBBB (565)

                int r, g, b, a;

                switch (Format)
                {
                    case 88:
                        a = pixelData[offset + 1];
                        r = pixelData[offset];
                        g = pixelData[offset];
                        b = pixelData[offset];

                        break;
                    case 4444:

                        a = extractBits(pixel, 4, 12) * 17;
                        r = extractBits(pixel, 4, 8) * 17;
                        g = extractBits(pixel, 4, 4) * 17;
                        b = extractBits(pixel, 4, 0) * 17;

                        break;
                    case 1555:
                        const float multiple = (255 / 31f);

                        a = extractBits(pixel, 1, 15) * 255;
                        r = (int)(extractBits(pixel, 5, 10) * multiple);
                        g = (int)(extractBits(pixel, 5, 5) * multiple);
                        b = (int)(extractBits(pixel, 5, 0) * multiple);

                        break;
                    case 565:
                    default: // 565
                        a = 255;
                        r = (int)(extractBits(pixel, 5, 11) * (255 / 31f));
                        g = (int)(extractBits(pixel, 6, 5) * (255 / 63f));
                        b = (int)(extractBits(pixel, 5, 0) * (255 / 31f));

                        break;
                }

                return Color.FromArgb(a, r, g, b);
            }
            else if (Channels == 1)
            {
                // Get the BGR color values
                byte r, g, b;

                byte a = 255;

                if (Palette != null)
                {
                    if (IsTransparent)
                        a = Palette[pixelData[offset] * PaletteBytesPerColor + 3];

                    r = Palette[pixelData[offset] * PaletteBytesPerColor + 2];
                    g = Palette[pixelData[offset] * PaletteBytesPerColor + 1];
                    b = Palette[pixelData[offset] * PaletteBytesPerColor + 0];
                }
                else
                {
                    if (IsTransparent)
                        a = pixelData[offset];
                    r = pixelData[offset];
                    g = pixelData[offset];
                    b = pixelData[offset];
                }

                return Color.FromArgb(a, r, g, b);
            }
            else
            {
                throw new BinarySerializableException("The number of channels is not valid");
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the repeat byte to the most appropriate value, based on the main image
        /// </summary>
        public void UpdateRepeatByte()
        {
            if (Channels < 3)
                throw new NotImplementedException("The repeat byte can currently only be updated when 3 or 4 channels are present");

            // Keep track of the occurrence for each value
            var tempValues = new int[Byte.MaxValue + 1];

            // Enumerate each channel
            foreach (var channel in GetBGRAChannels())
            {
                // Enumerate each byte in the channel
                foreach (var b in channel)
                {
                    tempValues[b]++;
                }
            }

            // Get the min value
            var min = tempValues.Min();

            // Save old repeat byte for logging
            var old = RepeatByte;

            // Set the repeat byte to the index with the minimum value
            RepeatByte = (byte)tempValues.FindItemIndex(x => x == min);

            RCFCore.Logger?.LogDebugSource($"The repeat byte has been updated for a .gf file from {old} to {RepeatByte}");
        }

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

            // IDEA: Make a pull request for 24bpp images - should be the same but with 3 bytes instead of 4
            // Fast lock currently only supports 32bpp images
            if (IsTransparent)
            {
                // Lock the bitmap for faster reading/writing
                using var lockedBmp = bmp.FastLock();

                // Set each pixel
                for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    lockedBmp.SetPixel(x, y, Pixels[(int)(x * widthScale), (int)(y * heightScale)]);
            }
            else
            {
                // Set each pixel
                for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    bmp.SetPixel(x, y, Pixels[(int)(x * widthScale), (int)(y * heightScale)]);
            }

            // Return the bitmap
            return bmp;
        }

        /// <summary>
        /// Gets a bitmap from the image
        /// </summary>
        /// <returns>The bitmap</returns>
        public Bitmap GetBitmap()
        {
            // Return a bitmap from the full image
            return GetBitmap(Pixels, IsTransparent);
        }

        /// <summary>
        /// Gets all available bitmaps from the image, including mipmaps
        /// </summary>
        /// <param name="includeMain">Indicates if the main image should be included</param>
        /// <returns>The available bitmaps</returns>
        public IEnumerable<Bitmap> GetBitmaps(bool includeMain)
        {
            if (includeMain)
                // Return main image
                yield return GetBitmap(Pixels, IsTransparent);

            // Return mipmaps
            foreach (var bmp in MipmapPixels.
                // Make sure there are any pixels
                Where(x => x.Any()).
                // Convert to bitmap
                Select(m => GetBitmap(m, IsTransparent))) 
                // Return the bitmap
                yield return bmp;
        }

        /// <summary>
        /// Gets a bitmap from the image
        /// </summary>
        /// <param name="pixels">The pixels to use in the bitmap</param>
        /// <param name="isTransparent">Indicates if the image is transparent and the alpha channel should be used</param>
        /// <returns>The bitmap</returns>
        public static Bitmap GetBitmap(Color[,] pixels, bool isTransparent)
        {
            // Get the dimensions
            var width = pixels.GetLength(0);
            var height = pixels.GetLength(1);

            // Create the bitmap
            Bitmap bmp = new Bitmap(width, height, isTransparent ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);

            // IDEA: Make a pull request for 24bpp images - should be the same but with 3 bytes instead of 4
            // Fast lock currently only supports 32bpp images
            if (isTransparent)
            {
                // Lock the bitmap for faster reading/writing
                using var lockedBmp = bmp.FastLock();

                // Set each pixel
                for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    lockedBmp.SetPixel(x, y, pixels[x, y]);
            }
            else
            {
                // Set each pixel
                for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    bmp.SetPixel(x, y, pixels[x, y]);
            }

            // Return the bitmap
            return bmp;
        }

        /// <summary>
        /// Imports a bitmap image into the file, keeping the structure based on the properties and generating mipmaps if needed
        /// </summary>
        /// <param name="bmp">The bitmap to import</param>
        public void ImportFromBitmap(Bitmap bmp)
        {
            // Set size
            Width = (uint)bmp.Width;
            Height = (uint)bmp.Height;

            // Create the pixels array for the main image
            Pixels = new Color[Width, Height];

            // Create the pixels arrays for the mipmaps
            MipmapPixels = new Color[RealMipmapCount][,];

            int mipmapIndex = 0;

            // Set the mipmap sizes
            foreach (var size in GetMipmapSizes())
            {
                MipmapPixels[mipmapIndex] = new Color[size.Width, size.Height];

                mipmapIndex++;
            }

            // TODO: Allow this to be configured
            // Indicated if changing the transparency is allowed
            bool allowChangedTransparency = false;

            RCFCore.Logger?.LogDebugSource($"A bitmap is being imported to a .gf file with the size '{Width} x {Height}'");

            // Helper method for setting the pixels from a bitmap, returning a value indicating if the image is transparent
            static bool SetPixels(Color[,] pixelArray, int width, int height, Bitmap bitmap)
            {
                // Keep track of if the image is transparent
                var isBitmapTransparent = false;

                // Set each pixel for the main image
                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        // Get the pixel data
                        var pixel = bitmap.GetPixel(x, y);

                        // Set the pixel
                        pixelArray[x, y] = pixel;

                        // Check if it's transparent
                        if (pixel.A != Byte.MaxValue)
                            isBitmapTransparent = true;
                    }
                }

                return isBitmapTransparent;
            }

            // Set the pixels for the main image
            var isTransparent = SetPixels(Pixels, (int)Width, (int)Height, bmp);

            RCFCore.Logger?.LogDebugSource($"The bitmap image {(isTransparent ? "is" : "is not")} transparent");

            // Check if the transparency value should be updated
            if (allowChangedTransparency && IsTransparent != isTransparent)
            {
                RCFCore.Logger?.LogDebugSource($"The bitmap image transparency has changed");

                // TODO: Format needs to be updated too

                // Update channels if they are 3 or more
                if (Channels == 3)
                    Channels = 4;
                else if (Channels == 4)
                    Channels = 3;

                // Update transparency
                IsTransparent = isTransparent;
            }

            // Update the repeat byte
            UpdateRepeatByte();

            // Set mipmaps if available
            if (RealMipmapCount > 0)
            {
                mipmapIndex = 0;

                // Set each mipmap
                foreach (Size size in GetMipmapSizes())
                {
                    // Ignore if one value is 0
                    if (size.Width == 0 || size.Height == 0)
                        break;

                    // Resize the bitmap
                    using var resizedBmp = bmp.ResizeImage(size.Width, size.Height, false);

                    // Add the pixels from the resized image
                    SetPixels(MipmapPixels[mipmapIndex], size.Width, size.Height, resizedBmp);

                    mipmapIndex++;
                }
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

            // Read the channels
            Channels = reader.Read<byte>();

            // Default the mipmap count to 0
            MipmapCount = 0;

            // Check if mipmaps are used
            if (Settings.EngineVersion == OpenSpaceEngineVersion.Rayman3 && Settings.Game != OpenSpaceGame.Dinosaur && Settings.Game != OpenSpaceGame.LargoWinch)
                MipmapCount = reader.Read<byte>();

            // Create the pixels array for the main image
            Pixels = new Color[Width, Height];

            // Create the pixels arrays for the mipmaps
            MipmapPixels = new Color[RealMipmapCount][,];

            // Set the pixel count
            uint pixelCount = Width * Height;

            int mipmapIndex = 0;

            // Enumerate each mipmap size
            foreach (Size size in GetMipmapSizes())
            {
                // Get the mipmap pixel count
                var count = (uint)(size.Width * size.Height);

                // Create the pixel array
                MipmapPixels[mipmapIndex] = new Color[size.Width, size.Height];

                // Add to the total pixel count
                pixelCount += count;

                mipmapIndex++;
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

                pixelCount = reader.Read<uint>(); // Hype has mipmaps
                var montrealType = reader.Read<byte>();

                if (PaletteNumColors != 0 && PaletteBytesPerColor != 0)
                    Palette = reader.ReadBytes(PaletteBytesPerColor * PaletteNumColors);

                Format = montrealType switch
                {
                    5 => 0u,
                    10 => 565u,
                    11 => 1555u,
                    12 => 4444u,
                    _ => throw new BinarySerializableException($"Unknown Montreal GF format {montrealType}")
                };
            }

            // Read the channels
            var pixelData = ReadChannels(reader, pixelCount);

            // Check if the texture is transparent
            if (Channels >= 3)
            {
                IsTransparent = Channels == 4;
            }
            else if (Channels == 2)
            {
                if (Format == 1555 || Format == 4444) 
                    IsTransparent = true;
            }
            else if (Channels == 1)
            {
                if (Palette != null && PaletteBytesPerColor == 4) 
                    IsTransparent = true;
            }

            // Keep track of the channel offset
            int channelOffset = 0;

            // Set each pixel, including for each mipmap
            foreach (var mipmapArray in GetAllPixels())
            {
                // If we are not deserializing mipmaps, break once an offset has been set
                if (!Settings.DeserializeMipmaps && channelOffset > 0)
                    break;

                // Get the sizes
                var width = mipmapArray.GetLength(0);
                var height = mipmapArray.GetLength(1);

                // Enumerate each pixel
                for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    // Get the offset for the pixel colors
                    var offset = (width * y + x) * Channels + channelOffset;

                    // Set the pixel color
                    mipmapArray[x, height - y - 1] = GetColor(pixelData, offset);
                }
                
                channelOffset += mipmapArray.Length * Channels;
            }
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            // Make sure the file was not deserialized without mipmaps if needing them
            if (MipmapPixels.Length != (RealMipmapCount))
                throw new BinarySerializableException("The file can not be serialized due to the mipmap count not matching the available mipmaps");

            // Write the format or version
            if (Settings.EngineVersion == OpenSpaceEngineVersion.Montreal)
                writer.Write(Version);
            else if (Settings.Platform != OpenSpacePlatform.iOS && Settings.Game != OpenSpaceGame.TonicTroubleSpecialEdition)
                writer.Write(Format);

            // Write the size
            writer.Write(Width);
            writer.Write(Height);

            // Write the channel count
            writer.Write(Channels);

            // Write the number of mipmaps
            if (Settings.EngineVersion == OpenSpaceEngineVersion.Rayman3 && Settings.Game != OpenSpaceGame.Dinosaur && Settings.Game != OpenSpaceGame.LargoWinch)
                writer.Write(MipmapCount);

            // Write the repeat byte
            writer.Write(RepeatByte);

            if (Settings.EngineVersion == OpenSpaceEngineVersion.Montreal)
            {
                throw new NotImplementedException("Serializing Ubisoft Montreal .gf files is currently not supported");
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
                //    _ => throw new BinarySerializableException($"Unknown Montreal GF format {montrealType}")
                //};
            }

            // TODO: Rewrite this to follow how the deserializer handles it
            if (Channels >= 3)
            {
                // Helper method for writing a channel
                void WriteChannel(IReadOnlyList<byte> channelData)
                {
                    int pixelIndex = 0;

                    // Enumerate each pixel
                    while (pixelIndex < channelData.Count)
                    {
                        // Get the pixel
                        var pixelData = channelData[pixelIndex];

                        // Check if it equals the next two pixels or the repeat byte
                        if ((pixelIndex + 2 < channelData.Count && pixelData == channelData[pixelIndex + 1] && pixelData == channelData[pixelIndex + 2]) || pixelData == RepeatByte)
                        {
                            // Get the value to repeat
                            var repeatValue = pixelData;

                            // Start repeating by writing the repeat byte
                            writer.Write(RepeatByte);

                            // Write the pixel to repeat
                            writer.Write(repeatValue);

                            // Keep track of how many times we repeat
                            byte repeatCount = 0;

                            // Check each value until we break
                            while (pixelIndex < channelData.Count)
                            {
                                // Get the data
                                pixelData = channelData[pixelIndex];

                                // Make sure it's still equal to the value and we haven't reached the maximum value
                                if (pixelData != repeatValue || repeatCount >= Byte.MaxValue)
                                    break;

                                // Increment the index and count
                                pixelIndex++;
                                repeatCount++;
                            }

                            // Write the repeat count
                            writer.Write(repeatCount);
                        }
                        else
                        {
                            // Write the pixel
                            writer.Write(pixelData);

                            // Increment the index
                            pixelIndex++;
                        }
                    }
                }

                // Create a list of each channel
                var channels = new List<byte>[IsTransparent ? 4 : 3];

                // Create each channel list
                for (int i = 0; i < channels.Length; i++)
                    channels[i] = new List<byte>();

                // Get the channels for every mipmap and add them
                foreach (var c in GetAllPixels().Select(x => GetBGRAChannels(x, IsTransparent)))
                {
                    int index = 0;

                    // Add every channel from this mipmap
                    foreach (var channel in c)
                    {
                        // Add the channel data
                        channels[index].AddRange(channel);

                        index++;
                    }
                }

                // Write the channels
                foreach (var c in channels)
                    WriteChannel(c);
            }
            else if (Channels == 2)
            {
                throw new NotImplementedException("Serializing .gf files with 2 channels is currently not supported");
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
                throw new NotImplementedException("Serializing .gf files with 1 channel is currently not supported");

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
                throw new BinarySerializableException("The number of channels is not valid");
            }
        }

        #endregion
    }
}