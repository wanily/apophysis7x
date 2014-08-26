using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows
{
	[PublicAPI]
	public static class ApophysisApplication
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			using (var window = GetWindow<Editor>())
			{
				var flame = new Flame();

				window.Transforms = flame.Transforms;

				flame.Transforms.Add();
				flame.Transforms[1].Origin.X = 1;

				Application.Run(window);
			}
		}

		public static T GetWindow<T>() where T : Form, new()
		{
			return new T();
		}
	}
}
