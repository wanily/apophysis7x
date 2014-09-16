using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class FlamePropertiesController : WindowController<FlameProperties>
	{
		private Lock mInitialize = new Lock();
		private MainController mParent;
		private Flame mFlame;

		private FlamePropertiesUndoController mUndoController;
		private FlamePropertiesPreviewController mPreviewController;
		private FlamePropertiesToolbarController mToolbarController;

		public FlamePropertiesController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			mParent = parent;

			mUndoController = new FlamePropertiesUndoController(View, this);
			mPreviewController = new FlamePropertiesPreviewController(View, this);
			mToolbarController = new FlamePropertiesToolbarController(View, this);
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mToolbarController != null)
				{
					mToolbarController.Dispose();
					mToolbarController = null;
				}

				if (mPreviewController != null)
				{
					mPreviewController.Dispose();
					mPreviewController = null;
				}

				if (mUndoController != null)
				{
					mUndoController.Dispose();
					mUndoController = null;
				}
			}

			mFlame = null;
			mParent = null;
		}

		protected override void AttachView()
		{
			View.DepthBlurDragPanel.ValueChanged += OnDepthBlurChanged;
			View.PitchDragPanel.ValueChanged += OnPitchChanged;
			View.YawDragPanel.ValueChanged += OnYawChanged;
			View.HeightDragPanel.ValueChanged += OnHeightChanged;
			View.PerspectiveDragPanel.ValueChanged += OnPerspectiveChanged;
			View.ScaleDragPanel.ValueChanged += OnScaleChanged;
			View.ZoomDragPanel.ValueChanged += OnZoomChanged;
			View.ZoomScrollBar.ValueChanged += OnZoomChanged;
			View.XPositionDragPanel.ValueChanged += OnXPositionChanged;
			View.XPositionScrollBar.ValueChanged += OnXPositionChanged;
			View.YPositionDragPanel.ValueChanged += OnYPositionChanged;
			View.YPositionScrollBar.ValueChanged += OnYPositionChanged;
			View.RotationDragPanel.ValueChanged += OnRotationChanged;
			View.RotationScrollBar.ValueChanged += OnRotationChanged;
			View.GammaDragPanel.ValueChanged += OnGammaChanged;
			View.GammaScrollBar.ValueChanged += OnGammaChanged;
			View.BrightnessDragPanel.ValueChanged += OnBrightnessChanged;
			View.BrightnessScrollBar.ValueChanged += OnBrightnessChanged;
			View.VibrancyDragPanel.ValueChanged += OnVibrancyChanged;
			View.VibrancyScrollBar.ValueChanged += OnVibrancyChanged;
			View.GammaThresholdDragPanel.ValueChanged += OnGammaThresholdChanged;
			View.BackgroundPictureBox.Click += OnRequestBackgroundChange;

			mPreviewController.Initialize();
			mToolbarController.Initialize();
			mUndoController.Initialize();
		}
		protected override void DetachView()
		{
			View.DepthBlurDragPanel.ValueChanged -= OnDepthBlurChanged;
			View.PitchDragPanel.ValueChanged -= OnPitchChanged;
			View.YawDragPanel.ValueChanged -= OnYawChanged;
			View.HeightDragPanel.ValueChanged -= OnHeightChanged;
			View.PerspectiveDragPanel.ValueChanged -= OnPerspectiveChanged;
			View.ScaleDragPanel.ValueChanged -= OnScaleChanged;
			View.ZoomDragPanel.ValueChanged -= OnZoomChanged;
			View.ZoomScrollBar.ValueChanged -= OnZoomChanged;
			View.XPositionDragPanel.ValueChanged -= OnXPositionChanged;
			View.XPositionScrollBar.ValueChanged -= OnXPositionChanged;
			View.YPositionDragPanel.ValueChanged -= OnYPositionChanged;
			View.YPositionScrollBar.ValueChanged -= OnYPositionChanged;
			View.RotationDragPanel.ValueChanged -= OnRotationChanged;
			View.RotationScrollBar.ValueChanged -= OnRotationChanged;
			View.GammaDragPanel.ValueChanged -= OnGammaChanged;
			View.GammaScrollBar.ValueChanged -= OnGammaChanged;
			View.BrightnessDragPanel.ValueChanged -= OnBrightnessChanged;
			View.BrightnessScrollBar.ValueChanged -= OnBrightnessChanged;
			View.VibrancyDragPanel.ValueChanged -= OnVibrancyChanged;
			View.VibrancyScrollBar.ValueChanged -= OnVibrancyChanged;
			View.GammaThresholdDragPanel.ValueChanged -= OnGammaThresholdChanged;
			View.BackgroundPictureBox.Click -= OnRequestBackgroundChange;
		}

		private void OnRequestBackgroundChange(object sender, EventArgs e)
		{
			using (var dialog = new ColorDialog())
			{
				dialog.Color = mFlame.Background;

				var result = dialog.ShowDialog(View);
				if (result == DialogResult.Cancel)
					return;

				mFlame.Background = dialog.Color;
				View.BackgroundPictureBox.BackColor = mFlame.Background;
			}

			UndoController.CommitChange(mFlame);
			RaiseFlameChanged();
			mPreviewController.UpdatePreview();
		}
		private void OnGammaChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			if (ReferenceEquals(View.GammaScrollBar, sender))
			{
				using (mInitialize.Enter())
					View.GammaDragPanel.Value = View.GammaScrollBar.Value / 1000.0;
			}
			else if (ReferenceEquals(View.GammaDragPanel, sender))
			{
				using (mInitialize.Enter())
					View.GammaScrollBar.Value = (int)(View.GammaDragPanel.Value * 1000);
			}

			mFlame.Gamma = View.GammaDragPanel.Value;
		}
		private void OnBrightnessChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			if (ReferenceEquals(View.BrightnessScrollBar, sender))
			{
				using (mInitialize.Enter())
					View.BrightnessDragPanel.Value = View.BrightnessScrollBar.Value / 1000.0;
			}
			else if (ReferenceEquals(View.BrightnessDragPanel, sender))
			{
				using (mInitialize.Enter())
					View.BrightnessScrollBar.Value = (int)(View.BrightnessDragPanel.Value * 1000);
			}

			mFlame.Brightness = View.BrightnessDragPanel.Value;
		}
		private void OnVibrancyChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			if (ReferenceEquals(View.VibrancyScrollBar, sender))
			{
				using (mInitialize.Enter())
					View.VibrancyDragPanel.Value = View.VibrancyScrollBar.Value / 1000.0;
			}
			else if (ReferenceEquals(View.VibrancyDragPanel, sender))
			{
				using (mInitialize.Enter())
					View.VibrancyScrollBar.Value = (int)(View.VibrancyDragPanel.Value * 1000);
			}

			mFlame.Vibrancy = View.VibrancyDragPanel.Value;
		}
		private void OnGammaThresholdChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.GammaThreshold = View.GammaThresholdDragPanel.Value;
		}
		private void OnXPositionChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			if (ReferenceEquals(View.XPositionScrollBar, sender))
			{
				using (mInitialize.Enter())
					View.XPositionDragPanel.Value = View.XPositionScrollBar.Value / 1000.0;
			}
			else if (ReferenceEquals(View.XPositionDragPanel, sender))
			{
				using (mInitialize.Enter())
					View.XPositionScrollBar.Value = (int)(View.XPositionDragPanel.Value*1000);
			}

			mFlame.Origin.X = View.XPositionDragPanel.Value;
		}
		private void OnYPositionChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			if (ReferenceEquals(View.YPositionScrollBar, sender))
			{
				using (mInitialize.Enter())
					View.YPositionDragPanel.Value = View.YPositionScrollBar.Value / 1000.0;
			}
			else if (ReferenceEquals(View.YPositionDragPanel, sender))
			{
				using (mInitialize.Enter())
					View.YPositionScrollBar.Value = (int)(View.YPositionDragPanel.Value * 1000);
			}

			mFlame.Origin.Y = View.YPositionDragPanel.Value;
		}
		private void OnRotationChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			if (ReferenceEquals(View.RotationScrollBar, sender))
			{
				using (mInitialize.Enter())
					View.RotationDragPanel.Value = View.RotationScrollBar.Value;
			}
			else if (ReferenceEquals(View.RotationDragPanel, sender))
			{
				using (mInitialize.Enter())
					View.RotationScrollBar.Value = (int)(View.RotationDragPanel.Value);
			}

			mFlame.Angle = (-View.RotationDragPanel.Value * System.Math.PI / 180.0);
		}
		private void OnZoomChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			if (ReferenceEquals(View.ZoomScrollBar, sender))
			{
				using (mInitialize.Enter())
					View.ZoomDragPanel.Value = View.ZoomScrollBar.Value / 1000.0;
			}
			else if (ReferenceEquals(View.ZoomDragPanel, sender))
			{
				using (mInitialize.Enter())
					View.ZoomScrollBar.Value = (int)(View.ZoomDragPanel.Value * 1000);
			}

			mFlame.Zoom = View.ZoomDragPanel.Value;
		}
		private void OnDepthBlurChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.DepthOfField = View.DepthBlurDragPanel.Value;
		}
		private void OnPitchChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.Pitch = View.PitchDragPanel.Value * System.Math.PI / 180.0;
		}
		private void OnYawChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.Yaw = View.YawDragPanel.Value * System.Math.PI / 180.0;
		}
		private void OnHeightChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.Height = View.HeightDragPanel.Value;
		}
		private void OnPerspectiveChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.Perspective = View.PerspectiveDragPanel.Value;
		}
		private void OnScaleChanged(object sender, EventArgs e)
		{
			if (mFlame == null || mInitialize.IsBusy)
				return;

			mFlame.PixelsPerUnit = View.ScaleDragPanel.Value * mFlame.CanvasSize.Width / 100;
		}

		public void UpdateToolbar()
		{
			mToolbarController.UpdateButtonStates();
		}
		public void UpdateWindowTitle()
		{
			View.Text = string.Format("Adjustment - {0}", mFlame.CalculatedName);
		}
		public void UpdatePreview()
		{
			mPreviewController.UpdatePreview();
		}

		[NotNull]
		internal Lock Initializer
		{
			get { return mInitialize; }
		}

		internal void UpdatePreviewsGlobally()
		{
			mParent.UpdatePreviews(false);
		}

		public UndoController UndoController
		{
			get { return mParent.UndoController; }
		}
		public Flame Flame
		{
			get { return mFlame; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");

				mFlame = value;

				using (mInitialize.Enter())
				{
					AfterReset();
				}

				UpdateWindowTitle();
			}
		}

		private void AfterReset()
		{
			View.DepthBlurDragPanel.Value = mFlame.DepthOfField;
			View.PitchDragPanel.Value = mFlame.Pitch * 180.0 / System.Math.PI;
			View.YawDragPanel.Value = mFlame.Yaw * 180.0 / System.Math.PI;
			View.HeightDragPanel.Value = mFlame.Height;
			View.PerspectiveDragPanel.Value = mFlame.Perspective;
			View.GammaDragPanel.Value = mFlame.Gamma;
			View.GammaScrollBar.Value = (int)(mFlame.Gamma * 1000);
			View.BrightnessDragPanel.Value = mFlame.Brightness;
			View.BrightnessScrollBar.Value = (int)(mFlame.Brightness * 1000);
			View.VibrancyDragPanel.Value = mFlame.Vibrancy;
			View.VibrancyScrollBar.Value = (int)(mFlame.Vibrancy * 1000);
			View.GammaThresholdDragPanel.Value = mFlame.GammaThreshold;
			View.BackgroundPictureBox.BackColor = mFlame.Background;

			UpdateCamera();
			UpdateToolbar();
			UpdatePreview();
		}

		internal event EventHandler FlameChanged;
		internal void RaiseFlameChanged()
		{
			if (FlameChanged != null)
				FlameChanged(this, new EventArgs());
		}

		public void UpdateCamera()
		{
			var sz = System.Math.Max(-3, System.Math.Min(mFlame.Zoom, 3));
			var sx = System.Math.Max(-10, System.Math.Min(mFlame.Origin.X, 10));
			var sy = System.Math.Max(-10, System.Math.Min(mFlame.Origin.Y, 10));

			View.ScaleDragPanel.Value = mFlame.PixelsPerUnit * 100.0 / mFlame.CanvasSize.Width;
			View.ZoomDragPanel.Value = mFlame.Zoom;
			View.ZoomScrollBar.Value = (int)(sz * 1000);
			View.XPositionDragPanel.Value = mFlame.Origin.X;
			View.XPositionScrollBar.Value = (int)(sx * 1000);
			View.YPositionDragPanel.Value = mFlame.Origin.Y;
			View.YPositionScrollBar.Value = (int)(sy * 1000);
			View.RotationDragPanel.Value = -mFlame.Angle * 180 / System.Math.PI;
			View.RotationScrollBar.Value = (int)(View.RotationDragPanel.Value);
		}
	}
}