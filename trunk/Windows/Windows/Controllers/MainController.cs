using System;
using System.Linq;
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
		private FlameCollection mFlames;

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

				if (mFlames != null)
				{
					mFlames.ContentChanged -= OnFlameCollectionChanged;
					mFlames = null;
				}
			}
		}

		protected override void AttachView()
		{
			mEditorController.View.Owner = View;

			mEditorController.Initialize();
			mMenuController.Initialize();
			mToolbarController.Initialize();
			mBatchListController.Initialize();

			Flames = new FlameCollection();

			//todo temp
			for(int i = 0; i < 10; i++)
				Flames.Append();
		}
		protected override void DetachView()
		{
			mEditorController.View.Owner = null;
		}

		[NotNull]
		internal Lock Initializer
		{
			get { return mInitialize; }
		}

		private void AfterReset()
		{
			mBatchListController.BuildFlameList();
			mBatchListController.SelectFlame(mFlames.First());
		}

		public FlameCollection Flames
		{
			get { return mFlames; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");

				if (mFlames != null)
				{
					mFlames.ContentChanged -= OnFlameCollectionChanged;
				}

				mFlames = value;
				mFlames.ContentChanged += OnFlameCollectionChanged;

				using (mInitialize.Enter())
				{
					AfterReset();
				}

				View.Text = string.Format("Apophysis - {0}", mFlames.CalculatedName);
			}
		}

		private void OnFlameCollectionChanged(object sender, EventArgs e)
		{
			if (mInitialize.IsBusy)
				return;

			var selection = mBatchListController.GetSelectedFlame();

			mBatchListController.BuildFlameList();
			mBatchListController.SelectFlame(selection ?? mFlames.First());
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

			mEditorController.Flame = flame;
			mUndoController.Reset(flame);
		}
		public void NotifyFlameNameChanged(Flame flame)
		{
			if (ReferenceEquals(flame, mEditorController.Flame))
			{
				mEditorController.UpdateWindowTitle();
			}
		}
		public void DeleteFlameIfPossibleWithConfirm(Flame flame)
		{
			if (!mFlames.CanRemove())
				return;

			if (!mFlames.Contains(flame))
				return;

			if (DialogResult.Yes == MessageBox.Show(
				string.Format("Do you really want to remove \"{0}\" from the batch? This can't be undone!", flame.CalculatedName),
				Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
			{
				var selection = mBatchListController.GetSelectedFlame();
				var index = selection == null ? 0 : mFlames.IndexOf(selection);

				if (index < 0)
					index = 0;

				mFlames.Remove(flame);

				if (index >= mFlames.Count)
					index = mFlames.Count - 1;

				mBatchListController.SelectFlame(mFlames[index]);
			}
		}

		public void ShowEditor()
		{
			mEditorController.View.Show();
		}
	}
}