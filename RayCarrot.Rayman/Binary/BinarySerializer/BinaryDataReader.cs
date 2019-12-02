using System;
using System.IO;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The wrapper binary data reader for the <see cref="BinaryDataSerializer{T}"/>
    /// </summary>
    public class BinaryDataReader : IDisposable
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="reader">The underlying reader</param>
        public BinaryDataReader(BinaryReader reader)
        {
            Reader = reader;

        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The underlying reader
        /// </summary>
        protected BinaryReader Reader { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Reads the specified type
        /// </summary>
        /// <typeparam name="T">The type to read. This is either a supported value type or implements <see cref="IBinarySerializable"/></typeparam>
        /// <returns>The value</returns>
        public T Read<T>()
        {
            // Helper method which returns an object so we can cast it
            object ReadObject()
            {
                // Get the type
                var type = typeof(T);

                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Object:
                        // Make sure the type implements the interface
                        if (!typeof(IBinarySerializable).IsAssignableFrom(type))
                            throw new NotSupportedException($"The specified generic type does not implement {nameof(IBinarySerializable)}");

                        // Create a new instance
                        var instance = type.CreateInstance<IBinarySerializable>();

                        // Deserialize the type
                        instance.Deserialize(this);

                        // Return the instance
                        return instance;

                    case TypeCode.Boolean:
                        return Reader.ReadBoolean();

                    case TypeCode.Char:
                        return Reader.ReadChar();

                    case TypeCode.SByte:
                        return Reader.ReadSByte();

                    case TypeCode.Byte:
                        return Reader.ReadByte();

                    case TypeCode.Int16:
                        return Reader.ReadInt16();

                    case TypeCode.UInt16:
                        return Reader.ReadUInt16();

                    case TypeCode.Int32:
                        return Reader.ReadInt32();

                    case TypeCode.UInt32:
                        return Reader.ReadUInt32();

                    case TypeCode.Int64:
                        return Reader.ReadInt64();

                    case TypeCode.UInt64:
                        return Reader.ReadUInt16();

                    case TypeCode.Single:
                        return Reader.ReadSingle();

                    case TypeCode.Double:
                        return Reader.ReadDouble();

                    case TypeCode.Decimal:
                        return Reader.ReadDecimal();

                    case TypeCode.String:
                        return Reader.ReadString();

                    case TypeCode.DateTime:
                    case TypeCode.Empty:
                    case TypeCode.DBNull:
                    default:
                        throw new NotSupportedException("The specified generic type can not be read from the reader");
                }
            }

            // Return the object cast to the generic type
            return (T)ReadObject();
        }

        /// <summary>
        /// Reads the specified number of bytes
        /// </summary>
        /// <returns>The number of bytes to read</returns>
        public byte[] ReadBytes(int count)
        {
            return Reader.ReadBytes(count);
        }

        /// <summary>
        /// Reads the remaining bytes
        /// </summary>
        /// <returns>The remaining bytes</returns>
        public byte[] ReadRemainingBytes()
        {
            return Reader.BaseStream.ReadRemainingBytes();
        }

        public void Dispose()
        {
            Reader?.Dispose();
        }

        #endregion
    }
}