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
			View.ZoomDragPanel.EndEdit += OnRequestCommit;
			View.ZoomTextBox.LostFocus += OnRequestCommit;
			View.ZoomScrollBar.MouseUp += OnRequestCommit;
			View.XPositionDragPanel.EndEdit += OnRequestCommit;
			View.XPositionTextBox.LostFocus += OnRequestCommit;
			View.XPositionScrollBar.MouseUp += OnRequestCommit;
			View.YPositionDragPanel.EndEdit += OnRequestCommit;
			View.YPositionTextBox.LostFocus += OnRequestCommit;
			View.YPositionScrollBar.MouseUp += OnRequestCommit;
			View.RotationDragPanel.EndEdit += OnRequestCommit;
			View.RotationTextBox.LostFocus += OnRequestCommit;
			View.RotationScrollBar.MouseUp += OnRequestCommit;
			View.GammaDragPanel.EndEdit += OnRequestCommit;
			View.GammaTextBox.LostFocus += OnRequestCommit;
			View.GammaScrollBar.MouseUp += OnRequestCommit;
			View.BrightnessDragPanel.EndEdit += OnRequestCommit;
			View.BrightnessTextBox.LostFocus += OnRequestCommit;
			View.BrightnessScrollBar.MouseUp += OnRequestCommit;
			View.VibrancyDragPanel.EndEdit += OnRequestCommit;
			View.VibrancyTextBox.LostFocus += OnRequestCommit;
			View.VibrancyScrollBar.MouseUp += OnRequestCommit;
			View.GammaThresholdDragPanel.EndEdit += OnRequestCommit;
			View.GammaThresholdTextBox.LostFocus += OnRequestCommit;
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
			View.ZoomDragPanel.EndEdit -= OnRequestCommit;
			View.ZoomTextBox.LostFocus -= OnRequestCommit;
			View.ZoomScrollBar.MouseUp -= OnRequestCommit;
			View.XPositionDragPanel.EndEdit -= OnRequestCommit;
			View.XPositionTextBox.LostFocus -= OnRequestCommit;
			View.XPositionScrollBar.MouseUp -= OnRequestCommit;
			View.YPositionDragPanel.EndEdit -= OnRequestCommit;
			View.YPositionTextBox.LostFocus -= OnRequestCommit;
			View.YPositionScrollBar.MouseUp -= OnRequestCommit;
			View.RotationDragPanel.EndEdit -= OnRequestCommit;
			View.RotationTextBox.LostFocus -= OnRequestCommit;
			View.RotationScrollBar.MouseUp -= OnRequestCommit;
			View.GammaDragPanel.EndEdit -= OnRequestCommit;
			View.GammaTextBox.LostFocus -= OnRequestCommit;
			View.GammaScrollBar.MouseUp -= OnRequestCommit;
			View.BrightnessDragPanel.EndEdit -= OnRequestCommit;
			View.BrightnessTextBox.LostFocus -= OnRequestCommit;
			View.BrightnessScrollBar.MouseUp -= OnRequestCommit;
			View.VibrancyDragPanel.EndEdit -= OnRequestCommit;
			View.VibrancyTextBox.LostFocus -= OnRequestCommit;
			View.VibrancyScrollBar.MouseUp -= OnRequestCommit;
			View.GammaThresholdDragPanel.EndEdit -= OnRequestCommit;
			View.GammaThresholdTextBox.LostFocus -= OnRequestCommit;
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