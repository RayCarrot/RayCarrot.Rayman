using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using RayCarrot.CarrotFramework.Abstractions;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// The data used for a .gf file for OpenSpace games
    /// </summary>
    public class OpenSpaceGFFile : IBinarySerializable<OpenSpaceSettings>
    {
        #region Public Static Properties

        /// <summary>
        /// Gets the default serializer
        /// </summary>
        public static BinaryDataSerializer<OpenSpaceGFFile, OpenSpaceSettings> GetSerializer(OpenSpaceSettings settings) => new BinaryDataSerializer<OpenSpaceGFFile, OpenSpaceSettings>(settings);

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
        /// Indicates if the image is transparent
        /// </summary>
        public bool IsTransparent => GetBitmapChannelCount() == 4;

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
        /// <param name="pixelData">The pixel data to get the color from</param>
        /// <param name="offset">The offset for the specific pixel in the data array</param>
        /// <returns>The color for the pixel in the BGR(A) format</returns>
        protected IEnumerable<byte> GetBitmapColorPixels(byte[] pixelData, long offset)
        {
            if (Channels >= 3)
            {
                // Get the BGR color values
                yield return pixelData[offset + 0];
                yield return pixelData[offset + 1];
                yield return pixelData[offset + 2];

                // If transparent, get the alpha value
                if (Channels == 4)
                    yield return pixelData[offset + 3];
            }
            else if (Channels == 2)
            {
                // Helper method for extracting bits
                static int extractBits(int number, int count, int offset2) => (((1 << count) - 1) & (number >> (offset2)));

                ushort pixel = BitConverter.ToUInt16(new byte[]
                {
                    pixelData[offset],
                    pixelData[offset + 1]
                }, 0); // RRRRR, GGGGGG, BBBBB (565)

                switch (Format)
                {
                    case 88:
                        yield return pixelData[offset];
                        yield return pixelData[offset];
                        yield return pixelData[offset];
                        yield return pixelData[offset + 1];

                        break;

                    case 4444:

                        yield return (byte)(extractBits(pixel, 4, 0) * 17);
                        yield return (byte)(extractBits(pixel, 4, 4) * 17);
                        yield return (byte)(extractBits(pixel, 4, 8) * 17);
                        yield return (byte)(extractBits(pixel, 4, 12) * 17);

                        break;

                    case 1555:
                        const float multiple = (255 / 31f);

                        yield return (byte)(extractBits(pixel, 5, 0) * multiple);
                        yield return (byte)(extractBits(pixel, 5, 5) * multiple);
                        yield return (byte)(extractBits(pixel, 5, 10) * multiple);
                        yield return (byte)(extractBits(pixel, 1, 15) * 255);

                        break;

                    case 565:
                    default: // 565
                        yield return (byte)(extractBits(pixel, 5, 0) * (255 / 31f));
                        yield return (byte)(extractBits(pixel, 6, 5) * (255 / 63f));
                        yield return (byte)(extractBits(pixel, 5, 11) * (255 / 31f));

                        break;
                }
            }
            else if (Channels == 1)
            {
                if (Palette != null)
                {
                    for (int i = 0; i < PaletteBytesPerColor; i++)
                        yield return Palette[pixelData[offset] * PaletteBytesPerColor + i];
                }
                else
                {
                    yield return pixelData[offset];
                    yield return pixelData[offset];
                    yield return pixelData[offset];
                }
            }
            else
            {
                throw new Exception("The number of channels is not valid");
            }
        }

        /// <summary>
        /// Gets the pixel color from the pixel data in the .gf format
        /// </summary>
        /// <param name="bmpPixelData">The bitmap pixel data to get the color from, always 4 bytes</param>
        /// <returns>The color for the pixel in the .gf format</returns>
        protected IEnumerable<byte> GetGfPixels(byte[] bmpPixelData)
        {
            if (Channels >= 3)
            {
                // Get the BGR color values
                yield return bmpPixelData[0];
                yield return bmpPixelData[1];
                yield return bmpPixelData[2];

                // If transparent, get the alpha value
                if (Channels == 4)
                    yield return bmpPixelData[3];
            }
            else if (Channels == 2)
            {
                throw new NotImplementedException("Importing from files with 2 channels is currently not supported");
            }
            else if (Channels == 1)
            {
                if (Palette != null)
                {
                    throw new NotImplementedException("Importing from files with a palette is currently not supported");
                }
                else
                {
                    yield return bmpPixelData[0];
                }
            }
            else
            {
                throw new Exception("The number of channels is not valid");
            }
        }

        /// <summary>
        /// Gets the bitmap channel count for the image, either 3 or 4 channels
        /// </summary>
        /// <returns></returns>
        protected int GetBitmapChannelCount()
        {
            if (Channels >= 3)
                return Channels;

            if (Channels == 2)
            {
                if (Format == 88 || Format == 4444 || Format == 1555)
                    return 4;
                
                else if (Format == 565)
                    return 3;
                
                else
                    throw new Exception("The format is not valid");
            }
            else if (Channels == 1)
            {
                if (Palette == null)
                    return 3;

                if (PaletteBytesPerColor == 4)
                    return 4;
                
                else if (PaletteBytesPerColor == 3)
                    return 3;
              
                else
                    throw new Exception("The number of palette bytes per color is not valid");
            }
            else
            {
                throw new Exception("The number of channels is not valid");
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

            // Get the pixel format
            PixelFormat pixelFormat = GetBitmapChannelCount() == 4 ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb;

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
                foreach (var b in GetBitmapColorPixels(PixelData, pixelOffset))
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

            RCFCore.Logger?.LogDebugSource($"The repeat byte has been updated for a .gf file from {old} to {RepeatByte}");
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
        /// <param name="updateGfTransparencyIfPossible">Indicates if the .gf format should be updated based on if the imported image supports transparency, if possible</param>
        public void ImportFromBitmap(OpenSpaceSettings settings, RawBitmapData bmp, bool generateMipmaps, bool updateGfTransparencyIfPossible)
        {
            // Helper method for writing the pixel data
            void WritePixelData(RawBitmapData bitmapData, long offset)
            {
                // Get the number of bitmap channels
                var bmpChannels = Image.GetPixelFormatSize(bitmapData.PixelFormat) / 8;

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
                    foreach (var b in GetGfPixels(bmpColorData))
                    {
                        PixelData[pixelOffset] = b;
                        pixelOffset++;
                    }
                }
            }

            // Get the number of bitmap channels
            var bitmapChannels = Image.GetPixelFormatSize(bmp.PixelFormat) / 8;

            // Make sure the channel count is 3 or 4
            if (!(bitmapChannels == 3 || bitmapChannels == 4))
                throw new Exception("GF files only support importing from bitmaps with 3 or 4 channels");

            // Check if the format should be updated for transparency
            if (updateGfTransparencyIfPossible && bitmapChannels != GetBitmapChannelCount())
            {
                // Currently only supported for 3 or 4 channels...
                if (Channels >= 3)
                {
                    // Update the channel count
                    Channels = (byte)bitmapChannels;

                    // Update the format
                    Format = Channels == 3 ? 0888u : 8888u;
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
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(IBinaryDataReader<OpenSpaceSettings> reader)
        {
            // Read the format
            if (reader.SerializerSettings.EngineVersion == OpenSpaceEngineVersion.Montreal)
            {
                Version = reader.Read<byte>();
                Format = 1555;
            }
            else if (reader.SerializerSettings.Platform == OpenSpacePlatform.iOS || reader.SerializerSettings.Game == OpenSpaceGame.TonicTroubleSpecialEdition)
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
            if (SupportsMipmaps(reader.SerializerSettings))
                MipmapCount = reader.Read<byte>();

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

            RepeatByte = reader.Read<byte>();

            if (reader.SerializerSettings.EngineVersion == OpenSpaceEngineVersion.Montreal)
            {
                PaletteNumColors = reader.Read<ushort>();
                PaletteBytesPerColor = reader.Read<byte>();

                MontrealByte1 = reader.Read<byte>();
                MontrealByte2 = reader.Read<byte>();
                MontrealByte3 = reader.Read<byte>();
                MontrealNum4 = reader.Read<uint>();

                PixelCount = reader.Read<uint>(); // Hype has mipmaps
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

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(IBinaryDataWriter<OpenSpaceSettings> writer)
        {
            // Write the format or version
            if (writer.SerializerSettings.EngineVersion == OpenSpaceEngineVersion.Montreal)
                writer.Write(Version);
            else if (writer.SerializerSettings.Platform != OpenSpacePlatform.iOS && writer.SerializerSettings.Game != OpenSpaceGame.TonicTroubleSpecialEdition)
                writer.Write(Format);

            // Write the size
            writer.Write(Width);
            writer.Write(Height);

            // Write the channel count
            writer.Write(Channels);

            // Write the number of mipmaps
            if (SupportsMipmaps(writer.SerializerSettings))
                writer.Write(MipmapCount);

            // Write the repeat byte
            writer.Write(RepeatByte);

            if (writer.SerializerSettings.EngineVersion == OpenSpaceEngineVersion.Montreal)
            {
                writer.Write(PaletteNumColors);
                writer.Write(PaletteBytesPerColor);

                writer.Write(MontrealByte1);
                writer.Write(MontrealByte2);
                writer.Write(MontrealByte3);
                writer.Write(MontrealNum4);

                writer.Write(PixelCount);
                writer.Write((byte)(Format switch
                {
                    0 => 5,
                    565 => 10,
                    1555 => 11,
                    4444 => 12,
                    _ => throw new BinarySerializableException($"Unknown Montreal GF format {Format}")
                }));

                if (PaletteNumColors != 0 && PaletteBytesPerColor != 0)
                    writer.Write(Palette);
            }

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
                        writer.Write(RepeatByte);

                        // Write the pixel to repeat
                        writer.Write(repeatValue);

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

                channel++;
            }
        }

        #endregion
    }
}