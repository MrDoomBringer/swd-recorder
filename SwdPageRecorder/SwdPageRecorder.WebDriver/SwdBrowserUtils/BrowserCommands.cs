using OpenQA.Selenium;
using SwdPageRecorder.WebDriver.JsCommand;
using System;

namespace SwdPageRecorder.WebDriver.SwdBrowserUtils
{
	public static class BrowserCommands
	{
		private const string javaScript_GetCommand = @"return document.swdpr_command === undefined ? '' : document.swdpr_command;";

		private static string lastCommandId = null;

		public static BrowserCommand GetNextCommand(IWebDriver webDriver)
		{
			BrowserCommand result = null;
			IJavaScriptExecutor js = webDriver as IJavaScriptExecutor;

			string jsonCommand = string.Empty;

			try
			{
				jsonCommand = js.ExecuteScript(javaScript_GetCommand) as string;
			}
			catch (Exception e)
			{
				MyLog.Exception(e);
				jsonCommand = "";
				throw;
			}

			if (!String.IsNullOrWhiteSpace(jsonCommand))
			{
				var unknownCommand = BrowserCommandParser.ParseCommand<BrowserCommand>(jsonCommand);

				if (lastCommandId == unknownCommand.CommandId) return null;

				if (unknownCommand.Command == @"GetXPathFromElement")
				{
					result = BrowserCommandParser.ParseCommand<GetXPathFromElement>(jsonCommand);
				}
				else if ((unknownCommand.Command == @"AddElement"))
				{
					result = BrowserCommandParser.ParseCommand<AddElement>(jsonCommand);
				}
				lastCommandId = unknownCommand.CommandId;
			}

			return result;
		}
	}
}