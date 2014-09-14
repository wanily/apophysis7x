using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class FlamePropertiesPreviewController : Controller<FlameProperties>
	{
		private FlamePropertiesController mParent;
		private ThumbnailRenderer mRenderer;
		private TimeLock mUpdateTimeLock;

		private Bitmap mBitmap;
		private DensityLevel mPreviewDensityLevel;

		public FlamePropertiesPreviewController([NotNull] FlameProperties view, [NotNull] FlamePropertiesController parent) : base(view)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;
			mRenderer = new ThumbnailRenderer();
			mUpdateTimeLock = new TimeLock(UpdatePreview);
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mUpdateTimeLock != null)
				{
					mUpdateTimeLock.Dispose();
					mUpdateTimeLock = null;
				}

				if (mBitmap != null)
				{
					mBitmap.Dispose();
					mBitmap = null;
				}
			}

			mParent = null;
			mRenderer = null;
		}

		protected override void AttachView()
		{
			View.LowQualityMenuItem.Click += OnQualitySelect;
			View.MediumQualityMenuItem.Click += OnQualitySelect;
			View.HighQualityMenuItem.Click += OnQualitySelect;

			View.Activated += OnActivated;

			View.DepthBlurDragPanel.ValueChanged += OnRequestCommit;
			View.PitchDragPanel.ValueChanged += OnRequestCommit;
			View.YawDragPanel.ValueChanged += OnRequestCommit;
			View.HeightDragPanel.ValueChanged += OnRequestCommit;
			View.PerspectiveDragPanel.ValueChanged += OnRequestCommit;
			View.ScaleDragPanel.ValueChanged += OnRequestCommit;
			View.DepthBlurTextBox.LostFocus += OnRequestCommit;
			View.PitchTextBox.LostFocus += OnRequestCommit;
			View.YawTextBox.LostFocus += OnRequestCommit;
			View.HeightTextBox.LostFocus += OnRequestCommit;
			View.PerspectiveTextBox.LostFocus += OnRequestCommit;
			View.ScaleTextBox.LostFocus += OnRequestCommit;
			View.ZoomDragPanel.ValueChanged += OnRequestCommit;
			View.ZoomTextBox.LostFocus += OnRequestCommit;
			View.ZoomScrollBar.ValueChanged += OnRequestCommit;
			View.XPositionDragPanel.ValueChanged += OnRequestCommit;
			View.XPositionTextBox.LostFocus += OnRequestCommit;
			View.XPositionScrollBar.ValueChanged += OnRequestCommit;
			View.YPositionDragPanel.ValueChanged += OnRequestCommit;
			View.YPositionTextBox.LostFocus += OnRequestCommit;
			View.YPositionScrollBar.ValueChanged += OnRequestCommit;
			View.RotationDragPanel.ValueChanged += OnRequestCommit;
			View.RotationTextBox.LostFocus += OnRequestCommit;
			View.RotationScrollBar.ValueChanged += OnRequestCommit;
			View.GammaDragPanel.ValueChanged += OnRequestCommit;
			View.GammaTextBox.LostFocus += OnRequestCommit;
			View.GammaScrollBar.ValueChanged += OnRequestCommit;
			View.BrightnessDragPanel.ValueChanged += OnRequestCommit;
			View.BrightnessTextBox.LostFocus += OnRequestCommit;
			View.BrightnessScrollBar.ValueChanged += OnRequestCommit;
			View.VibrancyDragPanel.ValueChanged += OnRequestCommit;
			View.VibrancyTextBox.LostFocus += OnRequestCommit;
			View.VibrancyScrollBar.ValueChanged += OnRequestCommit;
			View.GammaThresholdDragPanel.ValueChanged += OnRequestCommit;
			View.GammaThresholdTextBox.LostFocus += OnRequestCommit;

			using (mParent.Initializer.Enter())
			{
				PreviewDensityLevel = ApophysisSettings.FlamePropertiesPreviewDensityLevel;

				switch (PreviewDensityLevel)
				{
					case DensityLevel.Low:
						View.LowQualityMenuItem.Checked = true;
						break;
					case DensityLevel.Medium:
						View.MediumQualityMenuItem.Checked = true;
						break;
					case DensityLevel.High:
						View.HighQualityMenuItem.Checked = true;
						break;
				}
			}
		}
		protected override void DetachView()
		{
			View.LowQualityMenuItem.Click -= OnQualitySelect;
			View.MediumQualityMenuItem.Click -= OnQualitySelect;
			View.HighQualityMenuItem.Click -= OnQualitySelect;

			View.Activated -= OnActivated;

			View.DepthBlurDragPanel.ValueChanged -= OnRequestCommit;
			View.PitchDragPanel.ValueChanged -= OnRequestCommit;
			View.YawDragPanel.ValueChanged -= OnRequestCommit;
			View.HeightDragPanel.ValueChanged -= OnRequestCommit;
			View.PerspectiveDragPanel.ValueChanged -= OnRequestCommit;
			View.ScaleDragPanel.ValueChanged -= OnRequestCommit;
			View.DepthBlurTextBox.LostFocus -= OnRequestCommit;
			View.PitchTextBox.LostFocus -= OnRequestCommit;
			View.YawTextBox.LostFocus -= OnRequestCommit;
			View.HeightTextBox.LostFocus -= OnRequestCommit;
			View.PerspectiveTextBox.LostFocus -= OnRequestCommit;
			View.ScaleTextBox.LostFocus -= OnRequestCommit;
			View.ZoomDragPanel.ValueChanged -= OnRequestCommit;
			View.ZoomTextBox.LostFocus -= OnRequestCommit;
			View.ZoomScrollBar.ValueChanged -= OnRequestCommit;
			View.XPositionDragPanel.ValueChanged -= OnRequestCommit;
			View.XPositionTextBox.LostFocus -= OnRequestCommit;
			View.XPositionScrollBar.ValueChanged -= OnRequestCommit;
			View.YPositionDragPanel.ValueChanged -= OnRequestCommit;
			View.YPositionTextBox.LostFocus -= OnRequestCommit;
			View.YPositionScrollBar.ValueChanged -= OnRequestCommit;
			View.RotationDragPanel.ValueChanged -= OnRequestCommit;
			View.RotationTextBox.LostFocus -= OnRequestCommit;
			View.RotationScrollBar.ValueChanged -= OnRequestCommit;
			View.GammaDragPanel.ValueChanged -= OnRequestCommit;
			View.GammaTextBox.LostFocus -= OnRequestCommit;
			View.GammaScrollBar.ValueChanged -= OnRequestCommit;
			View.BrightnessDragPanel.ValueChanged -= OnRequestCommit;
			View.BrightnessTextBox.LostFocus -= OnRequestCommit;
			View.BrightnessScrollBar.ValueChanged -= OnRequestCommit;
			View.VibrancyDragPanel.ValueChanged -= OnRequestCommit;
			View.VibrancyTextBox.LostFocus -= OnRequestCommit;
			View.VibrancyScrollBar.ValueChanged -= OnRequestCommit;
			View.GammaThresholdDragPanel.ValueChanged -= OnRequestCommit;
			View.GammaThresholdTextBox.LostFocus -= OnRequestCommit;

			View.PreviewPicture.Image = null;
			ApophysisSettings.FlamePropertiesPreviewDensityLevel = PreviewDensityLevel;
		}

		public DensityLevel PreviewDensityLevel
		{
			get { return mPreviewDensityLevel; }
			set
			{
				mPreviewDensityLevel = value;

				var densities = new[]
				{
					ApophysisSettings.PreviewLowQualityDensity,
					ApophysisSettings.PreviewMediumQualityDensity,
					ApophysisSettings.PreviewHighQualityDensity
				};

				mUpdateTimeLock.Delay = (int)(ApophysisSettings.MiniPreviewUpdateResolution * densities[(int)value]);
				UpdatePreview();
			}
		}
		public void UpdatePreview()
		{
			if (mParent.Flame == null || mParent.Initializer.IsBusy)
				return;

			if (mBitmap != null)
			{
				mBitmap.Dispose();
			}

			var densities = new[]
			{
				ApophysisSettings.PreviewLowQualityDensity,
				ApophysisSettings.PreviewMediumQualityDensity,
				ApophysisSettings.PreviewHighQualityDensity
			};

			var density = densities[(int)mPreviewDensityLevel];
			var renderSize = mParent.Flame.CanvasSize.FitToFrame(View.PreviewPicture.ClientSize);

			mBitmap = mRenderer.CreateBitmap(mParent.Flame, density, renderSize);

			View.PreviewPicture.BackgroundImage = mBitmap;
			View.PreviewPicture.Refresh();
		}

		private void OnActivated(object sender, EventArgs e)
		{
			UpdatePreview();
		}
		private void OnRequestCommit(object sender, EventArgs e)
		{
			mUpdateTimeLock.Enter();
		}
		private void OnQualitySelect(object sender, EventArgs e)
		{
			var menuItem = sender as ToolStripMenuItem;
			if (menuItem == null)
				return;

			View.LowQualityMenuItem.Checked = false;
			View.MediumQualityMenuItem.Checked = false;
			View.HighQualityMenuItem.Checked = false;

			menuItem.Checked = true;

			if (ReferenceEquals(menuItem, View.LowQualityMenuItem))
				PreviewDensityLevel = DensityLevel.Low;

			if (ReferenceEquals(menuItem, View.MediumQualityMenuItem))
				PreviewDensityLevel = DensityLevel.Medium;

			if (ReferenceEquals(menuItem, View.HighQualityMenuItem))
				PreviewDensityLevel = DensityLevel.High;
		}
	}
}