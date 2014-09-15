using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Threading;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class BatchListController : Controller<Main>
	{
		private ThreadController mPreviewThreadController;
		private MainController mParent;
		private bool mIsIconViewEnabled;

		private Bitmap mWaitImage;
		private ImageList mPreviewImages;
		private int mPreviewSize;
		private double mPreviewDensity;

		public BatchListController([NotNull] Main view, [NotNull] MainController parent) : base(view)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;
			mPreviewThreadController = new ThreadController(ThreadPriority.Lowest);

			mPreviewImages = new ImageList
			{
				ColorDepth = ColorDepth.Depth32Bit, 
				ImageSize = new Size(120, 120)
			};
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mPreviewImages != null)
				{
					DisposeIcons();
					mPreviewImages.Dispose();
					mPreviewImages = null;
				}

				if (mWaitImage != null)
				{
					mWaitImage.Dispose();
					mWaitImage = null;
				}

				if (mPreviewThreadController != null)
				{
					mPreviewThreadController.Dispose();
					mPreviewThreadController = null;
				}
			}

			mParent = null;
		}

		protected override void AttachView()
		{
			View.RootSplitter.SplitterDistance = ApophysisSettings.BatchListSize;
			View.RootSplitter.Panel1Collapsed = !ApophysisSettings.IsBatchListVisible;
			View.BatchListView.LargeImageList = mPreviewImages;

			mParent.UndoController.CurrentReplaced += OnCurrentFlameReplaced;

			View.BatchListView.SelectedIndexChanged += OnListSelectionChanged;
			View.BatchListView.AfterLabelEdit += OnListLabelEdited;

			using (mParent.Initializer.Enter())
			{
				IsIconViewEnabled = ApophysisSettings.BatchListUsePreviews;
				ListPreviewSize = ApophysisSettings.BatchListPreviewSize;
				ListPreviewDensity = ApophysisSettings.BatchListPreviewDensity;
			}

			mWaitImage = WaitImageController.DrawWaitImage(new Size(mPreviewSize, mPreviewSize), Color.Black, Color.White);
		}
		protected override void DetachView()
		{
			View.BatchListView.LargeImageList = null;

			mParent.UndoController.CurrentReplaced -= OnCurrentFlameReplaced;

			View.BatchListView.SelectedIndexChanged -= OnListSelectionChanged;
			View.BatchListView.AfterLabelEdit -= OnListLabelEdited;

			ApophysisSettings.BatchListUsePreviews = mIsIconViewEnabled;
			ApophysisSettings.BatchListPreviewSize = mPreviewSize;
			ApophysisSettings.BatchListPreviewDensity = mPreviewDensity;
			ApophysisSettings.IsBatchListVisible = !View.RootSplitter.Panel1Collapsed;
			ApophysisSettings.BatchListSize = View.RootSplitter.SplitterDistance;
		}

		public void BuildFlameList()
		{
			View.BatchListView.Items.Clear();
			if (mParent.Flames == null)
				return;

			DisposeIcons();

			foreach (var flame in mParent.Flames)
			{
				var item = View.BatchListView.Items.Add(flame.CalculatedName);

				SetItemProperties(item, flame, false);
				item.ImageIndex = 0;
			}

			RedrawIcons();
		}

		public void SelectFlame([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			if (mParent.Initializer.IsBusy || mParent.Flames == null)
				return;

			using (mParent.Initializer.Enter())
			{
				SetListSelection(flame);
			}

			mParent.LoadFlameAndEraseHistory(flame);
			mParent.EditorController.Flame = flame;
			mParent.FlamePropertiesController.Flame = flame;
		}
		public Flame GetSelectedFlame()
		{
			var item = GetSelectedItem();
			if (item == null)
				return null;

			return item.Tag as Flame;
		}

		public void UpdateSelectedPreview()
		{
			var item = GetSelectedItem();
			RedrawIcon(item, null);
		}

		public bool IsIconViewEnabled
		{
			get { return mIsIconViewEnabled; }
			set
			{
				if (Equals(value, mIsIconViewEnabled))
					return;

				mIsIconViewEnabled = value;
				View.BatchListView.View = value ? System.Windows.Forms.View.LargeIcon : System.Windows.Forms.View.Details;

				if (!value)
				{
					View.UpdateBatchListColumnSize();
				}

				if (mParent.Initializer.IsBusy)
					return;

				if (value)
				{
					RedrawIcons();
				}
				else
				{
					DisposeIcons();
				}
			}
		}
		public int ListPreviewSize
		{
			get { return mPreviewSize; }
			set
			{
				if (Equals(value, mPreviewSize))
					return;

				if (value < 50 || value > 120) 
					throw new ArgumentOutOfRangeException();

				mPreviewSize = value;
				mPreviewImages.ImageSize = new Size(value, value);

				if (mParent.Initializer.IsBusy)
					return;

				if (mWaitImage != null)
				{
					mWaitImage.Dispose();
				}

				mWaitImage = WaitImageController.DrawWaitImage(new Size(mPreviewSize, mPreviewSize), Color.Black, Color.White);

				if (mIsIconViewEnabled)
				{
					RedrawIcons();
				}
			}
		}
		public double ListPreviewDensity
		{
			get { return mPreviewDensity; }
			set
			{
				if (Equals(value, mPreviewDensity))
					return;

				if (value <= 0 || value > 100)
					throw new ArgumentOutOfRangeException();

				mPreviewDensity = value;

				if (mParent.Initializer.IsBusy)
					return;

				if (mIsIconViewEnabled)
				{
					RedrawIcons();
				}
			}
		}

		private void SetItemProperties(ListViewItem item, Flame flame, bool updatePreview)
		{
			item.Text = flame.CalculatedName;
			item.Tag = flame;

			if (updatePreview)
			{
				RedrawIcon(item, null);
			}
		}
		private ListViewItem GetSelectedItem()
		{
			return View.BatchListView.Items.OfType<ListViewItem>().FirstOrDefault(x => x.Selected);
		}
		private void SetListSelection([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			var index = mParent.Flames.IndexOf(flame);
			if (index < 0)
				return;

			View.BatchListView.Items[index].Selected = true;
			View.FlameNameLabel.Text = flame.CalculatedName;
		}

		private void RedrawIcon(ListViewItem item, ThreadStateToken threadState)
		{
			var flame = item.Tag as Flame;
			Debug.Assert(flame != null);

			var renderer = new ThumbnailRenderer();
			var bitmap = renderer.CreateBitmap(flame, mPreviewDensity, new Size(mPreviewSize, mPreviewSize), threadState);

			View.Invoke(new Action(() =>
				{
					mPreviewImages.Images.Add(bitmap);
					item.ImageIndex = mPreviewImages.Images.Count - 1;
				}));
		}
		private void RedrawIcons()
		{
			DisposeIcons();
			mPreviewThreadController.StartThread(RedrawIconsThread);
		}
		private void RedrawIconsThread(ThreadStateToken threadState)
		{
			var items = new List<ListViewItem>();
			View.Invoke(new Action(() => items.AddRange(View.BatchListView.Items.OfType<ListViewItem>())));

			foreach (ListViewItem item in items)
			{
				if (threadState.IsCancelling)
					break;

				if (threadState.IsSuspended)
				{
					Thread.Sleep(10);
					continue;
				}

				RedrawIcon(item, threadState);
			}
		}

		private void DisposeIcons()
		{
			mPreviewThreadController.Cancel();

			foreach (Image image in mPreviewImages.Images)
			{
				image.Dispose();
			}

			mPreviewImages.Images.Clear();
			mPreviewImages.Images.Add(mWaitImage);
		}

		private void OnListSelectionChanged(object sender, EventArgs e)
		{
			var flame = GetSelectedFlame();
			if (flame == null)
				return;

			mParent.LoadFlameAndEraseHistory(flame);
			mParent.UpdateToolbar();
			mParent.UpdateMenu();
		}
		private void OnListLabelEdited(object sender, LabelEditEventArgs e)
		{
			var item = View.BatchListView.Items[e.Item];
			var flame = item.Tag as Flame;

			if (flame == null)
				return;

			if (!string.IsNullOrEmpty(e.Label) && !string.IsNullOrEmpty(e.Label.Trim()))
			{
				flame.Name = e.Label;
				mParent.NotifyFlameNameChanged(flame);
			}
			else
			{
				e.CancelEdit = true;
			}
		}
		private void OnCurrentFlameReplaced(object sender, EventArgs e)
		{
			var selected = GetSelectedItem();
			if (selected == null)
				return;

			var flame = selected.Tag as Flame;
			if (flame == null)
				return;

			using (mParent.Initializer.Enter())
			{
				mParent.Flames.Replace(flame, mParent.UndoController.Current);
				SetItemProperties(selected, mParent.UndoController.Current, true);
				mParent.EditorController.Flame = mParent.UndoController.Current;
				mParent.FlamePropertiesController.Flame = mParent.UndoController.Current;
			}

			mParent.UpdatePreviews();
		}
	}
}