using System;
using System.Linq;
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
		}
		protected override void DetachView()
		{
			View.ResetAllIteratorsButton.Click -= OnResetAllIteratorsClick;

			View.AddIteratorButton.Click -= OnAddIteratorClick;
			View.DuplicateIteratorButton.Click -= OnDuplicateIteratorClick;
			View.RemoveIteratorButton.Click -= OnRemoveIteratorClick;

			View.UndoButton.Click -= OnUndoClick;
			View.RedoButton.Click -= OnRedoClick;
		}

		private void OnResetAllIteratorsClick(object sender, EventArgs e)
		{
			using (mParent.Initializer.Enter())
			{
				var toRemove = mParent.Flame.Iterators.Skip(1).ToArray();
				foreach (var item in toRemove)
				{
					mParent.Flame.Iterators.Remove(item);
				}

				mParent.Flame.Iterators[0].Reset();
			}

			mParent.AfterReset();
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

		public void UpdateButtonStates()
		{
			View.DuplicateIteratorButton.Enabled = View.IteratorCanvas.SelectedIterator != null;
			View.RemoveIteratorButton.Enabled = mParent.Flame.Iterators.Count > 1;

			View.UndoButton.Enabled = mParent.UndoController.CanUndo;
			View.RedoButton.Enabled = mParent.UndoController.CanRedo;
		}
	}
}