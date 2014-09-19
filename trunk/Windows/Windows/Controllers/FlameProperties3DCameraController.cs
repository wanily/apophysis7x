using System;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class FlameProperties3DCameraController : Controller<FlameProperties>
	{
		private FlamePropertiesController mParent;

		public FlameProperties3DCameraController(FlameProperties view, [NotNull] FlamePropertiesController parent)
			: base(view)
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
			View.DepthBlurDragPanel.ValueChanged += OnDepthBlurChanged;
			View.PitchDragPanel.ValueChanged += OnPitchChanged;
			View.YawDragPanel.ValueChanged += OnYawChanged;
			View.HeightDragPanel.ValueChanged += OnHeightChanged;
			View.PerspectiveDragPanel.ValueChanged += OnPerspectiveChanged;
		}
		protected override void DetachView()
		{
			View.DepthBlurDragPanel.ValueChanged -= OnDepthBlurChanged;
			View.PitchDragPanel.ValueChanged -= OnPitchChanged;
			View.YawDragPanel.ValueChanged -= OnYawChanged;
			View.HeightDragPanel.ValueChanged -= OnHeightChanged;
			View.PerspectiveDragPanel.ValueChanged -= OnPerspectiveChanged;
		}

		public void Update()
		{
			if (mParent.Flame == null)
				return;

			View.DepthBlurDragPanel.Value = mParent.Flame.DepthOfField;
			View.PitchDragPanel.Value = mParent.Flame.Pitch * 180.0 / System.Math.PI;
			View.YawDragPanel.Value = mParent.Flame.Yaw * 180.0 / System.Math.PI;
			View.HeightDragPanel.Value = mParent.Flame.Height;
			View.PerspectiveDragPanel.Value = mParent.Flame.Perspective;
		}

		private void OnDepthBlurChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			mParent.Flame.DepthOfField = View.DepthBlurDragPanel.Value;
		}
		private void OnPitchChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			mParent.Flame.Pitch = View.PitchDragPanel.Value * System.Math.PI / 180.0;
		}
		private void OnYawChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			mParent.Flame.Yaw = View.YawDragPanel.Value * System.Math.PI / 180.0;
		}
		private void OnHeightChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			mParent.Flame.Height = View.HeightDragPanel.Value;
		}
		private void OnPerspectiveChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			mParent.Flame.Perspective = View.PerspectiveDragPanel.Value;
		}
	}
}