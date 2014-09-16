using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class EditorUndoController : Controller<Editor>
	{
		private EditorController mParent;

		public EditorUndoController([NotNull] Editor view, [NotNull] EditorController parent) : base(view)
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
			mParent.UndoController.StackChanged += OnUndoStackChanged;

			View.IteratorCanvas.EndEdit += OnRequestCommit;
			View.IteratorColorDragPanel.EndEdit += OnRequestCommit;
			View.IteratorColorScrollBar.Scroll += OnScrollbarCommit;
			View.IteratorColorSpeedDragPanel.EndEdit += OnRequestCommit;
			View.IteratorDirectColorDragPanel.EndEdit += OnRequestCommit;
			View.IteratorWeightDragPanel.EndEdit += OnRequestCommit;
			View.IteratorWeightTextBox.LostFocus += OnRequestCommit;
			View.IteratorNameTextBox.LostFocus += OnRequestCommit;
			View.IteratorOpacityDragPanel.EndEdit += OnRequestCommit;
			View.IteratorPointOxTextBox.LostFocus += OnRequestCommit;
			View.IteratorPointOyTextBox.LostFocus += OnRequestCommit;
			View.IteratorPointXxTextBox.LostFocus += OnRequestCommit;
			View.IteratorPointXyTextBox.LostFocus += OnRequestCommit;
			View.IteratorPointYxTextBox.LostFocus += OnRequestCommit;
			View.IteratorPointYyTextBox.LostFocus += OnRequestCommit;
			View.IteratorPreAffineOxTextBox.LostFocus += OnRequestCommit;
			View.IteratorPreAffineOyTextBox.LostFocus += OnRequestCommit;
			View.IteratorPreAffineXxTextBox.LostFocus += OnRequestCommit;
			View.IteratorPreAffineXyTextBox.LostFocus += OnRequestCommit;
			View.IteratorPreAffineYxTextBox.LostFocus += OnRequestCommit;
			View.IteratorPreAffineYyTextBox.LostFocus += OnRequestCommit;
			View.IteratorPostAffineOxTextBox.LostFocus += OnRequestCommit;
			View.IteratorPostAffineOyTextBox.LostFocus += OnRequestCommit;
			View.IteratorPostAffineXxTextBox.LostFocus += OnRequestCommit;
			View.IteratorPostAffineXyTextBox.LostFocus += OnRequestCommit;
			View.IteratorPostAffineYxTextBox.LostFocus += OnRequestCommit;
			View.IteratorPostAffineYyTextBox.LostFocus += OnRequestCommit;
			View.VariablesGrid.CellEndEdit += OnRequestCommit;
			View.VariationsGrid.CellEndEdit += OnRequestCommit;
			View.ClearVariationsButton.Click += OnRequestCommit;
			View.PaletteSelectComboBox.SelectedIndexChanged += OnRequestCommit;
		}
		protected override void DetachView()
		{
			mParent.UndoController.StackChanged -= OnUndoStackChanged;

			View.IteratorCanvas.EndEdit -= OnRequestCommit;
			View.IteratorColorDragPanel.EndEdit -= OnRequestCommit;
			View.IteratorColorScrollBar.Scroll -= OnScrollbarCommit;
			View.IteratorColorSpeedDragPanel.EndEdit -= OnRequestCommit;
			View.IteratorDirectColorDragPanel.EndEdit -= OnRequestCommit;
			View.IteratorWeightDragPanel.EndEdit -= OnRequestCommit;
			View.IteratorWeightTextBox.LostFocus -= OnRequestCommit;
			View.IteratorNameTextBox.LostFocus -= OnRequestCommit;
			View.IteratorOpacityDragPanel.EndEdit -= OnRequestCommit;
			View.IteratorPointOxTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPointOyTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPointXxTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPointXyTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPointYxTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPointYyTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPreAffineOxTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPreAffineOyTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPreAffineXxTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPreAffineXyTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPreAffineYxTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPreAffineYyTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPostAffineOxTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPostAffineOyTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPostAffineXxTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPostAffineXyTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPostAffineYxTextBox.LostFocus -= OnRequestCommit;
			View.IteratorPostAffineYyTextBox.LostFocus -= OnRequestCommit;
			View.VariablesGrid.CellEndEdit -= OnRequestCommit;
			View.VariationsGrid.CellEndEdit -= OnRequestCommit;
			View.ClearVariationsButton.Click -= OnRequestCommit;
			View.PaletteSelectComboBox.SelectedIndexChanged -= OnRequestCommit;
		}

		private void OnScrollbarCommit(object sender, ScrollEventArgs e)
		{
			if (e.Type == ScrollEventType.EndScroll)
				OnRequestCommit(sender, e);
		}
		private void OnRequestCommit(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;

			mParent.UndoController.CommitChange(mParent.Flame);
			mParent.RaiseFlameChanged();
		}
		private void OnUndoStackChanged(object sender, EventArgs e)
		{
			mParent.UpdateToolbar();
		}
	}
}