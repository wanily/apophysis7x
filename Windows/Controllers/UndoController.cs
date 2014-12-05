using System;
using System.Collections.Generic;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Interfaces;

namespace Xyrus.Apophysis.Windows.Controllers
{
	[PublicAPI]
	public class UndoController : Controller, IUndoController
	{
		private Stack<Flame> mUndoStack;
		private Stack<Flame> mRedoStack;

		private Flame mCurrent;

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

		public void Reset(Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");

			mCurrent = flame.Copy();
			mUndoStack = new Stack<Flame>();
			mRedoStack = new Stack<Flame>();
		}

		public void CommitChange(Flame flame)
		{
			if (flame == null) throw new ArgumentNullException("flame");
			if (flame.IsEqual(mCurrent))
				return;
			
			var old = mCurrent.Copy();

			mCurrent = flame.Copy();
			mUndoStack.Push(old);
			mRedoStack.Clear();
			RaiseStackChanged();
			RaiseChangeCommitted();
		}

		public bool CanUndo
		{
			get { return mUndoStack != null && mUndoStack.Count >= 1; }
		}
		public bool CanRedo
		{
			get { return mRedoStack != null && mRedoStack.Count >= 1; }
		}

		public void Undo()
		{
			if (!CanUndo)
			{
				throw new InvalidOperationException();
			}

			var pop = mUndoStack.Pop();
			mRedoStack.Push(mCurrent);
			mCurrent = pop;//.Copy();
			RaiseStackChanged();
			RaiseCurrentReplaced();
		}
		public void Redo()
		{
			if (!CanRedo)
			{
				throw new InvalidOperationException();
			}

			var pop = mRedoStack.Pop();
			mUndoStack.Push(mCurrent);
			mCurrent = pop;//.Copy();
			RaiseStackChanged();
			RaiseCurrentReplaced();
		}

		private void RaiseStackChanged()
		{
			if (StackChanged != null)
				StackChanged(this, new EventArgs());
		}
		public event EventHandler StackChanged;

		private void RaiseCurrentReplaced()
		{
			if (CurrentReplaced != null)
				CurrentReplaced(this, new EventArgs());
		}
		public event EventHandler CurrentReplaced;

		private void RaiseChangeCommitted()
		{
			if (ChangeCommitted != null)
				ChangeCommitted(this, new EventArgs());
		}
		public event EventHandler ChangeCommitted;

		public Flame RequestCurrent()
		{
			return mCurrent.Copy();
		}
	}
}