using System;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Interfaces;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class AutosaveController : Controller, IAutosaveController
	{
		private LazyResolver<NativeTimer> mTimer;
		private LazyResolver<IMainController> mParent;
		private bool mIsDisposed;

		protected override void Dispose(bool disposing)
		{
			if (mIsDisposed)
				return;

			if (disposing)
			{
				mParent.Object.EditorController.FlameChanged -= OnChangeCommitted;
				mParent.Object.FlamePropertiesController.FlameChanged -= OnChangeCommitted;
			}

			mIsDisposed = true;
		}
		private void OnChangeCommitted(object sender, EventArgs e)
		{
			if (mTimer.Object.GetElapsedTimeInSeconds() < ApophysisSettings.Autosave.Threshold)
				return;

			var flame = mParent.Object.BatchListController.GetSelectedFlame();
			if (flame == null)
				return;

			var time = DateTime.Now;
			ForceCommit(flame, string.Format("{5} - {0:0000}-{1:00}-{2:00} {3:00}.{4:00}", time.Year, time.Month, time.Day, time.Hour, time.Minute, flame.CalculatedName));
		}

		public void ForceCommit(Flame flame, string name)
		{
			if (!ApophysisSettings.Autosave.Enabled)
				return;

			var path = Environment.ExpandEnvironmentVariables(ApophysisSettings.Autosave.TargetPath);

			flame = flame.Copy();
			flame.Name = name;

			mParent.Object.SaveFlame(flame, path, 50, "autosave");
			mTimer.Object.SetStartingTime();
		}
		public void Initialize()
		{
			mParent.Object.EditorController.FlameChanged += OnChangeCommitted;
			mParent.Object.FlamePropertiesController.FlameChanged += OnChangeCommitted;

			mTimer.Object.SetStartingTime();
		}
	}
}