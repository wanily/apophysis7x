using System;
using System.ComponentModel;

namespace Xyrus.Apophysis.Windows.Controls
{
	[PublicAPI]
	public class EditorCommands : Component
	{
		private EditorCanvas mEditor;

		public EditorCommands([NotNull] EditorCanvas editor)
		{
			if (editor == null) throw new ArgumentNullException("editor");
			mEditor = editor;
		}
		protected override void Dispose(bool disposing)
		{
			mEditor = null;
			base.Dispose(disposing);
		}

		public bool CanUndo()
		{
			//todo
			return false;
		}
		public bool CanRedo()
		{
			//todo
			return false;
		}

		public void Undo() /*todo*/
		{
			throw new NotImplementedException();
		}
		public void Redo() /*todo*/
		{
			throw new NotImplementedException();
		}

		public void NewTransform()
		{
			mEditor.Transforms.Add();

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.Refresh();
		}

		public void FlipAllHorizontally()
		{
			foreach (var transform in mEditor.Transforms)
			{
				transform.Origin.X = -transform.Origin.X;
			}

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.Refresh();
		}
		public void FlipAllVertically()
		{
			foreach (var transform in mEditor.Transforms)
			{
				transform.Origin.Y = -transform.Origin.Y;
			}

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.Refresh();
		}
	}
}