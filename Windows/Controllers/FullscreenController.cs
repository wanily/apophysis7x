using System;
using System.Drawing;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Interfaces.Calculation;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class FullscreenController : Controller<Fullscreen>, IFullscreenController
	{
		private Resolver<IWaitImageController> mWaitImageController;

		private readonly Lock mHiding;

		private Renderer mRenderer;
		private IterationManagerBase mIterationManager;
		private NativeTimer mElapsedTimer;

		private MainController mParent;
		private Bitmap mBitmap;

		private float mLastBitmapProgress;
		private float mNextBitmapProgress;

		public FullscreenController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;

			mIterationManager = new ProgressiveIterationManager();
			mElapsedTimer = new NativeTimer();
			mHiding = new Lock();
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
			mIterationManager.Progress += OnRendererProgress;
			mIterationManager.Finished += OnRendererFinished;

			var progressive = mIterationManager as IProgressive;
			if (progressive != null)
			{
				progressive.BitmapReady += OnBitmapReady;
			}

			View.Hidden += OnHidden;

			UpdateThreadCount();
		}
		protected override void DetachView()
		{
			mIterationManager.Progress -= OnRendererProgress;
			mIterationManager.Finished -= OnRendererFinished;

			var progressive = mIterationManager as IProgressive;
			if (progressive != null)
			{
				progressive.BitmapReady -= OnBitmapReady;
			}

			View.Hidden -= OnHidden;
		}

		private void SetBitmap(Bitmap bitmap)
		{
			if (bitmap == null || IsViewDisposed)
				return;

			if (mBitmap != null)
			{
				mBitmap.Dispose();
			}

			mBitmap = bitmap;

			View.Invoke(new Action(() =>
			{
				var oldImage = View.BackgroundImage;

				View.BackgroundImage = bitmap;
				View.Refresh();

				if (oldImage != null)
				{
					oldImage.Dispose();
				}
			}));
		}
		private void SetIsInProgress(bool isInProgress)
		{
			if (IsViewDisposed)
				return;

			View.Invoke(new Action(() => View.IsInProgress = isInProgress));
		}
		private void SetProgress(float progress)
		{
			if (IsViewDisposed)
				return;

			View.Invoke(new Action(() => View.Progress = (int)(progress * 100)));
		}
		private void SetElapsed(TimeSpan elapsed)
		{
			if (IsViewDisposed)
				return;

			View.Invoke(new Action(() => View.TimeElapsed = string.Format("Elapsed: {0}", GetTimespanString(elapsed))));
		}
		private void SetRemaining(TimeSpan? remaining)
		{
			if (IsViewDisposed)
				return;

			var banner = mIterationManager is IProgressive ? "Remaining until next: {0}" : "Remaining: {0}";

			View.Invoke(new Action(() => View.TimeRemaining = string.Format(banner, remaining == null ? "calculating..." : GetTimespanString(remaining.Value))));
		}

		private string GetTimespanString(TimeSpan time)
		{
			return string.Format("{0}:{1}:{2}",
				time.Hours.ToString("#0", InputController.Culture).PadLeft(2, '0'),
				time.Minutes.ToString("#0", InputController.Culture).PadLeft(2, '0'),
				time.Seconds.ToString("#0", InputController.Culture).PadLeft(2, '0'));
		}

		private void OnHidden(object sender, EventArgs e)
		{
			using (mHiding.Enter())
			{
				mIterationManager.Cancel();
			}
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
			var progress = sender as ProgressProvider;
			if (progress == null)
				return;

			if (sender is IProgressive)
			{
				SetProgress((progress.IterationProgress - mLastBitmapProgress) / (mNextBitmapProgress - mLastBitmapProgress));
				SetRemaining(((IProgressive)sender).TimeUntilNextBitmap);
			}
			else
			{
				SetProgress(progress.IterationProgress);
				SetRemaining(progress.RemainingTime);
			}

			SetElapsed(TimeSpan.FromSeconds(mElapsedTimer.GetElapsedTimeInSeconds()));
			SetRemaining(progress.RemainingTime);
		}
		private void OnRendererFinished(object sender, FinishedEventArgs e)
		{
			if (!e.Cancelled)
			{
				var bitmap = mRenderer.Histogram.CreateBitmap();
				SetBitmap(bitmap);
			}

			mRenderer.Dispose();
			if (mHiding.IsBusy)
				return;

			SetProgress(0);
			SetElapsed(TimeSpan.FromSeconds(mElapsedTimer.GetElapsedTimeInSeconds()));
			SetRemaining(TimeSpan.FromSeconds(0));
			SetIsInProgress(false);
		}

		private void UpdatePreview()
		{
			var flame = mParent.BatchListController.SelectedFlame;
			var density = (float)mParent.MainPreviewController.PreviewDensity;
			var canvasSize = View.ClientSize;
			var renderSize = flame.CanvasSize.FitToFrame(canvasSize);

			View.BackgroundImage = mWaitImageController.Object.DrawWaitImage(renderSize, Color.Black, Color.White);

			mIterationManager.Cancel();
			mElapsedTimer.SetStartingTime();

			mRenderer = new Renderer(flame, renderSize, ApophysisSettings.Preview.Oversample, ApophysisSettings.Preview.FilterRadius);
			mRenderer.Initialize();

			UpdateThreadCount();
			mLastBitmapProgress = 0;

			var progressive = mIterationManager as IProgressive;
			if (progressive != null)
			{
				progressive.StartIterate(mRenderer.Histogram);
			}
			else
			{
				mIterationManager.StartIterate(mRenderer.Histogram, density);
			}

			SetIsInProgress(true);
		}

		public void EnterFullscreen()
		{
			View.Show();
			UpdatePreview();	
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
			UpdatePreview();
		}
	}
}
