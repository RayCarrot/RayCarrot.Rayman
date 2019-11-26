using System.Collections.Generic;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The collection of level data used in Rayman Jungle Run
    /// </summary>
    public class JungleRunSaveDataLevelCollection : List<JungleRunSaveDataLevel>, IBinarySerializable
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="capacity">The capacity. This should be set to 70 if running the latest version of the game.</param>
        public JungleRunSaveDataLevelCollection(int capacity) : base(capacity)
        {
            
        }

        /// <summary>
        /// Deserializes the data from the stream into this instance
        /// </summary>
        /// <param name="stream">The stream to deserialize from</param>
        /// <param name="deserializer">The deserializer</param>
        public void Deserialize(FileStream stream, IBinaryDeserializer deserializer)
        {
            for (int i = 0; i < Capacity; i++)
            {
                var item = new JungleRunSaveDataLevel();

                item.Deserialize(stream, deserializer);

                Add(item);
            }
        }

        /// <summary>
        /// Serializes the data from this instance to the stream
        /// </summary>
        /// <param name="stream">The stream to serialize to</param>
        /// <param name="serializer">The serializer</param>
        public void Serialize(FileStream stream, IBinarySerializer serializer)
        {
            throw new System.NotImplementedException();
        }
    }
}