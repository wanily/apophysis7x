using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Input;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class MainController : Controller<Main>
	{
		private UndoController mUndoController;
		private EditorController mEditorController;
		private FullscreenController mFullscreenController;
		private FlamePropertiesController mFlamePropertiesController;

		private KeyboardController mKeyboardController;
		private MainMenuController mMenuController;
		private MainToolbarController mToolbarController;
		private MainUndoController mMainUndoController;
		private BatchListController mBatchListController;
		private MainPreviewController mMainPreviewController;

		private readonly Lock mInitialize = new Lock();
		private FlameCollection mFlames;
		private bool mHasChanges;

		public MainController()
		{
			mUndoController = new UndoController();
			mKeyboardController = new KeyboardController();
			mEditorController = new EditorController(this);
			mFullscreenController = new FullscreenController(this);
			mFlamePropertiesController = new FlamePropertiesController(this);

			mMenuController = new MainMenuController(View, this);
			mToolbarController = new MainToolbarController(View, this);
			mMainUndoController = new MainUndoController(View, this);
			mBatchListController = new BatchListController(View, this);
			mMainPreviewController = new MainPreviewController(View, this);
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mMainPreviewController != null)
				{
					mMainPreviewController.Dispose();
					mMainPreviewController = null;
				}

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

				if (mFullscreenController != null)
				{
					mFullscreenController.Dispose();
					mFullscreenController = null;
				}

				if (mFlamePropertiesController != null)
				{
					mFlamePropertiesController.Dispose();
					mFlamePropertiesController = null;
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
			mFlamePropertiesController.View.Owner = View;

			mEditorController.Initialize();
			mFlamePropertiesController.Initialize();
			mFullscreenController.Initialize();
			mMenuController.Initialize();
			mToolbarController.Initialize();
			mBatchListController.Initialize();
			mMainPreviewController.Initialize();
			mMainUndoController.Initialize();

			View.Load += OnViewLoaded;
			View.CameraEndEdit += OnCameraEndEdit;
			View.CameraChanged += OnCameraChanged;
		}
		protected override void DetachView()
		{
			mEditorController.View.Owner = null;
			mFlamePropertiesController.View.Owner = null;

			View.Load -= OnViewLoaded;
			View.CameraEndEdit -= OnCameraEndEdit;
			View.CameraChanged -= OnCameraChanged;
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

			View.LoadingStatusLabel.Text = mFlames.CalculatedName;
		}
		
		public void UpdatePreviews(bool withMainPreview = true)
		{
			mBatchListController.UpdateSelectedPreview();
			mEditorController.UpdatePreview();
			mFlamePropertiesController.UpdatePreview();

			if (withMainPreview)
			{
				mMainPreviewController.UpdatePreview();
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
		private void OnViewLoaded(object sender, EventArgs e)
		{
			GenerateRandomFlames(10, false);
		}
		private void OnCameraEndEdit(object sender, CameraEndEditEventArgs e)
		{
			var flame = mBatchListController.GetSelectedFlame();
			if (flame == null || Initializer.IsBusy)
				return;

			FlamePropertiesController.UndoController.CommitChange(flame);
			FlamePropertiesController.RaiseFlameChanged();

			View.LoadingStatusLabel.Text = mFlames.CalculatedName;
		}
		private void OnCameraChanged(object sender, CameraChangedEventArgs e)
		{
			var flame = mBatchListController.GetSelectedFlame();
			if (flame == null || Initializer.IsBusy)
				return;

			using (mFlamePropertiesController.Initializer.Enter())
			{
				flame.Angle = e.Data.Angle;
				flame.Origin = e.Data.Origin.Copy();
				flame.Zoom = e.Data.Zoom;
				flame.PixelsPerUnit = e.Data.Scale;
				mFlamePropertiesController.UpdateCamera();
			}
		}

		public UndoController UndoController
		{
			get { return mUndoController; }
		}
		public EditorController EditorController
		{
			get { return mEditorController; }
		}
		public FullscreenController FullscreenController
		{
			get { return mFullscreenController; }
		}
		public FlamePropertiesController FlamePropertiesController
		{
			get { return mFlamePropertiesController; }
		}

		internal BatchListController BatchListController
		{
			get { return mBatchListController; }
		}
		internal MainMenuController MainMenuController
		{
			get { return mMenuController; }
		}
		internal MainPreviewController MainPreviewController
		{
			get { return mMainPreviewController; }
		}
		internal MainToolbarController ToolbarController
		{
			get { return mToolbarController; }
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
			mFlamePropertiesController.Flame = flame;

			mUndoController.Reset(flame);

			UpdatePreviews();
		}
		public void NotifyFlameNameChanged(Flame flame)
		{
			if (ReferenceEquals(flame, mEditorController.Flame))
			{
				mEditorController.UpdateWindowTitle();
			}

			if (ReferenceEquals(flame, mFlamePropertiesController.Flame))
			{
				mFlamePropertiesController.UpdateWindowTitle();
			}

			mUndoController.CommitChange(flame);
		}
		public void GenerateRandomFlames(int count, bool confirm = true)
		{
			var batch = new List<Flame>();
			for (int i = 0; i < count; i++)
				batch.Add(Flame.Random());

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
		public void ShowFlameProperties()
		{
			mFlamePropertiesController.View.Show();
			mFlamePropertiesController.View.Tabs.SelectTab(mFlamePropertiesController.View.CameraTab);
		}
		public void ShowImagingProperties()
		{
			mFlamePropertiesController.View.Show();
			mFlamePropertiesController.View.Tabs.SelectTab(mFlamePropertiesController.View.ImagingTab);
		}
		public void ShowPaletteProperties()
		{
			mFlamePropertiesController.View.Show();
			mFlamePropertiesController.View.Tabs.SelectTab(mFlamePropertiesController.View.PaletteTab);
		}
		public void ShowCanvasProperties()
		{
			mFlamePropertiesController.View.Show();
			mFlamePropertiesController.View.Tabs.SelectTab(mFlamePropertiesController.View.CanvasTab);
		}
	}
}