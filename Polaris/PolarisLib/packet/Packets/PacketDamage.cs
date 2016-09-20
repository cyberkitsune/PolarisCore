using PolarisLib.packet;
using System.IO;

namespace Polaris.Lib.Packet
{
	public class PacketDamage : Packet, ISendPacket
	{
		public uint playerID; // 0x8 - 0xB
		public byte[] unk1; // 0xC - 0x12, 8
		public uint targetID; // 0x14 - 0x17
		public byte[] unk2; // 0x18 - 0x1D, 6
		public ushort instanceID; // 0x1E - 1F
		public uint sourceID; // 0x20 - 0x23
		public byte[] unk3; // 0x24 - 0x2B, 8
		public uint atkID; // 0x2C - 0x2F
		public int value; // 0x30 - 0x33
		public byte[] unk4; // 0x34 - 0x43, 16
		public byte flags; // 0x44
		public byte[] unk5; // 0x45 - 0x50, 11

		public PacketDamage() : base()
		{
		}

		public byte[] ConstructPacket(PacketHeader header)
		{
			this.header = header;
			this.data = new byte[this.header.size - PacketHeader.HeaderSize];

			using (BinaryWriter writer = new BinaryWriter(new MemoryStream(this.data)))
			{
				writer.Write(playerID);
				writer.Write(unk1);
				writer.Write(targetID);
				writer.Write(unk2);
				writer.Write(instanceID);
				writer.Write(sourceID);
				writer.Write(unk3);
				writer.Write(atkID);
				writer.Write(value);
				writer.Write(unk4);
				writer.Write(flags);
				writer.Write(unk5);
			}

			BuildPacket();
			return this.packet;
		}
	}
}
