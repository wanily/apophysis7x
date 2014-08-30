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

				window.Iterators = flame.Iterators;

				flame.Iterators.Add();
				flame.Iterators[1].PreAffine.Origin.X = 1;

				Application.Run(window);
			}
		}

		public static T GetWindow<T>() where T : Form, new()
		{
			return new T();
		}
	}
}
