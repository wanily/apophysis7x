using System;
using System.Collections.Generic;
using System.Drawing;
using Xyrus.Apophysis.Interfaces.Threading;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;
using Xyrus.Apophysis.Windows.Interfaces.Views;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public class BatchListController : Controller<IMainView>, IBatchListController
	{
		private LazyResolver<IWaitImageController> mWaitImageController;
		private LazyResolver<IThreadController> mListPreviewThreadController;
		private LazyResolver<IMainController> mMainController;

		private IFlameListView mFlameList;

		protected override void DisposeOverride(bool disposing)
		{
			if (!disposing) 
				return;

			mListPreviewThreadController.Reset();
			mWaitImageController.Reset();
		}

		protected override void AttachView()
		{
			mMainController.Object.UndoController.CurrentReplaced += OnCurrentFlameReplaced;

			mFlameList = View.FlameListView;

			mFlameList.FlameSelected += OnListSelectionChanged;
			mFlameList.FlameRenamed += OnListLabelEdited;

			mFlameList.ThreadController = mListPreviewThreadController.Object;
			mFlameList.WaitImageController = mWaitImageController.Object;

			mFlameList.LoadSettings();
		}
		protected override void DetachView()
		{
			mMainController.Object.UndoController.CurrentReplaced -= OnCurrentFlameReplaced;

			if (mFlameList != null)
			{
				mFlameList.FlameSelected -= OnListSelectionChanged;
				mFlameList.FlameRenamed -= OnListLabelEdited;

				mFlameList.SaveSettings();
				mFlameList.Dispose();

				mFlameList = null;
			}
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

		private void OnListSelectionChanged(object sender, FlameSelectEventArgs e)
		{
			mMainController.Object.LoadFlameAndEraseHistory(e.Flame);
			mMainController.Object.AutosaveController.ForceCommit(e.Flame, e.Flame.CalculatedName + " - selected");

			mMainController.Object.UpdateToolbar();
			mMainController.Object.UpdateMenu();
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
			using (mMainController.Object.Initializer.Enter())
			{
				var newFlame = mMainController.Object.UndoController.RequestCurrent();

				mMainController.Object.Flames.Replace(mFlameList.SelectedFlame, newFlame);
				mMainController.Object.EditorController.Flame = newFlame;
				mMainController.Object.FlamePropertiesController.Flame = newFlame;
				mMainController.Object.RenderController.UpdateSelection();

				mFlameList.ReplaceCurrent(newFlame);
			}

			mMainController.Object.UpdatePreviews();
		}
	}
}