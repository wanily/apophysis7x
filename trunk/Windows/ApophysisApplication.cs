using System;
using System.Windows.Forms;
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

			using (var window = GetWindow<DragPanelTest>())
			{
				Application.Run(window);
			}
		}

		public static T GetWindow<T>() where T : Form, new()
		{
			return new T();
		}
	}
}
