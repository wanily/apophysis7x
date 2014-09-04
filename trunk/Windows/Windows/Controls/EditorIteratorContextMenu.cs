using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Properties;

namespace Xyrus.Apophysis.Windows.Controls
{
	sealed class EditorIteratorContextMenu : ContextMenuStrip
	{
		private EditorCanvas mEditor;
		private Iterator mIterator;

		private ToolStripButton mRemoveIterator;

		public EditorIteratorContextMenu([NotNull] EditorCanvas editor)
		{
			if (editor == null) throw new ArgumentNullException("editor");

			mEditor = editor;

			var items = new ToolStripItem[]
			{
				new ToolStripButton("Reset transform", Resources.ResetIterator, OnResetIteratorClick) { ImageTransparentColor = Color.Fuchsia },

				new ToolStripSeparator(),

				new ToolStripButton("Reset position", Resources.ResetIteratorOrigin, OnResetOriginClick) { ImageTransparentColor = Color.Fuchsia },
				new ToolStripButton("Reset angle", Resources.ResetIteratorAngle, OnResetRotationClick) { ImageTransparentColor = Color.Fuchsia },
				new ToolStripButton("Reset scale", Resources.ResetIteratorScale, OnResetScaleClick) { ImageTransparentColor = Color.Fuchsia },

				new ToolStripSeparator(),

				new ToolStripButton("Duplicate transform", Resources.DuplicateIterator, OnDuplicateIteratorClick) { ImageTransparentColor = Color.Fuchsia },
				mRemoveIterator = new ToolStripButton("Remove transform", Resources.RemoveIterator, OnRemoveIteratorClick) { ImageTransparentColor = Color.Fuchsia },

				new ToolStripSeparator(),

				new ToolStripButton("Rotate 90° counter-clockwise", Resources.Rotate90CounterClockwise, OnRotate90CcwClick) { ImageTransparentColor = Color.Fuchsia },
				new ToolStripButton("Rotate 90° clockwise", Resources.Rotate90Clockwise, OnRotate90CwClick) { ImageTransparentColor = Color.Fuchsia },
				new ToolStripButton("Flip vertically", Resources.FlipAllVertical, OnFlipVerticallyClick) { ImageTransparentColor = Color.Fuchsia },
				new ToolStripButton("Flip horizontally", Resources.FlipAllHorizontal, OnFlipHorizontallyClick) { ImageTransparentColor = Color.Fuchsia }
			};

			Items.AddRange(items);
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			mRemoveIterator = null;
			mIterator = null;
			mEditor = null;
		}

		public Iterator Iterator
		{
			get { return mIterator; }
			set
			{
				mIterator = value;
				CanRemoveIterator = value != null && mEditor.Iterators.CanRemove(value.GroupIndex);
			}
		}

		public bool CanRemoveIterator
		{
			get { return mRemoveIterator.Enabled; }
			set { mRemoveIterator.Enabled = value; }
		}

		private void OnDuplicateIteratorClick(object sender, EventArgs e)
		{
			if (mIterator == null)
				return;

			mEditor.Commands.DuplicateIterator(mIterator);
		}
		private void OnRemoveIteratorClick(object sender, EventArgs e)
		{
			if (mIterator == null)
				return;

			mEditor.Commands.RemoveIterator(mIterator);
		}

		private void OnRotate90CcwClick(object sender, EventArgs e)
		{
			if (mIterator == null)
				return;

			mEditor.Commands.RotateIterator(mIterator, System.Math.PI / 2.0);
		}
		private void OnRotate90CwClick(object sender, EventArgs e)
		{
			if (mIterator == null)
				return;

			mEditor.Commands.RotateIterator(mIterator, -System.Math.PI / 2.0);
		}
		private void OnFlipVerticallyClick(object sender, EventArgs e)
		{
			if (mIterator == null)
				return;

			mEditor.Commands.FlipVertically(mIterator);
		}
		private void OnFlipHorizontallyClick(object sender, EventArgs e)
		{
			if (mIterator == null)
				return;

			mEditor.Commands.FlipHorizontally(mIterator);
		}

		private void OnResetIteratorClick(object sender, EventArgs e)
		{
			if (mIterator == null)
				return;

			mEditor.Commands.ResetIterator(mIterator);
		}
		private void OnResetOriginClick(object sender, EventArgs e)
		{
			if (mIterator == null)
				return;

			mEditor.Commands.ResetIteratorOrigin(mIterator);
		}
		private void OnResetRotationClick(object sender, EventArgs e)
		{
			if (mIterator == null)
				return;

			mEditor.Commands.ResetIteratorAngle(mIterator);
		}
		private void OnResetScaleClick(object sender, EventArgs e)
		{
			if (mIterator == null)
				return;

			mEditor.Commands.ResetIteratorScale(mIterator);
		}
	}
}