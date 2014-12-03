using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Threading;
using Xyrus.Apophysis.Windows.Forms;
using Xyrus.Apophysis.Windows.Input;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class BatchListController : Controller<Main>
	{
		private TimeLock mIconUpdateTimeLock;
		private ThreadController mPreviewThreadController;
		private MainController mParent;
		private bool mIsIconViewEnabled;

		private Bitmap mWaitImage;
		private ImageList mPreviewImages;
		private int mPreviewSize;
		private float mPreviewDensity;

		public BatchListController([NotNull] Main view, [NotNull] MainController parent) : base(view)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;
			mPreviewThreadController = new ThreadController(ThreadPriority.Lowest);
			mIconUpdateTimeLock = new TimeLock(UpdateSelectedPreview);

			mPreviewImages = new ImageList
			{
				ColorDepth = ColorDepth.Depth32Bit, 
				ImageSize = new Size(120, 120),
			};
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (disposing)
			{
				if (mIconUpdateTimeLock != null)
				{
					mIconUpdateTimeLock.Dispose();
					mIconUpdateTimeLock = null;
				}

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
			View.RootSplitter.SplitterDistance = ApophysisSettings.View.BatchListSize;
			View.RootSplitter.Panel1Collapsed = !ApophysisSettings.View.IsBatchListVisible;
			View.BatchListView.LargeImageList = mPreviewImages;

			mParent.UndoController.CurrentReplaced += OnCurrentFlameReplaced;

			View.BatchListView.SelectedIndexChanged += OnListSelectionChanged;
			View.BatchListView.AfterLabelEdit += OnListLabelEdited;
			View.BatchListView.SizeChanged += OnListResized;
			View.CameraChanged += OnCameraChanged;

			using (mParent.Initializer.Enter())
			{
				IsIconViewEnabled = ApophysisSettings.View.BatchListUsePreviews;
				ListPreviewSize = ApophysisSettings.View.BatchListPreviewSize;
				ListPreviewDensity = ApophysisSettings.Preview.BatchListPreviewDensity;
			}

			mWaitImage = WaitImageController.DrawWaitImage(new Size(mPreviewSize, mPreviewSize), Color.Black, Color.White);
		}
		protected override void DetachView()
		{
			View.BatchListView.LargeImageList = null;

			mParent.UndoController.CurrentReplaced -= OnCurrentFlameReplaced;

			View.BatchListView.SelectedIndexChanged -= OnListSelectionChanged;
			View.BatchListView.AfterLabelEdit -= OnListLabelEdited;
			View.BatchListView.SizeChanged -= OnListResized;
			View.CameraChanged -= OnCameraChanged;

			ApophysisSettings.View.BatchListUsePreviews = mIsIconViewEnabled;
			ApophysisSettings.View.BatchListPreviewSize = mPreviewSize;
			ApophysisSettings.Preview.BatchListPreviewDensity = mPreviewDensity;
			ApophysisSettings.View.IsBatchListVisible = !View.RootSplitter.Panel1Collapsed;
			ApophysisSettings.View.BatchListSize = View.RootSplitter.SplitterDistance;
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
			mParent.AutosaveController.ForceCommit(flame, flame.CalculatedName + " - selected");
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
		public float ListPreviewDensity
		{
			get { return mPreviewDensity; }
			set
			{
				if (Equals(value, mPreviewDensity))
					return;

				if (value <= 0 || value > 100)
					throw new ArgumentOutOfRangeException();

				mPreviewDensity = value;
				mIconUpdateTimeLock.Delay = (int)(ApophysisSettings.Preview.MiniPreviewUpdateResolution * value);

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
		}

		private void RedrawIcon(ListViewItem item, ThreadStateToken threadState)
		{
			if (item == null)
				return;

			var flame = item.Tag as Flame;
			Debug.Assert(flame != null);

			var canvas = new Size(mPreviewSize, mPreviewSize);
			var size = flame.CanvasSize.FitToFrame(canvas);

			var renderer = new ThumbnailRenderer();
			var bitmap = renderer.CreateBitmap(flame, mPreviewDensity, size, threadState);

			var fitBitmap = new Bitmap(canvas.Width, canvas.Height, PixelFormat.Format32bppArgb);
			using (var graphics = Graphics.FromImage(fitBitmap))
			{
				graphics.DrawImageUnscaled(bitmap, canvas.Width / 2 - size.Width / 2, canvas.Height / 2 - size.Height / 2);
				bitmap.Dispose();
			}

			View.Invoke(new Action(() =>
				{
					mPreviewImages.Images.Add(fitBitmap);
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
			if (flame == null || mParent.Initializer.IsBusy)
				return;

			mParent.LoadFlameAndEraseHistory(flame);
			mParent.AutosaveController.ForceCommit(flame, flame.CalculatedName + " - selected");

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
				var newFlame = mParent.UndoController.RequestCurrent();

				mParent.Flames.Replace(flame, newFlame);
				SetItemProperties(selected, newFlame, true);
				mParent.EditorController.Flame = newFlame;
				mParent.FlamePropertiesController.Flame = newFlame;
				mParent.RenderController.UpdateSelection();
			}

			mParent.UpdatePreviews();
		}
		private void OnListResized(object sender, EventArgs e)
		{
			View.UpdateBatchListColumnSize();
		}
		private void OnCameraChanged(object sender, CameraChangedEventArgs args)
		{
			mIconUpdateTimeLock.Enter();
		}
	}
}