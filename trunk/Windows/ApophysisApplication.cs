using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Variations;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis
{
	[PublicAPI]
	public static class ApophysisApplication
	{
		public static BannerController Banner { get; private set; }
		public static EditorController Editor { get; private set; }

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Banner = new BannerController();
			Banner.Initialize();
			
			LoadVariations();

			Banner.BannerText = "Loading GUI";

			//todo more forms
			using (Editor = (EditorController)new EditorController(new UndoController()).Initialize())
			{
				Banner.Dispose();

				//todo use main form instead
				Application.Run(Editor.View);
			}
		}

		static void LoadVariations()
		{
			Banner.BannerText = "Loading variations";

			var types = typeof(Linear).Assembly.GetTypes();
			var registerMethod = typeof(VariationRegistry).GetMethod("Register", BindingFlags.Static | BindingFlags.Public);

			foreach (var type in types)
			{
				if (!typeof(Variation).IsAssignableFrom(type) || type.IsAbstract || type == typeof(ExternalVariation))
					continue;

				var method = registerMethod.MakeGenericMethod(type);
				var result = method.Invoke(null, new object[0]) as string;

				Debug.Assert(!string.IsNullOrEmpty(result));
				Banner.BannerText = result;
#if DEBUG
				Thread.Sleep(10);
#endif
			}

			var pluginDir = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath) ?? string.Empty, ApophysisSettings.PluginDirectoryName);
			if (Directory.Exists(pluginDir))
			{
				var files = Directory.GetFiles(pluginDir);
				var errorList = new List<string>();

				foreach (var file in files)
				{
					try
					{
						if (!ExternalVariation.FitsCurrentMachineType(file))
							continue;

						var name = VariationRegistry.RegisterDll(file);
						Banner.BannerText = name;
#if DEBUG
						Thread.Sleep(10);
#endif
					}
					catch (ApophysisException exception)
					{
						errorList.Add(string.Format("   - {0}: {1}", Path.GetFileName(file), exception.Message));
					}
				}

				if (errorList.Count > 0)
				{
					MessageBox.Show(
						string.Format("The following plugins could not be loaded:\r\n\r\n{0}", string.Join("\r\n", errorList.ToArray())),
						Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
				}
			}
		}
	}
}
