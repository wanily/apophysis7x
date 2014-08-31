using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows
{
	public abstract class WindowController<TView> : Controller<TView> where TView : Form, new()
	{
		public void OpenWindow()
		{
			View.Show();
		}
		public void StartApplication()
		{
			Application.Run(View);
		}
	}
}