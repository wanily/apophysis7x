using System;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class MainMenuController : Controller<Main>, IMainMenuController
	{
		private MainController mParent;

		public MainMenuController([NotNull] Main view, [NotNull] MainController parent) : base(view)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			mParent = parent;
		}
		protected override void DisposeOverride(bool disposing)
		{
			mParent = null;
		}

		protected override void AttachView()
		{
			View.NewFlameMenuItem.Click += OnNewFlameClick;
			View.OpenBatchMenuItem.Click += OnOpenBatchClick;
			View.RestoreAutosaveMenuItem.Click += OnRestoreAutosaveClick;
			View.SaveFlameMenuItem.Click += OnSaveFlameClick;
			View.SaveBatchMenuItem.Click += OnSaveBatchClick;
			View.PaletteFromImageMenuItem.Click += OnPaletteFromImageClick;
			View.BrowsePalettesMenuItem.Click += OnBrowsePalettesClick;
			View.RandomBatchMenuItem.Click += OnRandomBatchClick;
			View.RenderFlameMenuItem.Click += OnRenderFlameClick;
			View.RenderBatchMenuItem.Click += OnRenderBatchClick;
			View.ExitMenuItem.Click += OnExitClick;

			View.UndoMenuItem.Click += OnUndoClick;
			View.RedoMenuItem.Click += OnRedoClick;
			View.CopyMenuItem.Click += OnCopyClick;
			View.PasteMenuItem.Click += OnPasteClick;

			View.OpenFullscreenPreviewMenuItem.Click += OnFullscreenClick;
			View.EditorMenuItem.Click += OnEditorClick;
			View.FlamePropertiesMenuItem.Click += OnFlamePropertiesClick;
			View.PalettePropertiesMenuItem.Click += OnPalettePropertiesClick;
			View.CanvasPropertiesMenuItem.Click += OnCanvasPropertiesClick;
			View.MessagesMenuItem.Click += OnMessagesClick;
			View.OpenSettingsMenuItem.Click += OnSettingsClick;
			View.ShowToolBarMenuItem.Click += OnShowToolbarClick;
			View.ShowStatusBarMenuItem.Click += OnShowStatusbarClick;
			View.ShowBatchMenuItem.Click += OnShowBatchListClick;
			View.ResetLayoutMenuItem.Click += OnResetLayoutClick;

			View.ResetCameraMenuItem.Click += OnResetCameraClick;
			View.RandomizeFlameMenuItem.Click += OnRandomizeClick;
			View.SummarizeMenuItem.Click += OnSummarizeClick;

			View.OpenAboutMenuItem.Click += OnAboutClick;

			UpdateButtonStates();
		}
		protected override void DetachView()
		{
			View.NewFlameMenuItem.Click -= OnNewFlameClick;
			View.OpenBatchMenuItem.Click -= OnOpenBatchClick;
			View.RestoreAutosaveMenuItem.Click -= OnRestoreAutosaveClick;
			View.SaveFlameMenuItem.Click -= OnSaveFlameClick;
			View.SaveBatchMenuItem.Click -= OnSaveBatchClick;
			View.PaletteFromImageMenuItem.Click -= OnPaletteFromImageClick;
			View.BrowsePalettesMenuItem.Click -= OnBrowsePalettesClick;
			View.RandomBatchMenuItem.Click -= OnRandomBatchClick;
			View.RenderFlameMenuItem.Click -= OnRenderFlameClick;
			View.RenderBatchMenuItem.Click -= OnRenderBatchClick;
			View.ExitMenuItem.Click -= OnExitClick;

			View.UndoMenuItem.Click -= OnUndoClick;
			View.RedoMenuItem.Click -= OnRedoClick;
			View.CopyMenuItem.Click -= OnCopyClick;
			View.PasteMenuItem.Click -= OnPasteClick;

			View.OpenFullscreenPreviewMenuItem.Click -= OnFullscreenClick;
			View.EditorMenuItem.Click -= OnEditorClick;
			View.FlamePropertiesMenuItem.Click -= OnFlamePropertiesClick;
			View.PalettePropertiesMenuItem.Click -= OnPalettePropertiesClick;
			View.CanvasPropertiesMenuItem.Click -= OnCanvasPropertiesClick;
			View.MessagesMenuItem.Click -= OnMessagesClick;
			View.OpenSettingsMenuItem.Click -= OnSettingsClick;
			View.ShowToolBarMenuItem.Click -= OnShowToolbarClick;
			View.ShowStatusBarMenuItem.Click -= OnShowStatusbarClick;
			View.ShowBatchMenuItem.Click -= OnShowBatchListClick;
			View.ResetLayoutMenuItem.Click -= OnResetLayoutClick;

			View.ResetCameraMenuItem.Click -= OnResetCameraClick;
			View.RandomizeFlameMenuItem.Click -= OnRandomizeClick;
			View.SummarizeMenuItem.Click -= OnSummarizeClick;

			View.OpenAboutMenuItem.Click -= OnAboutClick;
		}

		public void OnNewFlameClick(object sender, EventArgs e)
		{
			mParent.AppendFlame(new Flame());
		}
		public void OnOpenBatchClick(object sender, EventArgs e)
		{
			if (!mParent.ConfirmReplaceBatch())
				return;

			using (var dialog = new FileDialogController<OpenFileDialog>("Open batch...", FileDialogController.BatchFilesFilter, FileDialogController.AllFilesFilter))
			{
				var result = dialog.GetFileName();
				if (string.IsNullOrEmpty(result))
					return;

				mParent.ReadBatchFromFile(result);
			}
		}
		public void OnRestoreAutosaveClick(object sender, EventArgs e)
		{
			var path = Environment.ExpandEnvironmentVariables(ApophysisSettings.Autosave.TargetPath);
			if (!File.Exists(path))
			{
				MessageBox.Show(string.Format("Could not find autosave file. Perhaps no autosaves have been written yet."), View.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (!mParent.ConfirmReplaceBatch())
				return;

			using (var dialog = new FileDialogController<SaveFileDialog>("Choose location to copy autosave batch to...", FileDialogController.BatchFilesFilter, FileDialogController.AllFilesFilter))
			{
				var result = dialog.GetFileName();
				if (string.IsNullOrEmpty(result))
					return;

				mParent.RestoreAutosaveBatch(result);
			}
		}
		public void OnSaveFlameClick(object sender, EventArgs e)
		{
			using (var dialog = new FileDialogController<SaveFileDialog>("Save flame...", FileDialogController.BatchFilesFilter, FileDialogController.AllFilesFilter))
			{
				var result = dialog.GetFileName(false);
				if (string.IsNullOrEmpty(result))
					return;

				mParent.SaveCurrentFlame(result);
			}
		}
		public void OnSaveBatchClick(object sender, EventArgs e)
		{
			using (var dialog = new FileDialogController<SaveFileDialog>("Save batch...", FileDialogController.BatchFilesFilter, FileDialogController.AllFilesFilter))
			{
				var result = dialog.GetFileName(true);
				if (string.IsNullOrEmpty(result))
					return;

				mParent.SaveCurrentBatch(result);
			}
		}
		public void OnPaletteFromImageClick(object sender, EventArgs e)
		{
			//todo
		}
		public void OnBrowsePalettesClick(object sender, EventArgs e)
		{
			//todo
		}
		public void OnRandomBatchClick(object sender, EventArgs e)
		{
			mParent.GenerateRandomFlames(10);
		}
		public void OnRenderFlameClick(object sender, EventArgs e)
		{
			mParent.ShowRender();
		}
		public void OnRenderBatchClick(object sender, EventArgs e)
		{
			mParent.ShowRenderAll();
		}
		public void OnExitClick(object sender, EventArgs e)
		{
			if (!mParent.ConfirmExit())
				return;

			View.Close();
		}

		public void OnUndoClick(object sender, EventArgs e)
		{
			if (mParent.UndoController.CanUndo)
			{
				mParent.UndoController.Undo();
			}
		}
		public void OnRedoClick(object sender, EventArgs e)
		{
			if (mParent.UndoController.CanRedo)
			{
				mParent.UndoController.Redo();
			}
		}
		public void OnCopyClick(object sender, EventArgs e)
		{
			mParent.WriteCurrentFlameToClipboard();
		}
		public void OnPasteClick(object sender, EventArgs e)
		{
			mParent.ReadFlameFromClipboard();
		}

		public void OnFullscreenClick(object sender, EventArgs e)
		{
			mParent.FullscreenController.EnterFullscreen();
		}
		public void OnEditorClick(object sender, EventArgs e)
		{
			mParent.ShowEditor();
		}
		public void OnFlamePropertiesClick(object sender, EventArgs e)
		{
			mParent.ShowFlameProperties();
		}
		public void OnPalettePropertiesClick(object sender, EventArgs e)
		{
			mParent.ShowPaletteProperties();
		}
		public void OnCanvasPropertiesClick(object sender, EventArgs e)
		{
			mParent.ShowCanvasProperties();
		}
		public void OnMessagesClick(object sender, EventArgs e)
		{
			mParent.ShowMessages();
		}
		public void OnSettingsClick(object sender, EventArgs e)
		{
			mParent.ShowSettings();
		}
		public void OnShowToolbarClick(object sender, EventArgs e)
		{
			View.ShowToolBarMenuItem.Checked = !View.ShowToolBarMenuItem.Checked;
			View.ToolBar.Visible = View.ShowToolBarMenuItem.Checked;

			mParent.ToolbarController.UpdateRootPanelSize();
		}
		public void OnShowStatusbarClick(object sender, EventArgs e)
		{
			View.ShowStatusBarMenuItem.Checked = !View.ShowStatusBarMenuItem.Checked;
			View.BottomPanel.Visible = View.ShowStatusBarMenuItem.Checked;

			mParent.ToolbarController.UpdateRootPanelSize();
		}
		public void OnShowBatchListClick(object sender, EventArgs e)
		{
			View.ShowBatchMenuItem.Checked = !View.ShowBatchMenuItem.Checked;
			View.RootSplitter.Panel1Collapsed = !View.ShowBatchMenuItem.Checked;
		}
		public void OnResetLayoutClick(object sender, EventArgs e)
		{
			View.RootSplitter.SplitterDistance = 230;
			View.BottomPanel.Visible = true;
			View.ToolBar.Visible = true;
			View.RootSplitter.Panel1Collapsed = false;

			mParent.ToolbarController.UpdateRootPanelSize();

			UpdateButtonStates();
		}

		public void OnResetCameraClick(object sender, EventArgs e)
		{
			var flame = mParent.BatchListController.GetSelectedFlame();
			if (flame == null)
				return;

			flame.Angle = 0;
			flame.Origin = new Vector2();
			flame.Zoom = 0;
			flame.PixelsPerUnit = 25 * flame.CanvasSize.Width / 100.0f;

			mParent.UndoController.CommitChange(flame);
			mParent.FlamePropertiesController.RaiseFlameChanged();
			mParent.FlamePropertiesController.UpdateCamera();

			mParent.BatchListController.UpdateSelectedPreview();
		}
		public void OnRandomizeClick(object sender, EventArgs e)
		{
			var flame = mParent.BatchListController.GetSelectedFlame();
			if (flame == null)
				return;
			
			flame.Randomize();

			mParent.UndoController.CommitChange(flame);
			mParent.FlamePropertiesController.RaiseFlameChanged();
			mParent.FlamePropertiesController.UpdateCamera();

			mParent.BatchListController.UpdateSelectedPreview();
		}
		public void OnSummarizeClick(object sender, EventArgs e)
		{
			var flame = mParent.BatchListController.GetSelectedFlame();
			if (flame == null)
				return;

			mParent.MessagesController.Summarize(flame);
		}

		public void OnAboutClick(object sender, EventArgs e)
		{
			using (var about = new About())
			{
				about.Owner = ApophysisApplication.MainWindow.View as Form;
				about.ShowDialog();
			}
		}

		public void UpdateButtonStates()
		{
			View.UndoMenuItem.Enabled = mParent.UndoController.CanUndo;
			View.RedoMenuItem.Enabled = mParent.UndoController.CanRedo;

			View.ShowToolBarMenuItem.Checked = View.ToolBar.Visible;
			View.ShowStatusBarMenuItem.Checked = View.StatusBar.Visible;
			View.ShowBatchMenuItem.Checked = !View.RootSplitter.Panel1Collapsed;
		}
	}
}