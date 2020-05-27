using RayCarrot.Rayman.OpenSpace;
using RayCarrot.Rayman.Ray1;
using RayCarrot.Rayman.UbiArt;

namespace RayCarrot.Rayman
{
    /// <summary>
    /// The OpenSpace game modes
    /// </summary>
    public enum GameMode
    {
        #region Ray1

        [Ray1GameModeInfo("Rayman (PC)", Ray1Game.Rayman1, Platform.PC)]
        Rayman1PC,

        [Ray1GameModeInfo("Rayman Designer (PC)", Ray1Game.RayKit, Platform.PC)]
        RayKitPC,

        #endregion

        #region OpenSpace

        [OpenSpaceGameModeInfo("Rayman 2 (PC)", OpenSpaceGame.Rayman2, Platform.PC)]
        Rayman2PC,

        [OpenSpaceGameModeInfo("Rayman 2 (DreamCast)", OpenSpaceGame.Rayman2, Platform.DreamCast)] 
        Rayman2DC,

        [OpenSpaceGameModeInfo("Rayman 2 (iOS)", OpenSpaceGame.Rayman2, Platform.iOS)] 
        Rayman2IOS,

        [OpenSpaceGameModeInfo("Rayman 2 (PlayStation 2)", OpenSpaceGame.Rayman2Revolution, Platform.PlayStation2)] 
        Rayman2PS2,

        [OpenSpaceGameModeInfo("Rayman 2 (Nintendo 64)", OpenSpaceGame.Rayman2, Platform.Nintendo64)] 
        Rayman2N64,

        [OpenSpaceGameModeInfo("Rayman 2 (Nintendo DS)", OpenSpaceGame.Rayman2, Platform.NintendoDS)]
        Rayman2DS,

        [OpenSpaceGameModeInfo("Rayman 2 (Nintendo 3DS)", OpenSpaceGame.Rayman2, Platform.Nintendo3DS)] 
        Rayman23DS,

        [OpenSpaceGameModeInfo("Rayman 2 Demo 1 (PC)", OpenSpaceGame.Rayman2Demo, Platform.PC)] 
        Rayman2PCDemo1,

        [OpenSpaceGameModeInfo("Rayman 2 Demo 2 (PC)", OpenSpaceGame.Rayman2Demo, Platform.PC)]
        Rayman2PCDemo2,

        [OpenSpaceGameModeInfo("Rayman M (PC)", OpenSpaceGame.RaymanM, Platform.PC)] 
        RaymanMPC,

        [OpenSpaceGameModeInfo("Rayman Arena (PC)", OpenSpaceGame.RaymanArena, Platform.PC)] 
        RaymanArenaPC,

        [OpenSpaceGameModeInfo("Rayman Arena (GameCube)", OpenSpaceGame.RaymanArena, Platform.GameCube)] 
        RaymanArenaGC,

        [OpenSpaceGameModeInfo("Rayman 3 (PC)", OpenSpaceGame.Rayman3, Platform.PC)] 
        Rayman3PC,

        [OpenSpaceGameModeInfo("Rayman 3 (GameCube)", OpenSpaceGame.Rayman3, Platform.GameCube)] 
        Rayman3GC,

        [OpenSpaceGameModeInfo("Rayman Raving Rabbids (Nintendo DS)", OpenSpaceGame.RaymanRavingRabbids, Platform.NintendoDS)] 
        RaymanRavingRabbidsDS,

        [OpenSpaceGameModeInfo("Tonic Trouble (PC)", OpenSpaceGame.TonicTrouble, Platform.PC)] 
        TonicTroublePC,

        [OpenSpaceGameModeInfo("Tonic Trouble Special Edition (PC)", OpenSpaceGame.TonicTroubleSpecialEdition, Platform.PC)]
        TonicTroubleSEPC,

        [OpenSpaceGameModeInfo("Donald Duck: Quack Attack (PC)", OpenSpaceGame.DonaldDuckQuackAttack, Platform.PC)]
        DonaldDuckPC,

        [OpenSpaceGameModeInfo("Donald Duck: Quack Attack (DreamCast)", OpenSpaceGame.DonaldDuckQuackAttack, Platform.DreamCast)] 
        DonaldDuckDC,

        [OpenSpaceGameModeInfo("Donald Duck: Quack Attack (Nintendo 64)", OpenSpaceGame.DonaldDuckQuackAttack, Platform.Nintendo64)] 
        DonaldDuckN64,

        [OpenSpaceGameModeInfo("Donald Duck: PK (GameCube)", OpenSpaceGame.DonaldDuckPK, Platform.GameCube)] 
        DonaldDuckPKGC,

        [OpenSpaceGameModeInfo("Playmobil: Hype (PC)", OpenSpaceGame.PlaymobilHype, Platform.PC)] 
        PlaymobilHypePC,

        [OpenSpaceGameModeInfo("Playmobil: Laura (PC)", OpenSpaceGame.PlaymobilLaura, Platform.PC)] 
        PlaymobilLauraPC,

        [OpenSpaceGameModeInfo("Playmobil: Alex (PC)", OpenSpaceGame.PlaymobilAlex, Platform.PC)]
        PlaymobilAlexPC,

        [OpenSpaceGameModeInfo("Disney's Dinosaur (PC)", OpenSpaceGame.Dinosaur, Platform.PC)]
        DinosaurPC,

        [OpenSpaceGameModeInfo("Largo Winch (PC)", OpenSpaceGame.LargoWinch, Platform.PC)]
        LargoWinchPC,

        #endregion

        #region UbiArt

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

        #endregion
    }
}