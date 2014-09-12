using System;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class MainToolbarController : Controller<Main>
	{
		private MainController mParent;

		public MainToolbarController([NotNull] Main view, [NotNull] MainController parent) : base(view)
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
			View.NewFlameButton.Click += mParent.MainMenuController.OnNewFlameClick;
			View.OpenBatchButton.Click += mParent.MainMenuController.OnOpenBatchClick;
			View.SaveFlameButton.Click += mParent.MainMenuController.OnSaveFlameClick;

			View.BatchListViewButton.Click += OnBatchListViewClick;
			View.BatchIconViewButton.Click += OnBatchIconViewClick;

			View.UndoButton.Click += mParent.MainMenuController.OnUndoClick;
			View.RedoButton.Click += mParent.MainMenuController.OnRedoClick;

			View.EditorButton.Click += mParent.MainMenuController.OnEditorClick;

			UpdateButtonStates();
		}
		protected override void DetachView()
		{
			View.NewFlameButton.Click -= mParent.MainMenuController.OnNewFlameClick;
			View.OpenBatchButton.Click -= mParent.MainMenuController.OnOpenBatchClick;
			View.SaveFlameButton.Click -= mParent.MainMenuController.OnSaveFlameClick;

			View.BatchListViewButton.Click -= OnBatchListViewClick;
			View.BatchIconViewButton.Click -= OnBatchIconViewClick;

			View.UndoButton.Click -= mParent.MainMenuController.OnUndoClick;
			View.RedoButton.Click -= mParent.MainMenuController.OnRedoClick;

			View.EditorButton.Click -= mParent.MainMenuController.OnEditorClick;
		}

		private void OnBatchListViewClick(object sender, EventArgs e)
		{
			View.BatchListViewButton.Checked = !View.BatchListViewButton.Checked;
			View.BatchIconViewButton.Checked = !View.BatchListViewButton.Checked;
			mParent.BatchListController.IsIconViewEnabled = !View.BatchListViewButton.Checked;
		}
		private void OnBatchIconViewClick(object sender, EventArgs e)
		{
			View.BatchIconViewButton.Checked = !View.BatchIconViewButton.Checked;
			View.BatchListViewButton.Checked = !View.BatchIconViewButton.Checked;
			mParent.BatchListController.IsIconViewEnabled = View.BatchIconViewButton.Checked;
		}

		public void UpdateButtonStates()
		{
			View.UndoButton.Enabled = mParent.UndoController.CanUndo;
			View.RedoButton.Enabled = mParent.UndoController.CanRedo;

			View.BatchListViewButton.Checked = !mParent.BatchListController.IsIconViewEnabled;
			View.BatchIconViewButton.Checked = mParent.BatchListController.IsIconViewEnabled;
		}
	}
}