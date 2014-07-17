using System.Linq;

using TestStack.White.UIItems;
using TestStack.White.UIItems.ListBoxItems;

namespace SwdPageRecorder.TestModel
{
	public class BrowserSettingsTabWindow : MyBaseWindow
	{
		private ComboBox ddlBrowserToStart()
		{
			return Get<ComboBox>("ddlBrowserToStart");
		}

		private Button btnStart()
		{
			return Get<Button>("btnStartWebDriver");
		}

		private Button btnStop()
		{
			return btnStart(); // Magic!
		}

		public BrowserSettingsTabWindow SelectBrowser(string browserName)
		{
			ddlBrowserToStart()
				.Items
				.Where(n => n.Text == browserName)
				.First()
				.Select();

			return this;
		}

		public BrowserSettingsTabWindow StartBrowser()
		{
			btnStart().RaiseClickEvent();
			return this;
		}

		public BrowserSettingsTabWindow StopBrowser()
		{
			btnStop().RaiseClickEvent();
			return this;
		}
	}
}