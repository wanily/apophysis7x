namespace Xyrus.Apophysis.Threading
{
	[PublicAPI]
	public class ThreadState
	{
		public virtual bool IsCancelling { get { return false; } }
		public virtual bool IsSuspended { get { return false; } }
	}
}