using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class RotateCanvasOperation : CameraInputOperation
	{
		public double Angle { get; private set; }
		public double OriginalAngle { get; private set; }

		public RotateCanvasOperation([NotNull] Flame flame, double angle, double originalAngle) : base(flame)
		{
			Angle = angle;
			OriginalAngle = originalAngle;
		}

		public override string ToString()
		{
			var angle = Angle * 180.0 / System.Math.PI;
			if (angle < 0)
			{
				angle = 360 + angle;
			}

			return string.Format("Rotate: {0}°", (360 - angle).ToString("0", InputController.Culture));
		}
	}
}