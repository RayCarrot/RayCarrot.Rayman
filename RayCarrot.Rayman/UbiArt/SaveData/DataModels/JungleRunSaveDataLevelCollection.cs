namespace RayCarrot.Rayman
{
    /// <summary>
    /// The collection of the levels in Rayman Jungle Run with the fixed size of 70
    /// </summary>
    public class JungleRunSaveDataLevelCollection : BinarySerializableFixedList<JungleRunSaveDataLevel>
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public JungleRunSaveDataLevelCollection() : base(70)
        {
        }
    }
}