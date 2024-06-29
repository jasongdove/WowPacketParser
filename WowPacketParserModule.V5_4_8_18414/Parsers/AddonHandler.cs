using WowPacketParser.Enums;
using WowPacketParser.Misc;
using WowPacketParser.Parsing;

namespace WowPacketParserModule.V5_4_8_18414.Parsers
{
    public static class AddonHandler
    {
        [Parser(Opcode.SMSG_ADDON_INFO)]
        public static void HandleServerAddonsList(Packet packet)
        {
            packet.ReadBits("Banned Addons Count", 18);
            var addonsCount = packet.ReadBits("Addons Count", 23);

            var usePublicKey = new bool[addonsCount];
            var hasURL = new bool[addonsCount];
            var enabled = new bool[addonsCount];

            for (var i = 0; i < addonsCount; i++)
            {
                hasURL[i] = packet.ReadBit();
                enabled[i] = packet.ReadBit();
                usePublicKey[i] = packet.ReadBit();
            }

            for (var i = 0; i < addonsCount; i++)
            {
                if (usePublicKey[i])
                    packet.ReadBytes("Name MD5", 256, i);

                if (enabled[i])
                {
                    packet.ReadByte("Byte24", i);
                    packet.ReadInt32("Int24", i);
                }

                packet.ReadByte("Addon State", i);
            }
        }
    }
}
