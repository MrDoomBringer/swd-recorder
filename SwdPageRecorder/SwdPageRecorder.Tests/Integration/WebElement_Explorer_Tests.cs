using Microsoft.VisualStudio.TestTools.UnitTesting;
using SwdPageRecorder.TestModel;
using SwdPageRecorder.WebDriver;
using System;

namespace SwdPageRecorder.Tests.Integration
{
	[TestClass]
	public class WebElement_Explorer_Tests : MyTest
	{
		private const int firstFrame = 0;
		private const int secondFrame = 1;
		private const int thirdFrame = 2;

		[TestMethod, Ignore]
		public void WebElementExplorer_Test()
		{
			Helper.RunDefaultBrowser();
			Helper.LoadTestFile("page_with_frames.html");

			Helper.ToFrame(secondFrame);

			SwdBrowser.InjectVisualSearch();

			//Helper.ClickId();

			throw new NotImplementedException();
		}
	}
}