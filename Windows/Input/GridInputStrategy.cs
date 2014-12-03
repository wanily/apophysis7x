using System.Numerics;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Input
{
	class GridInputStrategy : CanvasInputStrategy<Grid>
	{
		public GridInputStrategy([NotNull] Control control, [NotNull] Grid canvas) : base(control, canvas)
		{
		}

		public override void NavigateOffset(Vector2 cursor)
		{
			if (!IsNavigating)
				return;

			Canvas.Pan(GetNextOffset(cursor));
		}
		public override void NavigateZoom(float delta)
		{
			Canvas.Zoom(delta);
		}
		public override void NavigateReset()
		{
			Canvas.Reset();
		}

		protected override Vector2 GetCurrentOffset()
		{
			return Canvas.Offset;
		}
	}
}