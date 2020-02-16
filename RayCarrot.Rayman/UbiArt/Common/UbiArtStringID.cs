using System;
using System.ComponentModel;
using System.Globalization;
using RayCarrot.Extensions;

namespace RayCarrot.Rayman.UbiArt
{
    [TypeConverter(typeof(UbiArtStringIDTypeConverter))]
    public class UbiArtStringID : IBinarySerializable<UbiArtSettings>
    {
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