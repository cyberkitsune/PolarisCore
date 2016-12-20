using Polaris.Lib.Extensions;
using Polaris.Lib.Packet.Common;
using Polaris.Lib.Packet.Packets;
using Polaris.Lib.Utility.Crypto;
using Polaris.Server.Modules.Logging;
using Polaris.Server.Modules.Ship;
using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Cryptography;
using static Polaris.Server.Shared.Common;

namespace Polaris.Server.Models
{
	public partial class Connection : IDisposable
	{
		private TcpClient _client;
		private ICryptoTransform _inputArc4 = null, _outputArc4 = null;

		public Block CurrentBlock { get; set; }
		public int ConnectionID { get; set; }
		public int BlockID { get { return CurrentBlock.BlockID; } }
		public Player Player { get; set; }

		internal static RSACryptoServiceProvider RSACSP = null;

		#region Events

		public delegate void ConnectionLostDelegate();
		public event ConnectionLostDelegate OnDisconnect;

		public delegate void PacketSendDelegate(byte[] packet);
		public event PacketSendDelegate BeforePacketSend;
		public event PacketSendDelegate AfterPacketSend;

		public delegate void PacketReceiveDelegate(byte[] packet, int size);
		public event PacketReceiveDelegate OnPacketReceive;

		public delegate void PacketProcessDelegate(PacketBase pkt);
		public event PacketProcessDelegate OnPacketProcess;

		#endregion

		public Connection(TcpClient c)
		{
			_client = c;
			OnDisconnect += Dispose;
			OnDisconnect += () => { if (CurrentBlock != null) CurrentBlock.PushQueue(new ParameterizedAction() { Parameters = new object[] { ConnectionID }, Type = ActionType.BLK_DISCONN }); };
		}

		~Connection()
		{
			OnDisconnectEvents();
		}

		public void Dispose()
		{
			_client.Close(true);
		}

		public async void SendPacket(PacketBase p)
		{
			if (!(p is IPacketSent))
				throw new Exception($"Packet { p.PacketID } does not implement IPacketSent");

			((IPacketSent)p).ConstructPayload();
			var pkt = p.Packet();

			BeforePacketSend(pkt);
			await _client.Client.SendAsync(new ArraySegment<byte>(pkt), SocketFlags.None);
			AfterPacketSend(pkt);
		}

		public static void Initialize(string RSAPrivateKeyFile)
		{
			RSACSP = new RSACryptoServiceProvider();
			RSACSP.ImportCspBlob(File.ReadAllBytes(RSAPrivateKeyFile));
		}

		public async void Listen()
		{
			ArraySegment<byte> buffer = new ArraySegment<byte>(new byte[MaxBufferSize]);

			try
			{
				#region Handshake (RSA Key Exchange)
				{
					int size = await _client.Client.ReceiveAsync(buffer, SocketFlags.None);
					var ID = PacketBase.GetPacketID(buffer.Array);
	
					//Decrypt 11.0B
					if (ID != 0x110B)
						throw new Exception($"Expected 11.0B, got {ID:X} instead.");

					PacketClientHandshake p = new PacketClientHandshake(buffer.Array);
					p.ParsePacket();
					byte[] decryptBlob = RSACSP.Decrypt(p.Blob, false);

					if (decryptBlob.Length < 0x20)
						throw new Exception($"11.0B Decrypt blob failed");

					var arc4Key = new byte[16];
					Array.Copy(decryptBlob, 0x10, arc4Key, 0, 0x10);

					var arc4 = new Arc4Managed { Key = arc4Key };
					_inputArc4 = arc4.CreateDecryptor();

					arc4 = new Arc4Managed { Key = arc4Key };
					_outputArc4 = arc4.CreateEncryptor();

					arc4 = new Arc4Managed { Key = arc4Key };
					var tempDecryptor = arc4.CreateDecryptor();

					//Encrypt message and send back
					var decryptedToken = new byte[16];
					tempDecryptor.TransformBlock(decryptBlob, 0, 0x10, decryptedToken, 0);

					var h = new PacketServerHandshake(0x11, 0x0C);
					h.Blob = decryptedToken;

					BeforePacketSend += (byte[] pkt) => { _outputArc4.TransformBlock(pkt, 0, pkt.Length, pkt, 0); };
					//TODO: Avoid the null check some other way, maybe
					AfterPacketSend += (byte[] pkt) => { };

					OnPacketReceive += (byte[] pkt, int s) => { _inputArc4.TransformBlock(pkt, 0, s, pkt, 0); };
					OnPacketProcess += (PacketBase pkt) => { ExecuteHandler(pkt); };

					SendPacket(h);
				}
				#endregion

				while (_client.Connected)
				{
					int size = await _client.Client.ReceiveAsync(buffer, SocketFlags.None);
					byte[] pkt = new byte[size];
					Array.Copy(buffer.Array, pkt, size);

					OnPacketReceive(pkt, size);

					ushort ID = PacketBase.GetPacketID(pkt);

					if (!PacketBase.PacketMap.ContainsKey(ID))
						throw new Exception($"Unknown packet type {ID:X4}");

					if (!typeof(IPacketRecv).IsAssignableFrom(PacketBase.PacketMap[ID]))
						throw new Exception($"Packet {ID:X4} does not implement IPacketRecv");

					PacketBase p = (PacketBase)Activator.CreateInstance(PacketBase.PacketMap[ID], pkt);
					((IPacketRecv)p).ParsePacket();

					OnPacketProcess(p);
				}
			}
			catch (CryptographicException ex)
			{
				//Probably a wrong key
				Log.WriteError($"[Connection [{BlockID}:{ConnectionID} ({_client.Client.RemoteEndPoint})] Failed to crypto with message: {ex.Message} (probably invalid key)");
			}
			catch (SocketException ex)
			{
				Log.WriteError($"[Connection [{BlockID}:{ConnectionID} ({_client.Client.RemoteEndPoint})] Disconnected with message: {ex.Message}");

			}
			catch (Exception ex)
			{
				Log.WriteError($"[Connection [{BlockID}:{ConnectionID} ({_client.Client.RemoteEndPoint})] {ex.Message}");
			}
			finally
			{
				OnDisconnectEvents();
			}
		}

		private void ExecuteHandler(PacketBase pkt)
		{
			//Send packet to a handler within the class to modify state as necessary
			throw new NotImplementedException();
		}

		private void OnDisconnectEvents()
		{
			OnDisconnect();
		}
	}
}
