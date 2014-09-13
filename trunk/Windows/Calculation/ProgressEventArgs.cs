using System;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class ProgressEventArgs : EventArgs
	{
		public ProgressEventArgs(double progress, TimeSpan? timeRemaining = null)
		{
			Progress = progress;
			TimeRemaining = timeRemaining;
		}

		public TimeSpan? TimeRemaining
		{
			get; 
			private set;
		}
		public double Progress
		{
			get; 
			private set;
		}
	}
}