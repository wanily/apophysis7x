using System;
using Xyrus.Apophysis.Windows.Math;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public abstract class Canvas
	{
		private Vector2 mSize;

		protected Canvas(Vector2 size)
		{
			if (size == null) throw new ArgumentNullException("size");
			if (size.IsNaN || size.X <= 0 || size.Y <= 0)
				throw new ArgumentOutOfRangeException("size");

			mSize = size;
		}

		public Vector2 Size
		{
			get { return mSize; }
		}
		public virtual double Scale
		{
			get { return 1.0; }
		}
		public abstract Vector2 Ratio
		{
			get;
		}

		public Vector2 Snap(Vector2 point, double gridSize, CanvasSnapBehavior snapBehavior = CanvasSnapBehavior.Round)
		{
			if (point == null) throw new ArgumentNullException("point");
			if (gridSize <= 0) throw new ArgumentOutOfRangeException("gridSize");

			Func<double, double> func;

			switch (snapBehavior)
			{
				case CanvasSnapBehavior.Round:
					func = System.Math.Round;
					break;
				case CanvasSnapBehavior.Floor:
					func = System.Math.Floor;
					break;
				case CanvasSnapBehavior.Ceil:
					func = System.Math.Ceiling;
					break;
				default:
					throw new ArgumentOutOfRangeException("snapBehavior");
			}

			var x = func(point.X / gridSize) * gridSize;
			var y = func(point.Y / gridSize) * gridSize;

			return new Vector2 { X = x, Y = y};
		}
		public double Snap(double value, double gridSize, CanvasSnapBehavior snapBehavior = CanvasSnapBehavior.Round)
		{
			return Snap(new Vector2 {X = value}, gridSize, snapBehavior).X;
		}

		public void Resize(Vector2 newSize)
		{
			if (newSize == null) throw new ArgumentNullException("newSize");
			if (newSize.IsNaN || newSize.X <= 0 || newSize.Y <= 0)
				throw new ArgumentOutOfRangeException("newSize");

			mSize = newSize;
			ResizeOverride(mSize);
		}
		protected virtual void ResizeOverride(Vector2 newSize)
		{

		}

		public abstract Vector2 CanvasToWorld(Vector2 canvas);
		public abstract Vector2 WorldToCanvas(Vector2 world);

		public double CanvasToWorld(double canvas)
		{
			return CanvasToWorld(new Vector2 { X = canvas, Y = canvas }).X;
		}
		public double WorldToCanvas(double world)
		{
			return WorldToCanvas(new Vector2 { X = world, Y = world }).X;
		}

		public bool IsOnCanvas(Vector2 worldPoint)
		{
			var canvas = WorldToCanvas(worldPoint);
			return canvas.X >= 0 && canvas.Y >= 0 && canvas.X < mSize.X && canvas.Y < mSize.Y;
		}
	}
}
