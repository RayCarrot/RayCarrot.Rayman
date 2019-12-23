namespace RayCarrot.Rayman
{
    /// <summary>
    /// The collection of the levels in Rayman Fiesta Run Windows 10 Edition with the fixed size of 72
    /// </summary>
    public class FiestaRunWin10EditionSaveDataLevelCollection : BinarySerializableFixedList<FiestaRunSaveDataLevel>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        // TODO: Include added levels
        public FiestaRunWin10EditionSaveDataLevelCollection() : base(72)
        {
        }
    }
}