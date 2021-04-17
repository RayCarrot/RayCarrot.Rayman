using RayCarrot.Binary;
using System;

namespace RayCarrot.Rayman.UbiArt
{
    /// <summary>
    /// UbiArt Xbox 360 texture data
    /// </summary>
    public class UbiArtXbox360Texture : IBinarySerializable
    {
        #region Public Properties

        public byte[] Header_0 { get; set; }
        public TextureCompressionType CompressionType { get; set; }
        public int Dimensions { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public byte[] Header_1 { get; set; }
        public byte[] ImgData { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Header_0 = s.SerializeArray<byte>(Header_0, 35, name: nameof(Header_0));
            CompressionType = s.Serialize<TextureCompressionType>(CompressionType, name: nameof(CompressionType));
            Dimensions = s.Serialize<int>(Dimensions, name: nameof(Dimensions));
            Width = BitHelpers.ExtractBits(Dimensions, 13, 0) + 1;
            Height = BitHelpers.ExtractBits(Dimensions, 13, 13) + 1;
            Header_1 = s.SerializeArray<byte>(Header_1, 12, name: nameof(Header_1));
            ImgData = s.SerializeArray<byte>(ImgData, (int)(s.Stream.Length - s.Stream.Position), name: nameof(ImgData));
        }

        public byte[] Untile(bool swapBytes)
        {
            var imgData = swapBytes ? new byte[ImgData.Length] : ImgData;

            if (swapBytes)
            {
                for (int i = 0; i < ImgData.Length / 2; i++)
                {
                    imgData[i * 2] = ImgData[i * 2 + 1];
                    imgData[i * 2 + 1] = ImgData[i * 2];
                }
            }

            return CompressionType switch
            {
                TextureCompressionType.DXT1 => UntileTexture(imgData, Width, Height, 8, 128, 128, 4, 4),
                TextureCompressionType.DXT3 => UntileTexture(imgData, Width, Height, 16, 128, 128, 4, 4),
                TextureCompressionType.DXT5 => UntileTexture(imgData, Width, Height, 16, 128, 128, 4, 4),
                _ => throw new ArgumentOutOfRangeException(nameof(CompressionType), CompressionType, null)
            };
        }

        #endregion

        #region Enums

        public enum TextureCompressionType : byte
        {
            DXT1 = 0x52,
            DXT3 = 0x53,
            DXT5 = 0x54
        }

        #endregion

        #region Private Static Methods

        // Based on https://github.com/gildor2/UEViewer/blob/eaba2837228f9fe39134616d7bff734acd314ffb/Unreal/UnrealMaterial/UnTexture.cpp#L562
        private static byte[] UntileTexture(byte[] srcData, int originalWidth, int originalHeight, int bytesPerBlock, int alignX, int alignY, int blockSizeX, int blockSizeY)
        {
            var dstData = new byte[srcData.Length];

            int alignedWidth = Align(originalWidth, alignX);
            int alignedHeight = Align(originalHeight, alignY);

            int tiledBlockWidth = alignedWidth / blockSizeX;       // width of image in blocks
            int originalBlockWidth = originalWidth / blockSizeX;   // width of image in blocks
            int tiledBlockHeight = alignedHeight / blockSizeY;     // height of image in blocks
            int originalBlockHeight = originalHeight / blockSizeY; // height of image in blocks
            int logBpp = AppLog2(bytesPerBlock);

            // XBox360 has packed multiple lower mip levels into a single tile - should use special code
            // to unpack it. Textures are aligned to bottom-right corder.
            // Packing looks like this:
            // ....CCCCBBBBBBBBAAAAAAAAAAAAAAAA
            // ....CCCCBBBBBBBBAAAAAAAAAAAAAAAA
            // E.......BBBBBBBBAAAAAAAAAAAAAAAA
            // ........BBBBBBBBAAAAAAAAAAAAAAAA
            // DD..............AAAAAAAAAAAAAAAA
            // ................AAAAAAAAAAAAAAAA
            // ................AAAAAAAAAAAAAAAA
            // ................AAAAAAAAAAAAAAAA
            // (Where mips are A,B,C,D,E - E is 1x1, D is 2x2 etc)
            // Force sxOffset=0 and enable DEBUG_MIPS in UnRender.cpp to visualize this layout.
            // So we should offset X coordinate when unpacking to the width of mip level.
            // Note: this doesn't work with non-square textures.
            var sxOffset = 0;
            var syOffset = 0;

            // We're handling only size=16 here.
            if (tiledBlockWidth >= originalBlockWidth * 2 && originalWidth == 16)
                sxOffset = originalBlockWidth;

            if (tiledBlockHeight >= originalBlockHeight * 2 && originalHeight == 16)
                syOffset = originalBlockHeight;

            int numImageBlocks = tiledBlockWidth * tiledBlockHeight;    // used for verification

            // Iterate over image blocks
            for (int dy = 0; dy < originalBlockHeight; dy++)
            {
                for (int dx = 0; dx < originalBlockWidth; dx++)
                {
                    // Unswizzle only once for a whole block
                    uint swzAddr = GetTiledOffset(dx + sxOffset, dy + syOffset, tiledBlockWidth, logBpp);

                    if (swzAddr >= numImageBlocks) 
                        throw new Exception("Error in Xbox 360 texture parsing");

                    int sy = (int)(swzAddr / tiledBlockWidth);
                    int sx = (int)(swzAddr % tiledBlockWidth);

                    int dstStart = (dy * originalBlockWidth + dx) * bytesPerBlock;
                    int srcStart = (sy * tiledBlockWidth + sx) * bytesPerBlock;
                    Array.Copy(srcData, srcStart, dstData, dstStart, bytesPerBlock);
                }
            }

            return dstData;
        }

        private static uint GetTiledOffset(int x, int y, int width, int logBpb)
        {
            if (width > 8192)
                throw new Exception($"Xbox 360 texture: Width {width} too large");

            if (width <= x)
                throw new Exception($"Xbox 360 texture: X {x} too large for width {width}");

            int alignedWidth = Align(width, 32);
            // top bits of coordinates
            int macro = ((x >> 5) + (y >> 5) * (alignedWidth >> 5)) << (logBpb + 7);
            // lower bits of coordinates (result is 6-bit value)
            int micro = ((x & 7) + ((y & 0xE) << 2)) << logBpb;
            // mix micro/macro + add few remaining x/y bits
            int offset = macro + ((micro & ~0xF) << 1) + (micro & 0xF) + ((y & 1) << 4);
            // mix bits again
            return (uint)((((offset & ~0x1FF) << 3) +            // upper bits (offset bits [*-9])
                           ((y & 16) << 7) +                           // next 1 bit
                           ((offset & 0x1C0) << 2) +                   // next 3 bits (offset bits [8-6])
                           (((((y & 8) >> 2) + (x >> 3)) & 3) << 6) +  // next 2 bits
                           (offset & 0x3F)                             // lower 6 bits (offset bits [5-0])
                ) >> logBpb);
        }

        private static int Align(int value, int align) => (value % align != 0) ? ((value / align) + 1) * (align) : value;

        private static int AppLog2(int n)
        {
            int r;
            for (r = -1; n != 0; n >>= 1, r++) { /*empty*/ }
            return r;
        }

        #endregion
    }
}