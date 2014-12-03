using System.Numerics;
using Xyrus.Apophysis.Math;

namespace Xyrus.Apophysis.Windows
{
	[PublicAPI]
	public class Grid : Canvas
	{
		private const float mBaseUnitToPixelRatio = 150.0f;

		private const float mZoomStepPositive = 1.25f;
		private const float mZoomStepNegative = 0.8f;
		private const float mZoomConstraint = 12;

		private float mZoom = 1.0f;
		private Vector2 mBase, mPan;

		public Grid(Vector2 size) : base(size)
		{
			mPan = new Vector2();
			ResizeOverride(size);
		}

		public Vector2 Offset
		{
			get { return mPan; }
		}
		public override float Scale
		{
			get { return Float.Power(10, -Float.Round(Float.Log(mZoom))); }
		}
		public override Vector2 Ratio
		{
			get
			{
				var ratio = mBaseUnitToPixelRatio*mZoom;
				return new Vector2(ratio, ratio);
			}
		}

		public override Vector2 CanvasToWorld(Vector2 canvas)
		{
			var ratio = Ratio;
			return new Vector2
			{
				X = (canvas.X - mBase.X) / ratio.X + mPan.X,
				Y = (canvas.Y - mBase.Y) / -ratio.Y - mPan.Y
			};
		}
		public override Vector2 WorldToCanvas(Vector2 world)
		{
			var ratio = Ratio;
			return new Vector2
			{
				X = (world.X - mPan.X) * ratio.X + mBase.X,
				Y = (world.Y + mPan.Y) * -ratio.Y + mBase.Y
			};
		}

		protected override sealed void ResizeOverride(Vector2 newSize)
		{
			mBase = (newSize/2.0f);
		}

		public override void BringIntoView(Rectangle rectangle)
		{
			var length = new Vector2(1, -1) * (rectangle.Size / Ratio);
			var corner = CanvasToWorld(rectangle.Corner);
			var center = corner + length * 0.5f;

			mPan = (new Vector2(1, -1) * center);

			var abs = Vector2.Abs(rectangle.Size);
			if (abs.X > abs.Y)
			{
				mZoom *= Size.X / rectangle.Size.X;
			}
			else
			{
				mZoom *= Size.Y / rectangle.Size.Y;
			}

			Zoom(-1);
		}
		public void Zoom(float delta)
		{
			var stepUp = mZoom*mZoomStepPositive;
			var stepDown = mZoom*mZoomStepNegative;

			var outOfBounds = 
				delta > 0 && System.Math.Round(System.Math.Log10(stepUp)) > mZoomConstraint ||
				delta < 0 && System.Math.Round(System.Math.Log10(stepDown)) < -mZoomConstraint;

			if (outOfBounds)
				return;

			mZoom = delta > 0 ? stepUp : stepDown;
		}
		public void Pan(Vector2 offset)
		{
			mPan = offset;
		}
		public void Reset()
		{
			mPan = new Vector2();
			mZoom = 1.0f;
		}
	}
}