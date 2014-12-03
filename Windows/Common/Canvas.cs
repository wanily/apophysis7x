using System;
using System.Numerics;
using Xyrus.Apophysis.Math;

namespace Xyrus.Apophysis.Windows
{
	[PublicAPI]
	public abstract class Canvas
	{
		private Vector2 mSize;

		protected Canvas(Vector2 size)
		{
			if (size == null) throw new ArgumentNullException("size");
			if (size.X <= 0 || size.Y <= 0)
				throw new ArgumentOutOfRangeException("size");

			mSize = size;
		}

		public Vector2 Size
		{
			get { return mSize; }
		}
		public virtual float Scale
		{
			get { return 1.0f; }
		}
		public abstract Vector2 Ratio
		{
			get;
		}

		public Vector2 Snap(Vector2 point, Vector2 gridSize, CanvasSnapBehavior snapBehaviorX = CanvasSnapBehavior.Round, CanvasSnapBehavior snapBehaviorY = CanvasSnapBehavior.Round)
		{
			if (point == null) throw new ArgumentNullException("point");
			if (gridSize == null) throw new ArgumentNullException("gridSize");

			Func<float, float> funcX, funcY;

			switch (snapBehaviorX)
			{
				case CanvasSnapBehavior.Round:
					funcX = Float.Round;
					break;
				case CanvasSnapBehavior.Floor:
					funcX = Float.Floor;
					break;
				case CanvasSnapBehavior.Ceil:
					funcX = Float.Ceiling;
					break;
				default:
					throw new ArgumentOutOfRangeException("snapBehaviorX");
			}

			switch (snapBehaviorY)
			{
				case CanvasSnapBehavior.Round:
					funcY = Float.Round;
					break;
				case CanvasSnapBehavior.Floor:
					funcY = Float.Floor;
					break;
				case CanvasSnapBehavior.Ceil:
					funcY = Float.Ceiling;
					break;
				default:
					throw new ArgumentOutOfRangeException("snapBehaviorY");
			}

			var x = funcX(point.X / gridSize.X) * gridSize.X;
			var y = funcY(point.Y / gridSize.Y) * gridSize.Y;

			return new Vector2(x, y);
		}
		public void Resize(Vector2 newSize)
		{
			if (newSize == null) throw new ArgumentNullException("newSize");
			if (newSize.X <= 0 || newSize.Y <= 0)
				throw new ArgumentOutOfRangeException("newSize");

			mSize = newSize;
			ResizeOverride(mSize);
		}
		protected virtual void ResizeOverride(Vector2 newSize)
		{

		}

		public abstract Vector2 CanvasToWorld(Vector2 canvas);
		public abstract Vector2 WorldToCanvas(Vector2 world);

		public abstract void BringIntoView(Rectangle rectangle);
	}
}
