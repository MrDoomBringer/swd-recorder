using System;
using System.Diagnostics;
using TestStack.White.UIItems;

namespace SwdPageRecorder.TestModel
{
	public abstract class MyBaseWindow
	{
		public T Get<T>(string primaryIdentification) where T : IUIItem
		{
			MainWindow.GetWindow().Focus();
			return MainWindow.GetWindow().Get<T>(primaryIdentification);
		}

		protected Label lblWaitingIndicator()
		{
			return Get<Label>("lblLoadingInProgress");
		}

		public void WaitWhileWaitingIndicatorDisplayed()
		{
			Stopwatch sw = new Stopwatch();

			try
			{
				System.Threading.Thread.Sleep(100);
				var waitingIndicator = lblWaitingIndicator();

				sw.Start();
				while (waitingIndicator.Visible)
				{
					System.Threading.Thread.Sleep(100);
					if (sw.Elapsed > TimeSpan.FromSeconds(30))
					{
						Console.WriteLine("WaitWhileWaitingIndicatorDisplayedL Operation took more than 30 seconds :(");
						return;
					}
				}
			}
			catch { }
		}
	}
}