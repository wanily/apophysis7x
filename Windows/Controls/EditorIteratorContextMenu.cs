using System;
using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Resources;

namespace Xyrus.Apophysis.Windows.Controls
{
	sealed class EditorIteratorContextMenu : ContextMenuStrip
	{
		private EditorCanvas mEditor;
		private Iterator mIterator;

		private ToolStripButton mRemoveIterator;
		private ToolStripButton mConvertToRegular;
		private ToolStripButton mConvertToFinal;

		public EditorIteratorContextMenu([NotNull] EditorCanvas editor)
		{
			if (editor == null) throw new ArgumentNullException("editor");

			mEditor = editor;

			var items = new ToolStripItem[]
			{
				new ToolStripButton("Reset transform", Icons.ResetIterator, OnResetIteratorClick) { ImageTransparentColor = Color.Fuchsia },

				new ToolStripSeparator(),

				new ToolStripButton("Reset position", Icons.ResetIteratorOrigin, OnResetOriginClick) { ImageTransparentColor = Color.Fuchsia },
				new ToolStripButton("Reset angle", Icons.ResetIteratorAngle, OnResetRotationClick) { ImageTransparentColor = Color.Fuchsia },
				new ToolStripButton("Reset scale", Icons.ResetIteratorScale, OnResetScaleClick) { ImageTransparentColor = Color.Fuchsia },

				new ToolStripSeparator(),

				new ToolStripButton("Duplicate transform", Icons.DuplicateIterator, OnDuplicateIteratorClick) { ImageTransparentColor = Color.Fuchsia },
				mRemoveIterator = new ToolStripButton("Remove transform", Icons.RemoveIterator, OnRemoveIteratorClick) { ImageTransparentColor = Color.Fuchsia },

				new ToolStripSeparator(),

				new ToolStripMenuItem("Convert to", null, new ToolStripItem[]
					{
						mConvertToRegular = new ToolStripButton("Transform", Icons.RegularIterator, OnConvertClick) { ImageTransparentColor = Color.Fuchsia }, 
						mConvertToFinal = new ToolStripButton("Final transform", Icons.FinalIterator, OnConvertClick) { ImageTransparentColor = Color.Fuchsia } 
					}), 

				new ToolStripSeparator(), 

				new ToolStripButton("Rotate 90° counter-clockwise", Icons.Rotate90CounterClockwise, OnRotate90CcwClick) { ImageTransparentColor = Color.Fuchsia },
				new ToolStripButton("Rotate 90° clockwise", Icons.Rotate90Clockwise, OnRotate90CwClick) { ImageTransparentColor = Color.Fuchsia },
				new ToolStripButton("Flip vertically", Icons.FlipAllVertical, OnFlipVerticallyClick) { ImageTransparentColor = Color.Fuchsia },
				new ToolStripButton("Flip horizontally", Icons.FlipAllHorizontal, OnFlipHorizontallyClick) { ImageTransparentColor = Color.Fuchsia }
			};

			Items.AddRange(items);
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			mConvertToFinal = null;
			mConvertToRegular = null;
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
				
				mRemoveIterator.Enabled = value != null && mEditor.Iterators.CanRemove(value.GroupIndex);
				mConvertToRegular.Enabled = value != null && value.GroupIndex != 0;
				mConvertToFinal.Enabled = value != null && value.GroupIndex != 1 && !value.IsSingleInGroup;
			}
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

			mEditor.Commands.RotateIterator(mIterator, Float.Pi / 2);
		}
		private void OnRotate90CwClick(object sender, EventArgs e)
		{
			if (mIterator == null)
				return;

			mEditor.Commands.RotateIterator(mIterator, -Float.Pi / 2);
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
		private void OnConvertClick(object sender, EventArgs e)
		{
			if (mIterator == null)
				return;

			int groupIndex;

			if (ReferenceEquals(sender, mConvertToRegular))
			{
				groupIndex = 0;
			}
			else if (ReferenceEquals(sender, mConvertToFinal))
			{
				groupIndex = 1;
			}
			else
			{
				return;
			}

			mEditor.Commands.ConvertIterator(mIterator, groupIndex);
		}
	}
}