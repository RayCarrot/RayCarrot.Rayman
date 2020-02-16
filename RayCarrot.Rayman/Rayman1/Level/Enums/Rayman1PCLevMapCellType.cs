namespace RayCarrot.Rayman.Rayman1
{
    /// <summary>
    /// The cell types for a <see cref="Rayman1PCLevMapCell"/>, 32 in total. Some are unused, while others are used in specific levels but appear to have no function. Some only function in the spin-offs (Designer etc.).
    /// </summary>
    public enum Rayman1PCLevMapCellType
    {
        Empty = 0,
        Reactionary = 1,
        Slope1 = 2,
        Slope2 = 3,
        Slope3 = 4,
        Slope4 = 5,
        Slope5 = 6,
        Slope6 = 7,
        Death = 8,
        Bounce = 9,
        Water = 10,
        Exit = 11,
        Climbing = 12,
        WaterNoSplash = 13,
        PassDown = 14,
        Solid = 15,
        Seed = 16,

        SlipperySlope1 = 18,
        SlipperySlope2 = 19,
        SlipperySlope3 = 20,
        SlipperySlope4 = 21,
        SlipperySlope5 = 22,
        SlipperySlope6 = 23,
        InstantDeath = 24,
        Falling = 25,

        Slippery = 30,
    }
}