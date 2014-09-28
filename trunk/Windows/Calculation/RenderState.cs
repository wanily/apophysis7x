using System;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class RenderState
	{
		public double AverageIterationsPerSecond { get; set; }
		public double IterationsPerSecond { get; set; }
		public double PureRenderingTime { get; set; }
		public double Progress { get; set; }

		public TimeSpan? RemainingTime { get; set; }

		public long TotalIterations { get; set; }
		public bool IsBusy { get; set; }
	}
}