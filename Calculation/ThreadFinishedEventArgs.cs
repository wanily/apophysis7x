namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class ThreadFinishedEventArgs : FinishedEventArgs
	{
		public ThreadFinishedEventArgs(int threadIndex, bool cancelled) : base(cancelled)
		{
			ThreadIndex = threadIndex;
		}

		public int ThreadIndex { get; private set; }
	}
}