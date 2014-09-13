using System;
using System.Net.Mime;
using System.Threading;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows;

namespace Xyrus.Apophysis.Threading
{
	public class ThreadController : Controller
	{
		private volatile bool mCancel;
		private volatile bool mSuspended;
		private volatile bool mRunning;

		private Thread mThread;
		private ThreadPriority mPriority;
		private Thread mParentThread;

		public ThreadController(ThreadPriority priority = ThreadPriority.Normal)
		{
			mPriority = priority;
			mParentThread = Thread.CurrentThread;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && mThread != null)
			{
				Cancel();
			}

			mParentThread = null;
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
		public void StartThread(Action<ThreadStateToken> threadAction, Action callback = null)
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
		public void StartThread<T>(Func<ThreadStateToken, T> threadAction, Action<T> callback = null)
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

				if (!mCancel)
				{
					if (callback != null)
						callback(result);
				}

				mCancel = false;
				mSuspended = false;
				mRunning = false;
				mThread = null;
			});

			mThread.Priority = mPriority;
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

		internal Thread ParentThread
		{
			get { return mParentThread; }
		}
	}
}
