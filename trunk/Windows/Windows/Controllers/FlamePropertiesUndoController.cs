using System;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class FlamePropertiesUndoController : Controller<FlameProperties>
	{
		private FlamePropertiesController mParent;

		public FlamePropertiesUndoController([NotNull] FlameProperties view, [NotNull] FlamePropertiesController parent) : base(view)
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

			//todo register all the fields :D
			//View.IteratorColorDragPanel.EndEdit += OnRequestCommit;
		}
		protected override void DetachView()
		{
			mParent.UndoController.StackChanged -= OnUndoStackChanged;

			//todo unregister all the fields :D
			//View.IteratorColorDragPanel.EndEdit -= OnRequestCommit;
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