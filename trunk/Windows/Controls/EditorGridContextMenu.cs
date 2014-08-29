using System;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Controls
{
	[PublicAPI]
	public sealed class EditorGridContextMenu : ContextMenuStrip
	{
		private EditorCanvas mEditor;

		private ToolStripItem mUndoButton;
		private ToolStripItem mRedoButton;

		private ToolStripButton mZoomAutomatically;
		private ToolStripButton mShowVariationPreview;
		private ToolStripButton mLockTransformAxes;
		private ToolStripButton mEditPostTransforms;

		public EditorGridContextMenu([NotNull] EditorCanvas editor)
		{
			if (editor == null) throw new ArgumentNullException("editor");
			mEditor = editor;

			var items = new []
			{
				mUndoButton = new ToolStripButton("Undo", null, OnUndoClick) { Enabled = editor.Commands.CanUndo() },
				mRedoButton = new ToolStripButton("Redo", null, OnRedoClick) { Enabled = editor.Commands.CanRedo() },

				new ToolStripSeparator(),

				new ToolStripButton("New transform", null, OnNewTransformClick),

				new ToolStripSeparator(),

				mZoomAutomatically = new ToolStripButton("Zoom automatically", null, OnZoomAutomaticallyClick) { Checked = editor.Settings.ZoomAutomatically },
				mShowVariationPreview = new ToolStripButton("Show variation preview", null, OnShowVariationPreviewClick) { Checked = editor.Settings.ShowVariationPreview },

				new ToolStripSeparator(),

				mLockTransformAxes = new ToolStripButton("Lock transform axes", null, OnLockTransformAxesClick) { Checked = editor.Settings.LockTransformAxes },
				mEditPostTransforms = new ToolStripButton("Enable / edit post-transform", null, OnTogglePostTransformClick) { Checked = editor.Settings.EditPostTransforms },

				new ToolStripSeparator(),

				new ToolStripButton("Flip all vertically", null, OnFlipAllVerticallyClick),
				new ToolStripButton("Flip all horizontally", null, OnFlipAllHorizontallyClick)
			};

			Items.AddRange(items);
		}
		protected override void Dispose(bool disposing)
		{
			mEditor = null;

			mUndoButton = null;
			mRedoButton = null;

			mZoomAutomatically = null;
			mShowVariationPreview = null;
			mLockTransformAxes = null;
			mEditPostTransforms = null;

			Items.Clear();
			base.Dispose(disposing);
		}

		private void OnUndoClick(object sender, EventArgs e)
		{
			if (!mEditor.Commands.CanUndo())
				return;

			mEditor.Commands.Undo();

			mUndoButton.Enabled = (mEditor.Commands.CanUndo());
			mRedoButton.Enabled = (mEditor.Commands.CanRedo());
		}
		private void OnRedoClick(object sender, EventArgs e)
		{
			if (!mEditor.Commands.CanRedo())
				return;

			mEditor.Commands.Redo();

			mUndoButton.Enabled = (mEditor.Commands.CanUndo());
			mRedoButton.Enabled = (mEditor.Commands.CanRedo());
		}

		private void OnNewTransformClick(object sender, EventArgs e)
		{
			mEditor.Commands.NewTransform();
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

		private void OnLockTransformAxesClick(object sender, EventArgs e)
		{
			var item = sender as ToolStripButton;
			if (item == null)
				return;

			item.Checked = (mEditor.Settings.LockTransformAxes = !mEditor.Settings.LockTransformAxes);
			mEditor.Refresh();
		}
		private void OnTogglePostTransformClick(object sender, EventArgs e)
		{
			var item = sender as ToolStripButton;
			if (item == null)
				return;

			item.Checked = (mEditor.Settings.EditPostTransforms = !mEditor.Settings.EditPostTransforms);
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

			if (mLockTransformAxes != null)
			{
				mLockTransformAxes.Checked = editorSettings != null && editorSettings.LockTransformAxes;
			}

			if (mEditPostTransforms != null)
			{
				mEditPostTransforms.Checked = editorSettings != null && editorSettings.EditPostTransforms;
			}
		}
	}
}
