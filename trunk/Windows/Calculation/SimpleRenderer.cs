using System;
using System.Drawing;
using Xyrus.Apophysis.Strings;
using Xyrus.Apophysis.Threading;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI, Obsolete]
	public class SimpleRenderer : IDisposable
	{
		private Renderer mRenderer;
		private ThreadController mThreadController;

		private bool mIsDisposed;
		private double mTotalTime;
		private int mThreadCount;
		private double mDensity;

		~SimpleRenderer()
		{
			Dispose(false);
		}
		public SimpleRenderer()
		{
			mThreadController = new ThreadController();
		}

		protected void Dispose(bool disposing)
		{
			if (mIsDisposed)
				return;

			if (disposing)
			{
				if (mThreadController != null)
				{
					mThreadController.Dispose();
					mThreadController = null;
				}
			}

			DisposeOverride(disposing);

			mRenderer = null;
			mRenderer = null;
			mIsDisposed = true;
		}
		protected virtual void DisposeOverride(bool disposing)
		{
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public void StartCreateBitmap(double density, [NotNull] Renderer renderer, Action<Bitmap> callback)
		{
			if (renderer == null) throw new ArgumentNullException(@"renderer");

			mDensity = density;
			mRenderer = renderer;
			mRenderer.Initialize();

			mThreadController.StartThread(CreateBitmap, callback, SendCompletedMessage, SendCancelledMessage);
		}

		private void SendCancelledMessage()
		{
			lock (mRenderer)
			{
				if (mRenderer != null)
				{
					mRenderer.Messenger.SendMessage(string.Format(@"{0} : {1}", DateTime.Now.ToString(@"T"), Messages.RenderTerminatedMessage));
				}
			}
			

			FinalizeRender();
		}
		private void SendCompletedMessage()
		{
			lock (mRenderer)
			{
				if (mRenderer != null)
				{
					mRenderer.Messenger.SendMessage(string.Format(@"  {0}",
						string.Format(Messages.RenderAverageSpeedMessage, mRenderer.AverageIterationsPerSecond)));
					mRenderer.Messenger.SendMessage(string.Format(@"  {0}",
						string.Format(Messages.RenderPureTimeMessage, TimeSpan.FromSeconds(mRenderer.PureRenderingTime))));
					mRenderer.Messenger.SendMessage(string.Format(@"  {0}",
						string.Format(Messages.RenderTotalTimeMessage, TimeSpan.FromSeconds(mTotalTime))));
				}
			}

			FinalizeRender();
		}

		private void FinalizeRender()
		{
			mTotalTime = 0;
			mDensity = 0;

			if (Exit != null)
				Exit(this, new EventArgs());
		}

		public void Suspend()
		{
			mThreadController.Suspend();
		}
		public void Resume()
		{
			mThreadController.Resume();
		}
		public void Cancel()
		{
			if (mRenderer != null)
			{
				mRenderer.Messenger.SendMessage(string.Format(@"{0} : {1}", DateTime.Now.ToString(@"T"), Messages.RenderTerminatingMessage));
			}
			mThreadController.Cancel();
		}

		public bool IsSuspended
		{
			get { return mThreadController.IsSuspended; }
		}
		public bool IsRunning
		{
			get { return mThreadController.IsRunning; }
		}
		public bool IsCancelling
		{
			get { return mThreadController.IsCancelling; }
		}

		public event ProgressEventHandler Progress;
		public event EventHandler Exit;

		public InvokeCallbackMode InvokeCallbackMode
		{
			get { return mThreadController.InvokeCallbackMode; }
			set { mThreadController.InvokeCallbackMode = value; }
		}

		private Bitmap CreateBitmap(ThreadStateToken threadState)
		{
			var stopwatch = new NativeTimer();
			var progress = new ProgressManager(threadState);

			progress.Progress += (o, e) => ProgressUpdate(e);

			stopwatch.SetStartingTime();

			mRenderer.Messenger.SendMessage(string.Format(@"{0} : {1}", DateTime.Now.ToString(@"T"), Messages.RenderInProgressMessage));
			mRenderer.Histogram.Iterate(mDensity, progress);

			if (threadState.IsCancelling)
				return null;

			mRenderer.Messenger.SendMessage(string.Format(@"{0} : {1}", DateTime.Now.ToString(@"T"), string.Format(Messages.RenderSamplingMessage, mRenderer.Histogram.CurrentDensity)));

			var result = mRenderer.Histogram.CreateBitmap();

			mTotalTime = stopwatch.GetElapsedTimeInSeconds();

			return result;
		}
		private void ProgressUpdate(ProgressEventArgs args)
		{
			if (Progress != null)
				Progress(this, args);
		}

		public void SetThreadCount(int? threadCount)
		{
			if (IsRunning)
			{
				throw new InvalidOperationException(Messages.AttemptedThreadCountChangeWhileRendererBusyErrorMessage);
			}

			var defaultCount = System.Math.Max(1, Environment.ProcessorCount - 1);
			mThreadCount = threadCount.GetValueOrDefault(defaultCount);
		}
	}
}