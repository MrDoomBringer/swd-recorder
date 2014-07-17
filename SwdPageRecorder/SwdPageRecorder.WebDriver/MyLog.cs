using NLog;
using System;

namespace SwdPageRecorder.WebDriver
{
	public static class MyLog
	{
		private static Logger logger = LogManager.GetLogger("SWD");

		public static void Exception(Exception e)
		{
			logger.Error(e.Message, e);
		}

		public static void Write(string message)
		{
			logger.Info(message);
		}

		public static void Error(string errorMessage)
		{
			logger.Error(errorMessage);
		}
	}
}