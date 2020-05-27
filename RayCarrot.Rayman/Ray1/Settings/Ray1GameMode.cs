namespace RayCarrot.Rayman.Ray1
{
    /// <summary>
    /// The Ray1 game modes
    /// </summary>
    public enum Ray1GameMode
    {
        [Ray1GameModeInfo("Rayman (PC)", Ray1Game.Rayman1, Platform.PC)]
        Rayman1PC,

        [Ray1GameModeInfo("Rayman Designer (PC)", Ray1Game.RayKit, Platform.PC)]
        RayKitPC,
    }
}