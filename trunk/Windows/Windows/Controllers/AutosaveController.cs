using System;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class AutosaveController : Controller
	{
		private NativeTimer mTimer;
		private MainController mParent;
		private bool mIsDisposed;

		public AutosaveController([NotNull] MainController parent)
		{
			if (parent == null) throw new ArgumentNullException("parent");
			mParent = parent;
			mTimer = new NativeTimer();
		}

		protected override void Dispose(bool disposing)
		{
			if (mIsDisposed)
				return;

			if (disposing)
			{
				mParent.EditorController.FlameChanged -= OnChangeCommitted;
				mParent.FlamePropertiesController.FlameChanged -= OnChangeCommitted;
			}

			mTimer = null;
			mParent = null;

			mIsDisposed = true;
		}
		private void OnChangeCommitted(object sender, EventArgs e)
		{
			if (mTimer.GetElapsedTimeInSeconds() < ApophysisSettings.Autosave.Threshold)
				return;

			var flame = mParent.BatchListController.GetSelectedFlame();
			if (flame == null)
				return;

			var time = DateTime.Now;
			ForceCommit(flame, string.Format("{5} - {0:0000}-{1:00}-{2:00} {3:00}.{4:00}", time.Year, time.Month, time.Day, time.Hour, time.Minute, flame.CalculatedName));
		}

		public void ForceCommit(Flame flame, string name)
		{
			var path = Environment.ExpandEnvironmentVariables(ApophysisSettings.Autosave.TargetPath);

			flame = flame.Copy();
			flame.Name = name;

			mParent.SaveFlame(flame, path, 50, "autosave");
			mTimer.SetStartingTime();
		}
		public void Initialize()
		{
			mParent.EditorController.FlameChanged += OnChangeCommitted;
			mParent.FlamePropertiesController.FlameChanged += OnChangeCommitted;

			mTimer.SetStartingTime();
		}
	}
}