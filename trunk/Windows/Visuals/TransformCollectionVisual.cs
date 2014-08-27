using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Input;
using Xyrus.Apophysis.Windows.Math;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Visuals
{
	[PublicAPI]
	public class TransformCollectionVisual : CanvasVisual<Canvas>, IEnumerable<TransformVisual>
	{
		private TransformInputOperationVisual mOperationVisual;

		private TransformCollection mCollection;
		private List<TransformVisual> mVisuals;

		private EventHandler mContentChanged;
		private bool mShowReference;

		public TransformCollectionVisual([NotNull] Control control, [NotNull] Canvas canvas) : base(control, canvas)
		{
			mVisuals = new List<TransformVisual>();
			mOperationVisual = new TransformInputOperationVisual(control, canvas);

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

		public TransformInputOperation CurrentOperation
		{
			get { return mOperationVisual == null ? null : mOperationVisual.Operation; }
			set
			{
				if (mOperationVisual == null)
					return;

				mOperationVisual.Operation = value;
			}
		}
		public TransformCollection Collection
		{
			get { return mCollection; }
			set
			{
				if (mCollection != null)
				{
					mCollection.ContentChanged -= OnCollectionChanged;
				}

				if (value != null)
				{
					value.ContentChanged += OnCollectionChanged;
				}

				mCollection = value;
				OnCollectionChanged(mCollection, new EventArgs());
			}
		}
		public TransformVisual this[int index]
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

		public event EventHandler ContentChanged
		{
			add { mContentChanged += value; }
			remove { mContentChanged -= value; }
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

			foreach (var transform in mCollection)
			{
				mVisuals.Add(new TransformVisual(AttachedControl, Canvas, transform));
			}

			if (mContentChanged != null)
				mContentChanged(this, new EventArgs());

			InvalidateControl();
		}
		protected override void OnControlPaint(Graphics graphics)
		{
			if (mVisuals == null)
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
				var sizeLx = graphics.MeasureString(lx, AttachedControl.Font);
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

		public IEnumerator<TransformVisual> GetEnumerator()
		{
			if (mVisuals == null)
				return new List<TransformVisual>().GetEnumerator();

			return mVisuals.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}