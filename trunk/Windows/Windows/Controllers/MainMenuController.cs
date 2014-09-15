using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class MainMenuController : Controller<Main>
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

			View.ShowToolBarMenuItem.Click += OnShowToolbarClick;
			View.ShowStatusBarMenuItem.Click += OnShowStatusbarClick;
			View.ShowBatchMenuItem.Click += OnShowBatchListClick;
			View.ResetLayoutMenuItem.Click += OnResetLayoutClick;

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

			View.ShowToolBarMenuItem.Click -= OnShowToolbarClick;
			View.ShowStatusBarMenuItem.Click -= OnShowStatusbarClick;
			View.ShowBatchMenuItem.Click -= OnShowBatchListClick;
			View.ResetLayoutMenuItem.Click -= OnResetLayoutClick;
		}

		internal void OnNewFlameClick(object sender, EventArgs e)
		{
			mParent.AppendFlame(new Flame());
		}
		internal void OnOpenBatchClick(object sender, EventArgs e)
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
		internal void OnRestoreAutosaveClick(object sender, EventArgs e)
		{
			//todo
		}
		internal void OnSaveFlameClick(object sender, EventArgs e)
		{
			//todo
		}
		internal void OnSaveBatchClick(object sender, EventArgs e)
		{
			mParent.SaveCurrentBatch();
		}
		internal void OnPaletteFromImageClick(object sender, EventArgs e)
		{
			//todo
		}
		internal void OnBrowsePalettesClick(object sender, EventArgs e)
		{
			//todo
		}
		internal void OnRandomBatchClick(object sender, EventArgs e)
		{
			mParent.GenerateRandomFlames(10);
		}
		internal void OnExitClick(object sender, EventArgs e)
		{
			View.Close();
		}

		internal void OnUndoClick(object sender, EventArgs e)
		{
			if (mParent.UndoController.CanUndo)
			{
				mParent.UndoController.Undo();
			}
		}
		internal void OnRedoClick(object sender, EventArgs e)
		{
			if (mParent.UndoController.CanRedo)
			{
				mParent.UndoController.Redo();
			}
		}
		internal void OnCopyClick(object sender, EventArgs e)
		{
			//todo
		}
		internal void OnPasteClick(object sender, EventArgs e)
		{
			mParent.ReadFlameFromClipboard();
		}

		internal void OnFullscreenClick(object sender, EventArgs e)
		{
			mParent.FullscreenController.EnterFullscreen();
		}
		internal void OnEditorClick(object sender, EventArgs e)
		{
			mParent.ShowEditor();
		}
		internal void OnFlamePropertiesClick(object sender, EventArgs e)
		{
			mParent.ShowFlameProperties();
		}
		internal void OnPalettePropertiesClick(object sender, EventArgs e)
		{
			mParent.ShowPaletteProperties();
		}
		internal void OnCanvasPropertiesClick(object sender, EventArgs e)
		{
			mParent.ShowCanvasProperties();
		}

		internal void OnShowToolbarClick(object sender, EventArgs e)
		{
			View.ShowToolBarMenuItem.Checked = !View.ShowToolBarMenuItem.Checked;
			View.ToolBar.Visible = View.ShowToolBarMenuItem.Checked;

			mParent.ToolbarController.UpdateRootPanelSize();
		}
		internal void OnShowStatusbarClick(object sender, EventArgs e)
		{
			View.ShowStatusBarMenuItem.Checked = !View.ShowStatusBarMenuItem.Checked;
			View.BottomPanel.Visible = View.ShowStatusBarMenuItem.Checked;

			mParent.ToolbarController.UpdateRootPanelSize();
		}
		internal void OnShowBatchListClick(object sender, EventArgs e)
		{
			View.ShowBatchMenuItem.Checked = !View.ShowBatchMenuItem.Checked;
			View.RootSplitter.Panel1Collapsed = !View.ShowBatchMenuItem.Checked;
		}
		internal void OnResetLayoutClick(object sender, EventArgs e)
		{
			View.RootSplitter.SplitterDistance = 230;
			View.BottomPanel.Visible = true;
			View.ToolBar.Visible = true;
			View.RootSplitter.Panel1Collapsed = false;

			mParent.ToolbarController.UpdateRootPanelSize();

			UpdateButtonStates();
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