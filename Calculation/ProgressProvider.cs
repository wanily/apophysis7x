using System;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public abstract class ProgressProvider
	{
		private const float mProgressThreshold = 1;
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
		public float TargetDensity
		{
			get { return mRenderState.TargetDensity; }
			protected set { mRenderState.TargetDensity = value; }
		}

		public float AverageIterationsPerSecond
		{
			get { return mRenderState.AverageIterationsPerSecond; }
			protected set { mRenderState.AverageIterationsPerSecond = value; }
		}
		public float IterationsPerSecond
		{
			get { return mRenderState.IterationsPerSecond; }
			protected set { mRenderState.IterationsPerSecond = value; }
		}

		public float CurrentDensity
		{
			get { return mRenderState.CurrentDensity; }
			protected set { mRenderState.CurrentDensity = value; }
		}
		public float IterationProgress
		{
			get { return mRenderState.Progress; }
			protected set { mRenderState.Progress = value; }
		}
		public long IterationCount
		{
			get { return mRenderState.IterationCount; }
			protected set { mRenderState.IterationCount = value; }
		}

		public TimeSpan? RemainingTime
		{
			get { return mRenderState.RemainingTime; }
			protected set { mRenderState.RemainingTime = value; }
		}
		public TimeSpan ElapsedTime
		{
			get { return mRenderState.ElapsedTime; }
			protected set { mRenderState.ElapsedTime = value; }
		}

		public bool IsBusy
		{
			get { return mRenderState.IsBusy; }
			protected set { mRenderState.IsBusy = value; }
		}

		protected static float ProgressThreshold
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

			Progress(this, new ProgressEventArgs());
		}
		protected void RaiseFinished(bool cancelled)
		{
			if (Finished == null)
				return;

			Finished(this, new FinishedEventArgs(cancelled));
		}
	}
}