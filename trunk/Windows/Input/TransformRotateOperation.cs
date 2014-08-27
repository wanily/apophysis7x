using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class TransformRotateOperation : TransformInputOperation
	{
		public TransformRotateOperation([NotNull] Transform transform, double rotationAngle)
			: base(transform)
		{
			RotationAngle = rotationAngle;
		}

		public double RotationAngle
		{
			get;
			private set;
		}

		protected override string GetInfoString()
		{
			var angle = RotationAngle * 180.0 / System.Math.PI;
			if (angle < 0)
			{
				angle = 360 + angle;
			}

			return string.Format("Rotate: {0:0.000}�", 360 - angle);
		}
	}
}