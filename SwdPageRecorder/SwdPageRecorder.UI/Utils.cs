using System;
using System.IO;
using System.Reflection;

namespace SwdPageRecorder.UI
{
	public static class Utils
	{
		internal static string[] SplitSingleLineToMultyLine(string singleLineSource)
		{
			string[] result = singleLineSource.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
			return result;
		}

		static public string AssemblyDirectory
		{
			get
			{
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}
	}
}