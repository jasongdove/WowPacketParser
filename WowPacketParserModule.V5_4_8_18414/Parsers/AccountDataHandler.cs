using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class AccountDataHandler
    {
        [Parser(Opcode.SMSG_ACCOUNT_DATA_TIMES)]
        public static void HandleAccountDataTimes(Packet packet)
        {
            packet.ReadBit("Unk Bit");

            for (var i = 0; i < 8; ++i)
            {
                packet.ReadTime("[" + (AccountDataType)i + "]" + " Time");
            }

            packet.ReadUInt32("unk24");
            packet.ReadTime("Server Time");
        }

        [Parser(Opcode.SMSG_UPDATE_ACCOUNT_DATA)]
        public static void HandleServerUpdateAccountData(Packet packet)
        {
            var guid = new byte[8];

            packet.ReadBitsE<AccountDataType>("Data Type", 3);

            packet.StartBitStream(guid, 5, 1, 3, 7, 0, 4, 2, 6);

            packet.ReadXORBytes(guid, 3, 1, 5);

            var decompCount = packet.ReadInt32();
            var compCount = packet.ReadInt32();

            var pkt = packet.Inflate(compCount, decompCount, false);
            var data = pkt.ReadWoWString(decompCount);
            pkt.ClosePacket(false);

            packet.AddValue("Account Data", data);

            packet.ReadXORBytes(guid, 7, 4, 0, 6, 2);

            packet.ReadTime("Login Time");

            packet.WriteGuid("GUID", guid);
        }

        [Parser(Opcode.CMSG_REQUEST_ACCOUNT_DATA)]
        public static void HandleRequestAccountData(Packet packet)
        {
            packet.ReadBitsE<AccountDataType>("Data Type", 3);
        }

        [Parser(Opcode.CMSG_UPDATE_ACCOUNT_DATA)]
        public static void HandleClientUpdateAccountData(Packet packet)
        {
            // TODO: this is broken
            packet.ReadToEnd();

            // var decompCount = packet.ReadInt32();
            // packet.ReadTime("Login Time");
            // var compCount = packet.ReadInt32();
            // var pkt = packet.Inflate(compCount, decompCount, false);
            // var data = pkt.ReadWoWString(decompCount);
            // data = System.Text.RegularExpressions.Regex.Replace(data, @"\p{C}+", string.Empty);
            // pkt.ClosePacket();
            // packet.ReadBitsE<AccountDataType>("Data Type", 3);
            // packet.AddValue("Account Data", data);
        }
    }
}