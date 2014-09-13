using System;
using System.Drawing;
using System.Globalization;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class MainPreviewController : Controller<Main>
	{
		private NativeTimer mElapsedTimer;
		private ThreadedRenderer mRenderer;
		private TimeLock mPreviewTimeLock;
		private MainController mParent;
		private Bitmap mBitmap;
		private int mPreviewDensity;

		public MainPreviewController([NotNull] Main view, [NotNull] MainController parent) : base(view)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;
			mPreviewTimeLock = new TimeLock(OnPreviewSizeChangedCallback);
			mPreviewTimeLock.Delay = 250;

			mRenderer = new ThreadedRenderer();
			mElapsedTimer = new NativeTimer();
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mRenderer != null)
				{
					mRenderer.Dispose();
					mRenderer = null;
				}

				if (mPreviewTimeLock != null)
				{
					mPreviewTimeLock.Dispose();
					mPreviewTimeLock = null;
				}

				if (mBitmap != null)
				{
					mBitmap.Dispose();
					mBitmap = null;
				}
			}

			mParent = null;
			mElapsedTimer = null;
		}

		protected override void AttachView()
		{
			View.PreviewDensityComboBox.SelectedIndexChanged += OnDensityChanged;
			View.PreviewDensityComboBox.LostFocus += OnDensityChanged;
			View.PreviewPicture.SizeChanged += OnPreviewSizeChanged;

			mRenderer.Progress += OnRendererProgress;
			mRenderer.Exit += OnRendererExit;

			using (mParent.Initializer.Enter())
			{
				PreviewDensity = ApophysisSettings.MainPreviewDensity;
			}
		}
		protected override void DetachView()
		{
			View.PreviewDensityComboBox.SelectedIndexChanged -= OnDensityChanged;
			View.PreviewDensityComboBox.LostFocus -= OnDensityChanged;
			View.PreviewPicture.SizeChanged -= OnPreviewSizeChanged;

			View.PreviewPicture.Image = null;

			mRenderer.Progress -= OnRendererProgress;
			mRenderer.Exit -= OnRendererExit;

			ApophysisSettings.MainPreviewDensity = PreviewDensity;
		}

		private void OnRendererProgress(object sender, ProgressEventArgs args)
		{
			SetProgress(args.Progress);
			SetElapsed(TimeSpan.FromSeconds(mElapsedTimer.GetElapsedTimeInSeconds()));
			SetRemaining(args.TimeRemaining);
		}
		private void OnRendererExit(object sender, EventArgs e)
		{
			SetProgress(0);
			SetElapsed(TimeSpan.FromSeconds(mElapsedTimer.GetElapsedTimeInSeconds()));
			SetRemaining(TimeSpan.FromSeconds(0));
		}

		private void SetProgress(double progress)
		{
			View.Invoke(new Action(() => View.PreviewProgressBar.Value = (int)(progress * 100)));
		}
		private void SetElapsed(TimeSpan elapsed)
		{
			View.Invoke(new Action(() => View.PreviewTimeElapsedLabel.Text = string.Format("Elapsed: {0}", elapsed)));
		}
		private void SetRemaining(TimeSpan? remaining)
		{
			View.Invoke(new Action(() => View.PreviewTimeRemainingLabel.Text = string.Format("Remaining: {0}", remaining == null ? "calculating..." : remaining.ToString())));
		}

		private void OnPreviewSizeChangedCallback()
		{
			UpdatePreview();
		}
		private void OnPreviewSizeChanged(object sender, EventArgs e)
		{
			mPreviewTimeLock.Enter();
		}
		private void OnDensityChanged(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;

			int value;
			if (!int.TryParse(View.PreviewDensityComboBox.Text, NumberStyles.Integer, InputController.Culture, out value))
				return;

			if (value <= 0)
				return;

			if (Equals(PreviewDensity, value))
				return;

			using (mParent.Initializer.Enter())
			{
				PreviewDensity = value;
			}

			UpdatePreview();
		}
		private void OnRendererFinished(Bitmap bitmap)
		{
			if (bitmap == null)
				return;

			if (mBitmap != null)
			{
				mBitmap.Dispose();
			}

			mBitmap = bitmap;

			View.PreviewPicture.Invoke(new Action(() =>
			{
				View.PreviewPicture.Image = bitmap;
				View.PreviewPicture.Refresh();
			}));
		}

		public int PreviewDensity
		{
			get { return mPreviewDensity; }
			set
			{
				if (Equals(mPreviewDensity, value))
					return;

				mPreviewDensity = value;
				View.PreviewDensityComboBox.Text = value.ToString(InputController.Culture);

				UpdatePreview();
			}
		}

		public void UpdatePreview()
		{
			var flame = mParent.BatchListController.GetSelectedFlame();
			if (mParent.Initializer.IsBusy || flame == null)
				return;

			var density = (double)PreviewDensity;
			var size = View.PreviewPicture.ClientSize;

			View.PreviewPicture.Image = null;

			mRenderer.Cancel();

			mElapsedTimer.SetStartingTime();
			mRenderer.StartCreateBitmap(flame, density, size, OnRendererFinished);
		}
	}
}