using System.Drawing;
using System.Drawing.Imaging;

namespace RayCarrot.Rayman
{
    // TODO: Compare with RayMap

    /// <summary>
    /// The data used for a .gf file for Rayman 2
    /// </summary>
    public class R2GFFile : IBinarySerializable
    {
        #region Public Properties

        /// <summary>
        /// The file signature
        /// </summary>
        public int Signature { get; set; }

        /// <summary>
        /// The image width in pixels
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The image height in pixels
        /// </summary>
        public int Height { get; set; }

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
        /// <returns>The bitmap</returns>
        public Bitmap GetBitmapThumbnail(int width, int height)
        {
            // Create the bitmap
            Bitmap bmp = new Bitmap(width, height, IsTransparent ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);

            // Get the scale factors
            var widthScale = Width / (double)width;
            var heightScale = Height / (double)height;

            // Set each pixel
            for (int y = 0; y < width; y++)
            for (int x = 0; x < height; x++)
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
            Bitmap bmp = new Bitmap(Width, Height, IsTransparent ? PixelFormat.Format32bppArgb : PixelFormat.Format24bppRgb);

            // Set each pixel
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                bmp.SetPixel(x, y, Pixels[x, y]);

            // Return the bitmap
            return bmp;
        }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="reader">The reader to use to read from the stream</param>
        public void Deserialize(BinaryDataReader reader)
        {
            // Read the signature
            Signature = reader.Read<int>();

            // Read the size
            Width = reader.Read<int>();
            Height = reader.Read<int>();

            // Get the pixel count
            var count = Width * Height;

            // Create the pixels array
            Pixels = new Color[Width, Height];

            // Read the channels
            int channels = reader.Read<byte>();

            byte repeatByte = reader.Read<byte>();

            // Helper method for reading a channel
            byte[] ReadChannel()
            {
                // Read the channel data
                byte[] channel = new byte[count];

                int pixel = 0;

                while (pixel < count)
                {
                    byte b1 = reader.Read<byte>();

                    if (b1 == repeatByte)
                    {
                        byte b2 = reader.Read<byte>();
                        byte b3 = reader.Read<byte>();

                        for (int i = 0; i < b3; ++i)
                        {
                            channel[pixel] = b2;
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
            byte[] blueChannel = ReadChannel();
            byte[] greenChannel = ReadChannel();
            byte[] redChannel = ReadChannel();
            byte[] alphaChannel = null;

            // Get if the image is transparent
            IsTransparent = channels == 4;

            // Set the alpha channel if transparent
            if (IsTransparent)
                alphaChannel = ReadChannel();

            // Set each pixel
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                Pixels[x, Height - y - 1] = IsTransparent
                    ? Color.FromArgb(alphaChannel[Width * y + x], redChannel[Width * y + x],
                        greenChannel[Width * y + x], blueChannel[Width * y + x])
                    : Color.FromArgb(redChannel[Width * y + x], greenChannel[Width * y + x],
                        blueChannel[Width * y + x]);
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="writer">The writer to use to write to the stream</param>
        public void Serialize(BinaryDataWriter writer)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}