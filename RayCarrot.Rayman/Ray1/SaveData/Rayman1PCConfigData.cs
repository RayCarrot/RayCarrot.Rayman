using RayCarrot.Binary;

namespace RayCarrot.Rayman.Ray1
{
    /// <summary>
    /// The config data for Rayman 1 on PC
    /// </summary>
    public class Rayman1PCConfigData : IBinarySerializable
    {
        public R1Languages Language { get; set; }

        public uint Port { get; set; }
        public uint Irq { get; set; }
        public uint Dma { get; set; }
        public uint Param { get; set; }
        public uint DeviceID { get; set; }
        public byte NumCard { get; set; }

        public ushort KeyJump { get; set; } // Keys are 0-3
        public ushort KeyWeapon { get; set; }
        public ushort Options_jeu_10 { get; set; }
        public ushort KeyAction { get; set; }

        public ushort MusicCdActive { get; set; } // Options_jeu_12
        public ushort VolumeSound { get; set; } // Options_jeu_13, set as (127 * value / 20), 0-20

        public ushort Options_jeu_14 { get; set; }
        public byte Kit_Byte_23 { get; set; }
        public byte Kit_Byte_24 { get; set; }
        public byte Mode_Pad { get; set; }
        public byte Port_Pad { get; set; }

        public ushort XPadMax { get; set; }
        public ushort XPadMin { get; set; }
        public ushort YPadMax { get; set; }
        public ushort YPadMin { get; set; }
        public ushort XPadCentre { get; set; }
        public ushort YPadCentre { get; set; }

        public byte[] NotBut { get; set; }
        public byte[] Tab_Key { get; set; } // Left, up, right, down, jump, fist, action

        public byte GameModeVideo { get; set; } // pci1 = 0, pci2 = 1
        public byte P486 { get; set; } // pci1 or pci2 = 0, vesa = 1
        public byte SizeScreen { get; set; } // 4/4 = 0, 3/4 = 1, 2/4 = 2, 1/4 = 3
        public byte Frequence { get; set; } // "60" = 0, "50" = 1, "max" = 2

        public bool FixOn { get; set; } // Scores enabled
        public bool BackgroundOptionOn { get; set; }
        public bool ScrollDiffOn { get; set; }

        public byte[] RefRam2VramNormalFix { get; set; }
        public byte[] RefRam2VramNormal { get; set; }
        public byte[] RefTransFondNormal { get; set; }
        public uint RefSpriteNormal { get; set; }
        public uint RefRam2VramX { get; set; }
        public uint RefVram2VramX { get; set; }
        public uint RefSpriteX { get; set; }

        /// <summary>
        /// Handles the serialization using the specified serializer
        /// </summary>
        /// <param name="s">The serializer</param>
        public void Serialize(IBinarySerializer s)
        {
            // Get the settings
            var settings = s.GetSettings<Ray1Settings>();

            if (settings.Game == Ray1Game.Rayman1)
                Language = s.Serialize<R1Languages>(Language, name: nameof(Language));

            Port = s.Serialize<uint>(Port, name: nameof(Port));
            Irq = s.Serialize<uint>(Irq, name: nameof(Irq));
            Dma = s.Serialize<uint>(Dma, name: nameof(Dma));
            Param = s.Serialize<uint>(Param, name: nameof(Param));
            DeviceID = s.Serialize<uint>(DeviceID, name: nameof(DeviceID));
            NumCard = s.Serialize<byte>(NumCard, name: nameof(NumCard));
            KeyJump = s.Serialize<ushort>(KeyJump, name: nameof(KeyJump));
            KeyWeapon = s.Serialize<ushort>(KeyWeapon, name: nameof(KeyWeapon));
            Options_jeu_10 = s.Serialize<ushort>(Options_jeu_10, name: nameof(Options_jeu_10));
            KeyAction = s.Serialize<ushort>(KeyAction, name: nameof(KeyAction));

            MusicCdActive = s.Serialize<ushort>(MusicCdActive, name: nameof(MusicCdActive));
            VolumeSound = s.Serialize<ushort>(VolumeSound, name: nameof(VolumeSound));

            Options_jeu_14 = s.Serialize<ushort>(Options_jeu_14, name: nameof(Options_jeu_14));

            if (settings.Game != Ray1Game.Rayman1)
            {
                Kit_Byte_23 = s.Serialize<byte>(Kit_Byte_23, name: nameof(Kit_Byte_23));
                Kit_Byte_24 = s.Serialize<byte>(Kit_Byte_24, name: nameof(Kit_Byte_24));
            }

            Mode_Pad = s.Serialize<byte>(Mode_Pad, name: nameof(Mode_Pad));
            Port_Pad = s.Serialize<byte>(Port_Pad, name: nameof(Port_Pad));

            XPadMax = s.Serialize<ushort>(XPadMax, name: nameof(XPadMax));
            XPadMin = s.Serialize<ushort>(XPadMin, name: nameof(XPadMin));
            YPadMax = s.Serialize<ushort>(YPadMax, name: nameof(YPadMax));
            YPadMin = s.Serialize<ushort>(YPadMin, name: nameof(YPadMin));
            XPadCentre = s.Serialize<ushort>(XPadCentre, name: nameof(XPadCentre));
            YPadCentre = s.Serialize<ushort>(YPadCentre, name: nameof(YPadCentre));

            NotBut = s.SerializeArray<byte>(NotBut, 4, name: nameof(NotBut));
            Tab_Key = s.SerializeArray<byte>(Tab_Key, 7, name: nameof(Tab_Key));

            GameModeVideo = s.Serialize<byte>(GameModeVideo, name: nameof(GameModeVideo));
            P486 = s.Serialize<byte>(P486, name: nameof(P486));
            SizeScreen = s.Serialize<byte>(SizeScreen, name: nameof(SizeScreen));
            Frequence = s.Serialize<byte>(Frequence, name: nameof(Frequence));
            FixOn = s.Serialize<bool>(FixOn, name: nameof(FixOn));
            BackgroundOptionOn = s.Serialize<bool>(BackgroundOptionOn, name: nameof(BackgroundOptionOn));
            ScrollDiffOn = s.Serialize<bool>(ScrollDiffOn, name: nameof(ScrollDiffOn));

            RefRam2VramNormalFix = s.SerializeArray<byte>(RefRam2VramNormalFix, 0x10, name: nameof(RefRam2VramNormalFix));
            RefRam2VramNormal = s.SerializeArray<byte>(RefRam2VramNormal, 0x10, name: nameof(RefRam2VramNormal));
            RefTransFondNormal = s.SerializeArray<byte>(RefTransFondNormal, 0x10, name: nameof(RefTransFondNormal));

            RefSpriteNormal = s.Serialize<uint>(RefSpriteNormal, name: nameof(RefSpriteNormal));
            RefRam2VramX = s.Serialize<uint>(RefRam2VramX, name: nameof(RefRam2VramX));
            RefVram2VramX = s.Serialize<uint>(RefVram2VramX, name: nameof(RefVram2VramX));
            RefSpriteX = s.Serialize<uint>(RefSpriteX, name: nameof(RefSpriteX));
        }
    }
}