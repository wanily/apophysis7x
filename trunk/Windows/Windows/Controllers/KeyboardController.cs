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
			HandleApplicationWideShortcuts(sender, args);

			HandleMainWindowToolbarShortcuts(sender, args);
			HandleMainWindowBatchListShortcuts(sender, args);
		}

		private void HandleApplicationWideShortcuts(object sender, KeyEventArgs args)
		{
			if ((args.KeyData & Keys.Control) == Keys.Control && (args.KeyData & Keys.V) == Keys.V)
			{
				ApophysisApplication.MainWindow.ReadFlameFromClipboard();
			}
		}
		private void HandleMainWindowToolbarShortcuts(object sender, KeyEventArgs args)
		{
			if ((args.KeyData & Keys.F4) == Keys.F4 && sender is Main)
			{
				ApophysisApplication.MainWindow.ShowEditor();
			}
		}
		private void HandleMainWindowBatchListShortcuts(object sender, KeyEventArgs args)
		{
			if ((args.KeyData & Keys.F2) == Keys.F2 && sender is Main)
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

			if ((args.KeyData & Keys.Delete) == Keys.Delete && sender is Main)
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