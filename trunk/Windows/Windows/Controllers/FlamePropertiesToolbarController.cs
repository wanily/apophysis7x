using System;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class FlamePropertiesToolbarController : Controller<FlameProperties>
	{
		private FlamePropertiesController mParent;

		public FlamePropertiesToolbarController([NotNull] FlameProperties view, [NotNull] FlamePropertiesController parent) : base(view)
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
			View.UndoButton.Click += OnUndoClick;
			View.RedoButton.Click += OnRedoClick;
		}
		protected override void DetachView()
		{
			View.UndoButton.Click -= OnUndoClick;
			View.RedoButton.Click -= OnRedoClick;
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
			View.UndoButton.Enabled = mParent.UndoController.CanUndo;
			View.RedoButton.Enabled = mParent.UndoController.CanRedo;
		}
	}
}