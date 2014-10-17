using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class FlamePropertiesImagingController : DataInputController<FlameProperties>
	{
		private FlamePropertiesController mParent;

		public FlamePropertiesImagingController(FlameProperties view, [NotNull] FlamePropertiesController parent)
			: base(view, parent.Initializer)
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
			View.BackgroundPictureBox.Click += OnRequestBackgroundChange;

			Register(View.GammaDragPanel,
				xx => mParent.Flame.Gamma = xx,
				() => mParent.Flame.Gamma);
			Register(View.BrightnessDragPanel,
				xx => mParent.Flame.Brightness = xx,
				() => mParent.Flame.Brightness);
			Register(View.VibrancyDragPanel,
				xx => mParent.Flame.Vibrancy = xx,
				() => mParent.Flame.Vibrancy);
			Register(View.GammaThresholdDragPanel,
				xx => mParent.Flame.GammaThreshold = xx,
				() => mParent.Flame.GammaThreshold);


			Register(View.GammaScrollBar,
				xx => mParent.Flame.Gamma = xx / 1000,
				() => mParent.Flame.Gamma * 1000);
			Register(View.BrightnessScrollBar,
				xx => mParent.Flame.Brightness = xx / 1000,
				() => mParent.Flame.Brightness * 1000);
			Register(View.VibrancyScrollBar,
				xx => mParent.Flame.Vibrancy = xx / 1000,
				() => mParent.Flame.Vibrancy * 1000);
		}
		protected override void DetachView()
		{
			View.BackgroundPictureBox.Click -= OnRequestBackgroundChange;

			Cleanup();
		}

		protected override void OnValueCommittedOverride(object control)
		{
			mParent.CommitValue();
		}
		protected override void OnValueChangedOverride(object control)
		{
			mParent.PreviewController.DelayedUpdatePreview();
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
	}
}