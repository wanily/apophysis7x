using System;
using Xyrus.Apophysis.Windows.Math;

namespace Xyrus.Apophysis.Windows.Drawing
{
	[PublicAPI]
	public class GridNavigationStrategy : CanvasNagivationStrategy<Grid>
	{
		public GridNavigationStrategy([NotNull] Grid canvas) : base(canvas)
		{
		}

		public override void NavigateOffset(Vector2 cursor)
		{
			if (!IsNavigating)
				return;

			Canvas.Pan(GetNextOffset(cursor));
			RaiseCanvasUpdated();
		}
		public override void NavigateRotate(Vector2 cursor)
		{
			throw new NotSupportedException("Rotation is not supported on a grid canvas");
		}
		public override void NavigateZoom(double delta)
		{
			Canvas.Zoom(delta);
			RaiseCanvasUpdated();
		}
		public override void NavigateReset()
		{
			Canvas.Reset();
			RaiseCanvasUpdated();
		}

		protected override Vector2 GetCurrentOffset()
		{
			return Canvas.Offset;
		}
	}
}