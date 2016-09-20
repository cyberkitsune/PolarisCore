using System;
using System.IO;

using Newtonsoft.Json;

namespace Polaris.Auth
{
	public class Config
	{
		public static Config Instance { get; set; } = new Config();

		public string ClientVersion { get; set; } = "4.0402.1";

		public string BindIP { get; set; } = "127.0.0.1";
		public uint Port { get; set; } = 12300;

		public string RSAPublicKey { get; set; } = "publicKey.blob"; // File Path
		public string RSAPrivateKey { get; set; } = "privateKey.blob"; // File Path

		public string DatabaseHost { get; set; } = "127.0.0.1";
		public string DatabaseUsername { get; set; } = "Polaris";
		public string DatabasePassword { get; set; } = "Polaris";
		public string DatabaseName { get; set; } = "Polaris";

		public bool FileLogging { get; set; } = true;

		public static void Create(string filename)
		{
			File.WriteAllText(filename, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
		}

		public static void Load(string filename)
		{
			Instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(filename));
		}
	}
}
