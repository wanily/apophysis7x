using System;
using System.Drawing;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class FullscreenController : WindowController<Fullscreen>
	{
		private NativeTimer mElapsedTimer;
		private ThreadedRenderer mRenderer;
		private MainController mParent;
		private Bitmap mBitmap;
		private Lock mHiding;

		public FullscreenController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;

			mRenderer = new ThreadedRenderer();
			mElapsedTimer = new NativeTimer();
			mHiding = new Lock();
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
			mRenderer.Progress += OnRendererProgress;
			mRenderer.Exit += OnRendererExit;

			View.Hidden += OnHidden;
		}
		protected override void DetachView()
		{
			mRenderer.Progress -= OnRendererProgress;
			mRenderer.Exit -= OnRendererExit;

			View.Hidden -= OnHidden;
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
				mRenderer.Cancel();
			}
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
		private void OnRendererProgress(object sender, ProgressEventArgs args)
		{
			SetProgress(args.Progress);
			SetElapsed(TimeSpan.FromSeconds(mElapsedTimer.GetElapsedTimeInSeconds()));
			SetRemaining(args.TimeRemaining);
		}
		private void OnRendererExit(object sender, EventArgs e)
		{
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

			View.BackgroundImage = WaitImageController.DrawWaitImage(renderSize, Color.Black);

			mRenderer.Cancel();

			mElapsedTimer.SetStartingTime();
			mRenderer.StartCreateBitmap(flame, density, renderSize, OnRendererFinished);
			SetIsInProgress(true);
		}
	}
}
