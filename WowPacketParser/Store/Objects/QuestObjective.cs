using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.SQL;

namespace WowPacketParser.Store.Objects
{
    [DBTableName("quest_objective", TargetedDatabaseFlag.MistsOfPandaria)]
    [DBTableName("quest_objectives", TargetedDatabaseFlag.Any ^ TargetedDatabaseFlag.MistsOfPandaria)]
    public sealed record QuestObjective : IDataModel
    {
        [DBFieldName("id", TargetedDatabaseFlag.MistsOfPandaria, true)]
        [DBFieldName("ID", TargetedDatabaseFlag.Any ^ TargetedDatabaseFlag.MistsOfPandaria, true)]
        public uint? ID;

        [DBFieldName("questId", TargetedDatabaseFlag.MistsOfPandaria)]
        [DBFieldName("QuestID", TargetedDatabaseFlag.Any ^ TargetedDatabaseFlag.MistsOfPandaria)]
        public uint? QuestID;

        [DBFieldName("type", TargetedDatabaseFlag.MistsOfPandaria)]
        [DBFieldName("Type", TargetedDatabaseFlag.Any ^ TargetedDatabaseFlag.MistsOfPandaria)]
        public QuestRequirementType? Type;

        [DBFieldName("Order", TargetedDatabaseFlag.SinceLegion | TargetedDatabaseFlag.AnyClassic)]
        public uint? Order;

        [DBFieldName("index", TargetedDatabaseFlag.MistsOfPandaria)]
        [DBFieldName("StorageIndex", TargetedDatabaseFlag.Any ^ TargetedDatabaseFlag.MistsOfPandaria)]
        public int? StorageIndex;

        [DBFieldName("objectId", TargetedDatabaseFlag.MistsOfPandaria)]
        [DBFieldName("ObjectID", TargetedDatabaseFlag.Any ^ TargetedDatabaseFlag.MistsOfPandaria)]
        public int? ObjectID;

        [DBFieldName("amount", TargetedDatabaseFlag.MistsOfPandaria)]
        [DBFieldName("Amount", TargetedDatabaseFlag.Any ^ TargetedDatabaseFlag.MistsOfPandaria)]
        public int? Amount;

        [DBFieldName("flags", TargetedDatabaseFlag.MistsOfPandaria)]
        [DBFieldName("Flags", TargetedDatabaseFlag.Any ^ TargetedDatabaseFlag.MistsOfPandaria)]
        public uint? Flags;

        [DBFieldName("Flags2", TargetedDatabaseFlag.SinceLegion | TargetedDatabaseFlag.AnyClassic)] // 7.1.0
        public uint? Flags2;

        [DBFieldName("ProgressBarWeight", TargetedDatabaseFlag.Any ^ TargetedDatabaseFlag.MistsOfPandaria)]
        public float? ProgressBarWeight;

        [DBFieldName("description", TargetedDatabaseFlag.MistsOfPandaria, LocaleConstant.enUS)]
        [DBFieldName("Description", TargetedDatabaseFlag.Any ^ TargetedDatabaseFlag.MistsOfPandaria, LocaleConstant.enUS)]
        public string Description;

        [DBFieldName("VerifiedBuild", TargetedDatabaseFlag.Any ^ TargetedDatabaseFlag.MistsOfPandaria)]
        public int? VerifiedBuild = ClientVersion.BuildInt;
    }
}