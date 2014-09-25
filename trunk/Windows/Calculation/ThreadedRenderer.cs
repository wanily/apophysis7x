using System;
using System.Drawing;
using Xyrus.Apophysis.Strings;
using Xyrus.Apophysis.Threading;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class ThreadedRenderer : IDisposable
	{
		private Renderer mRenderer;
		private RenderParameters mParameters;
		private ThreadController mThreadController;

		private bool mIsDisposed;
		private double mTotalTime;

		~ThreadedRenderer()
		{
			Dispose(false);
		}
		public ThreadedRenderer()
		{
			mThreadController = new ThreadController();
			mRenderer = new Renderer();
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

		public void StartCreateBitmap([NotNull] RenderParameters parameters, Action<Bitmap> callback)
		{
			if (parameters == null) throw new ArgumentNullException(@"parameters");

			mParameters = parameters;
			mThreadController.StartThread(CreateBitmap, callback, SendCompletedMessage, SendCancelledMessage);
		}

		private void SendCancelledMessage()
		{
			if (mParameters != null)
			{
				mParameters.Messenger.SendMessage(string.Format(@"{0} : {1}", DateTime.Now.ToString(@"T"), Messages.RenderTerminatedMessage));
			}

			FinalizeRender();
		}

		private void SendCompletedMessage()
		{
			if (mParameters != null)
			{
				mParameters.Messenger.SendMessage(string.Format(@"  {0}", string.Format(Messages.RenderAverageSpeedMessage, mRenderer.AverageIterationsPerSecond)));
				mParameters.Messenger.SendMessage(string.Format(@"  {0}", string.Format(Messages.RenderPureTimeMessage, TimeSpan.FromSeconds(mRenderer.PureRenderingTime))));
				mParameters.Messenger.SendMessage(string.Format(@"  {0}", string.Format(Messages.RenderTotalTimeMessage, TimeSpan.FromSeconds(mTotalTime))));
			}

			FinalizeRender();
		}
		private void FinalizeRender()
		{
			mParameters = null;
			mTotalTime = 0;

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
			if (mParameters != null)
			{
				mParameters.Messenger.SendMessage(string.Format(@"{0} : {1}", DateTime.Now.ToString(@"T"), Messages.RenderTerminatingMessage));
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
			stopwatch.SetStartingTime();

			var result = mRenderer.CreateBitmap(mParameters, ProgressUpdate, threadState);

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
			//todo multithreading
		}
	}
}