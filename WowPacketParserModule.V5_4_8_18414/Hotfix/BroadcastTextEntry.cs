﻿using WowPacketParser.Enums;
using WowPacketParser.Hotfix;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V5_4_8_18414.Hotfix
{
    [HotfixStructure(DB2Hash.BroadcastText)]
    public class BroadcastTextEntry : IBroadcastTextEntry
    {
        public int ID { get; set; }
        public int Language { get; set; }
        public string Text { get; set; }
        public string Text1 { get; set; }
        [HotfixArray(3)]
        public int[] EmoteID { get; set; }
        [HotfixArray(3)]
        public uint[] EmoteDelay { get; set; }
        public uint SoundID { get; set; }
        public uint UnkEmoteID { get; set; }
        public uint UnkType { get; set; }
    }
}
