﻿using Newtonsoft.Json;
using SwdPageRecorder.WebDriver;
using System.Diagnostics;
using System.IO;

namespace SwdPageRecorder.UI.CodeGeneration
{
	public class ExternalGenerator
	{
		public void SaveToJSonFile(SwdPageObject pageObject, string filePath)
		{
			string json = JsonConvert.SerializeObject(pageObject, Formatting.Indented);
			File.WriteAllText(filePath, json);
		}

		public string RunExternalProcess(string fullCommandLine)
		{
			return RunExternalProcess(fullCommandLine, true);
		}

		public string RunExternalProcess(string fullCommandLine, bool waitForExit)
		{
			Process compiler = new Process();
			compiler.StartInfo.FileName = "cmd.exe";
			compiler.StartInfo.Arguments = "/C " + fullCommandLine;
			compiler.StartInfo.UseShellExecute = false;
			compiler.StartInfo.RedirectStandardOutput = true;
			compiler.Start();

			string output = compiler.StandardOutput.ReadToEnd();

			if (waitForExit) compiler.WaitForExit();

			return output;
		}
	}
}