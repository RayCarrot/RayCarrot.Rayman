using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman.UbiArt
{
    [TypeConverter(typeof(UbiArtStringIDTypeConverter))]
    public class UbiArtStringID : IBinarySerializable<UbiArtSettings>
    {
        #region Constructors

        /// <summary>
        /// Constructor for an empty ID
        /// </summary>
        public UbiArtStringID()
        {
            ID = UInt32.MaxValue;
        }

        /// <summary>
        /// Constructor for an ID from a string
        /// </summary>
        /// <param name="str">The string to generate the ID from</param>
        public UbiArtStringID(string str) : this(Encoding.ASCII.GetBytes(str)) { }

        /// <summary>
        /// Constructor for an ID from the bytes
        /// </summary>
        /// <param name="bytes">The bytes to generate the ID from</param>
        public UbiArtStringID(IReadOnlyList<byte> bytes)
        {
            uint v2; // r11@1
            uint v3; // r4@1
            uint pos; // r9@1
            uint left; // r10@1
            uint v6; // r6@1
            uint v0; // r5@1
            uint v15; // r5@3
            uint v23; // r4@3
            uint v31; // r0@3
            uint v32; // r5@3
            uint v33; // r4@3
            uint v34; // r3@3
            uint v35; // r5@3
            uint v36; // r4@3
            uint v37; // r3@3
            uint v38; // r6@4
            uint v39; // r2@16
            uint v40; // r3@16
            uint v41; // r0@16
            uint v42; // r2@16
            uint v43; // r3@16
            uint v44; // r0@16
            uint v45; // r2@16
            uint v46;
            uint length = (uint)bytes.Count;
            v2 = length;
            v0 = 0x9E3779B9;
            v3 = 0x9E3779B9;
            pos = 0;
            v6 = 0;
            uint[] upper = new uint[length];
            for (int j = 0; j < length; j++)
            {
                upper[j] = ToUp(bytes[j]);
            }

            static uint ToUp(uint result)
            {
                if (result >= 97 && result <= 97 + 0x19)
                {
                    result -= 32;
                }
                return result;
            }
            while (pos + 12 <= length)
            {
                v15 = upper[pos + 0] + v0 + (upper[pos + 1] << 8) + (upper[pos + 2] << 16) + (upper[pos + 3] << 24);
                v23 = upper[pos + 4] + v3 + (upper[pos + 5] << 8) + (upper[pos + 6] << 16) + (upper[pos + 7] << 24);
                v31 = upper[pos + 8] + v6 + (upper[pos + 9] << 8) + (upper[pos + 10] << 16) + (upper[pos + 11] << 24);
                v32 = (v15 - v23 - v31) ^ (v31 >> 13);
                v33 = (v23 - v31 - v32) ^ (v32 << 8);
                v34 = (v31 - v32 - v33) ^ (v33 >> 13);
                v35 = (v32 - v33 - v34) ^ (v34 >> 12);
                v36 = (v33 - v34 - v35) ^ (v35 << 16);
                v37 = (v34 - v35 - v36) ^ (v36 >> 5);
                v0 = (v35 - v36 - v37) ^ (v37 >> 3);
                v3 = (v36 - v37 - v0) ^ (v0 << 10);
                v6 = (v37 - v0 - v3) ^ (v3 >> 15);
                pos += 12;
            }
            v38 = v6 + v2;
            left = length - pos;
            if (left > 0)
            {
                if (left >= 11) v38 += upper[pos + 10] << 24;
                if (left >= 10) v38 += upper[pos + 9] << 16;
                if (left >= 9) v38 += upper[pos + 8] << 8;
                if (left >= 8) v3 += upper[pos + 7] << 24;
                if (left >= 7) v3 += upper[pos + 6] << 16;
                if (left >= 6) v3 += upper[pos + 5] << 8;
                if (left >= 5) v3 += upper[pos + 4];
                if (left >= 4) v0 += upper[pos + 3] << 24;
                if (left >= 3) v0 += upper[pos + 2] << 16;
                if (left >= 2) v0 += upper[pos + 1] << 8;
                if (left >= 1) v0 += upper[pos + 0];
            }
            v39 = (v0 - v3 - v38) ^ (v38 >> 13);
            v40 = (v3 - v38 - v39) ^ (v39 << 8);
            v41 = (v38 - v39 - v40) ^ (v40 >> 13);
            v42 = (v39 - v40 - v41) ^ (v41 >> 12);
            v43 = (v40 - v41 - v42) ^ (v42 << 16);
            v44 = (v41 - v42 - v43) ^ (v43 >> 5);
            v45 = (v42 - v43 - v44) ^ (v44 >> 3);
            v46 = (v43 - v44 - v45) ^ (v45 << 10);
            ID = (v44 - v45 - v46) ^ (v46 >> 15);
        }

        #endregion

        #region Public Properties

        public uint ID { get; set; }

        #endregion

        #region Public Methods

        public void Deserialize(IBinaryDataReader<UbiArtSettings> reader)
        {
            ID = reader.Read<uint>();
        }

        public void Serialize(IBinaryDataWriter<UbiArtSettings> writer)
        {
            writer.Write(ID);
        }

        #endregion

        #region Type Converter

        // NOTE: We need to use this allow converting to/from strings as this is value if often used as a dictionary key, and formats such as JSON require them to be specific value types
        public class UbiArtStringIDTypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string) || sourceType == typeof(uint))
                    return true;
                else
                    return false;
            }

            public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == typeof(string) || destinationType == typeof(uint))
                    return true;
                else
                    return false;
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string s)
                    return new UbiArtStringID()
                    {
                        ID = UInt32.Parse(s)
                    };
                else if (value is uint u)
                    return new UbiArtStringID()
                    {
                        ID = u
                    };
                else
                    return null;
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                var v = value.CastTo<UbiArtStringID>();

                if (destinationType == typeof(string))
                    return v.ID.ToString();
                else if (destinationType == typeof(uint))
                    return v.ID;
                else
                    return null;
            }
        }

        #endregion
    }
}