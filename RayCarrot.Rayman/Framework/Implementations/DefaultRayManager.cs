using System;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Default manager for managing Rayman related requests
    /// </summary>
    public class DefaultRayManager : IRayManager
    {
        /// <summary>
        /// Gets a <see cref="RayGLI_Mode"/> from a string
        /// </summary>
        /// <param name="value">The value from a INI key</param>
        /// <returns>The <see cref="RayGLI_Mode"/></returns>
        public RayGLI_Mode ToRayGLI_Mode(string value)
        {
            if (value == null)
                return null;

            try
            {
                // Template:
                // 1 - 1920 x 1080 x 16

                // Split the values
                var values = value.Split('x', '-');

                // Trim the values
                for (int i = 0; i < values.Length; i++)
                    values[i] = values[i].Trim(' ');

                return new RayGLI_Mode()
                {
                    Windowed = Int32.Parse(values[0]),
                    ResX = Int32.Parse(values[1]),
                    ResY = Int32.Parse(values[2]),
                    ColorMode = Int32.Parse(values[3]),
                };

            }
            catch
            {
                return null;
            }
        }
    }
}