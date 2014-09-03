using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Visuals;

namespace Xyrus.Apophysis.Windows.Input
{
	class IteratorCollectionInputHandler : InputHandler, IEnumerable<IteratorInputHandler>
	{
		private EventHandler mBeginEdit;
		private EventHandler mEndEdit;
		private EventHandler mSelectionChanged;
		private EventHandler mEdit;

		private EditorSettings mSettings;
		private EditorSettings mDefaultSettings;

		private IteratorCollectionVisual mVisualCollection;
		private readonly List<IteratorInputHandler> mHandlers;
		private IteratorInputHandler mFinalHandler;
		private Canvas mCanvas;
		private IteratorMatrix mActiveMatrix;

		public IteratorCollectionInputHandler([NotNull] Control control, [NotNull] IteratorCollectionVisual visualCollection, [NotNull] Canvas canvas) : base(control)
		{
			if (visualCollection == null) throw new ArgumentNullException("visualCollection");
			if (canvas == null) throw new ArgumentNullException("canvas");

			mVisualCollection = visualCollection;
			mCanvas = canvas;

			mDefaultSettings = new EditorSettings { MoveAmount = 0.5, AngleSnap = 15, ScaleSnap = 125 };
			mSettings = mDefaultSettings;

			mVisualCollection.ContentChanged += OnCollectionChanged;
			mVisualCollection.FinalChanged += OnFinalChanged;

			mHandlers = new List<IteratorInputHandler>();
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (mVisualCollection != null)
			{
				mVisualCollection.ContentChanged -= OnCollectionChanged;
				mVisualCollection.FinalChanged -= OnFinalChanged;

				mVisualCollection = null;
			}

			mDefaultSettings = null;
			mSettings = null;
			mCanvas = null;
		}

		public IteratorMatrix ActiveMatrix
		{
			get { return mActiveMatrix; }
			set
			{
				mActiveMatrix = value;
				mVisualCollection.ActiveMatrix = value;

				if (mFinalHandler != null)
				{
					mFinalHandler.ActiveMatrix = value;
				}

				if (mSettings.ZoomAutomatically)
				{
					ZoomOptimally();
					InvalidateControl();
				}
			}
		}
		public Iterator SelectedIterator
		{
			get { return GetSelectedIterator(); }
			set
			{
				foreach (var visual in mVisualCollection)
				{
					visual.IsSelected = false;
				}

				if (mVisualCollection.FinalVisual != null)
				{
					mVisualCollection.FinalVisual.IsSelected = false;
				}

				var newVisual = mVisualCollection.FirstOrDefault(x => ReferenceEquals(x.Model, value));

				if (newVisual != null)
				{
					newVisual.IsSelected = true;
				}
				else
				{
					if (mVisualCollection.Collection.UseFinalIterator && mVisualCollection.FinalVisual != null && ReferenceEquals(value, mVisualCollection.FinalVisual.Model))
					{
						mVisualCollection.FinalVisual.IsSelected = true;
					}
				}

				InvalidateControl();
			}
		}

		[NotNull]
		public EditorSettings Settings
		{
			get { return mSettings; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mSettings = value;
			}
		}

		private void OnFinalChanged(object sender, EventArgs eventArgs)
		{
			if (mVisualCollection == null)
				return;

			mFinalHandler = mVisualCollection.FinalVisual == null
				? null
				: new IteratorInputHandler(AttachedControl, mVisualCollection.FinalVisual, mCanvas, mSettings ?? mDefaultSettings, mActiveMatrix);
		}
		private void OnCollectionChanged(object sender, EventArgs eventArgs)
		{
			if (mHandlers == null || mVisualCollection == null)
				return;

			foreach (var handler in mHandlers)
			{
				handler.Dispose();
			}

			mHandlers.Clear();

			foreach (var visual in mVisualCollection.Reverse())
			{
				mHandlers.Add(new IteratorInputHandler(AttachedControl, visual, mCanvas, mSettings ?? mDefaultSettings, mActiveMatrix));
				visual.IsSelected = false;
			}

			mVisualCollection.First().IsSelected = true;

			InvalidateControl();
		}
		private void SetOperation(IteratorInputHandler handler)
		{
			if (handler == null)
			{
				mVisualCollection.CurrentOperation = null;
				return;
			}

			mVisualCollection.CurrentOperation = handler.GetCurrentOperation();
		}

		protected override bool OnAttachedControlKeyPress(Keys key, Keys modifiers)
		{
			if (mHandlers == null || mVisualCollection == null)
				return false;

			if (key == Keys.Home)
			{
				ZoomOptimally();
				InvalidateControl();
				return true;
			}

			if (key == Keys.PageDown || key == Keys.PageUp)
			{
				var selected = mVisualCollection.Where(x => x.IsSelected).OrderByDescending(x => x.Model.Index).FirstOrDefault();
				if (selected == null)
				{
					if (mVisualCollection.FinalVisual != null && mVisualCollection.Collection.UseFinalIterator &&
					    mVisualCollection.FinalVisual.IsSelected)
					{
						selected = mVisualCollection.FinalVisual;
					}
					else
					{
						mVisualCollection.First().IsSelected = true;
						InvalidateControl();
						return true;
					}
				}

				var indexDictionary = mVisualCollection.Select(x => x).ToDictionary(x => x.Model.Index);
				var newIndex = (selected.Model.Index < 0 ? indexDictionary.Keys.Max() + 1 : selected.Model.Index) + (key == Keys.PageDown ? 1 : -1);

				if (mVisualCollection.FinalVisual != null && mVisualCollection.Collection.UseFinalIterator)
				{
					indexDictionary.Add(indexDictionary.Keys.Max() + 1, mVisualCollection.FinalVisual);
				}

				if (indexDictionary.ContainsKey(newIndex))
				{
					foreach (var visual in mVisualCollection)
					{
						visual.IsSelected = false;
					}

					var newVisual = indexDictionary[newIndex];
					newVisual.IsSelected = true;

					if (mSelectionChanged != null)
						mSelectionChanged(this, new EventArgs());

					InvalidateControl();
					return true;
				}
			}

			RaiseBeginEdit();
			if (mFinalHandler != null)
			{
				mFinalHandler.HandleKeyPress(key, modifiers);
			}

			foreach (var handler in mHandlers)
			{
				if (handler.HandleKeyPress(key, modifiers))
				{
					RaiseEdit();
					return true;
				}
			}
			RaiseEndEdit();

			return false;
		}

		protected override bool OnAttachedControlMouseMove(Vector2 cursor, MouseButtons button)
		{
			if (mHandlers == null)
				return false;

			mVisualCollection.CursorPosition = mCanvas.CanvasToWorld(cursor);

			if (mFinalHandler != null && mVisualCollection.Collection.UseFinalIterator && mFinalHandler.IsDragging)
			{
				if (mFinalHandler.HandleMouseMove(cursor, button))
				{
					SetOperation(mFinalHandler);
					if (mEdit != null)
					{
						mEdit(this, new EventArgs());
					}
					return true;
				}

				return true;
			}

			if (mHandlers.Any(x => x.IsDragging))
			{
				foreach (var handler in mHandlers.Where(x => x.IsDragging))
				{
					if (handler.HandleMouseMove(cursor, button))
					{
						SetOperation(handler);
						if (mEdit != null)
						{
							mEdit(this, new EventArgs());
						}
						return true;
					}
				}

				return true;
			}

			if (mFinalHandler != null)
			{
				mFinalHandler.InvalidateHitTest();
			}

			foreach (var handler in mHandlers)
			{
				handler.InvalidateHitTest();
			}

			if (mFinalHandler != null && mVisualCollection.Collection.UseFinalIterator)
			{
				if (mFinalHandler.HandleMouseMove(cursor, button))
				{
					mVisualCollection.CurrentOperation = new MouseOverOperation(mFinalHandler.Model);
					return true;
				}
			}

			foreach (var handler in mHandlers)
			{
				if (handler.HandleMouseMove(cursor, button))
				{
					mVisualCollection.CurrentOperation = (new MouseOverOperation(handler.Model));
					return true;
				}
			}

			mVisualCollection.CurrentOperation = null;
			return false;
		}
		protected override bool OnAttachedControlMouseWheel(double delta, MouseButtons button)
		{
			if (mHandlers == null)
				return false;

			if (mFinalHandler != null && mVisualCollection.Collection.UseFinalIterator)
			{
				if (mFinalHandler.HandleMouseWheel(delta, button))
				{
					return true;
				}
			}

			foreach (var handler in mHandlers)
			{
				if (handler.HandleMouseWheel(delta, button))
					return true;
			}

			return false;
		}

		protected override bool OnAttachedControlMouseDown(Vector2 cursor)
		{
			if (mHandlers == null)
				return false;

			var prepend = new List<IteratorInputHandler>();

			if (mFinalHandler != null && mVisualCollection.Collection.UseFinalIterator)
			{
				prepend.Add(mFinalHandler);
			}

			foreach (var handler in prepend.Concat(mHandlers))
			{
				if (handler.HandleMouseDown(cursor))
				{
					var searchHandler = handler;
					foreach (var visual in mVisualCollection.Except(mVisualCollection.Where(x => ReferenceEquals(x.Model, searchHandler.Model))))
					{
						visual.IsSelected = false;
					}

					RaiseBeginEdit();
					if (mSelectionChanged != null)
						mSelectionChanged(this, new EventArgs());

					SetOperation(handler);
					InvalidateControl();
					return true;
				}
			}

			return false;
		}
		protected override bool OnAttachedControlMouseUp()
		{
			if (mHandlers == null)
				return false;

			SetOperation(null);
			RaiseEndEdit();

			if (mFinalHandler != null && mVisualCollection.Collection.UseFinalIterator)
			{
				if (mFinalHandler.HandleMouseUp())
				{
					return true;
				}
			}

			foreach (var handler in mHandlers)
			{
				if (handler.HandleMouseUp())
				{
					InvalidateControl();
					return true;
				}
			}

			return false;
		}

		protected override bool OnAttachedControlMouseDoubleClick()
		{
			if (mHandlers == null)
				return false;

			if (mFinalHandler != null && mVisualCollection.Collection.UseFinalIterator)
			{
				if (mFinalHandler.HandleMouseDoubleClick())
				{
					return true;
				}
			}

			foreach (var handler in mHandlers)
			{
				if (handler.HandleMouseDoubleClick())
					return true;
			}

			ZoomOptimally();
			return true;
		}

		protected void RaiseBeginEdit()
		{
			if (mBeginEdit != null)
				mBeginEdit(this, new EventArgs());
		}
		protected void RaiseEndEdit()
		{
			if (mEndEdit != null)
				mEndEdit(this, new EventArgs());
		}
		protected void RaiseEdit()
		{
			if (mEdit != null)
				mEdit(this, new EventArgs());
		}

		public event EventHandler BeginEdit
		{
			add { mBeginEdit += value; }
			remove { mBeginEdit -= value; }
		}
		public event EventHandler EndEdit
		{
			add { mEndEdit += value; }
			remove { mEndEdit -= value; }
		}
		public event EventHandler Edit
		{
			add { mEdit += value; }
			remove { mEdit -= value; }
		}

		public event EventHandler SelectionChanged
		{
			add { mSelectionChanged += value; }
			remove { mSelectionChanged -= value; }
		}

		public IEnumerator<IteratorInputHandler> GetEnumerator()
		{
			if (mHandlers == null)
				return new List<IteratorInputHandler>().GetEnumerator();

			return mHandlers.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public Iterator HitTestIterator(Vector2 cursor)
		{
			if (mFinalHandler != null)
			{
				if (mFinalHandler.PerformHitTest(cursor))
				{
					return mFinalHandler.Model;
				}
			}

			foreach (var handler in mHandlers)
			{
				if (handler.PerformHitTest(cursor))
					return handler.Model;
			}

			return null;
		}
		public Iterator GetSelectedIterator()
		{
			var visual = mVisualCollection == null ? null : mVisualCollection.FirstOrDefault(x => x.IsSelected);
			if (visual == null)
			{
				if (mVisualCollection != null && mVisualCollection.FinalVisual != null)
				{
					if (mVisualCollection.FinalVisual.IsSelected)
					{
						return mVisualCollection.FinalVisual.Model;
					}
				}
				return null;
			}

			return visual.Model;
		}
		public void ZoomOptimally()
		{
			var bounds = mVisualCollection
				.Concat(mVisualCollection.FinalVisual != null && mVisualCollection.Collection.UseFinalIterator 
					? new[] { mVisualCollection.FinalVisual } 
					: new IteratorVisual[0])
				.Select(x => x.GetBounds()).ToList();

			var corner1 = new Vector2(bounds.Select(x => x.TopLeft.X).Min(), bounds.Select(x => x.TopLeft.Y).Min());
			var corner2 = new Vector2(bounds.Select(x => x.BottomRight.X).Max(), bounds.Select(x => x.BottomRight.Y).Max());

			var rectangle = new Rectangle(corner1, corner2 - corner1);

			mCanvas.BringIntoView(rectangle);
		}
	}
}