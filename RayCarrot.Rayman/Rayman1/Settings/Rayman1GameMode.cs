namespace RayCarrot.Rayman
{
    /// <summary>
    /// The available Rayman 1 game modes
    /// </summary>
    public enum Rayman1GameMode
    {
        /// <summary>
        /// Rayman 1 (PC)
        /// </summary>
        [Rayman1GameModeInfo("Rayman 1 (PC)", Rayman1Game.Rayman1, Rayman1Platform.PC)]
        Rayman1PC,

        /// <summary>
        /// Rayman Designer (PC)
        /// </summary>
        [Rayman1GameModeInfo("Rayman Designer (PC)", Rayman1Game.RaymanDesigner, Rayman1Platform.PC)]
        RaymanDesignerPC,
    }
}