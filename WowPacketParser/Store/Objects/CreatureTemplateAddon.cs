﻿using WowPacketParser.Enums;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("creature_template_addon")]
    public sealed record CreatureTemplateAddon : IDataModel
    {
        [DBFieldName("entry", true)]
        public uint? Entry;

        [DBFieldName("path_id", TargetedDatabaseFlag.TillShadowlands)]
        [DBFieldName("PathId", TargetedDatabaseFlag.SinceDragonflight)]
        public uint? PathID;

        [DBFieldName("mount")]
        public uint? MountID;

        [DBFieldName("StandState")]
        public byte? StandState;

        [DBFieldName("AnimTier")]
        public byte? AnimTier;

        [DBFieldName("VisFlags")]
        public byte? VisFlags;

        [DBFieldName("SheathState")]
        public byte? SheathState;

        [DBFieldName("PvpFlags")]
        public byte? PvpFlags;

        [DBFieldName("emote")]
        public uint? Emote;

        [DBFieldName("ai_anim_kit", TargetedDatabaseFlag.MistsOfPandaria)]
        [DBFieldName("aiAnimKit", TargetedDatabaseFlag.SinceLegion | TargetedDatabaseFlag.WotlkClassic)]
        public ushort? AIAnimKit;

        [DBFieldName("movement_anim_kit", TargetedDatabaseFlag.MistsOfPandaria)]
        [DBFieldName("movementAnimKit", TargetedDatabaseFlag.SinceLegion | TargetedDatabaseFlag.WotlkClassic)]
        public ushort? MovementAnimKit;

        [DBFieldName("melee_anim_kit", TargetedDatabaseFlag.MistsOfPandaria)]
        [DBFieldName("meleeAnimKit", TargetedDatabaseFlag.SinceLegion | TargetedDatabaseFlag.WotlkClassic)]
        public ushort? MeleeAnimKit;

        // visibilityDistanceType exists in all database versions but because UnitFlags2 to detect the value from sniff doesn't exist in earlier client version
        // we pretend the field doesn't exist
        [DBFieldName("visibilityDistanceType", TargetedDatabaseFlag.SinceWarlordsOfDraenor | TargetedDatabaseFlag.WotlkClassic)]
        public byte? VisibilityDistanceType;

        [DBFieldName("auras")]
        public string Auras;

        public string CommentAuras;
    }
}
