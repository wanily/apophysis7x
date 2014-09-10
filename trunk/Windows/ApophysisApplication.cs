using System;
using System.Collections.Generic;
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
				var errorList = new List<string>();

				foreach (var file in files)
				{
					try
					{
						if (!ExternalVariation.FitsCurrentMachineType(file))
							continue;

						VariationRegistry.RegisterDll(file);
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

			using (var editor = new EditorController(new UndoController()))
			{
				editor.Initialize();
				editor.StartApplication();
			}
		}
	}
}
