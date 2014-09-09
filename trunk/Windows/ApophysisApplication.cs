using System;
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

			using (var editor = new EditorController(new UndoController()))
			{
				editor.Initialize();
				editor.StartApplication();
			}
		}
	}
}
