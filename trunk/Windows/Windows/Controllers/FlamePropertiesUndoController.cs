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

			View.DepthBlurDragPanel.EndEdit += OnRequestCommit;
			View.PitchDragPanel.EndEdit += OnRequestCommit;
			View.YawDragPanel.EndEdit += OnRequestCommit;
			View.HeightDragPanel.EndEdit += OnRequestCommit;
			View.PerspectiveDragPanel.EndEdit += OnRequestCommit;
			View.ScaleDragPanel.EndEdit += OnRequestCommit;
			View.DepthBlurTextBox.LostFocus += OnRequestCommit;
			View.PitchTextBox.LostFocus += OnRequestCommit;
			View.YawTextBox.LostFocus += OnRequestCommit;
			View.HeightTextBox.LostFocus += OnRequestCommit;
			View.PerspectiveTextBox.LostFocus += OnRequestCommit;
			View.ScaleTextBox.LostFocus += OnRequestCommit;
		}
		protected override void DetachView()
		{
			mParent.UndoController.StackChanged -= OnUndoStackChanged;

			View.DepthBlurDragPanel.EndEdit -= OnRequestCommit;
			View.PitchDragPanel.EndEdit -= OnRequestCommit;
			View.YawDragPanel.EndEdit -= OnRequestCommit;
			View.HeightDragPanel.EndEdit -= OnRequestCommit;
			View.PerspectiveDragPanel.EndEdit -= OnRequestCommit;
			View.ScaleDragPanel.EndEdit -= OnRequestCommit;
			View.DepthBlurTextBox.LostFocus -= OnRequestCommit;
			View.PitchTextBox.LostFocus -= OnRequestCommit;
			View.YawTextBox.LostFocus -= OnRequestCommit;
			View.HeightTextBox.LostFocus -= OnRequestCommit;
			View.PerspectiveTextBox.LostFocus -= OnRequestCommit;
			View.ScaleTextBox.LostFocus -= OnRequestCommit;
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