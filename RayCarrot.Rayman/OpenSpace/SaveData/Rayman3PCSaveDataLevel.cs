using RayCarrot.Binary;

namespace RayCarrot.Rayman.OpenSpace
{
    /// <summary>
    /// The data for a Rayman 3 save file level on PC
    /// </summary>
    public class Rayman3PCSaveDataLevel : IBinarySerializable
    {
        /// <summary>
        /// The cages
        /// </summary>
        public int Cages { get; set; }

        /// <summary>
        /// The score
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Cages = s.Serialize<int>(Cages, name: nameof(Cages));
            Score = s.Serialize<int>(Score, name: nameof(Score));
        }
    }
}