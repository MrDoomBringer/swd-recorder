using HtmlAgilityPack;
using OpenQA.Selenium;

namespace SwdPageRecorder.WebDriver.SwdBrowserUtils
{
	public static class HtmlPageUtils
	{
		public static HtmlDocument GetPageSource(IWebDriver webDriver)
		{
			string currentPageSource = (webDriver.PageSource ?? "").Replace("\r\n", "");

			var htmlDoc = new HtmlDocument();
			htmlDoc.OptionFixNestedTags = true;

			// https://htmlagilitypack.codeplex.com/discussions/247206
			HtmlNode.ElementsFlags.Remove("form");

			htmlDoc.LoadHtml(currentPageSource);
			return htmlDoc;
		}
	}
}