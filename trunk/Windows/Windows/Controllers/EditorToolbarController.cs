using System;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class EditorToolbarController : Controller<Editor>
	{
		private EditorController mParent;

		public EditorToolbarController([NotNull] Editor view, [NotNull] EditorController parent) : base(view)
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
			View.ResetAllIteratorsButton.Click += OnResetAllIteratorsClick;

			View.AddIteratorButton.Click += OnAddIteratorClick;
			View.DuplicateIteratorButton.Click += OnDuplicateIteratorClick;
			View.RemoveIteratorButton.Click += OnRemoveIteratorClick;

			View.UndoButton.Click += OnUndoClick;
			View.RedoButton.Click += OnRedoClick;

			View.IteratorResetButton.Click += OnResetIteratorClick;
			View.IteratorResetOriginButton.Click += OnResetIteratorClick;
			View.IteratorResetAngleButton.Click += OnResetIteratorClick;
			View.IteratorResetScaleButton.Click += OnResetIteratorClick;
		}
		protected override void DetachView()
		{
			View.ResetAllIteratorsButton.Click -= OnResetAllIteratorsClick;

			View.AddIteratorButton.Click -= OnAddIteratorClick;
			View.DuplicateIteratorButton.Click -= OnDuplicateIteratorClick;
			View.RemoveIteratorButton.Click -= OnRemoveIteratorClick;

			View.UndoButton.Click -= OnUndoClick;
			View.RedoButton.Click -= OnRedoClick;

			View.IteratorResetButton.Click -= OnResetIteratorClick;
			View.IteratorResetOriginButton.Click -= OnResetIteratorClick;
			View.IteratorResetAngleButton.Click -= OnResetIteratorClick;
			View.IteratorResetScaleButton.Click -= OnResetIteratorClick;
		}

		private void OnResetAllIteratorsClick(object sender, EventArgs e)
		{
			View.IteratorCanvas.Commands.ResetAll();
		}

		private void OnAddIteratorClick(object sender, EventArgs e)
		{
			View.IteratorCanvas.Commands.NewIterator();
		}
		private void OnDuplicateIteratorClick(object sender, EventArgs e)
		{
			View.IteratorCanvas.Commands.DuplicateSelectedIterator();
		}
		private void OnRemoveIteratorClick(object sender, EventArgs e)
		{
			View.IteratorCanvas.Commands.RemoveSelectedIterator();
		}

		private void OnUndoClick(object sender, EventArgs e)
		{
			mParent.Flame = mParent.UndoController.Undo();
		}
		private void OnRedoClick(object sender, EventArgs e)
		{
			mParent.Flame = mParent.UndoController.Redo();
		}

		private void OnResetIteratorClick(object sender, EventArgs e)
		{
			Action action = null;

			if (ReferenceEquals(sender, View.IteratorResetButton)) action = View.IteratorCanvas.Commands.ResetSelectedIterator;
			if (ReferenceEquals(sender, View.IteratorResetOriginButton)) action = View.IteratorCanvas.Commands.ResetSelectedIteratorOrigin;
			if (ReferenceEquals(sender, View.IteratorResetAngleButton)) action = View.IteratorCanvas.Commands.ResetSelectedIteratorAngle;
			if (ReferenceEquals(sender, View.IteratorResetScaleButton)) action = View.IteratorCanvas.Commands.ResetSelectedIteratorScale;

			if (action != null)
			{
				action();
			}
		}

		public void UpdateButtonStates()
		{
			View.DuplicateIteratorButton.Enabled = View.IteratorCanvas.SelectedIterator != null;
			View.RemoveIteratorButton.Enabled = mParent.Flame.Iterators.Count > 1;

			View.UndoButton.Enabled = mParent.UndoController.CanUndo;
			View.RedoButton.Enabled = mParent.UndoController.CanRedo;
		}
	}
}