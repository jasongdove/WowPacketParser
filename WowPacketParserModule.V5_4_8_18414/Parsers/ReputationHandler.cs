using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class ReputationHandler
    {
        [Parser(Opcode.CMSG_RESET_FACTION_CHEAT)]
        public static void HandleResetFactionCheat(Packet packet)
        {
            packet.ReadUInt32("Faction Id");
            packet.ReadUInt32("Unk Int");
        }

        [Parser(Opcode.SMSG_SET_FORCED_REACTIONS)]
        public static void HandleForcedReactions(Packet packet)
        {
            var counter = packet.ReadBits("Factions", 6);
            for (var i = 0; i < counter; i++)
            {
                packet.ReadUInt32("Faction Id");
                packet.ReadUInt32("Reputation Rank");
            }
        }

        [Parser(Opcode.SMSG_INITIALIZE_FACTIONS)]
        public static void HandleInitializeFactions(Packet packet)
        {
            var factionFlags = new FactionFlag[256];
            var factionStandings = new ReputationRank[256];

            for (var i = 0; i < 256; i++)
            {
                factionFlags[i] = packet.ReadByteE<FactionFlag>();
                factionStandings[i] = packet.ReadUInt32E<ReputationRank>();
            }

            for (var i = 0; i < 256; i++)
            {
                packet.AddValue("Faction Flag", factionFlags[i], i);
                packet.AddValue("Faction Standing", factionStandings[i], i);
                packet.ReadBit("Bonus Reputation", i);
            }
        }
    }
}
