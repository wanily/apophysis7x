using System;
using System.Drawing;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Threading;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class FullscreenController : Controller<Fullscreen>
	{
		private readonly Lock mHiding;

		private Renderer mRenderer;
		private IterationManagerBase mIterationManager;
		private NativeTimer mElapsedTimer;

		private MainController mParent;
		private Bitmap mBitmap;

		public FullscreenController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;

			mIterationManager = new ThreadedIterationManager();
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

			View.Hidden += OnHidden;

			UpdateThreadCount();
		}
		protected override void DetachView()
		{
			mIterationManager.Progress -= OnRendererProgress;
			mIterationManager.Finished -= OnRendererFinished;

			View.Hidden -= OnHidden;
		}

		private void SetBitmap(Bitmap bitmap)
		{
			if (bitmap == null)
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
			View.Invoke(new Action(() => View.IsInProgress = isInProgress));
		}
		private void SetProgress(double progress)
		{
			View.Invoke(new Action(() => View.Progress = (int)(progress * 100)));
		}
		private void SetElapsed(TimeSpan elapsed)
		{
			View.Invoke(new Action(() => View.TimeElapsed = string.Format("Elapsed: {0}", GetTimespanString(elapsed))));
		}
		private void SetRemaining(TimeSpan? remaining)
		{
			View.Invoke(new Action(() => View.TimeRemaining = string.Format("Remaining: {0}", remaining == null ? "calculating..." : GetTimespanString(remaining.Value))));
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
		private void OnRendererProgress(object sender, ProgressEventArgs args)
		{
			var progress = sender as ProgressProvider;
			if (progress == null)
				return;

			SetProgress(progress.IterationProgress);
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

		public void EnterFullscreen()
		{
			var flame = mParent.BatchListController.GetSelectedFlame();
			if (flame == null)
				return;

			View.Show();

			var density = (double)mParent.MainPreviewController.PreviewDensity;
			var canvasSize = View.ClientSize;
			var renderSize = flame.CanvasSize.FitToFrame(canvasSize);

			View.BackgroundImage = WaitImageController.DrawWaitImage(renderSize, Color.Black, Color.White);

			mIterationManager.Cancel();
			mElapsedTimer.SetStartingTime();

			mRenderer = new Renderer(flame, renderSize, ApophysisSettings.Preview.Oversample, ApophysisSettings.Preview.FilterRadius);
			mRenderer.Initialize();

			UpdateThreadCount();
			mIterationManager.StartIterate(mRenderer.Histogram, density);
			SetIsInProgress(true);
		}
		public void UpdateThreadCount()
		{
			var threaded = mIterationManager as IThreaded;
			if (threaded != null)
			{
				threaded.SetThreadCount(ApophysisSettings.Preview.ThreadCount);
			}
		}
	}
}
