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

		public void NewIterator()
		{
			mEditor.RaiseBeginEdit();
			mEditor.Iterators.Add();

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.SelectedIterator = mEditor.Iterators.Last();

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();
			mEditor.RaiseSelectionChanged();

			mEditor.Refresh();
		}
		public void DuplicateIterator([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();
			mEditor.Iterators.Add(iterator.Copy());

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.SelectedIterator = mEditor.Iterators.Last();

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();
			mEditor.RaiseSelectionChanged();

			mEditor.Refresh();
		}
		public void DuplicateSelectedIterator()
		{
			if (mEditor.SelectedIterator == null)
				return;

			DuplicateIterator(mEditor.SelectedIterator);
		}
		public void RemoveIterator([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");
			if (mEditor.Iterators.Count <= 1)
				return;

			var index = mEditor.Iterators.IndexOf(iterator);

			mEditor.RaiseBeginEdit();
			mEditor.Iterators.Remove(iterator);

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.SelectedIterator = mEditor.Iterators[System.Math.Max(0, System.Math.Min(index, mEditor.Iterators.Count - 1))];

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();
			mEditor.RaiseSelectionChanged();

			mEditor.Refresh();
		}
		public void RemoveSelectedIterator()
		{
			if (mEditor.SelectedIterator == null || mEditor.Iterators.Count <= 1)
				return;

			RemoveIterator(mEditor.SelectedIterator);
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

			mEditor.RaiseBeginEdit();

			foreach (var iterator in mEditor.Iterators)
			{
				var origin = getOrigin(iterator);
				origin.X = -origin.X;
			}

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			mEditor.Refresh();
		}
		public void FlipHorizontally([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			var matrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;
			
			mEditor.RaiseBeginEdit();

			matrix.Matrix.X.X *= -1;
			matrix.Matrix.Y.X *= -1;

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

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

			mEditor.RaiseBeginEdit();

			foreach (var iterator in mEditor.Iterators)
			{
				var origin = getOrigin(iterator);
				origin.Y = -origin.Y;
			}

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			mEditor.Refresh();
		}
		public void FlipVertically([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			var matrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;
			
			mEditor.RaiseBeginEdit();

			matrix.Matrix.X.Y *= -1;
			matrix.Matrix.Y.Y *= -1;

			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			mEditor.Refresh();
		}

		public void RotateIterator([NotNull] Iterator iterator, double angle)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");
			var matrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine
				? iterator.PostAffine
				: iterator.PreAffine;

			mEditor.RaiseBeginEdit();

			matrix.Rotate(angle);

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			mEditor.Refresh();
		}
		public void RotateSelectedIterator(double angle)
		{
			if (mEditor.SelectedIterator == null)
				return;

			RotateIterator(mEditor.SelectedIterator, angle);
		}
		public void ScaleIterator([NotNull] Iterator iterator, double ratio)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");
			var matrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine
				? iterator.PostAffine
				: iterator.PreAffine;

			mEditor.RaiseBeginEdit();

			matrix.Scale(ratio);

			mEditor.Refresh();
			mEditor.RaiseEndEdit();
		}
		public void ScaleSelectedIterator(double ratio)
		{
			if (mEditor.SelectedIterator == null)
				return;

			ScaleIterator(mEditor.SelectedIterator, ratio);
		}
		public void MoveIterator([NotNull] Iterator iterator, Vector2 offset)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");
			var matrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine
				? iterator.PostAffine
				: iterator.PreAffine;

			mEditor.RaiseBeginEdit();

			matrix.Move(offset);

			mEditor.Refresh();
			mEditor.RaiseEndEdit();
		}
		public void MoveSelectedIterator(Vector2 offset)
		{
			if (mEditor.SelectedIterator == null)
				return;

			MoveIterator(mEditor.SelectedIterator, offset);
		}

		public void ResetAll()
		{
			mEditor.RaiseBeginEdit();

			mEditor.Iterators.Reset();
			mEditor.SelectedIterator = mEditor.Iterators.First();

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();
			mEditor.RaiseSelectionChanged();

			mEditor.Refresh();
		}

		public void ResetIterator([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();

			var matrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;

			matrix.Origin.X = 0;
			matrix.Origin.Y = 0;
			matrix.Matrix.X.X = 1;
			matrix.Matrix.X.Y = 0;
			matrix.Matrix.Y.X = 0;
			matrix.Matrix.Y.Y = 1;

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			mEditor.Refresh();
		}
		public void ResetSelectedIterator()
		{
			if (mEditor.SelectedIterator == null)
				return;

			ResetIterator(mEditor.SelectedIterator);
		}
		public void ResetIteratorOrigin([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();

			var matrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;

			matrix.Origin.X = 0;
			matrix.Origin.Y = 0;

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			mEditor.Refresh();
		}
		public void ResetSelectedIteratorOrigin()
		{
			if (mEditor.SelectedIterator == null)
				return;

			ResetIteratorOrigin(mEditor.SelectedIterator);
		}
		public void ResetIteratorAngle([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();

			var matrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;
			var scale = new Vector2(matrix.Matrix.X.Length, matrix.Matrix.Y.Length);

			var handle = matrix.Matrix.X;
			var axis = new Vector2(1, 0);

			var angle = System.Math.Atan2(handle.Y, handle.X) - System.Math.Atan2(axis.Y, axis.X) * 180.0 / System.Math.PI;
			var newAngle = System.Math.Round(angle / 90) * System.Math.PI / 2.0;

			var x = axis.Rotate(newAngle, new Vector2()) * scale.X;
			var y = x.Direction.Rotate(System.Math.PI / 2.0, new Vector2()) * scale.Y;

			matrix.Matrix.X.X = x.X;
			matrix.Matrix.X.Y = x.Y;
			matrix.Matrix.Y.X = y.X;
			matrix.Matrix.Y.Y = y.Y;

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			mEditor.Refresh();
		}
		public void ResetSelectedIteratorAngle()
		{
			if (mEditor.SelectedIterator == null)
				return;

			ResetIteratorAngle(mEditor.SelectedIterator);
		}
		public void ResetIteratorScale([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();

			var matrix = mEditor.ActiveMatrix == IteratorMatrix.PostAffine ? iterator.PostAffine : iterator.PreAffine;

			var x = matrix.Matrix.X.Direction;
			var y = matrix.Matrix.Y.Direction;

			matrix.Matrix.X.X = x.X;
			matrix.Matrix.X.Y = x.Y;
			matrix.Matrix.Y.X = y.X;
			matrix.Matrix.Y.Y = y.Y;

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			mEditor.Refresh();
		}
		public void ResetSelectedIteratorScale()
		{
			if (mEditor.SelectedIterator == null)
				return;

			ResetIteratorScale(mEditor.SelectedIterator);
		}
	}
}