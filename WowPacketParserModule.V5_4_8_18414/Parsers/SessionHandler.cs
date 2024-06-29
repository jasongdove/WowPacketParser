using System;
using WowPacketParser.Enums;
using WowPacketParser.Loading;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using CoreParsers = WowPacketParser.Parsing.Parsers;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class SessionHandler
    {
        [Parser(Opcode.SMSG_MOTD)]
        public static void HandleMessageOfTheDay(Packet packet)
        {
            var lineCount = packet.ReadBits("Line Count", 4);
            var lineLength = new int[lineCount];

            for (var i = 0; i < lineCount; i++)
                lineLength[i] = (int)packet.ReadBits(7);

            for (var i = 0; i < lineCount; i++)
                packet.ReadWoWString("Line", lineLength[i], i);
        }

        [Parser(Opcode.CMSG_PLAYER_LOGIN)]
        public static void HandlePlayerLogin(Packet packet)
        {
            var guid = new byte[8];

            packet.ReadSingle("Unk Float");

            packet.StartBitStream(guid, 1, 4, 7, 3, 2, 6, 5, 0);
            packet.ParseBitStream(guid, 5, 1, 0, 6, 2, 4, 7, 3);

            CoreParsers.SessionHandler.LoginGuid = new WowGuid64(BitConverter.ToUInt64(guid, 0));
            packet.Holder.PlayerLogin = new() { PlayerGuid = packet.WriteGuid("Guid", guid) };
        }

        [Parser(Opcode.SMSG_SET_TIME_ZONE_INFORMATION)]
        public static void HandleSetTimeZoneInformation(Packet packet)
        {
            var len1 = packet.ReadBits(7);
            var len2 = packet.ReadBits(7);
            packet.ReadWoWString("Server Location", len2);
            packet.ReadWoWString("Server Location", len1);
        }

        [Parser(Opcode.SMSG_LOGOUT_COMPLETE)]
        public static void HandleLogoutComplete(Packet packet)
        {
            var guid = new byte[8];

            packet.ReadBit(); // fake bit

            packet.StartBitStream(guid, 3, 2, 1, 4, 6, 7, 5, 0);
            packet.ParseBitStream(guid, 6, 4, 1, 2, 7, 3, 0, 5);

            packet.WriteGuid("Guid", guid);

            CoreParsers.SessionHandler.LoginGuid = new WowGuid64(BitConverter.ToUInt64(guid, 0));
        }

        [Parser(Opcode.SMSG_AUTH_CHALLENGE)]
        public static void HandleServerAuthChallenge(Packet packet)
        {
            packet.ReadUInt16("Unk1");
            packet.ReadBytes("Key", 32);
            packet.ReadByte("Unk Byte");
            packet.ReadUInt32("Server Seed");
        }

        [Parser(Opcode.SMSG_AUTH_RESPONSE)]
        public static void HandleAuthResponse(Packet packet)
        {
            var realmCount = 0u;
            var classCount = 0u;
            var raceCount = 0u;

            uint[] realmSizes = [];
            uint[] normalizedRealmSizes = [];
            bool[] homeRealms = [];

            var hasAccountData = packet.ReadBit("Has Account Data");
            if (hasAccountData)
            {
                realmCount = packet.ReadBits(21);

                realmSizes = new uint[realmCount];
                normalizedRealmSizes = new uint[realmCount];
                homeRealms = new bool[realmCount];

                for (var i = 0; i < realmCount; ++i)
                {
                    realmSizes[i] = packet.ReadBits(8);
                    normalizedRealmSizes[i] = packet.ReadBits(8);
                    homeRealms[i] = packet.ReadBit();
                }

                classCount = packet.ReadBits("Class Activation Count", 23);
                packet.ReadBits(21);
                packet.ReadBit();
                packet.ReadBit();
                packet.ReadBit();
                packet.ReadBit();

                raceCount = packet.ReadBits("Race Activation Count", 23);
                packet.ReadBit();
            }

            var isQueued = packet.ReadBit();
            if (isQueued)
            {
                packet.ReadBit("unk0");
                packet.ReadInt32("Queue Position");
            }

            if (hasAccountData)
            {
                for (var i = 0; i < realmCount; ++i)
                {
                    packet.ReadInt32("RealmId", i);
                    packet.ReadWoWString("Realm", realmSizes[i], i);
                    packet.ReadWoWString("Realm", normalizedRealmSizes[i], i);
                    packet.AddValue("Home Realm", homeRealms[i], i);
                }

                for (var i = 0; i < raceCount; ++i)
                {
                    packet.ReadByteE<ClientType>("Race Expansion", i);
                    packet.ReadByteE<Race>("Race", i);
                }

                for (var i = 0; i < classCount; ++i)
                {
                    packet.ReadByteE<ClientType>("Class Expansion", i);
                    packet.ReadByteE<Class>("Class", i);
                }

                packet.ReadUInt32();
                packet.ReadByteE<ClientType>("Active Expansion");
                packet.ReadUInt32();
                packet.ReadUInt32();
                packet.ReadByteE<ClientType>("Server Expansion");
                packet.ReadUInt32("Store Currency");
                packet.ReadUInt32();
                packet.ReadUInt32();
            }

            packet.ReadByteE<ResponseCode>("Auth Code");
        }
    }
}
