﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using RayCarrot.Binary;
using RayCarrot.Common;
using RayCarrot.Logging;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// The data used for a .gf file for OpenSpace games
    /// </summary>
    public class OpenSpaceGFFile : IBinarySerializable
    {
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
        /// The pixel count
        /// </summary>
        public uint PixelCount { get; set; }

        /// <summary>
        /// Unknown value (only used for <see cref="OpenSpaceEngineVersion.Montreal"/>)
        /// </summary>
        public byte MontrealByte1 { get; set; }

        /// <summary>
        /// Unknown value (only used for <see cref="OpenSpaceEngineVersion.Montreal"/>)
        /// </summary>
        public byte MontrealByte2 { get; set; }

        /// <summary>
        /// Unknown value (only used for <see cref="OpenSpaceEngineVersion.Montreal"/>)
        /// </summary>
        public byte MontrealByte3 { get; set; }

        /// <summary>
        /// Unknown value (only used for <see cref="OpenSpaceEngineVersion.Montreal"/>)
        /// </summary>
        public uint MontrealNum4 { get; set; }

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
        /// The pixel data, in the .gf format
        /// </summary>
        public byte[] PixelData { get; set; }

        /// <summary>
        /// The current pixel format
        /// </summary>
        public OpenSpaceGFFormat GFPixelFormat
        {
            get => GetFormat();
            set => SetFormat(value);
        }

        #endregion

        #region Protected Methods

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

        /// <summary>
        /// Gets the preferred mipmap count based on the current size
        /// </summary>
        /// <returns>The mipmap count</returns>
        protected byte GetMipmapCount()
        {
            // Get the largest size
            var width = Width;
            var height = Height;

            // Keep track of the count
            byte count = 1;

            // Enumerate each mipmap
            while (!(width == 1 && height == 1))
            {
                // Get the next mipmap size
                if (width != 1)
                    width >>= 1;

                if (height != 1)
                    height >>= 1;

                count++;
            }

            return count;
        }

        /// <summary>
        /// Gets the pixel color from the pixel data in the BGR(A) format
        /// </summary>
        /// <param name="format">The .gf pixel format</param>
        /// <param name="gfPixelData">The pixel data to get the color from</param>
        /// <param name="offset">The offset for the specific pixel in the data array</param>
        /// <returns>The color for the pixel in the BGR(A) format</returns>
        protected IEnumerable<byte> GetGBRAPixel(OpenSpaceGFFormat format, byte[] gfPixelData, long offset)
        {
            switch (format)
            {
                case OpenSpaceGFFormat.Format_32bpp_BGRA_8888:
                    // Get the BGR color values
                    yield return gfPixelData[offset + 0];
                    yield return gfPixelData[offset + 1];
                    yield return gfPixelData[offset + 2];
                    yield return gfPixelData[offset + 3];

                    break;

                case OpenSpaceGFFormat.Format_24bpp_BGR_888:
                    // Get the BGRA color values
                    yield return gfPixelData[offset + 0];
                    yield return gfPixelData[offset + 1];
                    yield return gfPixelData[offset + 2];

                    break;

                case OpenSpaceGFFormat.Format_16bpp_GrayAlpha_88:
                case OpenSpaceGFFormat.Format_16bpp_BGRA_4444:
                case OpenSpaceGFFormat.Format_16bpp_BGRA_1555:
                case OpenSpaceGFFormat.Format_16bpp_BGR_565:

                    // Helper method for extracting bits
                    static int extractBits(int number, int count, int offset2) => (((1 << count) - 1) & (number >> (offset2)));

                    ushort pixel = BitConverter.ToUInt16(new byte[]
                    {
                        gfPixelData[offset],
                        gfPixelData[offset + 1]
                    }, 0); // RRRRR, GGGGGG, BBBBB (565)

                    switch (format)
                    {
                        case OpenSpaceGFFormat.Format_16bpp_GrayAlpha_88:
                            yield return gfPixelData[offset];
                            yield return gfPixelData[offset];
                            yield return gfPixelData[offset];
                            yield return gfPixelData[offset + 1];

                            break;

                        case OpenSpaceGFFormat.Format_16bpp_BGRA_4444:

                            yield return (byte)(extractBits(pixel, 4, 0) * 17);
                            yield return (byte)(extractBits(pixel, 4, 4) * 17);
                            yield return (byte)(extractBits(pixel, 4, 8) * 17);
                            yield return (byte)(extractBits(pixel, 4, 12) * 17);

                            break;

                        case OpenSpaceGFFormat.Format_16bpp_BGRA_1555:
                            const float multiple = (255 / 31f);

                            yield return (byte)(extractBits(pixel, 5, 0) * multiple);
                            yield return (byte)(extractBits(pixel, 5, 5) * multiple);
                            yield return (byte)(extractBits(pixel, 5, 10) * multiple);
                            yield return (byte)(extractBits(pixel, 1, 15) * 255);

                            break;

                        case OpenSpaceGFFormat.Format_16bpp_BGR_565:
                        default: // 565
                            yield return (byte)(extractBits(pixel, 5, 0) * (255 / 31f));
                            yield return (byte)(extractBits(pixel, 6, 5) * (255 / 63f));
                            yield return (byte)(extractBits(pixel, 5, 11) * (255 / 31f));

                            break;
                    }

                    break;

                case OpenSpaceGFFormat.Format_8bpp_BGRA_Indexed:
                case OpenSpaceGFFormat.Format_8bpp_BGR_Indexed:
                    for (int i = 0; i < PaletteBytesPerColor; i++)
                        yield return Palette[gfPixelData[offset] * PaletteBytesPerColor + i];

                    break;

                case OpenSpaceGFFormat.Format_8bpp_Gray:
                    yield return gfPixelData[offset];
                    yield return gfPixelData[offset];
                    yield return gfPixelData[offset];

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        /// <summary>
        /// Gets the pixel color from the pixel data in the .gf format
        /// </summary>
        /// <param name="format">The .gf pixel format</param>
        /// <param name="bgraPixelData">The bitmap pixel data to get the color from, always 4 bytes</param>
        /// <returns>The color for the pixel in the .gf format</returns>
        protected IEnumerable<byte> GetGfPixel(OpenSpaceGFFormat format, byte[] bgraPixelData)
        {
            switch (format)
            {
                case OpenSpaceGFFormat.Format_32bpp_BGRA_8888:
                    // Get the BGR color values
                    yield return bgraPixelData[0];
                    yield return bgraPixelData[1];
                    yield return bgraPixelData[2];
                    yield return bgraPixelData[3];

                    break;

                case OpenSpaceGFFormat.Format_24bpp_BGR_888:
                    // Get the BGRA color values
                    yield return bgraPixelData[0];
                    yield return bgraPixelData[1];
                    yield return bgraPixelData[2];

                    break;

                case OpenSpaceGFFormat.Format_16bpp_GrayAlpha_88:
                case OpenSpaceGFFormat.Format_16bpp_BGRA_4444:
                case OpenSpaceGFFormat.Format_16bpp_BGRA_1555:
                case OpenSpaceGFFormat.Format_16bpp_BGR_565:
                    throw new NotImplementedException("Importing from files with 2 channels is currently not supported");

                case OpenSpaceGFFormat.Format_8bpp_BGRA_Indexed:
                case OpenSpaceGFFormat.Format_8bpp_BGR_Indexed:
                    throw new NotImplementedException("Importing from files with a palette is currently not supported");

                case OpenSpaceGFFormat.Format_8bpp_Gray:
                    yield return bgraPixelData[0];

                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        /// <summary>
        /// Gets the current format
        /// </summary>
        /// <returns>The current format</returns>
        protected OpenSpaceGFFormat GetFormat()
        {
            if (Channels == 4)
            {
                return OpenSpaceGFFormat.Format_32bpp_BGRA_8888;
            }
            else if (Channels >= 3)
            {
                return OpenSpaceGFFormat.Format_24bpp_BGR_888;
            }
            else if (Channels == 2)
            {
                return Format switch
                {
                    88 => OpenSpaceGFFormat.Format_16bpp_GrayAlpha_88,
                    4444 => OpenSpaceGFFormat.Format_16bpp_BGRA_4444,
                    1555 => OpenSpaceGFFormat.Format_16bpp_BGRA_1555,
                    565 => OpenSpaceGFFormat.Format_16bpp_BGR_565,
                    _ => OpenSpaceGFFormat.Format_16bpp_BGR_565
                };
            }
            else if (Channels == 1)
            {
                if (Palette != null)
                {
                    return PaletteBytesPerColor switch
                    {
                        3 => OpenSpaceGFFormat.Format_8bpp_BGR_Indexed,
                        4 => OpenSpaceGFFormat.Format_8bpp_BGRA_Indexed,
                        _ => throw new Exception("The number of palette bytes per color is not valid")
                    };
                }
                else
                {
                    return OpenSpaceGFFormat.Format_8bpp_Gray;
                }
            }
            else
            {
                throw new Exception("The number of channels is not valid");
            }
        }

        /// <summary>
        /// Sets the current format
        /// </summary>
        /// <param name="format">The format to set to</param>
        protected void SetFormat(OpenSpaceGFFormat format)
        {
            switch (format)
            {
                case OpenSpaceGFFormat.Format_32bpp_BGRA_8888:
                    Format = 8888;
                    Channels = 4;
                    break;

                case OpenSpaceGFFormat.Format_24bpp_BGR_888:
                    Format = 888;
                    Channels = 3;
                    break;

                case OpenSpaceGFFormat.Format_16bpp_GrayAlpha_88:
                    Format = 88;
                    Channels = 2;
                    break;

                case OpenSpaceGFFormat.Format_16bpp_BGRA_4444:
                    Format = 4444;
                    Channels = 2;
                    break;

                case OpenSpaceGFFormat.Format_16bpp_BGRA_1555:
                    Format = 1555;
                    Channels = 2;
                    break;

                case OpenSpaceGFFormat.Format_16bpp_BGR_565:
                    Format = 565;
                    Channels = 2;
                    break;

                case OpenSpaceGFFormat.Format_8bpp_BGRA_Indexed:
                    Format = 0;
                    Channels = 1;
                    PaletteBytesPerColor = 4;
                    break;

                case OpenSpaceGFFormat.Format_8bpp_BGR_Indexed:
                    Format = 0;
                    Channels = 1;
                    PaletteBytesPerColor = 3;
                    break;

                case OpenSpaceGFFormat.Format_8bpp_Gray:
                    Format = 8;
                    Channels = 1;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(format), format, null);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Indicates if mipmaps are supported. NOTE: This currently does not count the mipmapping used in Hype.
        /// </summary>
        public bool SupportsMipmaps(OpenSpaceSettings settings) => settings.EngineVersion == OpenSpaceEngineVersion.Rayman3 && settings.Game != OpenSpaceGame.Dinosaur && settings.Game != OpenSpaceGame.LargoWinch;

        /// <summary>
        /// Converts the .gf pixel data to raw bitmap data of a specified size
        /// </summary>
        /// <param name="width">The image width</param>
        /// <param name="height">The image height</param>
        /// <param name="offset">The offset in the pixel array</param>
        /// <returns>The raw image data</returns>
        public RawBitmapData GetRawBitmapData(int width, int height, int offset = 0)
        {
            // Check if the size is scaled
            var isScaled = Width != width || Height != height;

            // Get the scale factors
            var widthScale = Width / (double)width;
            var heightScale = Height / (double)height;

            // Get the format
            var format = GFPixelFormat;

            // Get the pixel format
            PixelFormat pixelFormat = format.SupportsTransparency() ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb;

            // Get the number of bitmap channels
            var bmpChannels = Image.GetPixelFormatSize(pixelFormat) / 8;

            // Create the pixel array
            var rawPixelData = new byte[width * height * bmpChannels];

            // Enumerate each pixel
            for (uint y = 0; y < height; y++)
            for (uint x = 0; x < width; x++)
            {
                // Get the offsets for the pixel colors
                var pixelOffset = isScaled 
                    ? (long)(((width * widthScale) * Math.Floor((y * heightScale)) + Math.Floor((x * widthScale))) * Channels + offset)
                    : (width * y + x) * Channels + offset;

                // NOTE: We reverse the Y-axis here since the .gf images are always flipper vertically
                var rawOffset = (width * (height - y - 1) + x) * bmpChannels;

                // Get the pixels
                foreach (var b in GetGBRAPixel(format, PixelData, pixelOffset))
                {
                    rawPixelData[rawOffset] = b;
                    rawOffset++;
                }
            }

            // Return the raw bitmap data
            return new RawBitmapData(width, height, rawPixelData, pixelFormat);
        }

        /// <summary>
        /// Converts the .gf pixel data to raw bitmap data
        /// </summary>
        /// <param name="offset">The offset in the pixel array</param>
        /// <returns>The raw image data</returns>
        public RawBitmapData GetRawBitmapData(int offset = 0) => GetRawBitmapData((int)Width, (int)Height, offset);

        /// <summary>
        /// Updates the repeat byte to the most appropriate value, based on the main image
        /// </summary>
        public void UpdateRepeatByte()
        {
            // Keep track of the occurrence for each value
            var tempValues = new int[Byte.MaxValue + 1];

            // Enumerate each byte
            foreach (var b in PixelData)
                tempValues[b]++;

            // Get the min value
            var min = tempValues.Min();

            // Save old repeat byte for logging
            var old = RepeatByte;

            // Set the repeat byte to the index with the minimum value
            RepeatByte = (byte)tempValues.FindItemIndex(x => x == min);

            RL.Logger?.LogDebugSource($"The repeat byte has been updated for a .gf file from {old} to {RepeatByte}");
        }

        /// <summary>
        /// Converts the .gf pixel data to raw bitmap data, including for the mipmaps
        /// </summary>
        /// <returns>The raw bitmap data for every image, including the mipmaps</returns>
        public IEnumerable<RawBitmapData> GetRawBitmapDatas()
        {
            int offset = 0;

            // Return the main image
            yield return GetRawBitmapData(offset);

            // Return mipmaps
            foreach (var mipmap in GetMipmapSizes()) 
            {
                // Calculate the size
                var size = mipmap.Width * mipmap.Height * Channels;

                // Make sure the size is valid
                if (size <= 0) 
                    continue;

                // Return the bitmap
                yield return GetRawBitmapData(mipmap.Width, mipmap.Height, offset);

                // Get the offset
                offset += size;
            }
        }

        /// <summary>
        /// Imports a bitmap image into the file, keeping the structure based on the properties and generating mipmaps if needed. This will reset the mipmaps, requiring them to be generated again.
        /// </summary>
        /// <param name="settings">The serializer settings</param>
        /// <param name="bmp">The bitmap data to import from</param>
        /// <param name="generateMipmaps">Indicates if mipmaps should be generated for the image</param>
        public void ImportFromBitmap(OpenSpaceSettings settings, RawBitmapData bmp, bool generateMipmaps)
        {
            // Helper method for writing the pixel data
            void WritePixelData(RawBitmapData bitmapData, long offset)
            {
                // Make sure the pixel format is supported
                if (bitmapData.PixelFormat != PixelFormat.Format32bppArgb && bitmapData.PixelFormat != PixelFormat.Format24bppRgb)
                    throw new Exception($"The bitmap pixel format {bitmapData.PixelFormat} is not supported for importing");

                // Get the number of bitmap channels
                var bmpChannels = Image.GetPixelFormatSize(bitmapData.PixelFormat) / 8;

                // Get the format
                var format = GFPixelFormat;

                byte[] bmpColorData = new byte[4];

                for (uint y = 0; y < bitmapData.Height; y++)
                for (uint x = 0; x < bitmapData.Width; x++)
                {
                    // Get the offsets for the pixel colors
                    var pixelOffset = (bitmapData.Width * y + x) * Channels + offset;

                    // NOTE: We reverse the Y-axis here since the .gf images are always flipper vertically
                    var rawOffset = (bitmapData.Width * (bitmapData.Height - y - 1) + x) * bmpChannels;

                    // Get the bitmap color bytes for this pixel
                    bmpColorData[0] = bitmapData.PixelData[rawOffset + 0];
                    bmpColorData[1] = bitmapData.PixelData[rawOffset + 1];
                    bmpColorData[2] = bitmapData.PixelData[rawOffset + 2];
                    bmpColorData[3] = bmpChannels == 4 ? bitmapData.PixelData[rawOffset + 3] : (byte)255;

                    // Get the pixels
                    foreach (var b in GetGfPixel(format, bmpColorData))
                    {
                        PixelData[pixelOffset] = b;
                        pixelOffset++;
                    }
                }
            }

            // Set size
            Width = (uint)bmp.Width;
            Height = (uint)bmp.Height;

            // Set the pixel count
            PixelCount = Width * Height;

            // Update the mipmap count
            if (generateMipmaps && SupportsMipmaps(settings))
                MipmapCount = GetMipmapCount();
            else
                MipmapCount = 0;

            // Enumerate each mipmap size
            foreach (Size size in GetMipmapSizes())
            {
                // Get the mipmap pixel count
                var count = (uint)(size.Width * size.Height);

                // Add to the total pixel count
                PixelCount += count;
            }

            // Create the data array
            PixelData = new byte[Channels * PixelCount];

            // Set the main pixel data
            WritePixelData(bmp, 0);

            // Keep track of the offset
            long mipmapOffset = Width * Height * Channels;

            // Generate mipmaps if available
            if (RealMipmapCount > 0)
            {
                // Get the bitmap
                using var bitmap = bmp.GetBitmap();

                // Generate every mipmap
                foreach (Size size in GetMipmapSizes())
                {
                    // Resize the bitmap
                    using var resizedBmp = bitmap.ResizeImage(size.Width, size.Height, false);

                    // Write the mipmap
                    WritePixelData(new RawBitmapData(resizedBmp), mipmapOffset);

                    // Increase the index
                    mipmapOffset += size.Height * size.Width * Channels;
                }
            }

            // Update the repeat byte
            UpdateRepeatByte();
        }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            // Get the settings
            var settings = s.GetSettings<OpenSpaceSettings>();

            // Serialize the version
            if (settings.EngineVersion == OpenSpaceEngineVersion.Montreal)
                Version = s.Serialize<byte>(Version, name: nameof(Version));

            // Serialize the format
            else if (settings.Platform != Platform.iOS && settings.Game != OpenSpaceGame.TonicTroubleSpecialEdition)
                Format = s.Serialize<uint>(Format, name: nameof(Format));

            // Serialize the size
            Width = s.Serialize<uint>(Width, name: nameof(Width));
            Height = s.Serialize<uint>(Height, name: nameof(Height));

            // Serialize the channels
            Channels = s.Serialize<byte>(Channels, name: nameof(Channels));

            if (settings.Platform == Platform.iOS || settings.Game == OpenSpaceGame.TonicTroubleSpecialEdition)
                Format = Channels == 4 ? 8888u : 888u;

            // Check if mipmaps are used
            MipmapCount = SupportsMipmaps(settings) ? s.Serialize<byte>(MipmapCount, name: nameof(MipmapCount)) : (byte)0;

            // Only calculate the pixel count if reading
            if (s.IsReading)
            {
                // Set the pixel count
                PixelCount = Width * Height;

                // Enumerate each mipmap size
                foreach (Size size in GetMipmapSizes())
                {
                    // Get the mipmap pixel count
                    var count = (uint)(size.Width * size.Height);

                    // Add to the total pixel count
                    PixelCount += count;
                }
            }

            // Serialize the repeat byte
            RepeatByte = s.Serialize<byte>(RepeatByte, name: nameof(RepeatByte));

            // Serialize Montreal specific values
            if (settings.EngineVersion == OpenSpaceEngineVersion.Montreal)
            {
                PaletteNumColors = s.Serialize<ushort>(PaletteNumColors, name: nameof(PaletteNumColors));
                PaletteBytesPerColor = s.Serialize<byte>(PaletteBytesPerColor, name: nameof(PaletteBytesPerColor));

                MontrealByte1 = s.Serialize<byte>(MontrealByte1, name: nameof(MontrealByte1));
                MontrealByte2 = s.Serialize<byte>(MontrealByte2, name: nameof(MontrealByte2));
                MontrealByte3 = s.Serialize<byte>(MontrealByte3, name: nameof(MontrealByte3));
                MontrealNum4 = s.Serialize<uint>(MontrealNum4, name: nameof(MontrealNum4));

                PixelCount = s.Serialize<uint>(PixelCount, name: nameof(PixelCount)); // Hype has mipmaps

                // Get the current Montreal type based on the format
                var montrealType = (byte)(Format switch
                {
                    0 => 5,
                    565 => 10,
                    1555 => 11,
                    4444 => 12,
                    _ => throw new BinarySerializableException($"Unknown Montreal GF format {Format}")
                });

                // Serialize the Montreal type
                montrealType = s.Serialize<byte>(montrealType, name: nameof(montrealType));

                // Set the format based on the Montreal type
                Format = montrealType switch
                {
                    5 => 0u,
                    10 => 565u,
                    11 => 1555u,
                    12 => 4444u,
                    _ => throw new BinarySerializableException($"Unknown Montreal GF format {montrealType}")
                };

                if (PaletteNumColors != 0 && PaletteBytesPerColor != 0)
                    Palette = s.SerializeArray<byte>(Palette, PaletteBytesPerColor * PaletteNumColors, name: nameof(Palette));
            }

            // Handle the byte data serialization differently depending on if we're reading or writing due to it being compressed
            if (s.IsReading)
            {
                // Create the data array
                PixelData = new byte[Channels * PixelCount];

                // Keep track of the current channel
                int channel = 0;

                // Enumerate each channel
                while (channel < Channels)
                {
                    int pixel = 0;

                    // Enumerate through each pixel
                    while (pixel < PixelCount)
                    {
                        // Read the pixel
                        byte b1 = s.Serialize<byte>(default, name: nameof(b1));

                        // If it's the repeat byte...
                        if (b1 == RepeatByte)
                        {
                            // Get the value to repeat
                            byte value = s.Serialize<byte>(default, name: nameof(value));

                            // Get the number of times to repeat
                            byte count = s.Serialize<byte>(default, name: nameof(count));

                            // Repeat the value the specified number of times
                            for (int i = 0; i < count; ++i)
                            {
                                // Set the value
                                PixelData[channel + pixel * Channels] = value;

                                pixel++;
                            }
                        }
                        else
                        {
                            // Set the value
                            PixelData[channel + pixel * Channels] = b1;
                            pixel++;
                        }
                    }

                    channel++;
                }
            }
            else
            {
                // Keep track of the current channel
                int channel = 0;

                // Enumerate each channel
                while (channel < Channels)
                {
                    int pixelIndex = 0;
                    int pixelDataIndex() => pixelIndex * Channels + channel;

                    // Enumerate through each pixel
                    while (pixelIndex < PixelCount)
                    {
                        // Get the pixel
                        var pixelData = PixelData[pixelDataIndex()];

                        // Check if it equals the next two pixels or the repeat byte
                        if ((pixelDataIndex() + 2 < PixelData.Length && pixelData == PixelData[pixelDataIndex() + 1] && pixelData == PixelData[pixelDataIndex() + 2]) || pixelData == RepeatByte)
                        {
                            // Get the value to repeat
                            var repeatValue = pixelData;

                            // Start repeating by writing the repeat byte
                            s.Serialize<byte>(RepeatByte, name: nameof(RepeatByte));

                            // Write the pixel to repeat
                            s.Serialize<byte>(repeatValue, name: nameof(repeatValue));

                            // Keep track of how many times we repeat
                            byte repeatCount = 0;

                            // Check each value until we break
                            while (pixelIndex < PixelCount)
                            {
                                // Get the data
                                pixelData = PixelData[pixelDataIndex()];

                                // Make sure it's still equal to the value and we haven't reached the maximum value
                                if (pixelData != repeatValue || repeatCount >= Byte.MaxValue)
                                    break;

                                // Increment the index and count
                                pixelIndex++;
                                repeatCount++;
                            }

                            // Write the repeat count
                            s.Serialize<byte>(repeatCount, name: nameof(repeatCount));
                        }
                        else
                        {
                            // Write the pixel
                            s.Serialize<byte>(pixelData, name: nameof(pixelData));

                            // Increment the index
                            pixelIndex++;
                        }
                    }

                    channel++;
                }
            }
        }

        #endregion
    }
}