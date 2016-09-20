using System;
using System.IO;

namespace Polaris.Lib.Utility
{
    public static class Logger
    {
        private static StreamWriter writer = File.CreateText("Polaris.log");

        public static void Write(string text, params object[] args)
        {
            WriteLine(ConsoleColor.White, string.Format(text, args));
            WriteFile(text, args);
        }

        public static void WriteInfo(string text, params object[] args)
        {
            WriteLine(ConsoleColor.Green, string.Format(text, args));
            WriteFile(text, args);
        }

        public static void WriteMessage(string text, params object[] args)
        {
            WriteLine(ConsoleColor.Cyan, string.Format(text, args));
            WriteFile(text, args);
        }

        public static void WriteWarning(string text, params object[] args)
        {
            WriteLine(ConsoleColor.Yellow, string.Format(text, args));
            WriteFile(text, args);
        }

        public static void WriteError(string text, params object[] args)
        {
            WriteLine(ConsoleColor.Red, string.Format(text, args));
            WriteFile(text, args);
        }

        public static void WriteException(string message, Exception e)
        {
            string text = string.Empty;

            text += string.Format("{0} - {1}: {2}", message, e.GetType(), e);
            if (e.InnerException != null)
                text += string.Format("\n[Error] Inner Exception: {0}", e.InnerException);

            WriteError(text);
        }

        public static void WriteHex(string text, byte[] array)
        {
            WriteLine(ConsoleColor.DarkCyan, text);

            // Calculate lines
            int lines = 0;
            for (int i = 0; i < array.Length; i++)
                if ((i % 16) == 0)
                    lines++;

            for (int i = 0; i < lines; i++)
            {
                string hexString = string.Empty;

                // Address
                hexString += string.Format("{0:X8} ", i * 16);

                // Bytes
                for (int j = 0; j < 16; j++)
                {
                    if (j + (i * 16) >= array.Length)
                        break;

                    hexString += string.Format("{0:X2} ", array[j + (i * 16)]);
                }

                // Spacing
                while (hexString.Length < 16 * 4)
                    hexString += ' ';

                // ASCII
                for (int j = 0; j < 16; j++)
                {
                    if (j + (i * 16) >= array.Length)
                        break;

                    var asciiChar = (char)array[j + (i * 16)];

                    if (asciiChar == (char)0x00)
                        asciiChar = '.';

                    hexString += asciiChar;
                }

                // Strip off unnecessary stuff
                hexString = hexString.Replace('\a', ' '); // Alert beeps
                hexString = hexString.Replace('\n', ' '); // Newlines
                hexString = hexString.Replace('\r', ' '); // Carriage returns
                hexString = hexString.Replace('\\', ' '); // Escape break

                WriteLine(ConsoleColor.Cyan, text);
                WriteFile(text);
            }
        }

        public static void WriteFile(string text, params object[] args)
        {
            if (args.Length > 0)
                writer.WriteLine(DateTime.Now + " - " + text, args);
            else
                writer.WriteLine(DateTime.Now + " - " + text);

            writer.Flush();
        }

        private static void WriteLine(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }
}
