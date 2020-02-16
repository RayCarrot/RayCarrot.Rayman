namespace RayCarrot.Rayman.Rayman1
{
    /// <summary>
    /// The data for an event for a Rayman 1 .lev file on PC
    /// </summary>
    public class Rayman1PCLevEvent : IBinarySerializable<Rayman1Settings>
    { 
        public uint DES { get; set; }

        public uint DES2 { get; set; }

        public uint DES3 { get; set; }

        public uint ETA { get; set; }

        public uint Unknown1 { get; set; }

        public uint Unknown2 { get; set; }
        
        public BinarySerializableList<byte> Unknown3 { get; set; }

        public uint XPosition { get; set; }
        
        public uint YPosition { get; set; }

        public BinarySerializableList<byte> Unknown4 { get; set; }

        public BinarySerializableList<byte> Unknown5 { get; set; }

        public uint Type { get; set; }

        public uint Unknown6 { get; set; }

        public byte OffsetBX { get; set; }

        public byte OffsetBY { get; set; }
        
        public ushort Unknown7 { get; set; }

        public byte SubEtat { get; set; }

        public byte Etat { get; set; }

        public ushort Unknown8 { get; set; }

        public uint Unknown9 { get; set; }

        public byte OffsetHY { get; set; }

        public byte FollowSprite { get; set; }

        public ushort HitPoints { get; set; }

        public byte Group { get; set; }

        public byte HitSprite { get; set; }

        public BinarySerializableList<byte> Unknown10 { get; set; }

        public byte Unknown11 { get; set; }

        public byte FollowEnabled { get; set; }

        public ushort Unknown12 { get; set; }

        public void Deserialize(IBinaryDataReader<Rayman1Settings> reader)
        {
            DES = reader.Read<uint>();
            DES2 = reader.Read<uint>();
            DES3 = reader.Read<uint>();
            ETA = reader.Read<uint>();

            Unknown1 = reader.Read<uint>();
            Unknown2 = reader.Read<uint>();

            Unknown3 = new BinarySerializableList<byte>(16);
            Unknown3.Deserialize(reader);

            XPosition = reader.Read<uint>();
            YPosition = reader.Read<uint>();

            Unknown4 = new BinarySerializableList<byte>(20);
            Unknown4.Deserialize(reader);

            Unknown5 = new BinarySerializableList<byte>(28);
            Unknown5.Deserialize(reader);

            Type = reader.Read<uint>();
            Unknown6 = reader.Read<uint>();

            OffsetBX = reader.Read<byte>();
            OffsetBY = reader.Read<byte>();

            Unknown7 = reader.Read<ushort>();

            SubEtat = reader.Read<byte>();
            Etat = reader.Read<byte>();
            
            Unknown8 = reader.Read<ushort>();
            Unknown9 = reader.Read<uint>();

            OffsetHY = reader.Read<byte>();
            FollowSprite = reader.Read<byte>();
            HitPoints = reader.Read<ushort>();
            Group = reader.Read<byte>();
            HitSprite = reader.Read<byte>();

            Unknown10 = new BinarySerializableList<byte>(6);
            Unknown10.Deserialize(reader);
            
            Unknown11 = reader.Read<byte>();
            FollowEnabled = reader.Read<byte>();
            Unknown12 = reader.Read<ushort>();
        }

        public void Serialize(IBinaryDataWriter<Rayman1Settings> writer)
        {
            writer.Write(DES);
            writer.Write(DES2);
            writer.Write(DES3);
            writer.Write(ETA);

            writer.Write(Unknown1);
            writer.Write(Unknown2);

            writer.Write(Unknown3);

            writer.Write(XPosition);
            writer.Write(YPosition);

            writer.Write(Unknown4);
            writer.Write(Unknown5);

            writer.Write(Type);
            writer.Write(Unknown6);

            writer.Write(OffsetBX);
            writer.Write(OffsetBY);

            writer.Write(Unknown7);

            writer.Write(SubEtat);
            writer.Write(Etat);

            writer.Write(Unknown8);
            writer.Write(Unknown9);

            writer.Write(OffsetHY);
            writer.Write(FollowSprite);
            writer.Write(HitPoints);
            writer.Write(Group);
            writer.Write(HitSprite);

            writer.Write(Unknown10);
            writer.Write(Unknown11);

            writer.Write(FollowEnabled);

            writer.Write(Unknown12);
        }
    }
}