using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class MainController : WindowController<Main>
	{
		private UndoController mUndoController;
		private EditorController mEditorController;
		private KeyboardController mKeyboardController;

		private MainMenuController mMenuController;
		private MainToolbarController mToolbarController;
		private BatchListController mBatchListController;

		private Lock mInitialize = new Lock();

		public MainController()
		{
			mUndoController = new UndoController();
			mKeyboardController = new KeyboardController();
			mEditorController = new EditorController(this);

			mMenuController = new MainMenuController(View, this);
			mToolbarController = new MainToolbarController(View, this);
			mBatchListController = new BatchListController(View, this);
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mBatchListController != null)
				{
					mBatchListController.Dispose();
					mBatchListController = null;
				}

				if (mToolbarController != null)
				{
					mToolbarController.Dispose();
					mToolbarController = null;
				}

				if (mMenuController != null)
				{
					mMenuController.Dispose();
					mMenuController = null;
				}

				if (mEditorController != null)
				{
					mEditorController.Dispose();
					mEditorController = null;
				}

				if (mKeyboardController != null)
				{
					mKeyboardController.Dispose();
					mKeyboardController = null;
				}

				if (mUndoController != null)
				{
					mUndoController.Dispose();
					mUndoController = null;
				}
			}
		}

		protected override void AttachView()
		{
			mEditorController.Initialize();
			mMenuController.Initialize();
			mToolbarController.Initialize();
			mBatchListController.Initialize();

			//todo select first from default batch instead (use batch list controller)
			LoadFlameAndEraseHistory(new Flame());
		}
		protected override void DetachView()
		{
		}

		public UndoController UndoController
		{
			get { return mUndoController; }
		}
		public KeyboardController KeyboardController
		{
			get { return mKeyboardController; }
		}

		public void ReadFlameFromClipboard()
		{
			var clipboard = Clipboard.GetText();
			if (!string.IsNullOrEmpty(clipboard))
			{
				try
				{
					var flame = new Flame();

					flame.ReadXml(XElement.Parse(clipboard, LoadOptions.None));
					LoadFlameAndEraseHistory(flame);
				}
				catch (ApophysisException exception)
				{
					MessageBox.Show(exception.Message, View.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				catch (XmlException exception)
				{
					MessageBox.Show(exception.Message, View.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
		public void LoadFlameAndEraseHistory([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");

			mEditorController.Flame = new Flame();
			mUndoController.Initialize(flame);
		}

		public void ShowEditor()
		{
			mEditorController.View.Show();
		}
	}
}