using System;
using Xyrus.Apophysis.Threading;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class IterationThreadStateToken : ThreadState
	{
		private readonly IterationManagerBase mManager;

		public IterationThreadStateToken([NotNull] IterationManagerBase manager)
		{
			if (manager == null) throw new ArgumentNullException("manager");
			mManager = manager;
		}

		public override bool IsCancelling
		{
			get { return mManager.IsCancelling; }
		}
		public override bool IsSuspended
		{
			get { return mManager.IsSuspended; }
		}
	}
}