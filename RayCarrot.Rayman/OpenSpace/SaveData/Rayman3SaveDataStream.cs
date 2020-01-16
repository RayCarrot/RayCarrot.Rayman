using System;
using System.Collections.Generic;
using System.IO;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The data stream to use for an encrypted Rayman 3 save file
    /// </summary>
    public class Rayman3SaveDataStream : Stream
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="baseStream">The base stream to use</param>
        /// <param name="leaveOpen">Indicates if the base stream should be left open when disposing</param>
        public Rayman3SaveDataStream(Stream baseStream, bool leaveOpen)
        {
            BaseStream = baseStream;
            LeaveOpen = leaveOpen;
            ByteEnumerator = GetByteEnumerator().GetEnumerator();
        }

        #endregion

        #region Protected Properties

        /// <summary>
        /// The base stream to use
        /// </summary>
        protected Stream BaseStream { get; }

        /// <summary>
        /// Indicates if the base stream should be left open when disposing
        /// </summary>
        protected bool LeaveOpen { get; }

        /// <summary>
        /// The current XOR key (changes throughout the operation)
        /// </summary>
        protected uint XORKey { get; set; }

        /// <summary>
        /// The enumerator used to read the bytes
        /// </summary>
        protected IEnumerator<byte> ByteEnumerator { get; }

        #endregion

        #region Public Override Properties

        public override bool CanRead => BaseStream.CanRead;
        
        public override bool CanSeek => false;
        
        public override bool CanWrite => BaseStream.CanWrite;
        
        public override long Length => BaseStream.Length;
        
        public override long Position
        {
            get => BaseStream.Position;
            set => throw new NotSupportedException();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Gets a new byte enumerator to use
        /// </summary>
        /// <returns>The byte enumerator</returns>
        protected IEnumerable<byte> GetByteEnumerator()
        {
            // Read the initial key
            var buffer = new byte[4];
            BaseStream.Read(buffer, 0, 4);
            XORKey = BitConverter.ToUInt32(buffer, 0) ^ 0xA55AA55A;

            // Keep track of the last byte
            byte lastByte;

            byte DecryptByte()
            {
                XORKey = (XORKey >> 3) | (XORKey << 29);
                lastByte = (byte)BaseStream.ReadByte();
                lastByte ^= (byte)XORKey;
                return lastByte;
            }

            // Enumerate each byte
            while (BaseStream.Position < BaseStream.Length)
            {
                // Decrypt the byte
                DecryptByte();

                if ((lastByte & 0x80) == 0)
                {
                    for (var i = 0; i < lastByte; i++)
                        yield return 0;
                }
                else
                {
                    var size = lastByte & 0x7F;
                    var byteArray = new byte[size];

                    for (var i = size; i > 0; --i)
                        byteArray[i - 1] = DecryptByte();

                    foreach (var b in byteArray)
                        yield return b;
                }
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.IO.Stream" /> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Dispose base class
            base.Dispose(disposing);
            
            // Dispose base stream
            if (!LeaveOpen)
                BaseStream?.Dispose();
        }

        #endregion

        #region Public Override Methods

        public override int Read(byte[] buffer, int offset, int count)
        {
            // Keep track of how many values have been read
            int readValues = 0;

            // Reach each value into the buffer
            for (int i = 0; i < count; i++)
            {
                // Move the enumeration forward
                if (!ByteEnumerator.MoveNext())
                    return readValues;

                // Get the value
                buffer[i] = ByteEnumerator.Current;
                
                readValues++;
            }

            // Return the amount of read values
            return readValues;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException("The Rayman 3 save data stream currently only supports reading");
        }

        public override void Flush()
        {
            BaseStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}