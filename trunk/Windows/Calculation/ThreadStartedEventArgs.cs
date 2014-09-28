namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class ThreadStartedEventArgs : StartedEventArgs
	{
		public ThreadStartedEventArgs(int threadIndex)
		{
			ThreadIndex = threadIndex;
		}

		public int ThreadIndex { get; private set; }
	}
}