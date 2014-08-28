using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Input;
using Xyrus.Apophysis.Windows.Math;
using Rectangle = System.Drawing.Rectangle;

namespace Xyrus.Apophysis.Windows.Visuals
{
	[PublicAPI]
	public class TransformInputOperationVisual : CanvasVisual<Canvas>
	{
		struct ColoredString
		{
			public string String;
			public Color Color;
		}

		private TransformInputOperation mOperation;
		private Color mReferenceColor;
		private Vector2 mCursorPosition;
		private Rectangle mHintTextRectangle;

		public TransformInputOperationVisual([NotNull] Control control, [NotNull] Canvas canvas) : base(control, canvas)
		{
			mReferenceColor = Color.Gray;
			mHintTextRectangle = new Rectangle(new Point(10, 10), new Size(0, 0));
			mCursorPosition = new Vector2();
		}
		protected override void DisposeOverride(bool disposing)
		{
			mOperation = null;
		}

		public TransformInputOperation Operation
		{
			get { return mOperation; }
			set { mOperation = value; }
		}
		public Color ReferenceColor
		{
			get { return mReferenceColor; }
			set
			{
				mReferenceColor = value;
				InvalidateControl();
			}
		}

		[NotNull]
		public Vector2 CursorPosition
		{
			get { return mCursorPosition; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");
				mCursorPosition = value;
			}
		}
		public Rectangle HintTextRectangle
		{
			get { return mHintTextRectangle; }
			set { mHintTextRectangle = value; }
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			var lines = new List<ColoredString>();

			using (var referenceBrush = new SolidBrush(ReferenceColor))
			using (var referencePen = new Pen(referenceBrush))
			{
				var move = mOperation as TransformMoveOperation;
				if (move != null)
				{
					var origin = Canvas.WorldToCanvas(move.Origin);
					var current = Canvas.WorldToCanvas(move.Current);

					var lh = new Line(new Vector2(0, origin.Y), new Vector2(Canvas.Size.X, origin.Y));
					var lv = new Line(new Vector2(origin.X, 0), new Vector2(origin.X, Canvas.Size.Y));

					var ch = new Line(new Vector2(0, current.Y), new Vector2(Canvas.Size.X, current.Y));
					var cv = new Line(new Vector2(current.X, 0), new Vector2(current.X, Canvas.Size.Y));

					graphics.DrawLine(referencePen, lh.A.ToPoint(), lh.B.ToPoint());
					graphics.DrawLine(referencePen, lv.A.ToPoint(), lv.B.ToPoint());

					graphics.DrawLine(referencePen, ch.A.ToPoint(), ch.B.ToPoint());
					graphics.DrawLine(referencePen, cv.A.ToPoint(), cv.B.ToPoint());

					lines.Add(new ColoredString
					{
						String = move.ToString(),
						Color = TransformVisual.GetTransformColor(move.Transform)
					});
				}

				var rotate = mOperation as TransformRotateOperation;
				if (rotate != null)
				{
					var origin = Canvas.WorldToCanvas(rotate.Transform.Origin);
					var x = Canvas.WorldToCanvas(rotate.Transform.Affine.X + rotate.Transform.Origin);

					var farRadius = System.Math.Max(Canvas.Size.X, Canvas.Size.Y);
					var radius = System.Math.Max(rotate.Transform.Affine.X.Length, rotate.Transform.Affine.Y.Length) * (Canvas.Ratio.X + Canvas.Ratio.Y) * 0.5;
					var rect = new Rectangle((int)(origin.X - radius), (int)(origin.Y - radius), (int)(radius * 2), (int)(radius * 2));

					var lh = new Line((x - origin).Direction * -farRadius + origin, (x - origin).Direction * farRadius + origin);
					var lv = new Line(lh.GetNormal() * -radius + origin, lh.GetNormal() * radius + origin);

					var loh = new Line(new Vector2(0, origin.Y), new Vector2(Canvas.Size.X, origin.Y));
					var lov = new Line(new Vector2(origin.X, 0), new Vector2(origin.X, Canvas.Size.Y));

					graphics.DrawLine(referencePen, loh.A.ToPoint(), loh.B.ToPoint());
					graphics.DrawLine(referencePen, lov.A.ToPoint(), lov.B.ToPoint());
					
					graphics.DrawEllipse(referencePen, rect);

					graphics.DrawLine(referencePen, lh.A.ToPoint(), lh.B.ToPoint());
					graphics.DrawLine(referencePen, lv.A.ToPoint(), lv.B.ToPoint());

					lines.Add(new ColoredString
					{
						String = rotate.ToString(),
						Color = TransformVisual.GetTransformColor(rotate.Transform)
					});
				}

				var scale = mOperation as TransformScaleOperation;
				if (scale != null)
				{
					var origin = Canvas.WorldToCanvas(scale.Transform.Origin);
					var x = Canvas.WorldToCanvas(scale.Transform.Affine.X + scale.Transform.Origin);
					var y = Canvas.WorldToCanvas(scale.Transform.Affine.Y + scale.Transform.Origin);

					var farRadius = System.Math.Max(Canvas.Size.X, Canvas.Size.Y);

					var lx = new Line((x - origin).Direction * -farRadius + origin, (x - origin).Direction * farRadius + origin);
					var ly = new Line((y - origin).Direction * -farRadius + origin, (y - origin).Direction * farRadius + origin);

					var loh = new Line(new Vector2(0, origin.Y), new Vector2(Canvas.Size.X, origin.Y));
					var lov = new Line(new Vector2(origin.X, 0), new Vector2(origin.X, Canvas.Size.Y));

					graphics.DrawLine(referencePen, loh.A.ToPoint(), loh.B.ToPoint());
					graphics.DrawLine(referencePen, lov.A.ToPoint(), lov.B.ToPoint());

					graphics.DrawLine(referencePen, lx.A.ToPoint(), lx.B.ToPoint());
					graphics.DrawLine(referencePen, ly.A.ToPoint(), ly.B.ToPoint());

					lines.Add(new ColoredString
					{
						String = scale.ToString(),
						Color = TransformVisual.GetTransformColor(scale.Transform)
					});
				}

				if (mOperation != null)
				{
					var text = new TransformMouseOverOperation(mOperation.Transform).ToString();
					var textSize = graphics.MeasureString(text, AttachedControl.Font);

					using (var brush = new SolidBrush(TransformVisual.GetTransformColor(mOperation.Transform)))
					{
						graphics.DrawString(text, AttachedControl.Font, brush,
							(int)Canvas.Size.X - mHintTextRectangle.Right - textSize.Width,
							(int)Canvas.Size.Y - mHintTextRectangle.Bottom - textSize.Height);
					}
				}

				lines.Add(new ColoredString
				{
					String = string.Format("Cursor:\t {0}\t {1}", 
						CursorPosition.X.ToString("0.000", CultureInfo.CurrentCulture).PadLeft(6),
						CursorPosition.Y.ToString("0.000", CultureInfo.CurrentCulture).PadLeft(6)),
					Color = AttachedControl.ForeColor
				});

				var lineHeight = graphics.MeasureString("fg", AttachedControl.Font).Height;
				var yText = (int)Canvas.Size.Y - HintTextRectangle.Bottom - lineHeight*lines.Count;

				foreach (var line in lines)
				{
					using (var brush = new SolidBrush(line.Color))
					{
						graphics.DrawString(line.String, AttachedControl.Font, brush, HintTextRectangle.Left, yText);
						yText += lineHeight;
					}
				}
			}
		}
	}
}