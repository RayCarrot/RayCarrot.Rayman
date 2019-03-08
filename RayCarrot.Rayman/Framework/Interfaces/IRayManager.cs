namespace RayCarrot.Rayman
{
    /// <summary>
    /// Manages Rayman related requests
    /// </summary>
    public interface IRayManager
    {
        /// <summary>
        /// Gets a <see cref="RayGLI_Mode"/> from a string
        /// </summary>
        /// <param name="value">The value from a INI key</param>
        /// <returns>The <see cref="RayGLI_Mode"/></returns>
        RayGLI_Mode ToRayGLI_Mode(string value);
    }
}