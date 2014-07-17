using FastColoredTextBoxNS;
using System;
using System.Windows.Forms;

namespace SwdPageRecorder.UI
{
	public partial class FullHtmlSourceTabView : UserControl, IView
	{
		private FullHtmlSourceTabPresenter presenter;

		public FullHtmlSourceTabView()
		{
			InitializeComponent();

			this.presenter = Presenters.PageObjectSourceCodePresenter;
			presenter.InitWithView(this);

			txtHtmlPageSource.Language = Language.HTML;
		}

		private void btnGetHtmlSource_Click(object sender, EventArgs e)
		{
			presenter.DisplayHtmlPageSource();
		}

		internal void FillHtmlCodeBox(string htmlText)
		{
			txtHtmlPageSource.Clear();
			txtHtmlPageSource.Text = htmlText;
		}

		private void txtHtmlPageSource_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.A)
			{
				txtHtmlPageSource.SelectAll();
			}
		}
	}
}