using System;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The binary data writer for the <see cref="BinaryDataSerializer{T}"/>
    /// </summary>
    public class BinaryDataWriter : IDisposable
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="writer">The underlying writer</param>
        public BinaryDataWriter(BinaryWriter writer)
        {
            Writer = writer;
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The underlying writer
        /// </summary>
        protected BinaryWriter Writer { get; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the position of the stream
        /// </summary>
        /// <param name="position">The position</param>
        public void SetPosition(long position)
        {
            Writer.BaseStream.Position = position;
        }

        /// <summary>
        /// Writes the specified type
        /// </summary>
        /// <typeparam name="T">The type of the value to write. This is either a supported value type or implements <see cref="IBinarySerializable"/></typeparam>
        /// <param name="value">The value to write</param>
        public void Write<T>(T value)
        {
            if (value is IBinarySerializable binarySerializable)
                binarySerializable.Serialize(this);

            else if (value is bool bo)
                Writer.Write(bo);

            else if (value is char ch)
                Writer.Write(ch);

            else if (value is sbyte sb)
                Writer.Write(sb);

            else if (value is byte[] ba)
                Writer.Write(ba);

            else if (value is byte by)
                Writer.Write(by);

            else if (value is short sh)
                Writer.Write(sh);

            else if (value is ushort ush)
                Writer.Write(ush);

            else if (value is int i32)
                Writer.Write(i32);

            else if (value is uint ui32)
                Writer.Write(ui32);

            else if (value is long lo)
                Writer.Write(lo);

            else if (value is ulong ulo)
                Writer.Write(ulo);

            else if (value is float fl)
                Writer.Write(fl);

            else if (value is double dou)
                Writer.Write(dou);

            else if (value is decimal de)
                Writer.Write(de);

            else if (value is string st)
                Writer.Write(st);

            else
                throw new NotSupportedException($"The specified generic type {typeof(T).Name} is not supported and does not implement {nameof(IBinarySerializable)}");
        }

        public void Dispose()
        {
            Writer?.Dispose();
        }

        #endregion
    }
}