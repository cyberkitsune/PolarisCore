using System;
using System.IO;

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

			Console.WriteLine("Hello world!");
			Console.WriteLine($"Client Version: {config.ClientVersion}");

			Console.ReadLine();
		}
	}
}
