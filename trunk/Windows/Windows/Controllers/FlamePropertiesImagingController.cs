using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class FlamePropertiesImagingController : Controller<FlameProperties>
	{
		private FlamePropertiesController mParent;

		public FlamePropertiesImagingController(FlameProperties view, [NotNull] FlamePropertiesController parent)
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
			View.GammaDragPanel.ValueChanged += OnGammaChanged;
			View.GammaScrollBar.ValueChanged += OnGammaChanged;
			View.BrightnessDragPanel.ValueChanged += OnBrightnessChanged;
			View.BrightnessScrollBar.ValueChanged += OnBrightnessChanged;
			View.VibrancyDragPanel.ValueChanged += OnVibrancyChanged;
			View.VibrancyScrollBar.ValueChanged += OnVibrancyChanged;
			View.GammaThresholdDragPanel.ValueChanged += OnGammaThresholdChanged;
			View.BackgroundPictureBox.Click += OnRequestBackgroundChange;
		}
		protected override void DetachView()
		{
			View.GammaDragPanel.ValueChanged -= OnGammaChanged;
			View.GammaScrollBar.ValueChanged -= OnGammaChanged;
			View.BrightnessDragPanel.ValueChanged -= OnBrightnessChanged;
			View.BrightnessScrollBar.ValueChanged -= OnBrightnessChanged;
			View.VibrancyDragPanel.ValueChanged -= OnVibrancyChanged;
			View.VibrancyScrollBar.ValueChanged -= OnVibrancyChanged;
			View.GammaThresholdDragPanel.ValueChanged -= OnGammaThresholdChanged;
			View.BackgroundPictureBox.Click -= OnRequestBackgroundChange;
		}

		public void Update()
		{
			if (mParent.Flame == null)
				return;

			View.GammaDragPanel.Value = mParent.Flame.Gamma;
			View.GammaScrollBar.Value = (int)(mParent.Flame.Gamma * 1000);
			View.BrightnessDragPanel.Value = mParent.Flame.Brightness;
			View.BrightnessScrollBar.Value = (int)(mParent.Flame.Brightness * 1000);
			View.VibrancyDragPanel.Value = mParent.Flame.Vibrancy;
			View.VibrancyScrollBar.Value = (int)(mParent.Flame.Vibrancy * 1000);
			View.GammaThresholdDragPanel.Value = mParent.Flame.GammaThreshold;
			View.BackgroundPictureBox.BackColor = mParent.Flame.Background;
		}

		private void OnRequestBackgroundChange(object sender, EventArgs e)
		{
			using (var dialog = new ColorDialog())
			{
				dialog.Color = mParent.Flame.Background;

				var result = dialog.ShowDialog(View);
				if (result == DialogResult.Cancel)
					return;

				mParent.Flame.Background = dialog.Color;
				View.BackgroundPictureBox.BackColor = mParent.Flame.Background;
			}

			mParent.UndoController.CommitChange(mParent.Flame);
			mParent.RaiseFlameChanged();
			mParent.PreviewController.UpdatePreview();
		}
		private void OnGammaChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			if (ReferenceEquals(View.GammaScrollBar, sender))
			{
				using (mParent.Initializer.Enter())
					View.GammaDragPanel.Value = View.GammaScrollBar.Value / 1000.0;
			}
			else if (ReferenceEquals(View.GammaDragPanel, sender))
			{
				using (mParent.Initializer.Enter())
					View.GammaScrollBar.Value = (int)(View.GammaDragPanel.Value * 1000);
			}

			mParent.Flame.Gamma = View.GammaDragPanel.Value;
		}
		private void OnBrightnessChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			if (ReferenceEquals(View.BrightnessScrollBar, sender))
			{
				using (mParent.Initializer.Enter())
					View.BrightnessDragPanel.Value = View.BrightnessScrollBar.Value / 1000.0;
			}
			else if (ReferenceEquals(View.BrightnessDragPanel, sender))
			{
				using (mParent.Initializer.Enter())
					View.BrightnessScrollBar.Value = (int)(View.BrightnessDragPanel.Value * 1000);
			}

			mParent.Flame.Brightness = View.BrightnessDragPanel.Value;
		}
		private void OnVibrancyChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			if (ReferenceEquals(View.VibrancyScrollBar, sender))
			{
				using (mParent.Initializer.Enter())
					View.VibrancyDragPanel.Value = View.VibrancyScrollBar.Value / 1000.0;
			}
			else if (ReferenceEquals(View.VibrancyDragPanel, sender))
			{
				using (mParent.Initializer.Enter())
					View.VibrancyScrollBar.Value = (int)(View.VibrancyDragPanel.Value * 1000);
			}

			mParent.Flame.Vibrancy = View.VibrancyDragPanel.Value;
		}
		private void OnGammaThresholdChanged(object sender, EventArgs e)
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			mParent.Flame.GammaThreshold = View.GammaThresholdDragPanel.Value;
		}
	}
}