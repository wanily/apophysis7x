using System;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class ThreadProgressEventArgs : ProgressEventArgs
	{
		public ThreadProgressEventArgs(int threadIndex, double progress, TimeSpan? timeRemaining = null)
			: base(progress, timeRemaining)
		{
			ThreadIndex = threadIndex;
		}

		public int ThreadIndex { get; private set; }
	}
}