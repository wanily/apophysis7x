using System;
using System.Drawing;
using System.Globalization;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Input;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class MainPreviewController : Controller<Main>
	{
		private NativeTimer mElapsedTimer;
		private IterationManagerBase mIterationManager;
		private Renderer mRenderer;
		private TimeLock mPreviewTimeLock;
		private MainController mParent;
		private Bitmap mBitmap;
		private Flame mFlame;
		private int mPreviewDensity;
		private bool mFitImage;
		private double mLastBitmapProgress;
		private double mNextBitmapProgress;

		public MainPreviewController([NotNull] Main view, [NotNull] MainController parent) : base(view)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;
			mPreviewTimeLock = new TimeLock(OnPreviewSizeChangedCallback)
			{
				Delay = 250
			};

			mIterationManager = new ProgressiveIterationManager();
			mElapsedTimer = new NativeTimer();
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mIterationManager != null)
				{
					mIterationManager.Dispose();
					mIterationManager = null;
				}

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
			View.ShowGuidelines = ApophysisSettings.View.ShowGuidelines;
			View.ShowTransparency = ApophysisSettings.View.ShowTransparency;
			View.CameraEditMode = ApophysisSettings.Editor.CameraEditMode;

			ReloadSettings();

			View.PreviewDensityComboBox.SelectedIndexChanged += OnDensityChanged;
			View.PreviewDensityComboBox.LostFocus += OnDensityChanged;
			View.PreviewPicture.SizeChanged += OnPreviewSizeChanged;

			mParent.EditorController.FlameChanged += OnChangeCommitted;
			mParent.FlamePropertiesController.FlameChanged += OnChangeCommitted;
			View.CameraChanged += OnCameraChanged;

			mIterationManager.Progress += OnRendererProgress;
			mIterationManager.Finished += OnRendererFinished;

			var progressive = mIterationManager as IProgressive;
			if (progressive != null)
			{
				progressive.BitmapReady += OnBitmapReady;
			}

			using (mParent.Initializer.Enter())
			{
				PreviewDensity = (int)ApophysisSettings.Preview.MainPreviewDensity;
				FitImage = ApophysisSettings.View.FitMainPreviewImage;
			}

			UpdateThreadCount();
		}
		protected override void DetachView()
		{
			View.PreviewDensityComboBox.SelectedIndexChanged -= OnDensityChanged;
			View.PreviewDensityComboBox.LostFocus -= OnDensityChanged;
			View.PreviewPicture.SizeChanged -= OnPreviewSizeChanged;

			mParent.EditorController.FlameChanged -= OnChangeCommitted;
			mParent.FlamePropertiesController.FlameChanged -= OnChangeCommitted;
			View.CameraChanged += OnCameraChanged;

			View.PreviewPicture.Image = null;

			mIterationManager.Progress -= OnRendererProgress;
			mIterationManager.Finished -= OnRendererFinished;

			var progressive = mIterationManager as IProgressive;
			if (progressive != null)
			{
				progressive.BitmapReady -= OnBitmapReady;
			}

			ApophysisSettings.Preview.MainPreviewDensity = PreviewDensity;
			ApophysisSettings.View.ShowGuidelines = View.ShowGuidelines;
			ApophysisSettings.View.ShowTransparency = View.ShowTransparency;
			ApophysisSettings.View.FitMainPreviewImage = FitImage;
			ApophysisSettings.Editor.CameraEditMode = View.CameraEditMode;
		}

		private void SetSpeed(double? speed)
		{
			if (IsViewDisposed)
				return;

			View.Invoke(new Action(() => View.IterationsPerSecondLabel.Text = speed.HasValue ? string.Format("{0:###,###,###,##0.00} i/s", speed) : null));
		}
		private void SetProgress(double progress)
		{
			if (IsViewDisposed)
				return;

			View.Invoke(new Action(() =>
			{
				View.PreviewProgressBar.Value = (int)(progress * 100);
			}));
		}
		private void SetElapsed(TimeSpan elapsed)
		{
			if (IsViewDisposed)
				return;

			View.Invoke(new Action(() => View.PreviewTimeElapsedLabel.Text = string.Format("Elapsed: {0}", GetTimespanString(elapsed))));
		}
		private void SetRemaining(TimeSpan? remaining)
		{
			if (IsViewDisposed)
				return;

			var banner = mIterationManager is IProgressive ? "Remaining until next: {0}" : "Remaining: {0}";

			View.Invoke(new Action(() => View.PreviewTimeRemainingLabel.Text = string.Format(banner, remaining == null ? "calculating..." : GetTimespanString(remaining.Value))));
		}
		private void SetBitmap(Bitmap bitmap)
		{
			if (bitmap == null || IsViewDisposed)
				return;

			View.Invoke(new Action(() =>
			{
				var oldOutput = mBitmap;

				View.PreviewedFlame = mFlame;
				View.PreviewImage = mBitmap = bitmap;

				if (oldOutput != null)
				{
					oldOutput.Dispose();
				}
			}));
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
		
		private void OnChangeCommitted(object sender, EventArgs e)
		{
			UpdatePreview();
			mParent.BatchListController.UpdateSelectedPreview();
		}
		private void OnBitmapReady(object sender, BitmapReadyEventArgs args)
		{
			var bitmap = mRenderer.Histogram.CreateBitmap();

			mLastBitmapProgress = mIterationManager.IterationProgress;
			mNextBitmapProgress = args.NextIssue;
			SetBitmap(bitmap);
		}
		private void OnRendererProgress(object sender, ProgressEventArgs args)
		{
			try
			{
				var progress = sender as ProgressProvider;
				if (progress == null)
					return;

				if (sender is IProgressive)
				{
					SetProgress((progress.IterationProgress - mLastBitmapProgress)/(mNextBitmapProgress - mLastBitmapProgress));
					SetRemaining(((IProgressive)sender).TimeUntilNextBitmap);
				}
				else
				{
					SetProgress(progress.IterationProgress);
					SetRemaining(progress.RemainingTime);
				}

				SetSpeed(mIterationManager.IterationsPerSecond <= 0 ? (double?)null : mIterationManager.IterationsPerSecond);
				SetElapsed(TimeSpan.FromSeconds(mElapsedTimer.GetElapsedTimeInSeconds()));
			}
			catch (ObjectDisposedException) { }
		}
		private void OnRendererFinished(object sender, FinishedEventArgs e)
		{
			try
			{
				if (!e.Cancelled)
				{
					var bitmap = mRenderer.Histogram.CreateBitmap();
					SetBitmap(bitmap);
				}

				SetSpeed(null);
				SetProgress(0);
				SetElapsed(TimeSpan.FromSeconds(mElapsedTimer.GetElapsedTimeInSeconds()));
				SetRemaining(TimeSpan.FromSeconds(0));
			}
			catch (ObjectDisposedException) { }
		}
		private void OnCameraChanged(object sender, CameraChangedEventArgs args)
		{
			View.LoadingStatusLabel.Text = args.Operation.ToString();
			View.StatusBar.Refresh();
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
				View.FitFrame = value;
				UpdatePreview();
			}
		}

		public void UpdatePreview()
		{
			var flame = mParent.BatchListController.GetSelectedFlame();
			if (mParent.Initializer.IsBusy || flame == null)
				return;

			mFlame = flame;

			var density = (double)PreviewDensity;
			var canvasSize = View.PreviewPicture.ClientSize;
			var renderSize = FitImage
				? canvasSize
				: mFlame.CanvasSize.FitToFrame(canvasSize);

			View.PreviewPicture.BackColor = SystemColors.Control;
			View.PreviewedFlame = null;

			mIterationManager.Cancel();
			mElapsedTimer.SetStartingTime();
			mLastBitmapProgress = 0;

			if (mRenderer != null)
			{
				mRenderer.Dispose();
			}

			mRenderer = new Renderer(mFlame, renderSize, ApophysisSettings.Preview.Oversample, ApophysisSettings.Preview.FilterRadius);

			if (FitImage)
			{
				mRenderer.AdjustPixelsPerUnit(mFlame.CanvasSize.FitToFrame(canvasSize));
			}

			mRenderer.Initialize();

			UpdateThreadCount();

			var progressive = mIterationManager as IProgressive;
			if (progressive != null)
			{
				progressive.StartIterate(mRenderer.Histogram);
			}
			else
			{
				mIterationManager.StartIterate(mRenderer.Histogram, density);
			}
		}
		public void UpdateThreadCount()
		{
			var threaded = mIterationManager as IThreaded;
			if (threaded != null)
			{
				threaded.SetThreadCount(ApophysisSettings.Preview.ThreadCount);
			}
		}

		public void ReloadSettings()
		{
			View.CameraEditUseScale = ApophysisSettings.Editor.CameraEditUseScale;
			UpdatePreview();
		}
	}
}