using System;
using System.ComponentModel;

namespace SwdPageRecorder.WebDriver
{
	[Serializable]
	public class WebElementLocator
	{
		[DisplayName("Search Method")]
		public LocatorSearchMethod HowToSearch { get; set; }

		[DisplayName("Locator")]
		public string Locator { get; set; }
	}

	// XmlAttribute were required in oder to fix
	// There is no possibility to save page object .pox file
	// https://github.com/dzhariy/swd-recorder/issues/7
}