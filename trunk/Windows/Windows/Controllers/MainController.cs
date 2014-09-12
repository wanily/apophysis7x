using System;
using System.Collections.Generic;
using System.IO;
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
		private MainUndoController mMainUndoController;
		private BatchListController mBatchListController;

		private Lock mInitialize = new Lock();
		private FlameCollection mFlames;
		private bool mHasChanges;

		public MainController()
		{
			mUndoController = new UndoController();
			mKeyboardController = new KeyboardController();
			mEditorController = new EditorController(this);

			mMenuController = new MainMenuController(View, this);
			mToolbarController = new MainToolbarController(View, this);
			mMainUndoController = new MainUndoController(View, this);
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

				if (mMainUndoController != null)
				{
					mMainUndoController.Dispose();
					mMainUndoController = null;
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
			mMainUndoController.Initialize();
			mBatchListController.Initialize();

			View.Load += OnViewLoaded;
		}
		protected override void DetachView()
		{
			mEditorController.View.Owner = null;

			View.Load -= OnViewLoaded;
		}

		[NotNull]
		internal Lock Initializer
		{
			get { return mInitialize; }
		}

		internal void UpdateToolbar()
		{
			mToolbarController.UpdateButtonStates();
		}
		internal void UpdateMenu()
		{
			mMenuController.UpdateButtonStates();
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

				AfterReset();
				View.Text = string.Format("Apophysis - {0}", mFlames.CalculatedName);
			}
		}
		private void AfterReset()
		{
			mBatchListController.BuildFlameList();
			mBatchListController.SelectFlame(mFlames.First());
			mHasChanges = false;
		}
		
		public void UpdatePreviews()
		{
			mEditorController.UpdatePreview();
		}

		private void OnFlameCollectionChanged(object sender, EventArgs e)
		{
			if (mInitialize.IsBusy)
				return;

			var selection = mBatchListController.GetSelectedFlame();

			mBatchListController.BuildFlameList();
			mBatchListController.SelectFlame(selection ?? mFlames.First());
		}
		private void OnViewLoaded(object sender, EventArgs e)
		{
			GenerateRandomFlames(10, false);
		}

		public UndoController UndoController
		{
			get { return mUndoController; }
		}
		public KeyboardController KeyboardController
		{
			get { return mKeyboardController; }
		}
		public EditorController EditorController
		{
			get { return mEditorController; }
		}

		internal BatchListController BatchListController
		{
			get { return mBatchListController; }
		}
		internal MainMenuController MainMenuController
		{
			get { return mMenuController; }
		}

		public void AppendFlame([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");

			var index = mFlames.Append(flame);
			mBatchListController.SelectFlame(mFlames[index]);
		}

		public void ReadBatchFromFile(string path)
		{
			try
			{
				var batch = new FlameCollection();

				Flame.ReduceCounter();
				batch.ReadXml(XElement.Parse(File.ReadAllText(path), LoadOptions.None));

				Flames = batch;
				View.Text = Application.ProductName + @" - " + path;
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
		public void ReadFlameFromClipboard()
		{
			var clipboard = Clipboard.GetText();
			if (!string.IsNullOrEmpty(clipboard))
			{
				try
				{
					var flame = new Flame();

					flame.ReadXml(XElement.Parse(clipboard, LoadOptions.None));
					if (!string.IsNullOrEmpty(flame.Name))
					{
						Flame.ReduceCounter();
					}

					AppendFlame(flame);
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

			UpdatePreviews();
		}
		public void NotifyFlameNameChanged(Flame flame)
		{
			if (ReferenceEquals(flame, mEditorController.Flame))
			{
				mEditorController.UpdateWindowTitle();
			}

			mUndoController.CommitChange(flame);
		}
		public void GenerateRandomFlames(int count, bool confirm = true)
		{
			var batch = new List<Flame>();
			for (int i = 0; i < count; i++)
				batch.Add(new Flame());

			if (confirm)
			{
				ReplaceBatchWithConfirm(new FlameCollection(batch));
			}
			else
			{
				Flames = new FlameCollection(batch);
				View.Text = Application.ProductName + @" - " + "Random batch";
			}
		}

		public bool ConfirmReplaceBatch()
		{
			if (!mHasChanges)
				return true;

			var result = MessageBox.Show(
				string.Format("Do you want to save the current batch before loading a new one?"),
				Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

			bool save;
			switch (result)
			{
				case DialogResult.Yes:
					save = true;
					break;
				case DialogResult.No:
					save = false;
					break;
				default:
					return false;
			}

			if (save)
			{
				SaveCurrentBatch();
			}

			return true;
		}

		public void ReplaceBatchWithConfirm([NotNull] FlameCollection batch)
		{
			if (batch == null) throw new ArgumentNullException("batch");

			if (!ConfirmReplaceBatch())
				return;

			Flames = batch;
		}
		public void DeleteFlameIfPossibleWithConfirm([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");
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

		public void SetDirty()
		{
			mHasChanges = true;
		}
		public void SaveCurrentBatch()
		{
			//todo
		}

		public void ShowEditor()
		{
			mEditorController.View.Show();
		}
	}
}