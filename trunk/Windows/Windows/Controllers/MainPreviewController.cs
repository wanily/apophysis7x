using System;
using System.Drawing;
using System.Globalization;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Math;
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
		private bool mFitImage;

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
			View.ShowGuidelines = ApophysisSettings.MainPreviewShowGuidelines;
			View.ShowTransparency = ApophysisSettings.MainPreviewShowTransparency;
			View.CameraEditMode = ApophysisSettings.CameraEditMode;
			View.CameraEditUseScale = ApophysisSettings.CameraEditUseScale;

			View.PreviewDensityComboBox.SelectedIndexChanged += OnDensityChanged;
			View.PreviewDensityComboBox.LostFocus += OnDensityChanged;
			View.PreviewPicture.SizeChanged += OnPreviewSizeChanged;

			mParent.EditorController.FlameChanged += OnChangeCommitted;
			mParent.FlamePropertiesController.FlameChanged += OnChangeCommitted;

			mRenderer.Progress += OnRendererProgress;
			mRenderer.Exit += OnRendererExit;

			using (mParent.Initializer.Enter())
			{
				PreviewDensity = ApophysisSettings.MainPreviewDensity;
				FitImage = ApophysisSettings.MainPreviewFitImage;
			}
		}
		protected override void DetachView()
		{
			View.PreviewDensityComboBox.SelectedIndexChanged -= OnDensityChanged;
			View.PreviewDensityComboBox.LostFocus -= OnDensityChanged;
			View.PreviewPicture.SizeChanged -= OnPreviewSizeChanged;

			mParent.EditorController.FlameChanged -= OnChangeCommitted;
			mParent.FlamePropertiesController.FlameChanged -= OnChangeCommitted;

			View.PreviewPicture.Image = null;

			mRenderer.Progress -= OnRendererProgress;
			mRenderer.Exit -= OnRendererExit;

			ApophysisSettings.MainPreviewDensity = PreviewDensity;
			ApophysisSettings.MainPreviewShowGuidelines = View.ShowGuidelines;
			ApophysisSettings.MainPreviewShowTransparency = View.ShowTransparency;
			ApophysisSettings.MainPreviewFitImage = FitImage;
			ApophysisSettings.CameraEditMode = View.CameraEditMode;
			ApophysisSettings.CameraEditUseScale = View.CameraEditUseScale;
		}

		private void SetProgress(double progress)
		{
			View.Invoke(new Action(() => View.PreviewProgressBar.Value = (int)(progress * 100)));
		}
		private void SetElapsed(TimeSpan elapsed)
		{
			View.Invoke(new Action(() => View.PreviewTimeElapsedLabel.Text = string.Format("Elapsed: {0}", GetTimespanString(elapsed))));
		}
		private void SetRemaining(TimeSpan? remaining)
		{
			View.Invoke(new Action(() => View.PreviewTimeRemainingLabel.Text = string.Format("Remaining: {0}", remaining == null ? "calculating..." : GetTimespanString(remaining.Value))));
		}
		private string GetTimespanString(TimeSpan time)
		{
			return string.Format("{0}:{1}:{2}",
				time.Hours.ToString("#0", InputController.Culture).PadLeft(2, '0'),
				time.Minutes.ToString("#0", InputController.Culture).PadLeft(2, '0'),
				time.Seconds.ToString("#0", InputController.Culture).PadLeft(2, '0'));
		}

		private void OnPreviewSizeChangedCallback()
		{
			UpdatePreview();
		}
		private void OnPreviewSizeChanged(object sender, EventArgs e)
		{
			if (mParent.Initializer.IsBusy)
				return;

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
				//var oldImage = View.PreviewImage;

				View.PreviewedFlame = mParent.BatchListController.GetSelectedFlame();
				View.PreviewImage = bitmap;

				/*if (oldImage != null)
				{
					oldImage.Dispose();
				}*/
			}));
		}
		private void OnChangeCommitted(object sender, EventArgs e)
		{
			UpdatePreview();
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
		public bool FitImage
		{
			get { return mFitImage; }
			set
			{
				if (Equals(mFitImage, value))
					return;

				mFitImage = value;
				UpdatePreview();
			}
		}

		public void UpdatePreview()
		{
			var flame = mParent.BatchListController.GetSelectedFlame();
			if (mParent.Initializer.IsBusy || flame == null)
				return;

			var density = (double)PreviewDensity;
			var canvasSize = View.PreviewPicture.ClientSize;
			var renderSize = FitImage
				? canvasSize 
				: flame.CanvasSize.FitToFrame(canvasSize);

			/*Color backgroundWait, foregroundWait;
			if (View.ShowTransparency)
			{
				backgroundWait = Color.Transparent;
				foregroundWait = Color.Black;
			}
			else
			{
				backgroundWait = Color.Transparent;
				foregroundWait = Color.White;
			}*/

			View.PreviewPicture.BackColor = flame.Background;
			View.PreviewedFlame = null;

			//View.PreviewImage = WaitImageController.DrawWaitImage(renderSize, backgroundWait, foregroundWait);

			mRenderer.Cancel();

			mElapsedTimer.SetStartingTime();
			mRenderer.StartCreateBitmap(flame, density, renderSize, OnRendererFinished);
		}
	}
}