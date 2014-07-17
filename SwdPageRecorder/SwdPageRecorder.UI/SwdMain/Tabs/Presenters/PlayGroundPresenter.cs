using FastColoredTextBoxNS;
using Microsoft.ClearScript.Windows;
using OpenQA.Selenium;
using SwdPageRecorder.WebDriver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SwdPageRecorder.UI
{
	public class PlayGroundPresenter : IPresenter<PlayGroundView>
	{
		private PlayGroundView view = null;

		public void InitWithView(PlayGroundView view)
		{
			this.view = view;

			// Subscribe to WebDriverUtils events
			SwdBrowser.OnDriverStarted += InitControls;
			SwdBrowser.OnDriverClosed += InitControls;

			Presenters.PageObjectDefinitionPresenter.OnPageObjectTreeChanged += PageObjectDefinitionPresenter_OnPageObjectTreeChanged;

			InitControls();
		}

		private void PageObjectDefinitionPresenter_OnPageObjectTreeChanged()
		{
			InitiCodeEditor();
		}

		private void InitControls()
		{
			var driverIsRunning = SwdBrowser.IsWorking;
			if (driverIsRunning)
			{
				view.Enabled = true;
			}
			else
			{
				view.Enabled = false;
			}
		}

		internal void InitiCodeEditor()
		{
			// This code is disabled
			// TODO: Complete. Handle crash
			return;

			AutocompleteMenu autocomplete = new AutocompleteMenu(view.txtJavaScriptCode);
			autocomplete.MinFragmentLength = 2;

			List<AutocompleteItem> autoComleteWords = new List<AutocompleteItem>()
            {
                new AutocompleteItem("driver"),
            };

			var pageObject = Presenters.PageObjectDefinitionPresenter.GetWebElementDefinitionFromTree();

			foreach (var webElementDefinition in pageObject.Items)
			{
				autoComleteWords.Add(new AutocompleteItem(webElementDefinition.Name));
				autoComleteWords.AddRange(BuildMethodsListForWebElement(webElementDefinition));
			}

			autocomplete.Items.SetAutocompleteItems(autoComleteWords);
		}

		private IEnumerable<MethodAutocompleteItem> BuildMethodsListForWebElement(WebElementDefinition webElementDefinition)
		{
			IEnumerable<string> IWebElementMethods = GetListOfMethods(typeof(IWebElement));
			IEnumerable<string> IWebElementProperties = GetListOfProperties(typeof(IWebElement));

			List<MethodAutocompleteItem> result = new List<MethodAutocompleteItem>();

			foreach (var propName in IWebElementProperties)
			{
				result.Add(new MethodAutocompleteItem(propName));
			}

			foreach (var methodName in IWebElementMethods)
			{
				result.Add(new MethodAutocompleteItem(methodName + "( );"));
			}

			return result;
		}

		private IEnumerable<string> GetListOfProperties(Type type)
		{
			var properties = from p in type.GetProperties()
							 select p.Name;

			return properties;
		}

		private IEnumerable<string> GetListOfMethods(Type type)
		{
			var methods = from m in type.GetMethods()
						  where !type
						  .GetProperties()
						  .Any(p => p.GetGetMethod() == m || p.GetSetMethod() == m)
						  select m.Name;

			return methods;
		}

		private Dictionary<string, Type> importedTypes = new Dictionary<string, Type>()
        {
            // OpenQA.Selenium:
            {"By", typeof(OpenQA.Selenium.By)},
            {"Keys", typeof(OpenQA.Selenium.Keys)},

            // OpenQA.Selenium.Interactions:
            {"Actions", typeof(OpenQA.Selenium.Interactions.Actions)},
            {"TouchActions", typeof(OpenQA.Selenium.Interactions.TouchActions)},
        };

		private void ImportTypes(JScriptEngine engine)
		{
			foreach (KeyValuePair<string, Type> pair in importedTypes)
			{
				engine.AddHostType(pair.Key, pair.Value);
			}
		}

		internal void RunScript(string code)
		{
			using (var engine = new JScriptEngine())
			{
				engine.AddHostObject("driver", SwdBrowser.GetDriver());

				ImportTypes(engine);

				var uiPageObject = Presenters.PageObjectDefinitionPresenter.GetWebElementDefinitionFromTree();

				foreach (var element in uiPageObject.Items)
				{
					IWebElement proxyElement = SwdBrowser.CreateWebElementProxy(element);
					string name = element.Name;
					engine.AddHostObject(name, proxyElement);
				}

				var result = engine.Evaluate(code) ?? "(none)";
				view.AppendConsole(result.ToString() + "\r\n");
			}
		}
	}
}