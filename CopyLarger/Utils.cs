using System;

namespace CopyLarger
{
	internal static class Utils
	{
		/// <summary>
		/// From http://stackoverflow.com/a/4975942
		/// </summary>
		internal static string BytesToString(long byteCount)
		{
			string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" };
			if (byteCount == 0)
				return "0" + suf[0];
			var bytes = Math.Abs(byteCount);
			var place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
			var num = Math.Round(bytes / Math.Pow(1024, place), 1);
			return Math.Sign(byteCount) * num + suf[place];
		}


		internal static void WriteExit(int exitCode)
		{
			Console.WriteLine(Environment.NewLine);
			Console.WriteLine("Press any key to continue");
			Console.ReadKey();
			Environment.Exit(exitCode);
		}
	}
}
