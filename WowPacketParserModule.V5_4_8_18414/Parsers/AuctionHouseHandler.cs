using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class AuctionHouseHandler
    {
        [Parser(Opcode.CMSG_AUCTION_REPLICATE_ITEMS)]
        public static void HandleAuctionReplicateItems(Packet packet)
        {
            // packet.ReadInt32("ChangeNumberGlobal");
            // packet.ReadInt32("ChangeNumberCursor");
            // packet.ReadInt32("ChangeNumberTombstone");
            // packet.ReadInt32("Count");

            packet.ReadUInt32("Unk1");
            packet.ReadUInt32("Unk2");
            packet.ReadUInt32("Unk3");
            packet.ReadUInt32("Unk4");

            var guid = new byte[8];
            packet.StartBitStream(guid, 6, 0, 1, 5, 3, 2, 4, 7);
            packet.ParseBitStream(guid, 7, 0, 6, 4, 1, 5, 3, 2);

            packet.WriteGuid("Auctioneer", guid);
        }
    }
}
