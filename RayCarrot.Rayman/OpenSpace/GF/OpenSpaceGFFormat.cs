namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// The available .gf image formats
    /// </summary>
    public enum OpenSpaceGFFormat
    {
        /// <summary>
        /// 32 bpp (8888), BGRA
        /// </summary>
        Format_32bpp_BGRA_8888,

        /// <summary>
        /// 24 bpp (888), BGR
        /// </summary>
        Format_24bpp_BGR_888,

        /// <summary>
        /// 16 bpp (88), gray-scale with alpha channel
        /// </summary>
        Format_16bpp_GrayAlpha_88,

        /// <summary>
        /// 16 bpp (4444), BGRA
        /// </summary>
        Format_16bpp_BGRA_4444,

        /// <summary>
        /// 16 bpp (1555), BGRA
        /// </summary>
        Format_16bpp_BGRA_1555,

        /// <summary>
        /// 16 bpp (565), BGR
        /// </summary>
        Format_16bpp_BGR_565,

        /// <summary>
        /// 8 bpp, BGRA
        /// </summary>
        Format_8bpp_BGRA_Indexed,

        /// <summary>
        /// 8 bpp, BGR
        /// </summary>
        Format_8bpp_BGR_Indexed,

        /// <summary>
        /// 8 bpp, gray-scale
        /// </summary>
        Format_8bpp_Gray,
    }
}