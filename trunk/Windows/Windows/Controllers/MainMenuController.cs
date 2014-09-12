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

			View.EditorMenuItem.Click += OnEditorClick;

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

			View.EditorMenuItem.Click -= OnEditorClick;
		}

		private void OnNewFlameClick(object sender, EventArgs e)
		{
			mParent.AppendFlame(new Flame());
		}
		private void OnOpenBatchClick(object sender, EventArgs e)
		{
			if (!mParent.ConfirmReplaceBatch())
				return;

			using (var dialog = new FileDialogController<OpenFileDialog>("Open batch...", FileDialogController.BatchFilesFilter, FileDialogController.AllFilesFilter))
			{
				var result = dialog.GetFileName();
				if (string.IsNullOrEmpty(result))
					return;

				mParent.ReadFlameFromFile(result);
			}
		}
		private void OnRestoreAutosaveClick(object sender, EventArgs e)
		{
			//todo
		}
		private void OnSaveFlameClick(object sender, EventArgs e)
		{
			//todo
		}
		private void OnSaveBatchClick(object sender, EventArgs e)
		{
			mParent.SaveCurrentBatch();
		}
		private void OnPaletteFromImageClick(object sender, EventArgs e)
		{
			//todo
		}
		private void OnBrowsePalettesClick(object sender, EventArgs e)
		{
			//todo
		}
		private void OnRandomBatchClick(object sender, EventArgs e)
		{
			mParent.GenerateRandomFlames(10);
		}
		private void OnExitClick(object sender, EventArgs e)
		{
			View.Close();
		}

		private void OnUndoClick(object sender, EventArgs e)
		{
			if (mParent.UndoController.CanUndo)
			{
				mParent.UndoController.Undo();
			}
		}
		private void OnRedoClick(object sender, EventArgs e)
		{
			if (mParent.UndoController.CanRedo)
			{
				mParent.UndoController.Redo();
			}
		}
		private void OnCopyClick(object sender, EventArgs e)
		{
			//todo
		}
		private void OnPasteClick(object sender, EventArgs e)
		{
			mParent.ReadFlameFromClipboard();
		}

		private void OnEditorClick(object sender, EventArgs e)
		{
			mParent.ShowEditor();
		}

		public void UpdateButtonStates()
		{
			View.UndoMenuItem.Enabled = mParent.UndoController.CanUndo;
			View.RedoMenuItem.Enabled = mParent.UndoController.CanRedo;
		}
	}
}