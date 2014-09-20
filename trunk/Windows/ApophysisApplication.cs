using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Variations;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis
{
	[PublicAPI]
	public static class ApophysisApplication
	{
		private static BannerController mBanner;

		public static MainController MainWindow { get; private set; }

		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			mBanner = new BannerController();
			mBanner.Initialize();
			
			LoadVariations();

			mBanner.BannerText = "Loading GUI";

			using (MainWindow = new MainController())
			{
				MainWindow.Initialize();

				mBanner.Dispose();
				mBanner = null;

				Application.Run(MainWindow.View);
			}
		}

		static void LoadVariations()
		{
			mBanner.BannerText = "Loading variations";

			Variation.VariationsIn15CStyle = ApophysisSettings.VariationsIn15CStyle;

			var types = typeof(Linear).Assembly.GetTypes();
			var registerMethod = typeof(VariationRegistry).GetMethod("Register", BindingFlags.Static | BindingFlags.Public);

			foreach (var type in types)
			{
				if (!typeof(Variation).IsAssignableFrom(type) || type.IsAbstract || type == typeof(ExternalVariation))
					continue;

				//special treatment for "linear3D"
				if (Variation.VariationsIn15CStyle && type == typeof (Linear3D))
					continue;
					

				var method = registerMethod.MakeGenericMethod(type);
				var result = method.Invoke(null, new object[0]) as string;

				Debug.Assert(!string.IsNullOrEmpty(result));
				mBanner.BannerText = result;
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
						mBanner.BannerText = name;
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
