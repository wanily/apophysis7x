using System;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class FlamePropertiesCameraController : Controller<FlameProperties>
	{
		private FlamePropertiesController mParent;

		public FlamePropertiesCameraController(FlameProperties view, [NotNull] FlamePropertiesController parent) : base(view)
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
			View.ZoomDragPanel.ValueChanged += OnZoomChanged;
			View.ZoomScrollBar.ValueChanged += OnZoomChanged;
			View.XPositionDragPanel.ValueChanged += OnXPositionChanged;
			View.XPositionScrollBar.ValueChanged += OnXPositionChanged;
			View.YPositionDragPanel.ValueChanged += OnYPositionChanged;
			View.YPositionScrollBar.ValueChanged += OnYPositionChanged;
			View.RotationDragPanel.ValueChanged += OnRotationChanged;
			View.RotationScrollBar.ValueChanged += OnRotationChanged;
			View.ScaleDragPanel.ValueChanged += OnScaleChanged;
		}
		protected override void DetachView()
		{
			View.ZoomDragPanel.ValueChanged -= OnZoomChanged;
			View.ZoomScrollBar.ValueChanged -= OnZoomChanged;
			View.XPositionDragPanel.ValueChanged -= OnXPositionChanged;
			View.XPositionScrollBar.ValueChanged -= OnXPositionChanged;
			View.YPositionDragPanel.ValueChanged -= OnYPositionChanged;
			View.YPositionScrollBar.ValueChanged -= OnYPositionChanged;
			View.RotationDragPanel.ValueChanged -= OnRotationChanged;
			View.RotationScrollBar.ValueChanged -= OnRotationChanged;
			View.ScaleDragPanel.ValueChanged -= OnScaleChanged;
		}

		public void Update()
		{
			if (mParent.Flame == null)
				return;

			var sz = System.Math.Max(-3, System.Math.Min(mParent.Flame.Zoom, 3));
			var sx = System.Math.Max(-10, System.Math.Min(mParent.Flame.Origin.X, 10));
			var sy = System.Math.Max(-10, System.Math.Min(mParent.Flame.Origin.Y, 10));

			View.ScaleDragPanel.Value = mParent.Flame.PixelsPerUnit * 100.0 / mParent.Flame.CanvasSize.Width;
			View.ZoomDragPanel.Value = mParent.Flame.Zoom;
			View.ZoomScrollBar.Value = (int)(sz * 1000);
			View.XPositionDragPanel.Value = mParent.Flame.Origin.X;
			View.XPositionScrollBar.Value = (int)(sx * 1000);
			View.YPositionDragPanel.Value = mParent.Flame.Origin.Y;
			View.YPositionScrollBar.Value = (int)(sy * 1000);
			View.RotationDragPanel.Value = -mParent.Flame.Angle * 180 / System.Math.PI;
			View.RotationScrollBar.Value = (int)(View.RotationDragPanel.Value);
		}

		private void OnXPositionChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			if (ReferenceEquals(View.XPositionScrollBar, sender))
			{
				using (mParent.Initializer.Enter())
					View.XPositionDragPanel.Value = View.XPositionScrollBar.Value / 1000.0;
			}
			else if (ReferenceEquals(View.XPositionDragPanel, sender))
			{
				using (mParent.Initializer.Enter())
					View.XPositionScrollBar.Value = (int)(View.XPositionDragPanel.Value * 1000);
			}

			mParent.Flame.Origin.X = View.XPositionDragPanel.Value;
		}
		private void OnYPositionChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			if (ReferenceEquals(View.YPositionScrollBar, sender))
			{
				using (mParent.Initializer.Enter())
					View.YPositionDragPanel.Value = View.YPositionScrollBar.Value / 1000.0;
			}
			else if (ReferenceEquals(View.YPositionDragPanel, sender))
			{
				using (mParent.Initializer.Enter())
					View.YPositionScrollBar.Value = (int)(View.YPositionDragPanel.Value * 1000);
			}

			mParent.Flame.Origin.Y = View.YPositionDragPanel.Value;
		}
		private void OnRotationChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			if (ReferenceEquals(View.RotationScrollBar, sender))
			{
				using (mParent.Initializer.Enter())
					View.RotationDragPanel.Value = View.RotationScrollBar.Value;
			}
			else if (ReferenceEquals(View.RotationDragPanel, sender))
			{
				using (mParent.Initializer.Enter())
					View.RotationScrollBar.Value = (int)(View.RotationDragPanel.Value);
			}

			mParent.Flame.Angle = (-View.RotationDragPanel.Value * System.Math.PI / 180.0);
		}
		private void OnZoomChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			if (ReferenceEquals(View.ZoomScrollBar, sender))
			{
				using (mParent.Initializer.Enter())
					View.ZoomDragPanel.Value = View.ZoomScrollBar.Value / 1000.0;
			}
			else if (ReferenceEquals(View.ZoomDragPanel, sender))
			{
				using (mParent.Initializer.Enter())
					View.ZoomScrollBar.Value = (int)(View.ZoomDragPanel.Value * 1000);
			}

			mParent.Flame.Zoom = View.ZoomDragPanel.Value;
		}
		private void OnScaleChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			mParent.Flame.PixelsPerUnit = View.ScaleDragPanel.Value * mParent.Flame.CanvasSize.Width / 100;
		}
	}
}