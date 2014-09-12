using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controllers
{
	class BatchListController : Controller<Main>
	{
		private MainController mParent;
		private bool mIsIconViewEnabled;

		private static Bitmap mWaitImage;

		private const char mWaitGlyph = '6';
		private static readonly Color mWaitBackground = Color.Black;
		private static readonly Size mIconSize = new Size(120, 120);

		private ImageList mPreviewImages;

		static BatchListController()
		{
			DrawWaitImage();
		}
		public BatchListController([NotNull] Main view, [NotNull] MainController parent) : base(view)
		{
			if (parent == null) throw new ArgumentNullException("parent");

			mParent = parent;

			mPreviewImages = new ImageList();
			mPreviewImages.ColorDepth = ColorDepth.Depth32Bit;
			mPreviewImages.ImageSize = mIconSize;
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
			}

			mParent = null;
		}

		protected override void AttachView()
		{
			View.BatchListView.LargeImageList = mPreviewImages;

			mParent.UndoController.CurrentReplaced += OnCurrentFlameReplaced;

			View.BatchListView.SelectedIndexChanged += OnListSelectionChanged;
			View.BatchListView.AfterLabelEdit += OnListLabelEdited;

			IsIconViewEnabled = ApophysisSettings.BatchListUsePreviews;
		}
		protected override void DetachView()
		{
			View.BatchListView.LargeImageList = null;

			mParent.UndoController.CurrentReplaced -= OnCurrentFlameReplaced;

			View.BatchListView.SelectedIndexChanged -= OnListSelectionChanged;
			View.BatchListView.AfterLabelEdit -= OnListLabelEdited;

			ApophysisSettings.BatchListUsePreviews = mIsIconViewEnabled;
		}

		public void BuildFlameList()
		{
			View.BatchListView.Items.Clear();
			if (mParent.Flames == null)
				return;

			DisposeIcons();

			var index = 0;
			foreach (var flame in mParent.Flames)
			{
				var item = View.BatchListView.Items.Add(flame.CalculatedName);

				SetItemProperties(item, flame);
				item.ImageIndex = 0;

				if (IsIconViewEnabled)
				{
					RedrawIcon(item, ++index);
				}
			}
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
		}
		public Flame GetSelectedFlame()
		{
			var item = GetSelectedItem();
			if (item == null)
				return null;

			return item.Tag as Flame;
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
				//View.BatchListView.OwnerDraw = value;

				if (value)
				{
					RedrawIcons();
				}
				else
				{
					DisposeIcons();
					View.UpdateBatchListColumnSize();
				}
			}
		}

		private void SetItemProperties(ListViewItem item, Flame flame)
		{
			item.Text = flame.CalculatedName;
			item.Tag = flame;
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

		private void RedrawIcon(ListViewItem item, int imageListIndex)
		{
			//todo
		}
		private void RedrawIcons()
		{
			DisposeIcons();

			var index = 0;
			foreach (ListViewItem item in View.BatchListView.Items)
				RedrawIcon(item, ++index);
		}
		private void DisposeIcons()
		{
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
				SetItemProperties(selected, mParent.UndoController.Current);
				mParent.EditorController.Flame = mParent.UndoController.Current;
			}
		}

		private static void DrawWaitImage()
		{
			mWaitImage = new Bitmap(mIconSize.Width, mIconSize.Height);

			using (var graphics = Graphics.FromImage(mWaitImage))
			using (var background = new SolidBrush(mWaitBackground))
			using (var foreground = new SolidBrush(Color.FromArgb(255 - mWaitBackground.R, 255 - mWaitBackground.G, 255 - mWaitBackground.B)))
			using (var frame = new Pen(foreground))
			{
				graphics.FillRectangle(background, new Rectangle(new Point(), mIconSize));
				graphics.DrawRectangle(frame, new Rectangle(new Point(), mIconSize));

				using (var glyphFont = new Font("Wingdings", 50f, FontStyle.Bold))
				{
					var glyph = new string(mWaitGlyph, 1);
					var glyphSize = graphics.MeasureString(glyph, glyphFont);

					graphics.DrawString(glyph, glyphFont, foreground, mIconSize.Width / 2f - glyphSize.Width / 2f, mIconSize.Height / 2f - glyphSize.Height / 2f);
				}
			}
		}
	}
}