using Polaris.Lib.Packet.Common;
using System;
using System.IO;
using System.Text;

namespace Polaris.Lib.packet.Packets
{
    public class PacketSystemMessage : PacketBase, IPacketSent
    {
        public PacketSystemMessage(byte type, byte subType) : base(type, subType)
        {
            Header.flag1 = 0x04;
            Header.flag2 = 0x00;
        }

        public override uint PKT_SUB { get { return 0xA2; } }
        public override uint PKT_XOR { get { return 0x78F7; } }

        public enum MessageType : uint
        {
            GoldenTicker = 0,
            AdminNotice,
            AdminMessage,
            SystemMessage,
            GenericMessage
        }

        public string Message;
        public MessageType Type;

        public void ConstructPayload()
        {
            Header.size = (uint)(HeaderSize + (Message.Length * 2) + sizeof(UInt32));
            Payload = new byte[Header.size - HeaderSize];
            using (BinaryWriter bw = new BinaryWriter(new MemoryStream(Payload)))
            {
                bw.Write(AddXor((uint)(Message.Length + 1)));
                bw.Write(Encoding.GetEncoding("UTF-16").GetBytes(Message));
                if (((Message.Length + 1) & 1) != 0)    // Apply padding as needed
                {
                    bw.Write((ushort)0);
                }
                bw.Write((uint)Type);
            }
        }
    }
}
