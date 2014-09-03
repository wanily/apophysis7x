using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Input;
using Rectangle = System.Drawing.Rectangle;

namespace Xyrus.Apophysis.Windows.Visuals
{
	class IteratorCollectionVisual : CanvasVisual<Canvas>, IEnumerable<IteratorVisual>
	{
		private InputOperationVisual mOperationVisual;

		private IteratorCollection mCollection;
		private List<IteratorVisual> mVisuals;
		private IteratorVisual mFinalVisual;

		private EventHandler mContentChanged;
		private EventHandler mFinalChanged;

		private bool mShowReference;
		private IteratorMatrix mActiveMatrix;

		public IteratorCollectionVisual([NotNull] Control control, [NotNull] Canvas canvas) : base(control, canvas)
		{
			mVisuals = new List<IteratorVisual>();
			mOperationVisual = new InputOperationVisual(control, canvas);

			ShowReference = true;
		}
		protected override void DisposeOverride(bool disposing)
		{
			if (mCollection != null)
			{
				mCollection.ContentChanged -= OnCollectionChanged;
				mCollection = null;
			}

			if (mOperationVisual != null)
			{
				mOperationVisual.Dispose();
				mOperationVisual = null;
			}

			if (mFinalVisual != null)
			{
				mFinalVisual.Dispose();
				mFinalVisual = null;
			}

			if (mVisuals != null)
			{
				foreach (var visual in mVisuals)
				{
					visual.Dispose();
				}

				mVisuals.Clear();
				mVisuals = null;
			}
		}

		public Color ReferenceColor
		{
			get { return mOperationVisual.ReferenceColor; }
			set { mOperationVisual.ReferenceColor = value; }
		}
		public bool ShowReference
		{
			get { return mShowReference; }
			set
			{
				mShowReference = value;
				InvalidateControl();
			}
		}

		public InputOperation CurrentOperation
		{
			get { return mOperationVisual == null ? null : mOperationVisual.Operation; }
			set
			{
				if (mOperationVisual == null)
					return;

				mOperationVisual.Operation = value;
			}
		}
		public IteratorCollection Collection
		{
			get { return mCollection; }
			set
			{
				mFinalVisual = null;

				if (mCollection != null)
				{
					mCollection.ContentChanged -= OnCollectionChanged;
				}

				if (value != null)
				{
					value.ContentChanged += OnCollectionChanged;
					mFinalVisual = new IteratorVisual(AttachedControl, Canvas, value.FinalIterator, ActiveMatrix);
					mFinalVisual.ForcedColor = Color.White;
				}

				mCollection = value;
				OnCollectionChanged(mCollection, new EventArgs());
				if (mFinalChanged != null)
				{
					mFinalChanged(this, new EventArgs());
				}
			}
		}

		public Vector2 CursorPosition
		{
			get { return mOperationVisual.CursorPosition; }
			set { mOperationVisual.CursorPosition = value; }
		}
		public Rectangle HintTextRectangle
		{
			get { return mOperationVisual.HintTextRectangle; }
			set { mOperationVisual.HintTextRectangle = value; }
		}
		public IteratorMatrix ActiveMatrix
		{
			get { return mActiveMatrix; }
			set
			{
				mActiveMatrix = value;
				mOperationVisual.ActiveMatrix = value;

				if (mFinalVisual != null)
				{
					mFinalVisual.ActiveMatrix = value;
				}

				OnCollectionChanged(mCollection, new EventArgs());
			}
		}

		public IteratorVisual this[int index]
		{
			get
			{
				if (mVisuals == null)
					throw new IndexOutOfRangeException();

				if (index < 0 || index >= mVisuals.Count)
					throw new IndexOutOfRangeException();

				return mVisuals[index];
			}
		}
		public IteratorVisual FinalVisual
		{
			get { return mFinalVisual; }
		}

		public event EventHandler ContentChanged
		{
			add { mContentChanged += value; }
			remove { mContentChanged -= value; }
		}
		public event EventHandler FinalChanged
		{
			add { mFinalChanged += value; }
			remove { mFinalChanged -= value; }
		}
		
		private void OnCollectionChanged(object sender, EventArgs eventArgs)
		{
			if (mVisuals == null || mCollection == null)
				return;

			foreach (var visual in mVisuals)
			{
				visual.Dispose();
			}

			mVisuals.Clear();

			foreach (var iterator in mCollection)
			{
				mVisuals.Add(new IteratorVisual(AttachedControl, Canvas, iterator, mActiveMatrix));
			}

			if (mContentChanged != null)
				mContentChanged(this, new EventArgs());

			InvalidateControl();
		}
		protected override void OnControlPaint(Graphics graphics)
		{
			if (mVisuals == null || mCollection == null)
				return;

			if (ShowReference)
			{
				const int distLabel = 4;

				const string lo = "O";
				const string lx = "X";
				const string ly = "Y";

				var o = Canvas.WorldToCanvas(new Vector2());
				var x = Canvas.WorldToCanvas(new Vector2(1, 0));
				var y = Canvas.WorldToCanvas(new Vector2(0, 1));

				var ox = new Line(o, x);
				var oy = new Line(o, y);
				var xy = new Line(x, y);

				var sizeLo = graphics.MeasureString(lo, AttachedControl.Font);
				var sizeLy = graphics.MeasureString(ly, AttachedControl.Font);

				var posLo = (ox.A + new Vector2(-distLabel - sizeLo.Width, distLabel)).ToPoint();
				var posLx = (ox.B + new Vector2(distLabel, distLabel)).ToPoint();
				var posLy = (oy.B + new Vector2(-distLabel - sizeLy.Width, -distLabel - sizeLy.Height)).ToPoint();

				using (var dashLinePen = new Pen(ReferenceColor))
				using (var labelBrush = new SolidBrush(ReferenceColor))
				{
					dashLinePen.DashPattern = new[] {6.0f, 4.0f};

					graphics.DrawLine(dashLinePen, ox.A.ToPoint(), ox.B.ToPoint());
					graphics.DrawLine(dashLinePen, oy.A.ToPoint(), oy.B.ToPoint());
					graphics.DrawLine(dashLinePen, xy.A.ToPoint(), xy.B.ToPoint());

					graphics.DrawString(lo, AttachedControl.Font, labelBrush, posLo.X, posLo.Y);
					graphics.DrawString(lx, AttachedControl.Font, labelBrush, posLx.X, posLx.Y);
					graphics.DrawString(ly, AttachedControl.Font, labelBrush, posLy.X, posLy.Y);

				}
			}

			if (mFinalVisual != null && mCollection.UseFinalIterator)
			{
				mFinalVisual.Paint(graphics);
			}

			if (mOperationVisual != null)
			{
				mOperationVisual.Paint(graphics);
			}

			foreach (var visual in mVisuals.Where(x => !x.IsActive))
			{
				visual.Paint(graphics);
			}

			foreach (var visual in mVisuals.Where(x => x.IsActive))
			{
				visual.Paint(graphics);
			}
		}

		public IEnumerator<IteratorVisual> GetEnumerator()
		{
			if (mVisuals == null)
				return new List<IteratorVisual>().GetEnumerator();

			return mVisuals.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}