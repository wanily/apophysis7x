using System;
using System.ComponentModel;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Windows.Models;

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

		public void NewIterator()
		{
			mEditor.Iterators.Add();

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.Refresh();
		}

		public void FlipAllHorizontally()
		{
			Func<Iterator, Vector2> getOrigin;

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					getOrigin = t => t.PreAffine.Origin;
					break;
				case IteratorMatrix.PostAffine:
					getOrigin = t => t.PostAffine.Origin;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			foreach (var iterator in mEditor.Iterators)
			{
				var origin = getOrigin(iterator);
				origin.X = -origin.X;
			}

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.Refresh();
		}
		public void FlipAllVertically()
		{
			Func<Iterator, Vector2> getOrigin;

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					getOrigin = t => t.PreAffine.Origin;
					break;
				case IteratorMatrix.PostAffine:
					getOrigin = t => t.PostAffine.Origin;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			foreach (var iterator in mEditor.Iterators)
			{
				var origin = getOrigin(iterator);
				origin.Y = -origin.Y;
			}

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.Refresh();
		}
	}
}