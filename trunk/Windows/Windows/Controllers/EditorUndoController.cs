using System;
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
			View.IteratorColorScrollBar.MouseUp += OnRequestCommit;
			View.IteratorColorSpeedDragPanel.EndEdit += OnRequestCommit;
			View.IteratorDirectColorDragPanel.EndEdit += OnRequestCommit;
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
		}
		protected override void DetachView()
		{
			mParent.UndoController.StackChanged -= OnUndoStackChanged;

			View.IteratorCanvas.EndEdit -= OnRequestCommit;
			View.IteratorColorDragPanel.EndEdit -= OnRequestCommit;
			View.IteratorColorScrollBar.MouseUp -= OnRequestCommit;
			View.IteratorColorSpeedDragPanel.EndEdit -= OnRequestCommit;
			View.IteratorDirectColorDragPanel.EndEdit -= OnRequestCommit;
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
		}

		private void OnRequestCommit(object sender, EventArgs e)
		{
			mParent.UndoController.CommitChange(mParent.Flame);
		}
		private void OnUndoStackChanged(object sender, EventArgs e)
		{
			mParent.UpdateToolbar();
		}
	}
}