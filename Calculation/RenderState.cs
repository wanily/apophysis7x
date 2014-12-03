using System;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class RenderState
	{
		public float AverageIterationsPerSecond { get; set; }
		public float IterationsPerSecond { get; set; }
		public float Progress { get; set; }

		public float CurrentDensity { get; set; }
		public float TargetDensity { get; set; }

		public TimeSpan ElapsedTime { get; set; }
		public TimeSpan? RemainingTime { get; set; }

		public long IterationCount { get; set; }
		public long TotalIterations { get; set; }

		public bool IsBusy { get; set; }
	}
}