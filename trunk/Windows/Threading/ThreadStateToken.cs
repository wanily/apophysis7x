using System;

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

		public void Dispose()
		{
			mController = null;
		}

		public bool IsCancelling
		{
			get { return mController.IsCancelling; }
		}
		public bool IsSuspended
		{
			get { return mController.IsSuspended; }
		}
	}
}