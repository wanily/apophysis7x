using System;
using System.ComponentModel;
using System.Linq;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;

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
			//todo

			if (UndoRedoCommitted != null)
				UndoRedoCommitted(this, new EventArgs());
		}
		public void Redo() /*todo*/
		{
			//todo

			if (UndoRedoCommitted != null)
				UndoRedoCommitted(this, new EventArgs());
		}

		public void NewIterator()
		{
			mEditor.Iterators.Add();

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.SelectedIterator = mEditor.Iterators.Last();
			mEditor.RaiseSelectionChanged();

			mEditor.Refresh();
		}
		public void DuplicateSelectedIterator()
		{
			if (mEditor.SelectedIterator == null)
				return;

			mEditor.Iterators.Add(mEditor.SelectedIterator.Copy());

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.SelectedIterator = mEditor.Iterators.Last();
			mEditor.RaiseSelectionChanged();

			mEditor.Refresh();
		}
		public void RemoveSelectedIterator()
		{
			if (mEditor.SelectedIterator == null || mEditor.Iterators.Count <= 1)
				return;

			var index = mEditor.Iterators.IndexOf(mEditor.SelectedIterator);
			mEditor.Iterators.Remove(mEditor.SelectedIterator);

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.SelectedIterator = mEditor.Iterators[System.Math.Max(0, System.Math.Min(index, mEditor.Iterators.Count - 1))];
			mEditor.RaiseSelectionChanged();

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

		public void RotateSelected(double angle)
		{
			if (mEditor.SelectedIterator == null)
				return;

			var matrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine
				? mEditor.SelectedIterator.PostAffine
				: mEditor.SelectedIterator.PreAffine;

			mEditor.RaiseBeginEdit();
			
			matrix.Rotate(angle);
			
			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			mEditor.Refresh();
		}
		public void ScaleSelected(double ratio)
		{
			if (mEditor.SelectedIterator == null)
				return;

			var matrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine
				? mEditor.SelectedIterator.PostAffine
				: mEditor.SelectedIterator.PreAffine;

			mEditor.RaiseBeginEdit();

			matrix.Scale(ratio);

			mEditor.Refresh();
			mEditor.RaiseEndEdit();
		}
		public void MoveSelected(Vector2 offset)
		{
			if (mEditor.SelectedIterator == null)
				return;

			var matrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine
				? mEditor.SelectedIterator.PostAffine
				: mEditor.SelectedIterator.PreAffine;

			mEditor.RaiseBeginEdit();

			matrix.Move(offset);

			mEditor.Refresh();
			mEditor.RaiseEndEdit();
		}

		public event EventHandler UndoRedoCommitted;

		
	}
}