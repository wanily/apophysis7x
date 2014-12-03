using System;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
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

		private Iterator GetLastIteratorOfGroup(int groupIndex)
		{
			return mEditor.Iterators.Last(x => Equals(x.GroupIndex, groupIndex));
		}
		private void CorrectZoomIfSettingEnabled()
		{
			if (mEditor.Settings.ZoomAutomatically)
			{
				mEditor.ZoomOptimally();
			}
		}

		public void NewIterator()
		{
			mEditor.RaiseBeginEdit();
			mEditor.Iterators.Add();

			mEditor.SelectedIterator = GetLastIteratorOfGroup(0);

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();
			mEditor.RaiseSelectionChanged();

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
		}
		public void NewFinalIterator()
		{
			mEditor.RaiseBeginEdit();
			mEditor.Iterators.Add(1);

			mEditor.SelectedIterator = GetLastIteratorOfGroup(1);

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();
			mEditor.RaiseSelectionChanged();

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
		}
		public void DuplicateIterator([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();
			mEditor.Iterators.Add(iterator.Copy());

			mEditor.SelectedIterator = GetLastIteratorOfGroup(iterator.GroupIndex);

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();
			mEditor.RaiseSelectionChanged();

			CorrectZoomIfSettingEnabled();
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
			if (!mEditor.Iterators.CanRemove(iterator.GroupIndex))
				return;

			var index = iterator.GroupItemIndex;
			var groupIndex = iterator.GroupIndex;

			mEditor.RaiseBeginEdit();
			mEditor.Iterators.Remove(iterator);

			var groupItems = mEditor.Iterators.Where(x => Equals(groupIndex, x.GroupIndex)).ToArray();
			if (!groupItems.Any())
			{
				var newGroupIndex = groupIndex - 1;
				mEditor.SelectedIterator = newGroupIndex < 0 
					? mEditor.Iterators.Last() 
					: GetLastIteratorOfGroup(newGroupIndex);
			}
			else
			{
				var maxItemIndex = groupItems.Max(x => x.GroupItemIndex);
				var newItemIndex = System.Math.Max(0, System.Math.Min(index, maxItemIndex));

				mEditor.SelectedIterator = mEditor.Iterators.First(x => Equals(x.GroupIndex, groupIndex) && Equals(x.GroupItemIndex, newItemIndex));
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();
			mEditor.RaiseSelectionChanged();

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
		}
		public void RemoveSelectedIterator()
		{
			if (mEditor.SelectedIterator == null || !mEditor.Iterators.CanRemove(mEditor.SelectedIterator.GroupIndex))
				return;

			RemoveIterator(mEditor.SelectedIterator);
		}
		public void ConvertIterator([NotNull] Iterator iterator, int groupIndex)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();

			var result = iterator.Convert(groupIndex);
			mEditor.SelectedIterator = result;

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();
			mEditor.RaiseSelectionChanged();

			mEditor.Refresh();
		}

		public void ConvertSelectedIterator(int groupIndex)
		{
			if (mEditor.SelectedIterator == null)
				return;

			ConvertIterator(mEditor.SelectedIterator, groupIndex);
		}

		public void FlipAllHorizontally()
		{
			Func<Iterator, Vector2> getOrigin;

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					getOrigin = t => new Vector2(t.PreAffine.M31, t.PreAffine.M32);
					break;
				case IteratorMatrix.PostAffine:
					getOrigin = t => new Vector2(t.PostAffine.M31, t.PostAffine.M32);
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

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
		}
		public void FlipHorizontally([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					iterator.PreAffine = iterator.PreAffine.Alter(iterator.PreAffine.M11 * -1, m21: iterator.PreAffine.M21 * -1);
					break;
				case IteratorMatrix.PostAffine:
					iterator.PostAffine = iterator.PostAffine.Alter(iterator.PostAffine.M11 * -1, m21: iterator.PostAffine.M21 * -1);
					break;
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
		}
		public void FlipAllVertically()
		{
			Func<Iterator, Vector2> getOrigin;

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					getOrigin = t => new Vector2(t.PreAffine.M31, t.PreAffine.M32);
					break;
				case IteratorMatrix.PostAffine:
					getOrigin = t => new Vector2(t.PostAffine.M31, t.PostAffine.M32);
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

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
		}
		public void FlipVertically([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					iterator.PreAffine = iterator.PreAffine.Alter(m12: iterator.PreAffine.M12 * -1, m22: iterator.PreAffine.M22 * -1);
					break;
				case IteratorMatrix.PostAffine:
					iterator.PostAffine = iterator.PostAffine.Alter(m12: iterator.PostAffine.M12 * -1, m22: iterator.PostAffine.M22 * -1);
					break;
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
		}

		public void RotateIterator([NotNull] Iterator iterator, float angle)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					iterator.PreAffine = iterator.PreAffine.Rotate(angle);
					break;
				case IteratorMatrix.PostAffine:
					iterator.PostAffine = iterator.PostAffine.Rotate(angle);
					break;
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
		}
		public void RotateSelectedIterator(float angle)
		{
			if (mEditor.SelectedIterator == null)
				return;

			RotateIterator(mEditor.SelectedIterator, angle);
		}
		public void ScaleIterator([NotNull] Iterator iterator, float ratio)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					iterator.PreAffine = iterator.PreAffine.Scale(ratio);
					break;
				case IteratorMatrix.PostAffine:
					iterator.PostAffine = iterator.PostAffine.Scale(ratio);
					break;
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
		}
		public void ScaleSelectedIterator(float ratio)
		{
			if (mEditor.SelectedIterator == null)
				return;

			ScaleIterator(mEditor.SelectedIterator, ratio);
		}
		public void MoveIterator([NotNull] Iterator iterator, Vector2 offset)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					iterator.PreAffine = iterator.PreAffine.Move(offset);
					break;
				case IteratorMatrix.PostAffine:
					iterator.PostAffine = iterator.PostAffine.Move(offset);
					break;
			}
			
			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
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

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
		}

		public void ResetIterator([NotNull] Iterator iterator)
		{
			if (iterator == null) throw new ArgumentNullException("iterator");

			mEditor.RaiseBeginEdit();

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					iterator.PreAffine = new Matrix3x2(1, 0, 0, 1, 0, 0);
					break;
				case IteratorMatrix.PostAffine:
					iterator.PostAffine = new Matrix3x2(1, 0, 0, 1, 0, 0);
					break;
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			CorrectZoomIfSettingEnabled();
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

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					iterator.PreAffine = iterator.PreAffine.Alter(m31: 0, m32: 0);
					break;
				case IteratorMatrix.PostAffine:
					iterator.PostAffine = iterator.PostAffine.Alter(m31: 0, m32: 0);
					break;
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			CorrectZoomIfSettingEnabled();
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
			var @mx = new Vector2(matrix.M11, matrix.M12);
			var @my = new Vector2(matrix.M21, matrix.M22);


			var scale = new Vector2(@mx.Length(), @my.Length());

			var handle = @mx;
			var axis = new Vector2(1, 0);

			var delta = Float.Atan2(@mx.Y, @mx.X) - Float.Atan2(@my.Y, @my.X);
			var angle = Float.Atan2(handle.Y, handle.X) - Float.Atan2(axis.Y, axis.X) * 180.0f / Float.Pi;
			var newAngle = Float.Round(angle / 90) * Float.Pi / 2.0f;

			var x = axis.Rotate(newAngle) * scale.X;
			var y = x.Normal().Rotate(-delta) * scale.Y;

			matrix = matrix.Alter(x.X, x.Y, y.X, y.Y);

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					iterator.PreAffine = matrix;
					break;
				case IteratorMatrix.PostAffine:
					iterator.PostAffine = matrix;
					break;
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			CorrectZoomIfSettingEnabled();
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

			var l0 = new Vector2(matrix.M11, matrix.M12).Length();
			var l1 = new Vector2(matrix.M21, matrix.M22).Length();

			matrix = new Matrix3x2(matrix.M11 / l0, matrix.M12 / l0, matrix.M21 / l1, matrix.M22 / l1, matrix.M31, matrix.M32);

			switch (mEditor.ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					iterator.PreAffine = matrix;
					break;
				case IteratorMatrix.PostAffine:
					iterator.PostAffine = matrix;
					break;
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
		}
		public void ResetSelectedIteratorScale()
		{
			if (mEditor.SelectedIterator == null)
				return;

			ResetIteratorScale(mEditor.SelectedIterator);
		}

		public void RotateWorld(float angle)
		{
			var origin = new Vector2(0, 0);

			mEditor.RaiseBeginEdit();

			foreach (var iterator in mEditor.Iterators)
			{
				var matrices = new[] {iterator.PreAffine, iterator.PostAffine};

				for (var i = 0; i < matrices.Length; i++)
				{
					var matrix = matrices[i];

					var o = new Vector2(matrix.M31, matrix.M32);
					var x = new Vector2(matrix.M11, matrix.M12) + o;
					var y = new Vector2(matrix.M21, matrix.M22) + o;

					o = o.Rotate(angle, origin);
					x = x.Rotate(angle, origin);
					y = y.Rotate(angle, origin);

					matrices[i] = new Matrix3x2(
						Float.Round(x.X - o.X, 6), Float.Round(x.Y - o.Y, 6), Float.Round(y.X - o.X, 6), 
						Float.Round(y.Y - o.Y, 6), Float.Round(o.X, 6), Float.Round(o.Y, 6));
				}

				iterator.PreAffine = matrices[0];
				iterator.PostAffine = matrices[1];
			}

			mEditor.RaiseEdit();
			mEditor.RaiseEndEdit();

			CorrectZoomIfSettingEnabled();
			mEditor.Refresh();
		}
	}
}