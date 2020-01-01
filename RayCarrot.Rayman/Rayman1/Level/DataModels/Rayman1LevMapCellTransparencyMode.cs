namespace RayCarrot.Rayman
{
    /// <summary>
    /// The transparency mode for a <see cref="Rayman1LevMapCell"/>
    /// </summary>
    public enum Rayman1LevMapCellTransparencyMode
    {
        /// <summary>
        /// Indicates that the cell has no transparency
        /// </summary>
        NoTransparency = 0,

        /// <summary>
        /// Indicates that the cell is fully transparent
        /// </summary>
        FullyTransparent = 1,

        /// <summary>
        /// Indicates that the cell is partially transparent
        /// </summary>
        PartiallyTransparent = 2
    }
}