using Xyrus.Apophysis.Interfaces.Threading;

namespace Xyrus.Apophysis.Threading
{
	[PublicAPI]
	public class ThreadState : IThreadState
	{
		public virtual bool IsCancelling { get { return false; } }
		public virtual bool IsSuspended { get { return false; } }
	}
}