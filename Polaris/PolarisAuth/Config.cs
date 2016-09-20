using System;
using System.IO;

using Newtonsoft.Json;

namespace Polaris.Auth
{
	public class Config
	{
		public string ClientVersion { get; } = "4.0402.1";

		public string BindIP { get; } = "127.0.0.1";
		public uint Port { get; } = 12300;

		public string RSAPublicKey { get; } = "publicKey.blob"; // File Path
		public string RSAPrivateKey { get; } = "privateKey.blob"; // File Path

		public string DatabaseHost { get; } = "127.0.0.1";
		public string DatabaseUsername { get; } = "Polaris";
		public string DatabasePassword { get; } = "Polaris";
		public string DatabaseName { get; } = "Polaris";

		public static void Create(string filename)
		{
			File.WriteAllText(filename, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
		}

		public static Config Load(string filename)
		{
			return JsonConvert.DeserializeObject<Config>(File.ReadAllText(filename));
		}
	}
}
