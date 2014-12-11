using System;
using System.Collections.Generic;
using Xyrus.Apophysis.Interfaces.Threading;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;
using Xyrus.Apophysis.Windows.Interfaces.Views;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class BatchListController : Controller<IMainView>, IBatchListController
	{
		private Resolver<IWaitImageController> mWaitImageController;
		private Resolver<IThreadController> mListPreviewThreadController;
		private Resolver<IMainController> mMainController;

		private IFlameListView mFlameList;
		private Lock mReplaceLock;

		public BatchListController()
		{
			mMainController.Object.FlameChanged += OnCurrentFlameReplaced;

			mFlameList = View.FlameListView;

			mFlameList.FlameSelected += OnListSelectionChanged;
			mFlameList.FlameRenamed += OnListLabelEdited;

			mFlameList.ThreadController = mListPreviewThreadController.Object;
			mFlameList.WaitImageController = mWaitImageController.Object;

			mFlameList.LoadSettings();

			mReplaceLock = new Lock();
		}

		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mMainController.IsResolved)
				{
					mMainController.Object.FlameChanged -= OnCurrentFlameReplaced;
				}

				if (mFlameList != null)
				{
					mFlameList.FlameSelected -= OnListSelectionChanged;
					mFlameList.FlameRenamed -= OnListLabelEdited;

					mFlameList.SaveSettings();
					mFlameList.Dispose();
				}

				if (mReplaceLock != null)
				{
					mReplaceLock.Dispose();
				}

				mListPreviewThreadController.Reset();
				mWaitImageController.Reset();

				mMainController.Reset(false);
			}

			mFlameList = null;
			mReplaceLock = null;
		}

		public IEnumerable<Flame> Flames
		{
			get { return mFlameList.Flames; }
			set { mFlameList.Flames = value; }
		}
		public Flame SelectedFlame
		{
			get { return mFlameList.SelectedFlame; }
			set
			{
				if (value == null) 
					throw new ArgumentNullException("value");

				mFlameList.SelectedFlame = value;

				mMainController.Object.LoadFlameAndEraseHistory(value);
				mMainController.Object.AutosaveController.ForceCommit(value, value.CalculatedName + " - selected");
			}
		}

		public bool IsIconViewEnabled
		{
			get { return mFlameList.IsIconViewEnabled; }
			set
			{
				mFlameList.IsIconViewEnabled = value;
			}
		}

		public int PreviewSize
		{
			get { return mFlameList.PreviewSize; }
			set
			{
				if (value < 50 || value > 120) 
					throw new ArgumentOutOfRangeException();

				mFlameList.PreviewSize = value;
			}
		}
		public float PreviewDensity
		{
			get { return mFlameList.PreviewDensity; }
			set
			{
				if (value <= 0 || value > 100)
					throw new ArgumentOutOfRangeException();

				mFlameList.PreviewDensity = value;
			}
		}

		public void UpdateSelectedPreview()
		{
			mFlameList.UpdateCurrent();
		}

		private void OnListSelectionChanged(object sender, FlameSelectEventArgs e)
		{
			mMainController.Object.LoadFlameAndEraseHistory(e.Flame);
			mMainController.Object.AutosaveController.ForceCommit(e.Flame, e.Flame.CalculatedName + " - selected");

			mMainController.Object.UpdateToolbar();
			mMainController.Object.UpdateMenu();

			using (mReplaceLock.Enter())
			{
				mMainController.Object.RaiseFlameChanged();
			}
		}
		private void OnListLabelEdited(object sender, FlameRenameEventArgs e)
		{
			var flame = mFlameList.SelectedFlame;

			if (!string.IsNullOrEmpty(e.Label) && !string.IsNullOrEmpty(e.Label.Trim()))
			{
				flame.Name = e.Label;
				mMainController.Object.NotifyFlameNameChanged(flame);
			}
			else
			{
				e.CancelEdit = true;
			}
		}
		private void OnCurrentFlameReplaced(object sender, EventArgs e)
		{
			if (mReplaceLock.IsBusy)
				return;

			var newFlame = mMainController.Object.UndoController.RequestCurrent();

			mMainController.Object.Flames.Replace(mFlameList.SelectedFlame, newFlame);
			mMainController.Object.RenderController.UpdateSelection();

			mFlameList.ReplaceCurrent(newFlame);

			mMainController.Object.UpdatePreviews();
		}
	}
}