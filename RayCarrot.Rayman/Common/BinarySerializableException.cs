using System;
using System.Runtime.Serialization;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// Exception for when serializing or deserializing a binary file
    /// </summary>
    public class BinarySerializableException : Exception
    {
        public BinarySerializableException()
        {
        }

        public BinarySerializableException(string message) : base(message)
        {
        }

        public BinarySerializableException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BinarySerializableException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}