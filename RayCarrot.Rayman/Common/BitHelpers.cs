﻿namespace RayCarrot.Rayman
{
    /// <summary>
    /// Bit helper methods
    /// </summary>
    public static class BitHelpers
    {
        /// <summary>
        /// Extracts the bits from a value
        /// </summary>
        /// <param name="value">The value to extract the bits from</param>
        /// <param name="count">The amount of bits to extract</param>
        /// <param name="offset">The offset to start from</param>
        /// <returns>The extracted bits as an integer</returns>
        public static int ExtractBits(int value, int count, int offset)
        {
            return (((1 << count) - 1) & (value >> (offset)));
        }

        public static int SetBits(int bits, int value, int count, int offset)
        {
            int mask = ((1 << count) - 1) << offset;
            bits = (bits & ~mask) | (value << offset);
            return bits;
        }
    }
}