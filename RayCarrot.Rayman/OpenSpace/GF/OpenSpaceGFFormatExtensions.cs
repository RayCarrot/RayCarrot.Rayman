using System;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// Extension methods for <see cref="OpenSpaceGFFormat"/>
    /// </summary>
    public static class OpenSpaceGFFormatExtensions
    {
        /// <summary>
        /// Indicates if the image format supports transparency (the alpha channel)
        /// </summary>
        /// <returns>True if transparency is supported, otherwise false</returns>
        public static bool SupportsTransparency(this OpenSpaceGFFormat format)
        {
            return format switch
            {
                OpenSpaceGFFormat.Format_32bpp_BGRA_8888 => true,
                OpenSpaceGFFormat.Format_24bpp_BGR_888 => false,
                OpenSpaceGFFormat.Format_16bpp_GrayAlpha_88 => true,
                OpenSpaceGFFormat.Format_16bpp_BGRA_4444 => true,
                OpenSpaceGFFormat.Format_16bpp_BGRA_1555 => true,
                OpenSpaceGFFormat.Format_16bpp_BGR_565 => false,
                OpenSpaceGFFormat.Format_8bpp_BGRA_Indexed => true,
                OpenSpaceGFFormat.Format_8bpp_BGR_Indexed => false,
                OpenSpaceGFFormat.Format_8bpp_Gray => false,
                _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
            };
        }
    }
}