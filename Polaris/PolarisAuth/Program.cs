using System;
using System.IO;

using Polaris.Lib.Utility;

namespace Polaris.Auth
{
	public class Program
	{
		static Config config;

		public static void Main(string[] args)
		{
			if (!File.Exists("PolarisAuth.json"))
				Config.Create("PolarisAuth.json");

			config = Config.Load("PolarisAuth.json");

			Logger.WriteToFile = config.FileLogging;
			Logger.Init();

			Logger.Write("Hello world!");
			Logger.WriteInfo($"Client Version: {config.ClientVersion}");

			Console.ReadLine();
		}
	}
}
