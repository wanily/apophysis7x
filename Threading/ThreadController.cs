using System;
using System.Threading;
using System.Windows.Forms;
using Xyrus.Apophysis.Interfaces.Threading;
using Xyrus.Apophysis.Windows;

namespace Xyrus.Apophysis.Threading
{
	[PublicAPI]
	public class ThreadController : Controller, IThreadController
	{
		private volatile bool mCancel;
		private volatile bool mSuspended;
		private volatile bool mRunning;

		private Thread mThread;
		private ThreadPriority mPriority = ThreadPriority.Normal;

		protected override void Dispose(bool disposing)
		{
			if (disposing && mThread != null)
			{
				Cancel();
			}
		}

		public ThreadPriority Priority
		{
			get { return mPriority; }
			set
			{
				if (mThread != null || mRunning)
				{
					throw new InvalidOperationException("Thread is busy");
				}

				mPriority = value;
			}
		}

		public void StartThread(Action threadAction, Action callback = null)
		{
			StartThread<object>(
				o =>
				{
					threadAction();
					return null;
				},
				o =>
				{
					if (callback != null)
						callback();
				});
		}
		public void StartThread(Action<IThreadStateToken> threadAction, Action callback = null)
		{
			StartThread<object>(
				o => 
					{ 
						threadAction(o);
						return null;
					},
				o =>
					{
						if (callback != null) 
							callback();
					});
		}
		public void StartThread<T>(Func<T> threadAction, Action<T> callback = null)
		{
			StartThread(o => threadAction(), callback);
		}
		public void StartThread<T>(Func<IThreadStateToken, T> threadAction, Action<T> callback = null, Action completionCallback = null, Action cancelledCallback = null)
		{
			if (mThread != null || mRunning)
			{
				throw new InvalidOperationException("Thread is busy");
			}

			mThread = new Thread(() =>
			{
				mRunning = true;
				mCancel = false;
				mSuspended = false;

				T result;
				using (var token = new ThreadStateToken(this))
				{
					result = threadAction(token);
				}

				if (!mCancel && InvokeCallbackMode == InvokeCallbackMode.BeforeReset)
				{
					if (callback != null)
						callback(result);
				}

				var cancelled = mCancel;

				mCancel = false;
				mSuspended = false;
				mRunning = false;
				mThread = null;

				if (!cancelled && InvokeCallbackMode == InvokeCallbackMode.AfterReset)
				{
					if (callback != null)
						callback(result);
				}

				if (!cancelled && completionCallback != null)
				{
					completionCallback();
				}

				if (cancelled && cancelledCallback != null)
				{
					cancelledCallback();
				}

			}) {Priority = mPriority};

			mThread.Start();
		}

		public void Suspend()
		{
			if (mThread == null || !mRunning || mSuspended)
				return;

			mSuspended = true;
		}
		public void Resume()
		{
			if (mThread == null || !mRunning || !mSuspended)
				return;

			mSuspended = false;
		}
		public void Wait()
		{
			while (mRunning)
			{
				Application.DoEvents();
			}
		}
		public void Cancel()
		{
			if (mThread == null || !mRunning)
				return;

			if (mSuspended)
				Resume();

			mCancel = true;
			Wait();
		}

		public bool IsRunning
		{
			get { return mRunning; }
		}
		public bool IsSuspended
		{
			get { return mSuspended; }
		}
		public bool IsCancelling
		{
			get { return mCancel; }
		}

		public InvokeCallbackMode InvokeCallbackMode
		{
			get; 
			set;
		}
	}
}
