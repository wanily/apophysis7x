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
			View.ToolBar.Visible = ApophysisSettings.IsMainToolbarVisible;
			View.BottomPanel.Visible = ApophysisSettings.IsMainStatusbarVisible;

			View.Load += OnViewLoaded;

			View.NewFlameButton.Click += mParent.MainMenuController.OnNewFlameClick;
			View.OpenBatchButton.Click += mParent.MainMenuController.OnOpenBatchClick;
			View.SaveFlameButton.Click += mParent.MainMenuController.OnSaveFlameClick;

			View.BatchListViewButton.Click += OnBatchListViewClick;
			View.BatchIconViewButton.Click += OnBatchIconViewClick;

			View.UndoButton.Click += mParent.MainMenuController.OnUndoClick;
			View.RedoButton.Click += mParent.MainMenuController.OnRedoClick;

			View.OpenFullscreenPreviewButton.Click += mParent.MainMenuController.OnFullscreenClick;
			View.EditorButton.Click += mParent.MainMenuController.OnEditorClick;
			View.FlamePropertiesButton.Click += mParent.MainMenuController.OnFlamePropertiesClick;
			View.PalettePropertiesButton.Click += mParent.MainMenuController.OnPalettePropertiesClick;
			View.CanvasPropertiesButton.Click += mParent.MainMenuController.OnCanvasPropertiesClick;

			View.ToggleGuidelinesButton.Click += OnToggleGuidelines;
			View.ToggleTransparencyButton.Click += OnToggleTransparency;
		}
		protected override void DetachView()
		{
			View.Load -= OnViewLoaded;

			View.NewFlameButton.Click -= mParent.MainMenuController.OnNewFlameClick;
			View.OpenBatchButton.Click -= mParent.MainMenuController.OnOpenBatchClick;
			View.SaveFlameButton.Click -= mParent.MainMenuController.OnSaveFlameClick;

			View.BatchListViewButton.Click -= OnBatchListViewClick;
			View.BatchIconViewButton.Click -= OnBatchIconViewClick;

			View.UndoButton.Click -= mParent.MainMenuController.OnUndoClick;
			View.RedoButton.Click -= mParent.MainMenuController.OnRedoClick;

			View.OpenFullscreenPreviewButton.Click -= mParent.MainMenuController.OnFullscreenClick;
			View.EditorButton.Click -= mParent.MainMenuController.OnEditorClick;
			View.FlamePropertiesButton.Click -= mParent.MainMenuController.OnFlamePropertiesClick;
			View.PalettePropertiesButton.Click -= mParent.MainMenuController.OnPalettePropertiesClick;
			View.CanvasPropertiesButton.Click -= mParent.MainMenuController.OnCanvasPropertiesClick;

			View.ToggleGuidelinesButton.Click -= OnToggleGuidelines;
			View.ToggleTransparencyButton.Click -= OnToggleTransparency;

			ApophysisSettings.IsMainToolbarVisible = View.ToolBar.Visible;
			ApophysisSettings.IsMainStatusbarVisible = View.BottomPanel.Visible;
		}

		private void OnViewLoaded(object sender, EventArgs e)
		{
			UpdateRootPanelSize();
			UpdateButtonStates();

			mParent.MainMenuController.UpdateButtonStates();
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
		private void OnToggleGuidelines(object sender, EventArgs e)
		{
			View.ToggleGuidelinesButton.Checked = !View.ToggleGuidelinesButton.Checked;
			View.ShowGuidelines = View.ToggleGuidelinesButton.Checked;
		}
		private void OnToggleTransparency(object sender, EventArgs e)
		{
			View.ToggleTransparencyButton.Checked = !View.ToggleTransparencyButton.Checked;
			View.ShowTransparency = View.ToggleTransparencyButton.Checked;
		}

		public void UpdateButtonStates()
		{
			View.UndoButton.Enabled = mParent.UndoController.CanUndo;
			View.RedoButton.Enabled = mParent.UndoController.CanRedo;

			View.BatchListViewButton.Checked = !mParent.BatchListController.IsIconViewEnabled;
			View.BatchIconViewButton.Checked = mParent.BatchListController.IsIconViewEnabled;

			View.ToggleGuidelinesButton.Checked = View.ShowGuidelines;
			View.ToggleTransparencyButton.Checked = View.ShowTransparency;
		}
		public void UpdateRootPanelSize()
		{
			View.RootSplitter.Height = 
				View.ClientPanel.Height
			    - View.BottomPanel.Height * (View.BottomPanel.Visible ? 1 : 0)
				;//+ View.ToolBar.Height * (View.ToolBar.Visible ? 0 : 1);
		}
	}
}