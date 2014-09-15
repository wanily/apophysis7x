using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class RotateCanvasOperation : CameraInputOperation
	{
		public double NewAngle { get; private set; }
		public double OldAngle { get; private set; }

		public RotateCanvasOperation([NotNull] Flame flame, double newAngle, double oldAngle) : base(flame)
		{
			NewAngle = newAngle;
			OldAngle = oldAngle;
		}

		public override string ToString()
		{
			var angle = NewAngle * 180.0 / System.Math.PI;
			if (angle < 0)
			{
				angle = 360 + angle;
			}

			return string.Format("Rotate: {0}°", (360 - angle).ToString("0", InputController.Culture));
		}
	}
}