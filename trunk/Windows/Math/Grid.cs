namespace Xyrus.Apophysis.Windows.Math
{
	[PublicAPI]
	public class Grid : Canvas
	{
		private const double mBaseUnitToPixelRatio = 150.0;

		private const double mZoomStepPositive = 1.25;
		private const double mZoomStepNegative = 0.8;
		private const double mZoomConstraint = 12;

		private double mZoom = 1.0;
		private ImmutableVector2 mBase, mPan;

		public Grid(Vector2 size) : base(size)
		{
			mPan = new ImmutableVector2(new Vector2());
			ResizeOverride(size);
		}

		public double ZoomRatio
		{
			get { return mZoom; }
		}
		public ImmutableVector2 Offset
		{
			get { return mPan; }
		}
		public override double Scale
		{
			get { return System.Math.Pow(10, -System.Math.Round(System.Math.Log10(mZoom))); }
		}
		public override Vector2 Ratio
		{
			get
			{
				var ratio = mBaseUnitToPixelRatio*mZoom;
				return new Vector2 { X = ratio, Y = ratio };
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
			mBase = (newSize/2.0).Freeze();
		}

		public void Zoom(double delta)
		{
			var stepUp = mZoom*mZoomStepPositive;
			var stepDown = mZoom*mZoomStepNegative;

			var outOfBounds = 
				delta > 0 && System.Math.Round(System.Math.Log10(stepUp)) > mZoomConstraint ||
				delta < 0 && System.Math.Round(System.Math.Log10(stepDown)) < -mZoomConstraint;

			if (outOfBounds)
				return;

			if (delta > 0) mZoom = stepUp;
			else mZoom = stepDown;
		}
		public void Pan(Vector2 offset)
		{
			mPan = offset.Freeze();
		}
		public void Reset()
		{
			mPan = new ImmutableVector2(new Vector2());
			mZoom = 1.0;
		}
	}
}