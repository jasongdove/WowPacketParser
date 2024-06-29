using System;

namespace WowPacketParser.Enums
{
    public enum TargetedDatabase : int
    {
        Zero                = 0,
        TheBurningCrusade   = 1,
        WrathOfTheLichKing  = 2,
        Cataclysm           = 3,
        WarlordsOfDraenor   = 4,
        Legion              = 5,
        BattleForAzeroth    = 6,
        Shadowlands         = 7,
        Dragonflight        = 8,

        // chose higher value to have some room for future
        Classic             = 20,
        WotlkClassic        = 21,
        CataClassic         = 22,

        MistsOfPandaria     = 30
    }

    [Flags]
    public enum TargetedDatabaseFlag : uint
    {
        // Retail
        Zero                              = 1 << TargetedDatabase.Zero,
        TheBurningCrusade                 = 1 << TargetedDatabase.TheBurningCrusade,
        WrathOfTheLichKing                = 1 << TargetedDatabase.WrathOfTheLichKing,
        Cataclysm                         = 1 << TargetedDatabase.Cataclysm,
        MistsOfPandaria                   = 1 << TargetedDatabase.MistsOfPandaria,
        WarlordsOfDraenor                 = 1 << TargetedDatabase.WarlordsOfDraenor,
        Legion                            = 1 << TargetedDatabase.Legion,
        BattleForAzeroth                  = 1 << TargetedDatabase.BattleForAzeroth,
        Shadowlands                       = 1 << TargetedDatabase.Shadowlands,
        Dragonflight                      = 1 << TargetedDatabase.Dragonflight,
        AnyRetail                         = TheBurningCrusade | WrathOfTheLichKing | Cataclysm | MistsOfPandaria | WarlordsOfDraenor | Legion | BattleForAzeroth | Shadowlands | Dragonflight,

        // Classic
        Classic                           = 1 << TargetedDatabase.Classic,
        WotlkClassic                      = 1 << TargetedDatabase.WotlkClassic,
        CataClassic                       = 1 << TargetedDatabase.CataClassic,
        AnyClassic                        = Classic | WotlkClassic | CataClassic,

        Any                               = AnyRetail | AnyClassic,

        // predefines
        TillWrathOfTheLichKing            = TheBurningCrusade | WrathOfTheLichKing,
        TillCataclysm                     = TheBurningCrusade | WrathOfTheLichKing | Cataclysm,
        TillMistsOfPandaria               = TheBurningCrusade | WrathOfTheLichKing | Cataclysm | MistsOfPandaria,
        TillWarlordsOfDraenor             = TheBurningCrusade | WrathOfTheLichKing | Cataclysm | MistsOfPandaria | WarlordsOfDraenor,
        TillLegion                        = TheBurningCrusade | WrathOfTheLichKing | Cataclysm | MistsOfPandaria | WarlordsOfDraenor | Legion,
        TillBattleForAzeroth              = TheBurningCrusade | WrathOfTheLichKing | Cataclysm | MistsOfPandaria | WarlordsOfDraenor | Legion | BattleForAzeroth,
        TillShadowlands                   = TheBurningCrusade | WrathOfTheLichKing | Cataclysm | MistsOfPandaria | WarlordsOfDraenor | Legion | BattleForAzeroth | Shadowlands,

        FromCataclysmTillBattleForAzeroth = Cataclysm | MistsOfPandaria | WarlordsOfDraenor | Legion | BattleForAzeroth,

        // update us when new expansion arrives
        SinceCataclysm                    = Cataclysm | MistsOfPandaria | WarlordsOfDraenor | Legion | BattleForAzeroth | Shadowlands | Dragonflight,
        SinceWarlordsOfDraenor            = WarlordsOfDraenor | Legion | BattleForAzeroth | Shadowlands | Dragonflight,
        SinceLegion                       = Legion | BattleForAzeroth | Shadowlands | Dragonflight,
        SinceBattleForAzeroth             = BattleForAzeroth | Shadowlands | Dragonflight,
        SinceShadowlands                  = Shadowlands | Dragonflight,
        SinceDragonflight                 = Dragonflight,

        SinceWarlordsOfDraenorTillShadowLands = WarlordsOfDraenor | Legion | BattleForAzeroth | Shadowlands
    }
}
