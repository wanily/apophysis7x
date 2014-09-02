using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Forms;

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

			using (var window = GetWindow<Editor, EditorController>())
			{
				window.StartApplication();
			}
		}

		public static TController GetWindow<TView, TController>() where TView : Form, new() where TController: WindowController<TView>, new()
		{
			var controller = new TController();
			controller.Initialize();
			return controller;
		}
	}
}
