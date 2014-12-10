using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Xyrus.Apophysis.Messaging;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Strings;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Input;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;
using Xyrus.Apophysis.Windows.Interfaces.Views;
using Messages = Xyrus.Apophysis.Strings.Messages;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class MainController : Controller<IMainView>, IMainController
	{
		private AutosaveController mAutosaveController;
		private UndoController mUndoController;
		private EditorController mEditorController;
		private FullscreenController mFullscreenController;
		private FlamePropertiesController mFlamePropertiesController;
		private MessagesController mMessagesController;
		private SettingsController mSettingsController;
		private RenderController mRenderController;

		private KeyboardController mKeyboardController;
		private MainMenuController mMenuController;
		private MainToolbarController mToolbarController;
		private MainUndoController mMainUndoController;
		private BatchListController mBatchListController;
		private MainPreviewController mMainPreviewController;

		private readonly Lock mInitialize = new Lock();
		private FlameCollection mFlames;
		private string mLastBatchPath;
		private bool mHasChanges;

		public MainController()
		{
			mUndoController = new UndoController();
			mKeyboardController = new KeyboardController();
			mAutosaveController = new AutosaveController();
			mEditorController = new EditorController(this);
			mFullscreenController = new FullscreenController(this);
			mFlamePropertiesController = new FlamePropertiesController(this);
			mMessagesController = new MessagesController();
			mSettingsController = new SettingsController(this);
			mRenderController = new RenderController(this);

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
				if (mAutosaveController != null)
				{
					mAutosaveController.Dispose();
					mAutosaveController = null;
				}

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

				if (mRenderController != null)
				{
					mRenderController.Dispose();
					mRenderController = null;
				}

				if (mSettingsController != null)
				{
					mSettingsController.Dispose();
					mSettingsController = null;
				}

				if (mMessagesController != null)
				{
					mMessagesController.Dispose();
					mMessagesController = null;
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
			mEditorController.Initialize();
			mFlamePropertiesController.Initialize();
			mMessagesController.Initialize();
			mSettingsController.Initialize();
			mRenderController.Initialize();
			mFullscreenController.Initialize();
			mMenuController.Initialize();
			mToolbarController.Initialize();
			mBatchListController.Initialize();
			mMainPreviewController.Initialize();
			mMainUndoController.Initialize();
			mAutosaveController.Initialize();

			View.Load += OnViewLoaded;
			View.CameraEndEdit += OnCameraEndEdit;
			View.CameraChanged += OnCameraChanged;
		}
		protected override void DetachView()
		{
			View.Load -= OnViewLoaded;
			View.CameraEndEdit -= OnCameraEndEdit;
			View.CameraChanged -= OnCameraChanged;
		}

		[NotNull]
		public Lock Initializer
		{
			get { return mInitialize; }
		}

		public void UpdateToolbar()
		{
			mToolbarController.UpdateButtonStates();
		}
		public void UpdateMenu()
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

			View.LoadingStatusLabel.Text = Application.ProductName;
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
			if (!string.IsNullOrEmpty(ApophysisApplication.BatchPathToOpen))
			{
				ReadBatchFromFile(ApophysisApplication.BatchPathToOpen);
			}
			else
			{
				GenerateRandomFlames(10, false);
			}
		}
		private void OnCameraEndEdit(object sender, CameraEndEditEventArgs e)
		{
			var flame = mBatchListController.GetSelectedFlame();
			if (flame == null || Initializer.IsBusy || !e.EditMade)
				return;

			FlamePropertiesController.UndoController.CommitChange(flame);
			FlamePropertiesController.RaiseFlameChanged();

			View.LoadingStatusLabel.Text = Application.ProductName;
		}
		private void OnCameraChanged(object sender, CameraChangedEventArgs e)
		{
			var flame = mBatchListController.GetSelectedFlame();
			if (flame == null || Initializer.IsBusy)
				return;

			using (mFlamePropertiesController.Initializer.Enter())
			{
				flame.Angle = e.Data.Angle;
				flame.Origin = e.Data.Origin;
				flame.Zoom = e.Data.Zoom;
				flame.PixelsPerUnit = e.Data.Scale <= 0 ? 0.01f : e.Data.Scale;
				mFlamePropertiesController.UpdateCamera();
			}
		}

		public IUndoController UndoController
		{
			get { return mUndoController; }
		}
		public IEditorController EditorController
		{
			get { return mEditorController; }
		}
		public IFullscreenController FullscreenController
		{
			get { return mFullscreenController; }
		}
		public IFlamePropertiesController FlamePropertiesController
		{
			get { return mFlamePropertiesController; }
		}
		public IMessagesController MessagesController
		{
			get { return mMessagesController; }
		}
		public ISettingsController SettingsController
		{
			get { return mSettingsController; }
		}
		public IRenderController RenderController
		{
			get { return mRenderController; }
		}
		public IBatchListController BatchListController
		{
			get { return mBatchListController; }
		}
		public IMainMenuController MainMenuController
		{
			get { return mMenuController; }
		}
		public IMainPreviewController MainPreviewController
		{
			get { return mMainPreviewController; }
		}
		public IMainToolbarController ToolbarController
		{
			get { return mToolbarController; }
		}
		public IAutosaveController AutosaveController
		{
			get { return mAutosaveController; }
		}

		public void ResizeWithoutUpdatingPreview()
		{
			var flame = mBatchListController.GetSelectedFlame();
			if (flame == null)
				return;

			using (mInitialize.Enter())
			{
				var frameSize = View.PreviewPicture.Size;
				var windowSize = View.Size;

				var delta = new Point(windowSize.Width - frameSize.Width, windowSize.Height - frameSize.Height);
				var newSize = new Size(flame.CanvasSize.Width + delta.X, flame.CanvasSize.Height + delta.Y);

				var bounds = Screen.FromControl(View).WorkingArea;
				var boundsFromPos = new Size(bounds.Width - bounds.Left, bounds.Height - bounds.Top);
				var boundSize = new Size(
					newSize.Width > boundsFromPos.Width ? boundsFromPos.Width : newSize.Width, 
					newSize.Height > boundsFromPos.Height ? boundsFromPos.Height : newSize.Height);

				View.Width = boundSize.Width;
				View.Height = boundSize.Height;
			}
		}

		public void AppendFlame([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");

			var index = mFlames.Append(flame);
			mBatchListController.SelectFlame(mFlames[index]);
		}

		public void RestoreAutosaveBatch(string targetPath)
		{
			var path = Environment.ExpandEnvironmentVariables(ApophysisSettings.Autosave.TargetPath);
			if (!File.Exists(path))
				throw new FileNotFoundException();

			var data = File.ReadAllText(path);

			File.WriteAllText(targetPath, data);
			ReadBatchFromFile(path, false);
			
		}
		public void ReadBatchFromFile(string path, bool withMessaging = true)
		{
			try
			{
				FlameCollection batch;

				if (withMessaging)
				{
					batch = FlameCollection.LoadFromXml(XElement.Parse(File.ReadAllText(path), LoadOptions.None));
				}
				else
				{
					using (MessageCenter.SuspendMessaging.Enter())
					{
						batch = FlameCollection.LoadFromXml(XElement.Parse(File.ReadAllText(path), LoadOptions.None));
					}
				}

				Flames = batch;
				View.Text = Application.ProductName + @" - " + path;
				mLastBatchPath = path;
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
		public void WriteCurrentFlameToClipboard()
		{
			var flame = mBatchListController.GetSelectedFlame();
			if (flame == null)
				return;

			XElement element; flame.WriteXml(out element);

			var data = element.ToString(SaveOptions.None);
			Clipboard.SetText(data);
		}

		public void LoadFlameAndEraseHistory([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");

			mEditorController.Flame = flame;
			mFlamePropertiesController.Flame = flame;

			View.FlameNameLabel.Text = flame.CalculatedName;

			mRenderController.UpdateSelection();
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
			}

			View.Text = Application.ProductName + @" - " + Common.RandomBatch;
		}

		public bool ConfirmExit()
		{
			if (!mHasChanges)
				return true;

			var result = MessageBox.Show(
				string.Format(Messages.ExitConfirmMessage),
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
				if (!string.IsNullOrEmpty(mLastBatchPath))
				{
					SaveCurrentBatch(mLastBatchPath);
				}
				else
				{
					mMenuController.OnSaveBatchClick(View, new EventArgs());
				}
			}

			return true;
		}
		public bool ConfirmReplaceBatch()
		{
			if (!mHasChanges)
				return true;

			var result = MessageBox.Show(
				string.Format(Messages.DiscardBatchConfirmMessage),
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
				if (!string.IsNullOrEmpty(mLastBatchPath))
				{
					SaveCurrentBatch(mLastBatchPath);
				}
				else
				{
					mMenuController.OnSaveBatchClick(View, new EventArgs());
				}
			}

			return true;
		}

		public void ReplaceBatchWithConfirm([NotNull] FlameCollection batch)
		{
			if (batch == null) throw new ArgumentNullException("batch");

			if (!ConfirmReplaceBatch())
				return;

			Flames = batch;
			mLastBatchPath = null;
		}
		public void DeleteFlameIfPossibleWithConfirm([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			if (!mFlames.CanRemove())
				return;

			if (!mFlames.Contains(flame))
				return;

			if (!ApophysisSettings.Common.ShowDeleteConfirmation || DialogResult.Yes == MessageBox.Show(
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

			SetDirty();
		}

		public void SetDirty()
		{
			mHasChanges = true;
		}
		public void SaveCurrentFlame(string path, int maxBatchSize = int.MaxValue)
		{
			var flame = mBatchListController.GetSelectedFlame();
			if (flame == null)
				return;

			SaveFlame(flame, path, maxBatchSize);
		}
		public void SaveFlame([NotNull] Flame flame, string path, int maxBatchSize = int.MaxValue, string batchName = null)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			FlameCollection batch;

			if (File.Exists(path))
			{
				using (MessageCenter.SuspendMessaging.Enter())
				{
					batch = FlameCollection.LoadFromXml(XElement.Parse(File.ReadAllText(path), LoadOptions.None));
				}

				batch.Name = batchName;

				for (int i = 0; i < (batch.Count + 1) - maxBatchSize; i++)
					batch.Remove(i);

				batch.Append(flame);
			}
			else
			{
				batch = new FlameCollection(new[] { flame }) { Name = batchName };
			}

			XElement outElement; batch.WriteXml(out outElement);
			File.WriteAllText(path, outElement.ToString(SaveOptions.None));
		}
		public void SaveCurrentBatch(string path)
		{
			XElement outElement; Flames.WriteXml(out outElement);
			File.WriteAllText(path, outElement.ToString(SaveOptions.None));

			View.Text = Application.ProductName + @" - " + path;
			mLastBatchPath = path;
		}

		private void ShowView(Form view)
		{
			if (view.Visible)
			{
				if (view.WindowState == FormWindowState.Minimized)
				{
					view.WindowState = FormWindowState.Normal;
				}
				view.BringToFront();
			}
			else
			{
				view.Show();
			}
		}

		public void ShowEditor()
		{
			ShowView(mEditorController.View);
		}
		public void ShowFlameProperties()
		{
			ShowView(mFlamePropertiesController.View);
			mFlamePropertiesController.View.Tabs.SelectTab(mFlamePropertiesController.View.CameraTab);
		}
		public void ShowImagingProperties()
		{
			ShowView(mFlamePropertiesController.View);
			mFlamePropertiesController.View.Tabs.SelectTab(mFlamePropertiesController.View.ImagingTab);
		}
		public void ShowPaletteProperties()
		{
			ShowView(mFlamePropertiesController.View);
			mFlamePropertiesController.View.Tabs.SelectTab(mFlamePropertiesController.View.PaletteTab);
		}
		public void ShowCanvasProperties()
		{
			ShowView(mFlamePropertiesController.View);
			mFlamePropertiesController.View.Tabs.SelectTab(mFlamePropertiesController.View.CanvasTab);
		}
		public void ShowMessages()
		{
			ShowView(mMessagesController.View);
		}
		public void ShowSettings()
		{
			ShowView(mSettingsController.View);
			mSettingsController.Update();
		}
		public void ShowRender()
		{
			ShowView(mRenderController.View);
			mRenderController.BatchMode = false;
			mRenderController.Update();
		}
		public void ShowRenderAll()
		{
			ShowView(mRenderController.View);
			mRenderController.BatchMode = true;
			mRenderController.Update();
		}

		public void ReloadSettings()
		{
			MainPreviewController.ReloadSettings();
		}
	}
}