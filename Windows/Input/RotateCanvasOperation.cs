using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class RotateCanvasOperation : CameraInputOperation
	{
		public float NewAngle { get; private set; }
		public float OldAngle { get; private set; }

		public RotateCanvasOperation(float newAngle, float oldAngle)
		{
			NewAngle = newAngle;
			OldAngle = oldAngle;
		}

		public override string ToString()
		{
			var angle = NewAngle * 180.0f / Float.Pi;
			if (angle < 0)
			{
				angle = 360 + angle;
			}

			return string.Format("Rotate: {0}°", (360 - angle).ToString("0", InputController.Culture));
		}
	}
}