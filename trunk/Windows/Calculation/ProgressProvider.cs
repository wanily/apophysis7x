using System;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public abstract class ProgressProvider
	{
		private const double mProgressThreshold = 1;
		private readonly RenderState mRenderState;

		protected ProgressProvider()
		{
			mRenderState = new RenderState();
		}

		public event StartedEventHandler Started;
		public event ProgressEventHandler Progress;
		public event FinishedEventHandler Finished;

		public long TotalIterations
		{
			get { return mRenderState.TotalIterations; }
			protected set { mRenderState.TotalIterations = value; }
		}

		public double AverageIterationsPerSecond
		{
			get { return mRenderState.AverageIterationsPerSecond; }
			protected set { mRenderState.AverageIterationsPerSecond = value; }
		}
		public double PureRenderingTime
		{
			get { return mRenderState.PureRenderingTime; }
			protected set { mRenderState.PureRenderingTime = value; }
		}

		public double IterationsPerSecond
		{
			get { return mRenderState.IterationsPerSecond; }
			protected set { mRenderState.IterationsPerSecond = value; }
		}
		public double IterationProgress
		{
			get { return mRenderState.Progress; }
			protected set { mRenderState.Progress = value; }
		}
		public TimeSpan? RemainingTime
		{
			get { return mRenderState.RemainingTime; }
			protected set { mRenderState.RemainingTime = value; }
		}

		public bool IsBusy
		{
			get { return mRenderState.IsBusy; }
			protected set { mRenderState.IsBusy = value; }
		}

		protected static double ProgressThreshold
		{
			get { return mProgressThreshold; }
		}

		protected void RaiseStarted()
		{
			if (Started == null)
				return;

			Started(this, new StartedEventArgs());
		}
		protected void RaiseProgress()
		{
			if (Progress == null)
				return;

			Progress(this, new ProgressEventArgs(IterationProgress, RemainingTime));
		}
		protected void RaiseFinished(bool cancelled)
		{
			if (Finished == null)
				return;

			Finished(this, new FinishedEventArgs(cancelled));
		}
	}
}