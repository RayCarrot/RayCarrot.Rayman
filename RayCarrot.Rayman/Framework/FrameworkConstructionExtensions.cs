using RayCarrot.CarrotFramework.Abstractions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Extension methods for <see cref="IFrameworkConstruction"/>
    /// </summary>
    public static class FrameworkConstructionExtensions
    {
        /// <summary>
        /// Adds all of the Rayman related default services
        /// </summary>
        /// <returns>The construction</returns>
        public static IFrameworkConstruction AddRaymanDefaults(this IFrameworkConstruction construction)
        {
            construction.AddTransient<IRayManager, DefaultRayManager>();

            return construction;
        }
    }
}