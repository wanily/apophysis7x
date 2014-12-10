using System;
using Xyrus.Apophysis.Interfaces.Threading;

namespace Xyrus.Apophysis.Threading
{
	[PublicAPI]
	public sealed class ThreadStateToken : ThreadState, IThreadStateToken
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

		public override bool IsCancelling
		{
			get { return mController.IsCancelling; }
		}
		public override bool IsSuspended
		{
			get { return mController.IsSuspended; }
		}
	}
}