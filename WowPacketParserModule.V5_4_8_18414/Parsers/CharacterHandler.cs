using System;
using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;
using WowPacketParser.Store;
using WowPacketParser.Store.Objects;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class CharacterHandler
    {
        [Parser(Opcode.SMSG_ENUM_CHARACTERS_RESULT)]
        public static void HandleCharEnum(Packet packet)
        {
            var unkCounter = packet.ReadBits("Unk Counter", 21);
            var count = packet.ReadBits("Char count", 16);

            var charGuids = new byte[count][];
            var guildGuids = new byte[count][];
            var firstLogins = new bool[count];
            var nameLengths = new uint[count];
            var boosted = new bool[count];

            for (int c = 0; c < count; ++c)
            {
                charGuids[c] = new byte[8];
                guildGuids[c] = new byte[8];

                guildGuids[c][4] = packet.ReadBit();
                charGuids[c][0] = packet.ReadBit();
                guildGuids[c][3] = packet.ReadBit();
                charGuids[c][3] = packet.ReadBit();
                charGuids[c][7] = packet.ReadBit();
                boosted[c] = packet.ReadBit();
                firstLogins[c] = packet.ReadBit();
                charGuids[c][6] = packet.ReadBit();
                guildGuids[c][6] = packet.ReadBit();
                nameLengths[c] = packet.ReadBits(6);
                charGuids[c][1] = packet.ReadBit();
                guildGuids[c][1] = packet.ReadBit();
                guildGuids[c][0] = packet.ReadBit();
                charGuids[c][4] = packet.ReadBit();
                guildGuids[c][7] = packet.ReadBit();
                charGuids[c][2] = packet.ReadBit();
                charGuids[c][5] = packet.ReadBit();
                guildGuids[c][2] = packet.ReadBit();
                guildGuids[c][5] = packet.ReadBit();
            }

            packet.ReadBit("Success");

            packet.ResetBitReader();

            for (int c = 0; c < count; ++c)
            {
                var pos = new Vector3();

                packet.ReadUInt32("Unk 2", c);

                packet.ReadXORByte(charGuids[c], 1);

                packet.ReadByte("List Order", c);
                packet.ReadByte("HairStyle", c);

                packet.ReadXORByte(guildGuids[c], 2);
                packet.ReadXORByte(guildGuids[c], 0);
                packet.ReadXORByte(guildGuids[c], 6);

                var name = packet.ReadWoWString("Name", (int)nameLengths[c], c);

                packet.ReadXORByte(guildGuids[c], 3);

                pos.X = packet.ReadSingle();
                packet.ReadUInt32("Unk 1", c);
                packet.ReadByte("Face", c);
                var klass = packet.ReadByteE<Class>("Class", c);

                packet.AddValue("Boosted", boosted[c], c);

                packet.ReadXORByte(guildGuids[c], 5);

                for (int j = 0; j < 23; ++j)
                {
                    packet.ReadInt32("Item EnchantID", c, j);
                    packet.ReadInt32("Item DisplayID", c, j);
                    packet.ReadByteE<InventoryType>("Item InventoryType", c, j);
                }

                packet.ReadUInt32E<CustomizationFlag>("CustomizationFlag", c);

                packet.ReadXORByte(charGuids[c], 3);
                packet.ReadXORByte(charGuids[c], 5);

                packet.ReadInt32("Pet Family", c);

                packet.ReadXORByte(guildGuids[c], 4);

                var mapId = packet.ReadInt32("Map", c);
                var race = packet.ReadByteE<Race>("Race", c);
                packet.ReadByte("Skin", c);

                packet.ReadXORByte(guildGuids[c], 1);

                var level = packet.ReadByte("Level", c);

                packet.ReadXORByte(charGuids[c], 0);
                packet.ReadXORByte(charGuids[c], 2);

                packet.ReadByte("Hair Color", c);
                packet.ReadByteE<Gender>("Gender", c);
                packet.ReadByte("Facial Hair", c);

                packet.ReadInt32("Pet Level", c);

                packet.ReadXORByte(charGuids[c], 4);
                packet.ReadXORByte(charGuids[c], 7);

                pos.Y = packet.ReadSingle();
                packet.ReadInt32("Pet Display ID", c);
                packet.ReadUInt32("Unk 3", c);

                packet.ReadXORByte(charGuids[c], 6);

                packet.ReadInt32E<CharacterFlag>("CharacterFlag", c);
                var zone = packet.ReadUInt32<ZoneId>("Zone Id", c);

                packet.ReadXORByte(guildGuids[c], 7);

                pos.Z = packet.ReadSingle();

                var playerGuid = new WowGuid64(BitConverter.ToUInt64(charGuids[c], 0));

                packet.AddValue("Position", pos, c);
                packet.WriteGuid("Character GUID", charGuids[c], c);
                packet.WriteGuid("Guild GUID", guildGuids[c], c);

                if (firstLogins[c])
                {
                    PlayerCreateInfo startPos = new PlayerCreateInfo { Race = race, Class = klass, Map = (uint)mapId, Zone = zone, Position = pos, Orientation = 0 };
                    Storage.StartPositions.Add(startPos, packet.TimeSpan);
                }

                var playerInfo = new Player { Race = race, Class = klass, Name = name, FirstLogin = firstLogins[c], Level = level, Type = ObjectType.Player };
                if (Storage.Objects.ContainsKey(playerGuid))
                    Storage.Objects[playerGuid] = new Tuple<WoWObject, TimeSpan?>(playerInfo, packet.TimeSpan);
                else
                    Storage.Objects.Add(playerGuid, playerInfo, packet.TimeSpan);
                StoreGetters.AddName(playerGuid, name);
            }

            for (var i = 0; i < unkCounter; ++i)
            {
                packet.ReadUInt32("Unk int", i);
                packet.ReadByte("Unk byte", i);
            }
        }

        [Parser(Opcode.SMSG_POWER_UPDATE)]
        public static void HandlePowerUpdate(Packet packet)
        {
            var guid = new byte[8];

            packet.StartBitStream(guid, 4, 6, 7, 5, 2, 3, 0, 1);

            var count = packet.ReadBits("Count", 21);

            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 2);
            packet.ReadXORByte(guid, 4);

            for (var i = 0; i < count; i++)
            {
                packet.ReadByteE<PowerType>("Power type"); // Actually powertype for class
                packet.ReadInt32("Value");
            }

            packet.ReadXORByte(guid, 6);

            packet.WriteGuid("Guid", guid);
        }

        [Parser(Opcode.SMSG_SETUP_CURRENCY)]
        public static void HandleInitCurrency(Packet packet)
        {
            var count = packet.ReadBits("Count", 21);
            if (count == 0)
                return;

            var hasWeekCount = new bool[count];
            var hasWeekCap = new bool[count];
            var hasSeasonTotal = new bool[count];
            var flags = new uint[count];

            for (var i = 0; i < count; ++i)
            {
                hasWeekCount[i] = packet.ReadBit();     // +28
                flags[i] = packet.ReadBits(5);          // +32
                hasWeekCap[i] = packet.ReadBit();       // +20
                hasSeasonTotal[i] = packet.ReadBit();   // +12
            }

            for (var i = 0; i < count; ++i)
            {
                packet.AddValue("Flags", flags[i], i); // +32

                if (hasSeasonTotal[i])
                    packet.ReadUInt32("Season total earned", i);    // +12

                packet.ReadUInt32("Currency id", i);    // +5

                if (hasWeekCount[i])
                    packet.ReadUInt32("Weekly count", i);    // +28

                packet.ReadUInt32("Currency count", i);    // +4

                if (hasWeekCap[i])
                    packet.ReadUInt32("Weekly cap", i);    // +20
            }
        }

        [Parser(Opcode.SMSG_LOG_XP_GAIN)]
        public static void HandleLogXPGain(Packet packet)
        {
            var guid = new byte[8];
            var hasBaseXP = !packet.ReadBit();
            guid[1] = packet.ReadBit();
            guid[2] = packet.ReadBit();
            guid[7] = packet.ReadBit();
            guid[4] = packet.ReadBit();
            guid[3] = packet.ReadBit();
            packet.ReadBit("Unk Bit");
            guid[0] = packet.ReadBit();
            guid[5] = packet.ReadBit();
            guid[6] = packet.ReadBit();
            var hasGroupRate = !packet.ReadBit();

            packet.ReadXORByte(guid, 4);
            packet.ReadXORByte(guid, 2);
            packet.ReadByte("XP type");

            if (hasGroupRate)
                packet.ReadSingle("Group rate");

            packet.ReadXORByte(guid, 7);
            packet.ReadXORByte(guid, 1);
            packet.ReadXORByte(guid, 3);
            packet.ReadXORByte(guid, 6);

            packet.ReadUInt32("Total XP");

            if (hasBaseXP)
                packet.ReadUInt32("Base XP");

            packet.ReadXORByte(guid, 0);
            packet.ReadXORByte(guid, 5);

            packet.WriteGuid("Guid", guid);
        }
    }
}
