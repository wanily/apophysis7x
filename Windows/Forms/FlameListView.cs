using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Interfaces.Threading;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;
using Xyrus.Apophysis.Windows.Interfaces.Views;

namespace Xyrus.Apophysis.Windows.Forms
{
	public class FlameListView : Component, IFlameListView
	{
		private Main mForm;

		private bool mIsDisposed;
		private int mPreviewSize;
		private bool mIsIconViewEnabled;
		private float mPreviewDensity;

		private TimeLock mIconUpdateTimeLock;
		private Lock mListSelectionLock;

		private FlameSelectEventHandler mListSelectionHandler;
		private FlameRenameEventHandler mListRenameHandler;

		private IThreadController mThreadController;
		private IWaitImageController mWaitImageController;

		private Image mWaitImage;
		private IList<Flame> mFlames;

		public FlameListView([NotNull] Main form)
		{
			if (form == null) throw new ArgumentNullException("form");

			mForm = form;

			mForm.BatchListView.SelectedIndexChanged += OnBatchListSelectionChanged;
			mForm.BatchListView.SizeChanged += OnBatchListSizeChanged;
			mForm.BatchListView.AfterLabelEdit += OnBatchListAfterLabelEdit;

			mForm.BatchListView.LargeImageList = new ImageList
			{
				ColorDepth = ColorDepth.Depth32Bit,
				ImageSize = new Size(120, 120),
			};

			mIconUpdateTimeLock = new TimeLock(UpdateSelectedPreview);
			mListSelectionLock = new Lock();
		}

		protected override void Dispose(bool disposing)
		{
			if (mIsDisposed)
				return;

			if (disposing)
			{
				if (mForm.BatchListView.LargeImageList != null)
				{
					DisposeIcons();
				}

				if (mIconUpdateTimeLock != null)
				{
					mIconUpdateTimeLock.Dispose();
					mIconUpdateTimeLock = null;
				}

				if (mListSelectionLock != null)
				{
					mListSelectionLock.Dispose();
					mListSelectionLock = null;
				}

				if (mWaitImage != null)
				{
					mWaitImage.Dispose();
					mWaitImage = null;
				}
			}

			mForm = null;
			mFlames = null;
			mThreadController = null;
			mWaitImageController = null;
			mListSelectionHandler = null;

			mIsDisposed = true;
		}

		private void OnBatchListSelectionChanged(object sender, EventArgs e)
		{
			if (mListSelectionLock.IsBusy)
				return;

			if (mListSelectionHandler != null)
				mListSelectionHandler(sender, new FlameSelectEventArgs(SelectedFlame));
		}
		private void OnBatchListSizeChanged(object sender, EventArgs e)
		{
			UpdateColumnSize();
		}
		private void OnBatchListAfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			if (mListRenameHandler != null)
				mListRenameHandler(sender, new FlameRenameEventArgs(e.Label));

			e.CancelEdit = e.CancelEdit;
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
		private void UpdateSelectedPreview()
		{
			RedrawIcon(mForm.BatchListView.Items.OfType<ListViewItem>().FirstOrDefault(x => x.Selected), null);
		}

		public void RedrawIcon(ListViewItem item, IThreadStateToken threadState)
		{
			if (item == null)
				return;

			var flame = item.Tag as Flame;
			Debug.Assert(flame != null);

			var canvas = new Size(PreviewSize, PreviewSize);
			var size = flame.CanvasSize.FitToFrame(canvas);

			var renderer = new ThumbnailRenderer();
			var bitmap = renderer.CreateBitmap(flame, PreviewDensity, size, threadState);

			var fitBitmap = new Bitmap(canvas.Width, canvas.Height, PixelFormat.Format32bppArgb);
			using (var graphics = Graphics.FromImage(fitBitmap))
			{
				graphics.DrawImageUnscaled(bitmap, canvas.Width / 2 - size.Width / 2, canvas.Height / 2 - size.Height / 2);
				bitmap.Dispose();
			}

			mForm.Invoke(new Action(() =>
			{
				mForm.BatchListView.LargeImageList.Images.Add(fitBitmap);
				item.ImageIndex = mForm.BatchListView.LargeImageList.Images.Count - 1;
			}));
		}

		private void RedrawIcons()
		{
			if (ThreadController == null)
			{
				RedrawIconsHandler(null);
			}
			else
			{
				ThreadController.StartThread(RedrawIconsHandler);
			}
		}
		private void DisposeIcons()
		{
			if (ThreadController != null)
			{
				ThreadController.Cancel();
			}

			foreach (Image image in mForm.BatchListView.LargeImageList.Images)
			{
				image.Dispose();
			}

			if (mWaitImage == null)
			{
				RenderWaitImage();
			}

			Debug.Assert(mWaitImage != null, "Wait image should be set!");

			mForm.BatchListView.LargeImageList.Images.Clear();
			mForm.BatchListView.LargeImageList.Images.Add(mWaitImage);
		}
		private void RedrawIconsHandler(IThreadStateToken threadState)
		{
			var items = new List<ListViewItem>();
			mForm.Invoke(new Action(() => items.AddRange(mForm.BatchListView.Items.OfType<ListViewItem>())));

			foreach (var item in items)
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
		private void RenderWaitImage()
		{
			mWaitImage = mWaitImageController != null
				? mWaitImageController.DrawWaitImage(new Size(PreviewSize, PreviewSize), Color.Black, Color.White)
				: new Bitmap(PreviewSize, PreviewSize, PixelFormat.Format32bppArgb);
		}

		public void LoadSettings()
		{
			mForm.RootSplitter.SplitterDistance = ApophysisSettings.View.BatchListSize;
			mForm.RootSplitter.Panel1Collapsed = !ApophysisSettings.View.IsBatchListVisible;

			mIsIconViewEnabled = ApophysisSettings.View.BatchListUsePreviews;
			mPreviewSize = ApophysisSettings.View.BatchListPreviewSize;
			mPreviewDensity = ApophysisSettings.Preview.BatchListPreviewDensity;
		}
		public void SaveSettings()
		{
			ApophysisSettings.View.IsBatchListVisible = !mForm.RootSplitter.Panel1Collapsed;
			ApophysisSettings.View.BatchListSize = mForm.RootSplitter.SplitterDistance;

			ApophysisSettings.View.BatchListUsePreviews = mIsIconViewEnabled;
			ApophysisSettings.View.BatchListPreviewSize = mPreviewSize;
			ApophysisSettings.Preview.BatchListPreviewDensity = mPreviewDensity;
		}

		public bool IsIconViewEnabled
		{
			get { return mIsIconViewEnabled; }
			set
			{
				if (Equals(value, mIsIconViewEnabled))
					return;

				mIsIconViewEnabled = value;
				mForm.BatchListView.View = value
					? View.LargeIcon
					: View.Details;

				if (!value)
				{
					UpdateColumnSize();
					DisposeIcons();
				}
				else
				{
					RedrawIcons();
				}
			}
		}
		public int PreviewSize
		{
			get { return mPreviewSize; }
			set
			{
				if (Equals(value, mPreviewSize))
					return;

				mPreviewSize = value;
				mForm.BatchListView.LargeImageList.ImageSize = new Size(value, value);

				if (mIsIconViewEnabled)
				{
					RedrawIcons();
				}

				if (mWaitImage != null)
				{
					mWaitImage.Dispose();
				}

				RenderWaitImage();
			}
		}
		public float PreviewDensity
		{
			get { return mPreviewDensity; }
			set
			{
				if (Equals(value, mPreviewDensity))
					return;

				mPreviewDensity = value;

				mIconUpdateTimeLock.Delay = (int)(ApophysisSettings.Preview.MiniPreviewUpdateResolution * value);

				if (mIsIconViewEnabled)
				{
					RedrawIcons();
				}
			}
		}

		public IThreadController ThreadController
		{
			get { return mThreadController; }
			set
			{
				if (mThreadController != null)
				{
					mThreadController.Cancel();
				}

				mThreadController = value;
			}
		}
		public IWaitImageController WaitImageController
		{
			get { return mWaitImageController; }
			set
			{
				if (mWaitImage != null)
				{
					mWaitImage.Dispose();
				}

				mWaitImageController = value;
				RenderWaitImage();
			}
		}

		public IEnumerable<Flame> Flames
		{
			get { return mFlames; }
			set
			{
				mForm.BatchListView.Items.Clear();
				mFlames = value.CastOrEnumerateToList();

				DisposeIcons();

				foreach (var flame in mFlames)
				{
					var item = mForm.BatchListView.Items.Add(flame.CalculatedName);

					SetItemProperties(item, flame, false);
					item.ImageIndex = 0;
				}

				RedrawIcons();
			}
		}

		public Flame SelectedFlame
		{
			get
			{
				var item = mForm.BatchListView.Items.OfType<ListViewItem>().FirstOrDefault(x => x.Selected);
				if (item == null)
					return null;

				return item.Tag as Flame;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("value");

				var index = mFlames.CastOrEnumerateToList().IndexOf(value);
				if (index < 0)
					return;

				using (mListSelectionLock.Enter())
				{
					mForm.BatchListView.Items[index].Selected = true;
				}
			}
		}

		public event FlameRenameEventHandler FlameRenamed
		{
			add { mListRenameHandler += value; }
			remove { mListRenameHandler -= value; }
		}
		public event FlameSelectEventHandler FlameSelected
		{
			add { mListSelectionHandler += value; }
			remove { mListSelectionHandler -= value; }
		}

		public void ReplaceCurrent(Flame newFlame)
		{
			SetItemProperties(mForm.BatchListView.Items.OfType<ListViewItem>().FirstOrDefault(x => x.Selected), newFlame, true);
		}

		public void UpdateCurrentFlameWithTimer()
		{
			mIconUpdateTimeLock.Enter();
		}

		public void UpdateColumnSize()
		{
			mForm.BatchListView.Columns[0].Width = mForm.BatchListView.ClientSize.Width - 3;
		}
	}
}