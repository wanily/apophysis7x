using System;
using System.Drawing;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class FullscreenController : Controller<Fullscreen>
	{
		private readonly Lock mHiding;

		private Renderer mRenderer;
		private NativeTimer mElapsedTimer;
		private SimpleRenderer mThreader;
		private MainController mParent;
		private Bitmap mBitmap;

		public FullscreenController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;

			mThreader = new SimpleRenderer();
			mElapsedTimer = new NativeTimer();
			mHiding = new Lock();
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mThreader != null)
				{
					mThreader.Dispose();
					mThreader = null;
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
			mThreader.Progress += OnRendererProgress;
			mThreader.Exit += OnRendererExit;

			View.Hidden += OnHidden;
		}
		protected override void DetachView()
		{
			mThreader.Progress -= OnRendererProgress;
			mThreader.Exit -= OnRendererExit;

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
				mThreader.Cancel();
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

			mThreader.Cancel();

			mElapsedTimer.SetStartingTime();

			mRenderer = new Renderer(flame, renderSize, ApophysisSettings.Preview.Oversample, ApophysisSettings.Preview.FilterRadius);

			mThreader.SetThreadCount(ApophysisSettings.Preview.ThreadCount);
			mThreader.StartCreateBitmap(density, mRenderer, OnRendererFinished);
			SetIsInProgress(true);
		}
	}
}
