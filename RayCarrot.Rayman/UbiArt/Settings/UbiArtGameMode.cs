namespace RayCarrot.Rayman.UbiArt
{
    /*
        Textures:

        Origins (PC):                 (.dds)
        Origins (Wii):                Reverse endianness (.dds)
        Origins (3DS):                (.???)
        Origins (Vita):               (.gxt)
        Just Dance 2017 (Wii U)       (.png, .jpg, .gtx)
        
        Legends (PS3):                Missing DDS header (.dds)
        
        Legends demo (Wii U):         44 byte TEX header, version 8 (.gtx) (GFX2)
        Legends (Vita):               44 byte TEX header, version 9 (.gxt)
        
        Child of Light (PC):          52 byte TEX header, version 12 (.dds)
        Child of Light (Vita):        52 byte TEX header, version 12 (.gxt)
        Legends (PC):                 52 byte TEX header, version 13 (.dds)
        Legends (Wii U):              52 byte TEX header, version 13 (.gtx) (GFX2)
        
        Legends (PS4):                52 byte TEX header, version 23 (.gnf)
        Legends (Switch):             52 byte TEX header, version 23 (.xtx) (DFvN)
        Gravity Falls (3DS):          52 byte TEX header, version 23 (.???)
        
        Valiant Heart (Android):      56 byte TEX header, version 16 (.dds)
        Valiant Heart (Switch):       56 byte TEX header, version 16 (.xtx)
        Rayman Adventures (Android):  56 byte TEX header, version 17 (.dds)
        Rayman Adventures (iOS):      56 byte TEX header, version 17 (.dds, .pvr)
        Rayman Mini (Mac):            56 byte TEX header, version 17 (.dds)

    */

    /*
        IPK:

        Rayman Origins (PC, Wii, PS3, PS Vita):      3 
        Rayman Origins (3DS):                        4
        Rayman Legends (PC, Wii U, PS Vita, Switch): 5
        Just Dance 2017 (Wii U):                     5
        Valiant Hearts (Android):                    7
        Child of Light (PC, PS Vita):                7
        Rayman Legends (PS4):                        7
        Gravity Falls (3DS):                         7
        Rayman Adventures (Android, iOS):            8
        Rayman Mini (Mac):                           8

    */

    // TODO: Different files in the games use different text encoding & byte order. Perhaps use multiple attributes for different file types?

    /// <summary>
    /// The UbiArt game modes
    /// </summary>
    public enum UbiArtGameMode
    {
        [UbiArtGameModeInfo("Rayman Origins (PC)", UbiArtGame.RaymanOrigins, Platform.PC)]
        RaymanOriginsPC,

        [UbiArtGameModeInfo("Rayman Origins (PS3)", UbiArtGame.RaymanOrigins, Platform.PlayStation3)]
        RaymanOriginsPS3,

        [UbiArtGameModeInfo("Rayman Origins (Wii)", UbiArtGame.RaymanOrigins, Platform.Wii)]
        RaymanOriginsWii,

        [UbiArtGameModeInfo("Rayman Origins (PS Vita)", UbiArtGame.RaymanOrigins, Platform.PSVita)]
        RaymanOriginsPSVita,

        [UbiArtGameModeInfo("Rayman Origins (3DS)", UbiArtGame.RaymanOrigins, Platform.Nintendo3DS)]
        RaymanOrigins3DS,

        [UbiArtGameModeInfo("Rayman Jungle Run (PC)", UbiArtGame.RaymanJungleRun, Platform.PC)]
        RaymanJungleRunPC,

        [UbiArtGameModeInfo("Rayman Jungle Run (Android)", UbiArtGame.RaymanJungleRun, Platform.Android)]
        RaymanJungleRunAndroid,

        [UbiArtGameModeInfo("Rayman Legends (PC)", UbiArtGame.RaymanLegends, Platform.PC)]
        RaymanLegendsPC,

        [UbiArtGameModeInfo("Rayman Legends (Wii U)", UbiArtGame.RaymanLegends, Platform.WiiU)]
        RaymanLegendsWiiU,

        [UbiArtGameModeInfo("Rayman Legends (PS Vita)", UbiArtGame.RaymanLegends, Platform.PSVita)]
        RaymanLegendsPSVita,

        [UbiArtGameModeInfo("Rayman Legends (PS4)", UbiArtGame.RaymanLegends, Platform.PlayStation4)]
        RaymanLegendsPS4,

        [UbiArtGameModeInfo("Rayman Legends (Switch)", UbiArtGame.RaymanLegends, Platform.NintendoSwitch)]
        RaymanLegendsSwitch,

        [UbiArtGameModeInfo("Rayman Fiesta Run (PC)", UbiArtGame.RaymanFiestaRun, Platform.PC)]
        RaymanFiestaRunPC,

        [UbiArtGameModeInfo("Rayman Fiesta Run (Android)", UbiArtGame.RaymanJungleRun, Platform.Android)]
        RaymanFiestaRunAndroid,

        [UbiArtGameModeInfo("Rayman Adventures (Android)", UbiArtGame.RaymanAdventures, Platform.Android)]
        RaymanAdventuresAndroid,

        [UbiArtGameModeInfo("Rayman Adventures (iOS)", UbiArtGame.RaymanAdventures, Platform.iOS)]
        RaymanAdventuresiOS,

        [UbiArtGameModeInfo("Rayman Mini (Mac)", UbiArtGame.RaymanMini, Platform.Mac)]
        RaymanMiniMac,

        [UbiArtGameModeInfo("Child of Light (PC)", UbiArtGame.ChildOfLight, Platform.PC)]
        ChildOfLightPC,

        [UbiArtGameModeInfo("Child of Light (PS Vita)", UbiArtGame.ChildOfLight, Platform.PSVita)]
        ChildOfLightPSVita,

        [UbiArtGameModeInfo("Valiant Hearts (Android)", UbiArtGame.ValiantHearts, Platform.Android)]
        ValiantHeartsAndroid,

        [UbiArtGameModeInfo("Just Dance 2017 (Wii U)", UbiArtGame.JustDance2017, Platform.WiiU)]
        JustDance2017WiiU,

        [UbiArtGameModeInfo("Gravity Falls (3DS)", UbiArtGame.GravityFalls, Platform.Nintendo3DS)]
        GravityFalls3DS,
    }
}