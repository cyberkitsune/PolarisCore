namespace Polaris.Lib.Packet
{
	/// Packets sent by the server and received by the client
	interface ISendPacket
    {
		byte[] ConstructPacket(PacketHeader header);
	}
}
