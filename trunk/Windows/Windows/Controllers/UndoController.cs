using System;
using System.Collections.Generic;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class UndoController : Controller
	{
		private Stack<Flame> mUndoStack;
		private Stack<Flame> mRedoStack;

		private Flame mCurrent;
		private bool mInitialized;

		protected override void Dispose(bool disposing)
		{
			if (mUndoStack != null)
			{
				mUndoStack.Clear();
				mUndoStack = null;
			}

			if (mRedoStack != null)
			{
				mRedoStack.Clear();
				mRedoStack = null;
			}

			mCurrent = null;
		}

		public void Initialize([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			if (mInitialized)
				return;

			mCurrent = flame.Copy();
			mUndoStack = new Stack<Flame>();
			mRedoStack = new Stack<Flame>();
			mInitialized = true;
		}
		public void CommitChange([NotNull] Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			if (flame.IsEqual(mCurrent))
				return;
			
			var old = mCurrent.Copy();

			mCurrent = flame.Copy();
			mUndoStack.Push(old);
			mRedoStack.Clear();
			RaiseStackChanged();
		}

		public bool CanUndo
		{
			get { return mUndoStack != null && mUndoStack.Count >= 1; }
		}
		public bool CanRedo
		{
			get { return mRedoStack != null && mRedoStack.Count >= 1; }
		}

		public Flame Undo()
		{
			if (!CanUndo)
			{
				throw new InvalidOperationException();
			}

			var pop = mUndoStack.Pop();
			mRedoStack.Push(mCurrent);
			mCurrent = pop.Copy();
			RaiseStackChanged();
			return pop;
		}
		public Flame Redo()
		{
			if (!CanRedo)
			{
				throw new InvalidOperationException();
			}

			var pop = mRedoStack.Pop();
			mUndoStack.Push(mCurrent);
			mCurrent = pop.Copy();
			RaiseStackChanged();
			return pop;
		}

		private void RaiseStackChanged()
		{
			if (StackChanged != null)
				StackChanged(this, new EventArgs());
		}
		public event EventHandler StackChanged;
	}
}