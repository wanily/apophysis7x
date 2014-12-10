namespace Xyrus.Apophysis.Interfaces.Threading
{
	public interface IThreadState
	{
		bool IsCancelling { get; }
		bool IsSuspended { get; }
	}
}