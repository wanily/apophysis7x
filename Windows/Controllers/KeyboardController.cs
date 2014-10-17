using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
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
			HandleEditorShortcuts(sender, args);
			HandleBatchListShortcuts(sender, args);
		}

		private void HandleEditorShortcuts(object sender, KeyEventArgs args)
		{
			if (args.Control && args.KeyData == Keys.V && sender is Editor)
			{
				ApophysisApplication.MainWindow.ReadFlameFromClipboard();
			}
		}
		private void HandleBatchListShortcuts(object sender, KeyEventArgs args)
		{
			if (args.KeyData == Keys.F2 && sender is Main)
			{
				var main = (Main)sender;
				if (main.BatchListView.Focused)
				{
					var item = main.BatchListView.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
					if (item != null)
					{
						item.BeginEdit();
					}
				}
			}

			if (args.KeyData == Keys.Delete && sender is Main)
			{
				var main = (Main)sender;
				if (main.BatchListView.Focused)
				{
					var item = main.BatchListView.SelectedItems.OfType<ListViewItem>().FirstOrDefault();
					var flame = item == null ? null : item.Tag as Flame;

					if (flame != null)
					{
						ApophysisApplication.MainWindow.DeleteFlameIfPossibleWithConfirm(flame);
					}
				}
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