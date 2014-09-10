using System;
using System.IO;
using System.Windows.Forms;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Variations;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis
{
	[PublicAPI]
	public static class ApophysisApplication
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			VariationRegistry.Register<Linear>();
			VariationRegistry.Register<Spherical>();
			VariationRegistry.Register<Swirl>();
			VariationRegistry.Register<Test>();

			var pluginDir = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath)??string.Empty, ApophysisSettings.PluginDirectoryName);
			if (Directory.Exists(pluginDir))
			{
				var files = Directory.GetFiles(pluginDir);
				foreach (var file in files)
				{
					VariationRegistry.RegisterDll(file);
				}
			}

			using (var editor = new EditorController(new UndoController()))
			{
				editor.Initialize();
				editor.StartApplication();
			}
		}
	}
}
