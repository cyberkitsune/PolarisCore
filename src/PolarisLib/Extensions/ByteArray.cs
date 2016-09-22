using System.Text;

namespace Polaris.Lib.Extensions
{
	public static class ByteArrayExtensions
	{
		public static string ToString(this byte[] array)
		{
			StringBuilder builder = new StringBuilder(array.Length * 2);

			foreach (var b in array)
				builder.AppendFormat("{0:x2}", b);

			return builder.ToString();
		}
	}
}
