using RayCarrot.Binary;

namespace RayCarrot.Rayman.Ray1
{
    /// <summary>
    /// The config data for Rayman 1 on PC
    /// </summary>
    public class Rayman1PCConfigData : IBinarySerializable
    {
        public R1Languages Language { get; set; }

        public uint Dword_01 { get; set; }
        public uint Dword_05 { get; set; }
        public uint Dword_09 { get; set; }
        public uint Dword_0D { get; set; }
        public uint Dword_11 { get; set; }
        public byte Byte_15 { get; set; }
        public ushort Word_16 { get; set; }
        public ushort Word_18 { get; set; }
        public ushort Word_1A { get; set; }
        public ushort Word_1C { get; set; }

        public ushort MusicEnabled { get; set; }
        
        // 0-20
        public ushort SoundVolume { get; set; }

        public ushort Word_22 { get; set; }
        public byte Byte_24 { get; set; }
        public byte Byte_25 { get; set; }

        public ushort JoystickMaxX { get; set; }
        public ushort JoystickMinX { get; set; }
        public ushort JoystickMaxY { get; set; }
        public ushort JoystickMinY { get; set; }
        public ushort JoystickMiddleX { get; set; }
        public ushort JoystickMiddleY { get; set; }

        public byte Byte_32 { get; set; }
        public byte Byte_33 { get; set; }
        public byte Byte_34 { get; set; }
        public byte Byte_35 { get; set; }

        public byte Button_Left { get; set; }
        public byte Button_Up { get; set; }
        public byte Button_Right { get; set; }
        public byte Button_Down { get; set; }
        public byte Button_Jump { get; set; }
        public byte Button_Fist { get; set; }
        public byte Button_Action { get; set; }

        // pci1 = 0, pci2 = 1
        public byte GraphicsMode { get; set; }

        // pci1 or pci2 = 0, vesa = 1
        public byte VesaMode { get; set; }

        // 4/4 = 0, 3/4 = 1, 2/4 = 2, 1/4 = 3
        public byte ZoneOfPlay { get; set; }

        // "60" = 0, "50" = 1, "max" = 2
        public byte FrequencyMode { get; set; }

        public bool ScoresEnabled { get; set; }

        public bool BackgroundsEnabled { get; set; }
        public bool ParallaxScrollingEnabled { get; set; }

        public byte[] Bytes_44 { get; set; }
        public byte[] Bytes_54 { get; set; }
        public byte[] Bytes_64 { get; set; }
        public uint Dword_74 { get; set; }
        public uint Dword_78 { get; set; }
        public uint Dword_7C { get; set; }
        public uint Dword_80 { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            Language = s.Serialize<R1Languages>(Language, name: nameof(Language));

            Dword_01 = s.Serialize<uint>(Dword_01, name: nameof(Dword_01));
            Dword_05 = s.Serialize<uint>(Dword_05, name: nameof(Dword_05));
            Dword_09 = s.Serialize<uint>(Dword_09, name: nameof(Dword_09));
            Dword_0D = s.Serialize<uint>(Dword_0D, name: nameof(Dword_0D));
            Dword_11 = s.Serialize<uint>(Dword_11, name: nameof(Dword_11));
            Byte_15 = s.Serialize<byte>(Byte_15, name: nameof(Byte_15));
            Word_16 = s.Serialize<ushort>(Word_16, name: nameof(Word_16));
            Word_18 = s.Serialize<ushort>(Word_18, name: nameof(Word_18));
            Word_1A = s.Serialize<ushort>(Word_1A, name: nameof(Word_1A));
            Word_1C = s.Serialize<ushort>(Word_1C, name: nameof(Word_1C));

            MusicEnabled = s.Serialize<ushort>(MusicEnabled, name: nameof(MusicEnabled));
            SoundVolume = s.Serialize<ushort>(SoundVolume, name: nameof(SoundVolume));

            Word_22 = s.Serialize<ushort>(Word_22, name: nameof(Word_22));
            Byte_24 = s.Serialize<byte>(Byte_24, name: nameof(Byte_24));
            Byte_25 = s.Serialize<byte>(Byte_25, name: nameof(Byte_25));

            JoystickMaxX = s.Serialize<ushort>(JoystickMaxX, name: nameof(JoystickMaxX));
            JoystickMinX = s.Serialize<ushort>(JoystickMinX, name: nameof(JoystickMinX));
            JoystickMaxY = s.Serialize<ushort>(JoystickMaxY, name: nameof(JoystickMaxY));
            JoystickMinY = s.Serialize<ushort>(JoystickMinY, name: nameof(JoystickMinY));
            JoystickMiddleX = s.Serialize<ushort>(JoystickMiddleX, name: nameof(JoystickMiddleX));
            JoystickMiddleY = s.Serialize<ushort>(JoystickMiddleY, name: nameof(JoystickMiddleY));

            Byte_32 = s.Serialize<byte>(Byte_32, name: nameof(Byte_32));
            Byte_33 = s.Serialize<byte>(Byte_33, name: nameof(Byte_33));
            Byte_34 = s.Serialize<byte>(Byte_34, name: nameof(Byte_34));
            Byte_35 = s.Serialize<byte>(Byte_35, name: nameof(Byte_35));

            Button_Left = s.Serialize<byte>(Button_Left, name: nameof(Button_Left));
            Button_Up = s.Serialize<byte>(Button_Up, name: nameof(Button_Up));
            Button_Right = s.Serialize<byte>(Button_Right, name: nameof(Button_Right));
            Button_Down = s.Serialize<byte>(Button_Down, name: nameof(Button_Down));
            Button_Jump = s.Serialize<byte>(Button_Jump, name: nameof(Button_Jump));
            Button_Fist = s.Serialize<byte>(Button_Fist, name: nameof(Button_Fist));
            Button_Action = s.Serialize<byte>(Button_Action, name: nameof(Button_Action));

            GraphicsMode = s.Serialize<byte>(GraphicsMode, name: nameof(GraphicsMode));
            VesaMode = s.Serialize<byte>(VesaMode, name: nameof(VesaMode));
            ZoneOfPlay = s.Serialize<byte>(ZoneOfPlay, name: nameof(ZoneOfPlay));
            FrequencyMode = s.Serialize<byte>(FrequencyMode, name: nameof(FrequencyMode));
            ScoresEnabled = s.Serialize<bool>(ScoresEnabled, name: nameof(ScoresEnabled));
            BackgroundsEnabled = s.Serialize<bool>(BackgroundsEnabled, name: nameof(BackgroundsEnabled));
            ParallaxScrollingEnabled = s.Serialize<bool>(ParallaxScrollingEnabled, name: nameof(ParallaxScrollingEnabled));

            Bytes_44 = s.SerializeArray<byte>(Bytes_44, 0x10, name: nameof(Bytes_44));
            Bytes_54 = s.SerializeArray<byte>(Bytes_54, 0x10, name: nameof(Bytes_54));
            Bytes_64 = s.SerializeArray<byte>(Bytes_64, 0x10, name: nameof(Bytes_64));

            Dword_74 = s.Serialize<uint>(Dword_74, name: nameof(Dword_74));
            Dword_78 = s.Serialize<uint>(Dword_78, name: nameof(Dword_78));
            Dword_7C = s.Serialize<uint>(Dword_7C, name: nameof(Dword_7C));
            Dword_80 = s.Serialize<uint>(Dword_80, name: nameof(Dword_80));
        }
    }
}