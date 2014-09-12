using System;
using System.Threading;

namespace Xyrus.Apophysis.Threading
{
	public sealed class ThreadStateToken : IDisposable
	{
		private ThreadController mController;

		internal ThreadStateToken([NotNull] ThreadController controller)
		{
			if (controller == null) throw new ArgumentNullException("controller");
			mController = controller;
		}

		~ThreadStateToken()
		{
			Dispose(false);
		}
		private void Dispose(bool disposing)
		{
			mController = null;
		}
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public bool IsCancelling
		{
			get { return mController.IsCancelling; }
		}
		public bool IsSuspended
		{
			get { return mController.IsSuspended; }
		}
		public Thread ParentThread
		{
			get { return mController.ParentThread; }
		}
	}
}