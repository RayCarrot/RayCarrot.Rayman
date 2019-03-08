using RayCarrot.CarrotFramework;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Shortcuts for the Carrot Framework
    /// </summary>
    public static class RCFRay
    {
        /// <summary>
        /// Gets the <see cref="IRayManager"/>
        /// </summary>
        public static IRayManager RayManager => RCF.GetService<IRayManager>();
    }
}