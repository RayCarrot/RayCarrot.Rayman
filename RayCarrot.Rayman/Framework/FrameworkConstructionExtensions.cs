using RayCarrot.CarrotFramework;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Extension methods for <see cref="FrameworkConstruction"/>
    /// </summary>
    public static class FrameworkConstructionExtensions
    {
        /// <summary>
        /// Adds all of the Rayman related default services
        /// </summary>
        /// <returns>The construction</returns>
        public static FrameworkConstruction AddRaymanDefaults(this FrameworkConstruction construction)
        {
            construction.AddTransient<IRayManager, DefaultRayManager>();

            return construction;
        }
    }
}