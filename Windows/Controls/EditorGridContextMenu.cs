using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Resources;

namespace Xyrus.Apophysis.Windows.Controls
{
	sealed class EditorGridContextMenu : ContextMenuStrip
	{
		private EditorCanvas mEditor;

		private ToolStripButton mZoomAutomatically;
		private ToolStripButton mShowVariationPreview;
		private ToolStripButton mLockAxes;
		private ToolStripButton mEditPostMatrix;

		public EditorGridContextMenu([NotNull] EditorCanvas editor)
		{
			if (editor == null) throw new ArgumentNullException("editor");
			mEditor = editor;

			var items = new ToolStripItem[]
			{
				new ToolStripButton("New transform", Icons.NewIterator, OnNewIteratorClick) { ImageTransparentColor = Color.Fuchsia },

				new ToolStripSeparator(),

				mZoomAutomatically = new ToolStripButton("Zoom automatically", Icons.ToggleAutoZoom, OnZoomAutomaticallyClick) { Checked = editor.Settings.ZoomAutomatically, ImageTransparentColor = Color.Fuchsia },
				mShowVariationPreview = new ToolStripButton("Show variation preview", Icons.ToggleVariationPrevie, OnShowVariationPreviewClick) { Checked = editor.Settings.ShowVariationPreview, ImageTransparentColor = Color.Fuchsia  },

				new ToolStripSeparator(),

				mLockAxes = new ToolStripButton("Lock transform axes", Icons.LockTransformAxes, OnLockAxesClick) { Checked = editor.Settings.LockAxes, ImageTransparentColor = Color.Fuchsia  },
				mEditPostMatrix = new ToolStripButton("Enable / edit post-transform", Icons.TogglePostMatrix, OnTogglePostMatrixClick) { Checked = editor.ActiveMatrix == IteratorMatrix.PostAffine, ImageTransparentColor = Color.Fuchsia  },

				new ToolStripSeparator(),

				new ToolStripButton("Flip all vertically", Icons.FlipAllVertical, OnFlipAllVerticallyClick) { ImageTransparentColor = Color.Fuchsia },
				new ToolStripButton("Flip all horizontally", Icons.FlipAllHorizontal, OnFlipAllHorizontallyClick) { ImageTransparentColor = Color.Fuchsia }
			};

			Items.AddRange(items);
		}
		protected override void Dispose(bool disposing)
		{
			mEditor = null;

			mZoomAutomatically = null;
			mShowVariationPreview = null;
			mLockAxes = null;
			mEditPostMatrix = null;

			Items.Clear();
			base.Dispose(disposing);
		}

		private void OnNewIteratorClick(object sender, EventArgs e)
		{
			mEditor.Commands.NewIterator();
		}

		private void OnZoomAutomaticallyClick(object sender, EventArgs e)
		{
			var item = sender as ToolStripButton;
			if (item == null)
				return;

			item.Checked = (mEditor.Settings.ZoomAutomatically = !mEditor.Settings.ZoomAutomatically);
			mEditor.Refresh();
		}
		private void OnShowVariationPreviewClick(object sender, EventArgs e)
		{
			var item = sender as ToolStripButton;
			if (item == null)
				return;

			item.Checked = (mEditor.Settings.ShowVariationPreview = !mEditor.Settings.ShowVariationPreview);
			mEditor.Refresh();
		}

		private void OnLockAxesClick(object sender, EventArgs e)
		{
			var item = sender as ToolStripButton;
			if (item == null)
				return;

			item.Checked = (mEditor.Settings.LockAxes = !mEditor.Settings.LockAxes);
			mEditor.Refresh();
		}
		private void OnTogglePostMatrixClick(object sender, EventArgs e)
		{
			var item = sender as ToolStripButton;
			if (item == null)
				return;

			mEditor.ActiveMatrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine 
				? IteratorMatrix.PreAffine 
				: IteratorMatrix.PostAffine;

			item.Checked = (mEditor.ActiveMatrix == IteratorMatrix.PostAffine);
			mEditor.Refresh();
		}

		private void OnFlipAllVerticallyClick(object sender, EventArgs e)
		{
			mEditor.Commands.FlipAllVertically();
		}
		private void OnFlipAllHorizontallyClick(object sender, EventArgs e)
		{
			mEditor.Commands.FlipAllHorizontally();
		}

		public void UpdateCheckedStates(EditorCanvas editorCanvas)
		{
			if (editorCanvas != null)
			{
				mEditPostMatrix.Checked = mEditor.ActiveMatrix == IteratorMatrix.PostAffine;
			}
		}
		public void UpdateCheckedStates(EditorSettings editorSettings)
		{
			if (mZoomAutomatically != null)
			{
				mZoomAutomatically.Checked = editorSettings != null && editorSettings.ZoomAutomatically;
			}

			if (mShowVariationPreview != null)
			{
				mShowVariationPreview.Checked = editorSettings != null && editorSettings.ShowVariationPreview;
			}

			if (mLockAxes != null)
			{
				mLockAxes.Checked = editorSettings != null && editorSettings.LockAxes;
			}
		}
	}
}
