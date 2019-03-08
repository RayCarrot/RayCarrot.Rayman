namespace RayCarrot.Rayman
{
    /// <summary>
    /// Formats a Rayman GLI_Mode key
    /// </summary>
    public class RayGLI_Mode
    {
        /// <summary>
        /// True if windowed, false if not
        /// </summary>
        public bool IsWindowed
        {
            get => Windowed == 0;
            set => Windowed = value ? 0 : 1;
        }

        /// <summary>
        /// 0 if windowed, 1 if not
        /// </summary>
        public int Windowed { get; set; }

        /// <summary>
        /// The horizontal resolution
        /// </summary>
        public int ResX { get; set; }

        /// <summary>
        /// The vertical resolution
        /// </summary>
        public int ResY { get; set; }

        /// <summary>
        /// The color mode, either 16 bit or 32 bit
        /// </summary>
        public int ColorMode { get; set; }

        public override string ToString()
        {
            return $"{Windowed} - {ResX} x {ResY} x {ColorMode}";
        }
    }
}