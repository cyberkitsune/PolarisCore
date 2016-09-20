using System;
using System.IO;

using Polaris.Lib.Utility;

namespace Polaris.Auth
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (!File.Exists("PolarisAuth.json"))
				Config.Create("PolarisAuth.json");

			Config.Load("PolarisAuth.json");

			Logger.WriteToFile = Config.Instance.FileLogging;
			Logger.Init();

			Logger.Write("Hello world!");
			Logger.WriteInfo($"Client Version: {Config.Instance.ClientVersion}");

			Console.ReadLine();
		}
	}
}
