namespace Xyrus.Apophysis.Windows.Input
{
	class RotateCanvasOperation : CameraInputOperation
	{
		public double Angle { get; private set; }

		public RotateCanvasOperation(double angle)
		{
			Angle = angle;
		}
	}
}