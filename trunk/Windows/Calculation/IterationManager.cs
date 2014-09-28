using System;
using System.Threading;

namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public class IterationManager : IterationManagerBase
	{
		private void UpdateState(ProgressManager manager)
		{
			IsBusy = manager.IsBusy;
			IterationsPerSecond = manager.IterationsPerSecond;
			IterationProgress = manager.IterationProgress;
			RemainingTime = manager.RemainingTime;
			AverageIterationsPerSecond = manager.AverageIterationsPerSecond;
			PureRenderingTime = manager.PureRenderingTime;
			TotalIterations = manager.TotalIterations;
		}

		private void OnStarted(object sender, StartedEventArgs e)
		{
			UpdateState((ProgressManager)sender);
			RaiseStarted();
		}
		private void OnProgress(object sender, ProgressEventArgs e)
		{
			UpdateState((ProgressManager)sender);
			RaiseProgress();
		}
		private void OnFinished(object sender, FinishedEventArgs e)
		{
			var manager = (ProgressManager) sender;

			UpdateState(manager);
			RaiseFinished(manager.ThreadState.IsCancelling);
		}

		public override void StartIterate(Histogram histogram, double density)
		{
			if (histogram == null) throw new ArgumentNullException("histogram");

			StateReset();

			var progress = new ProgressManager(new IterationThreadStateToken(this));
			var thread = new Thread(() => histogram.Iterate(density, progress));

			progress.Started += OnStarted;
			progress.Progress += OnProgress;
			progress.Finished += OnFinished;

			thread.Start();
		}
		public override void Iterate(Histogram histogram, double density)
		{
			if (histogram == null) throw new ArgumentNullException("histogram");

			StateReset();

			var progress = new ProgressManager(new IterationThreadStateToken(this));

			progress.Started += OnStarted;
			progress.Progress += OnProgress;
			progress.Finished += OnFinished;

			histogram.Iterate(density, progress);
		}
	}
}