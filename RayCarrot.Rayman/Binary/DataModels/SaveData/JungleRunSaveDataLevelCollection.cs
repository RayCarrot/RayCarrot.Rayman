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
        public JungleRunSaveDataLevelCollection() : base(70)
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
            foreach (var levelData in this)
                serializer.Serialize(stream, levelData);
        }
    }
}