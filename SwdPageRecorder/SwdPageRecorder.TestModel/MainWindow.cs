using System.Linq;

using TestStack.White.UIItems.WindowItems;

namespace SwdPageRecorder.TestModel
{
	public static class MainWindow
	{
		private static int prevHashCode = -1;
		private static Window cachedWindow = null;

		public static Window GetWindow()
		{
			var appInstance = App.GetInstance();

			int currentHash = appInstance.GetHashCode();
			bool shouldCreateNewWindow = false;
			shouldCreateNewWindow = (cachedWindow == null
									  || currentHash != prevHashCode
									  || !cachedWindow.IsCurrentlyActive
									);

			if (shouldCreateNewWindow)
			{
				cachedWindow = appInstance.GetWindows()
							   .First(w => w.Title.StartsWith("SWD"));
				prevHashCode = currentHash;
			}
			return cachedWindow;
		}
	}
}