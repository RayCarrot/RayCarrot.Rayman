using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// A bitmap wrapper for locking the pixels for faster accessing
    /// </summary>
    public class BitmapLock : IDisposable
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="bmp">The bitmap</param>
        public BitmapLock(Bitmap bmp)
        {
            // Get the bitmap
            SourceBmp = bmp;

            // Get width and height of bitmap
            Width = SourceBmp.Width;
            Height = SourceBmp.Height;

            // Create rectangle to lock
            Rectangle rect = new Rectangle(0, 0, Width, Height);

            // Get source bitmap pixel format size
            Bpp = Image.GetPixelFormatSize(SourceBmp.PixelFormat);

            // Check if the bits per pixel value is 8, 24, or 32
            if (Bpp != 8 && Bpp != 24 && Bpp != 32)
                throw new ArgumentException("Only 8, 24 and 32 bits per pixel images are supported");

            // Lock bitmap and return bitmap data
            BitmapData = SourceBmp.LockBits(rect, ImageLockMode.ReadWrite, SourceBmp.PixelFormat);

            // Create byte array to copy pixel values
            Pixels = new byte[Width * Height * (Bpp / 8)];

            // Get the pointer address
            Iptr = BitmapData.Scan0;

            // Copy data from pointer to array
            Marshal.Copy(Iptr, Pixels, 0, Pixels.Length);
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The source bitmap
        /// </summary>
        protected Bitmap SourceBmp { get; }
        
        /// <summary>
        /// The pointer address for the pixels
        /// </summary>
        protected IntPtr Iptr { get; }

        /// <summary>
        /// The bitmap data
        /// </summary>
        protected BitmapData BitmapData { get; }

        /// <summary>
        /// The bits per pixel depth
        /// </summary>
        protected int Bpp { get; }

        /// <summary>
        /// The bitmap width
        /// </summary>
        protected int Width { get; }
        
        /// <summary>
        /// The bitmap height
        /// </summary>
        protected int Height { get; }

        #endregion

        #region Public Properties

        /// <summary>
        /// The pixel array
        /// </summary>
        public byte[] Pixels { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the color of the specified pixel
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <returns>The color</returns>
        public Color GetPixel(int x, int y)
        {
            // Get color components count
            int cCount = Bpp / 8;

            // Get start index of the specified pixel
            int i = ((y * Width) + x) * cCount;

            if (i > Pixels.Length - cCount)
                throw new IndexOutOfRangeException();

            if (Bpp == 32)
            {
                byte b = Pixels[i + 0];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];
                byte a = Pixels[i + 3];

                return Color.FromArgb(a, r, g, b);
            }
            else if (Bpp == 24)
            {
                byte b = Pixels[i + 0];
                byte g = Pixels[i + 1];
                byte r = Pixels[i + 2];

                return Color.FromArgb(r, g, b);
            }
            else if (Bpp == 8)
            {
                byte c = Pixels[i];

                return Color.FromArgb(c, c, c);
            }
            else
            {
                throw new InvalidOperationException("Only 8, 24 and 32 bits per pixel images are supported");
            }
        }

        /// <summary>
        /// Sets the color of the specified pixel
        /// </summary>
        /// <param name="x">The x position</param>
        /// <param name="y">The y position</param>
        /// <param name="color">The color</param>
        public void SetPixel(int x, int y, Color color)
        {
            // Get color components count
            int cCount = Bpp / 8;

            // Get start index of the specified pixel
            int i = ((y * Width) + x) * cCount;

            if (Bpp == 32)
            {
                Pixels[i + 0] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
                Pixels[i + 3] = color.A;
            }
            else if (Bpp == 24)
            {
                Pixels[i + 0] = color.B;
                Pixels[i + 1] = color.G;
                Pixels[i + 2] = color.R;
            }
            else if (Bpp == 8)
            {
                Pixels[i] = color.B;
            }
            else
            {
                throw new InvalidOperationException("Only 8, 24 and 32 bits per pixel images are supported");
            }
        }

        /// <summary>
        /// Unlocks the bits of the bitmap
        /// </summary>
        public void Dispose()
        {
            // Copy data from byte array to pointer
            Marshal.Copy(Pixels, 0, Iptr, Pixels.Length);

            // Unlock bitmap data
            SourceBmp.UnlockBits(BitmapData);
        }

        #endregion
    }
}