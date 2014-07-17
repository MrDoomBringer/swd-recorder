using System;
using System.Windows.Forms;

namespace SwdPageRecorder.UI
{
	public static class WinformsAction
	{
		public static void DoInvokeAction<T>(this T control, Action action) where T : Control
		{
			if (control.InvokeRequired)
			{
				control.Invoke(action);
			}
			else
			{
				action();
			}
		}
	}
}