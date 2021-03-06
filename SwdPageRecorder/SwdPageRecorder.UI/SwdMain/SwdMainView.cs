﻿using SwdPageRecorder.WebDriver;
using System;
using System.Linq;
using System.Windows.Forms;
using FormKeys = System.Windows.Forms.Keys;

namespace SwdPageRecorder.UI
{
	public partial class SwdMainView : Form, IView
	{
		private SwdMainPresenter presenter = null;
		private System.Threading.ManualResetEvent startedEvent;

		public SwdMainView()
		{
			InitializeComponent();
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;

			presenter = Presenters.SwdMainPresenter;
			presenter.InitView(this);
		}

		public SwdMainView(System.Threading.ManualResetEvent startedEvent)
			: this()
		{
			this.startedEvent = startedEvent;
		}

		private void txtBrowserUrl_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == FormKeys.Enter)
			{
				presenter.SetBrowserUrl(txtBrowserUrl.Text);
			}
		}

		private void btnStartVisualSearch_Click(object sender, EventArgs e)
		{
			presenter.ChangeVisualSearchRunningState();
		}

		internal void UpdateVisualSearchResult(string xPathAttributeValue)
		{
			var action = (MethodInvoker)delegate
			{
				txtVisualSearchResult.Text = xPathAttributeValue;
			};

			if (txtVisualSearchResult.InvokeRequired)
			{
				txtVisualSearchResult.Invoke(action);
			}
			else
			{
				action();
			}
		}

		private void btnBrowser_Go_Click(object sender, EventArgs e)
		{
			presenter.SetBrowserUrl(txtBrowserUrl.Text);
		}

		internal void VisualSearchStopped()
		{
			var action = (MethodInvoker)delegate
			{
				btnStartVisualSearch.Text = "Start";
			};

			if (btnStartVisualSearch.InvokeRequired)
			{
				btnStartVisualSearch.Invoke(action);
			}
			else
			{
				action();
			}
		}

		internal void VisuaSearchStarted()
		{
			var action = (MethodInvoker)delegate
			{
				btnStartVisualSearch.Text = "Stop";
			};

			if (btnStartVisualSearch.InvokeRequired)
			{
				btnStartVisualSearch.Invoke(action);
			}
			else
			{
				action();
			}
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://swd-tools.com");
		}

		internal void ShowGlobalLoading()
		{
			var action = (MethodInvoker)delegate
			{
				pnlLoadingBar.Visible = true;
			};

			if (pnlLoadingBar.InvokeRequired)
			{
				pnlLoadingBar.Invoke(action);
			}
			else
			{
				action();
			}
		}

		internal void HideGlobalLoading()
		{
			var action = (MethodInvoker)delegate
			{
				pnlLoadingBar.Visible = false;
			};

			if (pnlLoadingBar.InvokeRequired)
			{
				pnlLoadingBar.Invoke(action);
			}
			else
			{
				action();
			}
		}

		private void SwdMainView_Shown(object sender, EventArgs e)
		{
			if (startedEvent != null) startedEvent.Set();
		}

		internal void SetDriverDependingControlsEnabled(bool shouldControlBeEnabled)
		{
			txtBrowserUrl.DoInvokeAction(() => txtBrowserUrl.Enabled = shouldControlBeEnabled);
			btnBrowser_Go.DoInvokeAction(() => btnBrowser_Go.Enabled = shouldControlBeEnabled);
			grpVisualSearch.DoInvokeAction(() => grpVisualSearch.Enabled = shouldControlBeEnabled);
			grpSwitchTo.DoInvokeAction(() => grpSwitchTo.Enabled = shouldControlBeEnabled);
		}

		internal void UpdateBrowserWindowsList(BrowserWindow[] currentWindows, string currentWindowHandle)
		{
			ddlWindows.DoInvokeAction(() =>
			{
				ddlWindows.Items.Clear();
				ddlWindows.Items.AddRange(currentWindows);

				ddlWindows.SelectedItem = currentWindows.First(win => (win.WindowHandle == currentWindowHandle));
			});
		}

		internal void UpdatePageFramesList(BrowserPageFrame[] currentPageFrames)
		{
			ddlFrames.DoInvokeAction(() =>
			{
				ddlFrames.Items.Clear();
				ddlFrames.Items.AddRange(currentPageFrames);

				ddlFrames.SelectedItem = currentPageFrames.First();
			});
		}

		private void btnRefresh_Click(object sender, EventArgs e)
		{
			presenter.RefreshSwitchToList();
		}

		internal void SetInitialRefreshMessageForSwitchToControls()
		{
			ddlFrames.Enabled = false;
			ddlWindows.Enabled = false;

			ddlWindows.Text = "Press Refresh button";
			ddlFrames.Text = "... please";
		}

		internal void EnableSwitchToControls()
		{
			ddlFrames.Enabled = true;
			ddlWindows.Enabled = true;
		}

		internal void DisableSwitchToControls()
		{
			ddlFrames.Enabled = false;
			ddlWindows.Enabled = false;
		}

		private void ddlFrames_SelectedIndexChanged(object sender, EventArgs e)
		{
			BrowserPageFrame frame = ddlFrames.SelectedItem as BrowserPageFrame;
			presenter.SwitchToFrame(frame);
		}

		private void ddlWindows_SelectedIndexChanged(object sender, EventArgs e)
		{
			BrowserWindow window = ddlWindows.SelectedItem as BrowserWindow;
			presenter.SwitchToWindow(window);
		}

		internal void DisableWebElementExplorerRunButton()
		{
			btnStartVisualSearch.DoInvokeAction(() =>
			{
				btnStartVisualSearch.Enabled = false;
			});
		}

		internal void EnableWebElementExplorerRunButton()
		{
			btnStartVisualSearch.DoInvokeAction(() =>
			{
				btnStartVisualSearch.Enabled = true;
			});
		}

		internal void DisableWebElementExplorerResultsField()
		{
			txtVisualSearchResult.DoInvokeAction(() =>
			{
				txtVisualSearchResult.Enabled = false;
			});
		}

		internal void EnableWebElementExplorerResultsField()
		{
			txtVisualSearchResult.DoInvokeAction(() =>
			{
				txtVisualSearchResult.Enabled = true;
			});
		}
	}
}