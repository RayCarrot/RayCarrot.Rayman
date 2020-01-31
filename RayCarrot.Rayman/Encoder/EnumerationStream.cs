using System;
using System.Collections.Generic;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// A Stream for reading an <see cref="IEnumerable{T}"/>
    /// </summary>
    public class EnumerationStream : Stream
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="enumerable">The enumerable</param>
        public EnumerationStream(IEnumerable<byte> enumerable)
        {
            Enumerator = enumerable.GetEnumerator();
        }

        /// <summary>
        /// The enumerator
        /// </summary>
        protected IEnumerator<byte> Enumerator { get; }

        public override void Flush() { }

        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

        public override void SetLength(long value) => throw new NotSupportedException();

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (offset != 0)
                throw new NotSupportedException("The offset has to be 0");

            // Keep track of how many values have been read
            int readValues = 0;

            // Reach each value into the buffer
            for (int i = 0; i < count; i++)
            {
                // Move the enumeration forward
                if (!Enumerator.MoveNext())
                    return readValues;

                // Get the value
                buffer[i] = Enumerator.Current;

                readValues++;
            }

            // Return the amount of read values
            return readValues;
        }

        public override int ReadByte()
        {
            if (!Enumerator.MoveNext())
                return -1;
            else
                return Enumerator.Current;
        }

        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => throw new NotSupportedException();

        public override long Position
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }
    }
}