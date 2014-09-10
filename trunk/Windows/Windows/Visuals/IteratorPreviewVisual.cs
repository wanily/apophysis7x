using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Math;
using Xyrus.Apophysis.Models;
using Rectangle = Xyrus.Apophysis.Math.Rectangle;

namespace Xyrus.Apophysis.Windows.Visuals
{
	class IteratorPreviewVisual : CanvasVisual<Canvas>
	{
		private IEnumerable<IteratorVisual> mCollection;

		public IteratorPreviewVisual([NotNull] Control control, [NotNull] Canvas canvas, [NotNull] IEnumerable<IteratorVisual> collection) : base(control, canvas)
		{
			if (collection == null) throw new ArgumentNullException("collection");
			mCollection = collection;
		}
		protected override void DisposeOverride(bool disposing)
		{
			mCollection = null;
		}

		private Vector2 IterateSample(Iterator model, Vector2 input, IEnumerable<Variation> variations, IterationData data)
		{
			var vecOut = new Vector2
			{
				X = model.PreAffine.Matrix.X.X * input.X + model.PreAffine.Matrix.X.Y * -input.Y + model.PreAffine.Origin.X,
				Y = model.PreAffine.Matrix.Y.X * -input.X + model.PreAffine.Matrix.Y.Y * input.Y + model.PreAffine.Origin.Y,
			};

			data.PreX = vecOut.X;
			data.PreY = vecOut.Y;
			data.PreZ = 0;
			data.PostX = 0;
			data.PostY = 0;
			data.PostZ = 0;

			foreach (var variation in variations)
			{
				data.Weight = variation.Weight;
				variation.Calculate(data);
			}

			vecOut.X = data.PostX;
			vecOut.Y = data.PostY;

			vecOut = new Vector2
			{
				X = model.PostAffine.Matrix.X.X * vecOut.X + model.PostAffine.Matrix.X.Y * -vecOut.Y + model.PostAffine.Origin.X,
				Y = model.PostAffine.Matrix.Y.X * -vecOut.X + model.PostAffine.Matrix.Y.Y * vecOut.Y + model.PostAffine.Origin.Y,
			};

			return vecOut;
		}
		private void DrawModel(Graphics graphics, Iterator model)
		{
			var variations = model.Variations.Where(x => System.Math.Abs((double)x.Weight) > double.Epsilon).ToArray();
			var p1 = new Vector2(-1, -1);
			var p2 = new Vector2(1, 1);
			var data = new IterationData();

			foreach (var variation in variations)
			{
				data.Weight = variation.Weight;
				variation.Prepare(data);
			}

			data.Weight = 0;

			const double step = 0.1;

			using (var brush = new SolidBrush(Color.FromArgb(0x80, model.GetColor())))
			//using (var pen = new Pen(brush))
			{
				//Vector2 lastPoint = null;

				var plot = new Action<double, double>((x, y) =>
				{
					var vout = IterateSample(model, new Vector2(x, y), variations, data);
					var pos = Canvas.WorldToCanvas(vout);

					// ReSharper disable once AccessToModifiedClosure
					//var pA = (lastPoint ?? pos).ToPoint();
					var pB = pos.ToPoint();

					// ReSharper disable once AccessToDisposedClosure
					graphics.FillEllipse(brush, pB.X - 1, pB.Y - 1, 3, 3);
					//graphics.DrawLine(pen, pA, pB);

					//lastPoint = pos;
				});

				for (double y = p1.Y; y <= p2.Y; y += step)
				{
					//lastPoint = null;

					for (double x = p1.X; x <= p2.X; x += step)
					{
						plot(x, y);
					}
				}
			}
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			foreach (var iterator in mCollection)
			{
				if (iterator.IsSelected)
				{
					DrawModel(graphics, iterator.Model);
				}
			}
		}
	}
}