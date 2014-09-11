using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class KeyboardController : Controller
	{
		private bool mIsDisposed;

		public KeyboardController()
		{
			InputController.GlobalKeyPressed += OnKeyDown;
		}

		private void OnKeyDown(object sender, KeyEventArgs args)
		{
			if ((args.KeyData & Keys.Control) == Keys.Control && (args.KeyData & Keys.V) == Keys.V)
			{
				ApophysisApplication.MainWindow.ReadFlameFromClipboard();
			}

			if ((args.KeyData & Keys.F4) == Keys.F4 && sender is Main)
			{
				ApophysisApplication.MainWindow.ShowEditor();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (mIsDisposed)
				return;

			if (disposing)
			{
				InputController.GlobalKeyPressed -= OnKeyDown;
			}

			mIsDisposed = true;
		}
	}
}