using System;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class ThreadProgressEventArgs : ProgressEventArgs
	{
		public ThreadProgressEventArgs(int threadIndex)
		{
			ThreadIndex = threadIndex;
		}

		public int ThreadIndex { get; private set; }
	}
}