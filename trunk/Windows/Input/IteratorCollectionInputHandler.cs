using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Visuals;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class IteratorCollectionInputHandler : InputHandler, IEnumerable<IteratorInputHandler>
	{
		private EventHandler mBeginEdit;
		private EventHandler mEndEdit;

		private EditorSettings mSettings;
		private EditorSettings mDefaultSettings;

		private IteratorCollectionVisual mVisualCollection;
		private List<IteratorInputHandler> mHandlers;
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

			mHandlers = new List<IteratorInputHandler>();
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (mVisualCollection != null)
			{
				mVisualCollection.ContentChanged -= OnCollectionChanged;

				//disposed somewhere else
				mVisualCollection = null;
			}

			mDefaultSettings = null;
			mSettings = null;
			mCanvas = null;
		}

		public IteratorCollection Collection
		{
			get { return mVisualCollection.Collection; }
		}
		public IteratorInputHandler this[int index]
		{
			get
			{
				if (mHandlers == null)
					throw new IndexOutOfRangeException();

				if (index < 0 || index >= mHandlers.Count)
					throw new IndexOutOfRangeException();

				return mHandlers[index];
			}
		}
		public IteratorMatrix ActiveMatrix
		{
			get { return mActiveMatrix; }
			set
			{
				mActiveMatrix = value;
				mVisualCollection.ActiveMatrix = value;

				if (mSettings.ZoomAutomatically)
				{
					ZoomOptimally();
					InvalidateControl();
				}
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
			}

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
			if (mHandlers == null)
				return false;

			if (key == Keys.Home)
			{
				ZoomOptimally();
				InvalidateControl();
				return true;
			}

			foreach (var handler in mHandlers)
			{
				if (handler.HandleKeyPress(key, modifiers))
				{
					return true;
				}
			}

			return false;
		}

		protected override bool OnAttachedControlMouseMove(Vector2 cursor, MouseButtons button)
		{
			if (mHandlers == null)
				return false;

			mVisualCollection.CursorPosition = mCanvas.CanvasToWorld(cursor);

			if (mHandlers.Any(x => x.IsDragging))
			{
				foreach (var handler in mHandlers.Where(x => x.IsDragging))
				{
					if (handler.HandleMouseMove(cursor, button))
					{
						SetOperation(handler);
						return true;
					}
				}

				return true;
			}

			foreach (var handler in mHandlers)
			{
				handler.InvalidateHitTest();
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

			foreach (var visual in mVisualCollection)
			{
				visual.IsSelected = false;
			}

			foreach (var handler in mHandlers)
			{
				if (handler.HandleMouseDown(cursor))
				{
					RaiseBeginEdit();
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

		public Iterator GetSelectedIterator()
		{
			var visual = mVisualCollection == null ? null : mVisualCollection.FirstOrDefault(x => x.IsSelected);
			if (visual == null)
				return null;

			return visual.Model;
		}
		public void ZoomOptimally()
		{
			var bounds = mVisualCollection.Select(x => x.GetBounds()).ToList();

			var corner1 = new Vector2(bounds.Select(x => x.TopLeft.X).Min(), bounds.Select(x => x.TopLeft.Y).Min());
			var corner2 = new Vector2(bounds.Select(x => x.BottomRight.X).Max(), bounds.Select(x => x.BottomRight.Y).Max());

			var rectangle = new Rectangle(corner1, corner2 - corner1);

			mCanvas.BringIntoView(rectangle);
		}
	}
}