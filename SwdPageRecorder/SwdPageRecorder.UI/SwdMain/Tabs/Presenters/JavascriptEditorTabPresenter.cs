﻿using OpenQA.Selenium;
using SwdPageRecorder.WebDriver;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace SwdPageRecorder.UI
{
	public class JavascriptEditorTabPresenter : IPresenter<JavaScriptEditorView>
	{
		private JavaScriptEditorView view = null;

		private TreeNode SnippetFilesRoot { get; set; }

		public void InitWithView(JavaScriptEditorView view)
		{
			this.view = view;

			// Subscribe to WebDriverUtils events
			SwdBrowser.OnDriverStarted += InitControls;
			SwdBrowser.OnDriverClosed += InitControls;
			InitControls();

			ReloadJavaScriptFilesTree();
		}

		public void ReloadJavaScriptFilesTree()
		{
			string snippetsRoot = GetSnippetsFolder();

			TreeNode root = new TreeNode();

			root.Text = "Snippets";
			root.Tag = snippetsRoot;
			ReloadJavaScriptFilesTree(root, snippetsRoot);

			view.UpdateTree(root);
			view.ExpandTree();
		}

		private static string GetSnippetsFolder()
		{
			string snippetsRoot = Path.Combine(Utils.AssemblyDirectory, "Snippets");
			return snippetsRoot;
		}

		public void ReloadJavaScriptFilesTree(TreeNode parentNode, string directoryPath)
		{
			try
			{
				foreach (string d in Directory.GetDirectories(directoryPath))
				{
					TreeNode directoryNode = new TreeNode();
					DirectoryInfo dirInfo = new DirectoryInfo(d);

					directoryNode.Text = dirInfo.Name;
					directoryNode.Tag = d;

					parentNode.Nodes.Add(directoryNode);

					ReloadJavaScriptFilesTree(directoryNode, d);
				}
				foreach (string f in Directory.GetFiles(directoryPath))
				{
					TreeNode fileNode = new TreeNode();

					FileInfo fileInfo = new FileInfo(f);
					fileNode.Text = fileInfo.Name;
					fileNode.Tag = f;

					if (IsJavaScriptFile(f))
					{
						parentNode.Nodes.Add(fileNode);
					}
				}
			}
			catch (System.Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		private bool IsJavaScriptFile(string filePath)
		{
			if (filePath == null) return false;
			if (!File.Exists(filePath)) return false;

			return filePath.EndsWith(".js");
		}

		private void InitControls()
		{
			var driverIsRunning = SwdBrowser.IsWorking;
			if (driverIsRunning)
			{
				view.SetControlsToEnabledState(true);
			}
			else
			{
				view.SetControlsToEnabledState(false);
			}
		}

		internal void TryOpenJavaScriptFile(TreeNode treeNode)
		{
			if (treeNode == null) return;
			if (treeNode.Tag == null) return;

			string filePath = treeNode.Tag as string;

			if (String.IsNullOrWhiteSpace(filePath)) return;
			if (!File.Exists(filePath)) return;
			if (!IsJavaScriptFile(filePath)) return;

			string fileContent = File.ReadAllText(filePath);
			view.UpdateFileEditor(fileContent);
		}

		internal void RunScript()
		{
			string scriptFromEditor = view.GetJavaScriptCodeFromEditor();

			Exception outException;
			bool isOk = false;
			object result = null;
			isOk = UIActions.PerformSlowOperation(
						"Operation: RunScript() / Executing JavaScript Snippet",
						() =>
						{
							result = SwdBrowser.ExecuteJavaScript(scriptFromEditor);
						},
							out outException,
							null,
							TimeSpan.FromMinutes(1)
						);

			if (!isOk)
			{
				MyLog.Error("RunScript() Failed to execute JavaScript snippet");
				MyLog.Exception(outException);
				if (outException != null) throw outException;
			}

			string consoleOut = DumpObject(result);
			view.AppendConsole(consoleOut);
		}

		private string DumpObject(object sourceObject)
		{
			StringBuilder result = new StringBuilder("Result:\r\n");

			if (sourceObject == null)
			{
				result.AppendLine("(null)");
			}
			else if (sourceObject is IWebElement)
			{
				result.AppendFormat(" > IWebElement\r\n");
			}
			else if (sourceObject is IDictionary)
			{
				IDictionary sourceDict = (IDictionary)sourceObject;

				foreach (string key in sourceDict.Keys)
				{
					string value = (sourceDict[key] ?? "(null)").ToString();
					result.AppendFormat(" > {0} : {1}\r\n", key, value);
				}
			}
			else if (sourceObject is IEnumerable && !(sourceObject is String))
			{
				foreach (var item in (IEnumerable)sourceObject)
				{
					string value = (item ?? "(null)").ToString();
					result.AppendFormat(" > {0}\r\n", value);
				}
			}
			else
			{
				result.AppendLine(sourceObject.ToString());
			}

			return result.ToString();
		}

		internal void ShowSnippetsFolder()
		{
			Process.Start(GetSnippetsFolder());
		}
	}
}