using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Controls;
using Xyrus.Apophysis.Windows.Input;
using Rectangle = System.Drawing.Rectangle;

namespace Xyrus.Apophysis.Windows.Visuals
{
	class InputOperationVisual : CanvasVisual<Canvas>
	{
		struct ColoredString
		{
			public string String;
			public Color Color;
		}

		private Color mReferenceColor;
		private Vector2 mCursorPosition;
		private Rectangle mHintTextRectangle;
		private InputOperation mOperation;

		public InputOperationVisual([NotNull] Control control, [NotNull] Canvas canvas) : base(control, canvas)
		{
			mReferenceColor = Color.Gray;
			mHintTextRectangle = new Rectangle(new Point(10, 10), new Size(0, 0));
			mCursorPosition = new Vector2();
		}
		protected override void DisposeOverride(bool disposing)
		{
			mOperation = null;
		}

		public InputOperation Operation
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
		public IteratorMatrix ActiveMatrix
		{
			get; 
			set;
		}

		private Matrix2X2 GetMatrix([NotNull] Iterator iterator)
		{
			if (iterator == null)
			{
				throw new ArgumentNullException("iterator");
			}

			switch (ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					return iterator.PreAffine.Matrix;
				case IteratorMatrix.PostAffine:
					return iterator.PostAffine.Matrix;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}
		private Vector2 GetOrigin([NotNull] Iterator iterator)
		{
			if (iterator == null)
			{
				throw new ArgumentNullException("iterator");
			}

			switch (ActiveMatrix)
			{
				case IteratorMatrix.PreAffine:
					return iterator.PreAffine.Origin;
				case IteratorMatrix.PostAffine:
					return iterator.PostAffine.Origin;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			var lines = new List<ColoredString>();

			using (var referenceBrush = new SolidBrush(ReferenceColor))
			using (var referencePen = new Pen(referenceBrush))
			{
				var move = mOperation as MoveOperation;
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
						Color = move.Iterator.GetColor()
					});
				}

				var rotate = mOperation as RotateOperation;
				if (rotate != null)
				{
					var matrix = GetMatrix(rotate.Iterator);
					var origin = GetOrigin(rotate.Iterator);

					var originCanvas = Canvas.WorldToCanvas(origin);
					var x = Canvas.WorldToCanvas((rotate.Axis == RotationAxis.X ? matrix.X : matrix.Y) + origin);

					var farRadius = System.Math.Max(Canvas.Size.X, Canvas.Size.Y);
					var radius = System.Math.Max(matrix.X.Length, matrix.Y.Length) * (Canvas.Ratio.X + Canvas.Ratio.Y) * 0.5;
					var rect = new Rectangle((int)(originCanvas.X - radius), (int)(originCanvas.Y - radius), (int)(radius * 2), (int)(radius * 2));

					var lh = new Line((x - originCanvas).Direction * -farRadius + originCanvas, (x - originCanvas).Direction * farRadius + originCanvas);
					var lv = new Line(lh.GetNormal() * -radius + originCanvas, lh.GetNormal() * radius + originCanvas);

					var loh = new Line(new Vector2(0, originCanvas.Y), new Vector2(Canvas.Size.X, originCanvas.Y));
					var lov = new Line(new Vector2(originCanvas.X, 0), new Vector2(originCanvas.X, Canvas.Size.Y));

					graphics.DrawLine(referencePen, loh.A.ToPoint(), loh.B.ToPoint());
					graphics.DrawLine(referencePen, lov.A.ToPoint(), lov.B.ToPoint());
					
					graphics.DrawEllipse(referencePen, rect);

					graphics.DrawLine(referencePen, lh.A.ToPoint(), lh.B.ToPoint());
					graphics.DrawLine(referencePen, lv.A.ToPoint(), lv.B.ToPoint());

					lines.Add(new ColoredString
					{
						String = rotate.ToString(),
						Color = rotate.Iterator.GetColor()
					});
				}

				var scale = mOperation as ScaleOperation;
				if (scale != null)
				{
					var matrix = GetMatrix(scale.Iterator);
					var origin = GetOrigin(scale.Iterator);

					var originCanvas = Canvas.WorldToCanvas(origin);
					var x = Canvas.WorldToCanvas(matrix.X + origin);
					var y = Canvas.WorldToCanvas(matrix.Y + origin);

					var farRadius = System.Math.Max(Canvas.Size.X, Canvas.Size.Y);

					var lx = new Line((x - originCanvas).Direction * -farRadius + originCanvas, (x - originCanvas).Direction * farRadius + originCanvas);
					var ly = new Line((y - originCanvas).Direction * -farRadius + originCanvas, (y - originCanvas).Direction * farRadius + originCanvas);

					var loh = new Line(new Vector2(0, originCanvas.Y), new Vector2(Canvas.Size.X, originCanvas.Y));
					var lov = new Line(new Vector2(originCanvas.X, 0), new Vector2(originCanvas.X, Canvas.Size.Y));

					graphics.DrawLine(referencePen, loh.A.ToPoint(), loh.B.ToPoint());
					graphics.DrawLine(referencePen, lov.A.ToPoint(), lov.B.ToPoint());

					graphics.DrawLine(referencePen, lx.A.ToPoint(), lx.B.ToPoint());
					graphics.DrawLine(referencePen, ly.A.ToPoint(), ly.B.ToPoint());

					lines.Add(new ColoredString
					{
						String = scale.ToString(),
						Color = scale.Iterator.GetColor()
					});
				}

				if (mOperation != null)
				{
					var text = new MouseOverOperation(mOperation.Iterator).ToString();
					foreach (var variation in mOperation.Iterator.Variations.Where(x => System.Math.Abs(x.Weight) > double.Epsilon))
					{
						text += Environment.NewLine + 
							variation.Name + " = " + 
							variation.Weight.ToString(InputController.DefaultFormat, InputController.Culture);
					}

					var textSize = graphics.MeasureString(text, AttachedControl.Font);
					using (var brush = new SolidBrush(mOperation.Iterator.GetColor()))
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