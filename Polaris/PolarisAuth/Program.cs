using System;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;

using Polaris.Lib.Utility;


namespace Polaris.Auth
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Logger.Write("Starting Authentication Server");
			Logger.Write("Loading Configuration from PolarisAuth.json...");
			InitConfig();
			WriteHeaderInfo();
			Logger.Write("Checking for RSA Keys...");
			CheckGenerateRSAKeys();
			Logger.Write("Connecting to authentication database...");
			CheckTestConnectAuthDB();
			Logger.Write("Authentication Server ready");

			//Setup and start listener thread
			SetupStartListener();
			Logger.Write($"Listening for connections on {Config.Instance.BindIP}:{Config.Instance.Port}...");
		}

		private static void SetupStartListener()
		{
			// TODO
		}

		private static void InitConfig()
		{
			if (!File.Exists("PolarisAuth.json"))
			{
				Logger.WriteWarning("Configuration file does not exist, creating default configuration...");
				Config.Create("PolarisAuth.json");
			}

			Config.Load("PolarisAuth.json");
			Logger.WriteToFile = Config.Instance.FileLogging;
		}

		private static void CheckTestConnectAuthDB()
		{
			//TODO
		}

		private static void WriteHeaderInfo()
		{
			//TODO: Include version info and other configurations in here
			Logger.WriteInfo($"Client Version: {Config.Instance.ClientVersion}");
		}

		private static void CheckGenerateRSAKeys()
		{
			// TODO: Use configurations for the file paths
			string keyPublic = Config.Instance.RSAPublicKey;
			string keyPrivate = Config.Instance.RSAPrivateKey;

			if (!File.Exists(keyPrivate) || !File.Exists(keyPublic))
			{
				if (!File.Exists(keyPrivate))
					Logger.WriteWarning($"Could not find existing private key at ${keyPrivate}");
				if (!File.Exists(keyPublic))
					Logger.WriteWarning($"Could not find existing private key at ${keyPublic}");

				Logger.WriteInfo("Creating new RSA key pair.");
				RSACryptoServiceProvider rcsp = new RSACryptoServiceProvider();

				using (FileStream outFile = File.Create(keyPrivate))
				{
					byte[] cspBlob = rcsp.ExportCspBlob(true);
					outFile.Write(cspBlob, 0, cspBlob.Length);
					Logger.WriteInfo($"Generated Private Key at {keyPrivate}");
				}

				using (FileStream outFile = File.Create(keyPublic))
				{
					byte[] cspBlob = rcsp.ExportCspBlob(false);
					outFile.Write(cspBlob, 0, cspBlob.Length);
					Logger.WriteInfo($"Generated Public Key at {keyPublic}");
				}
			}
		}
	}
}
