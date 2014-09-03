using System;
using System.Windows.Forms;
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

			using (var editor = new EditorController(new UndoController()))
			{
				editor.Initialize();
				editor.StartApplication();
			}
		}
	}
}
