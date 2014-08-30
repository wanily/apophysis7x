using System.Globalization;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class RotateOperation : InputOperation
	{
		public RotateOperation([NotNull] Iterator iterator, double rotationAngle)
			: base(iterator)
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

			return string.Format("Rotate:\t {0}°", (360 - angle).ToString("0", CultureInfo.CurrentCulture).PadLeft(5));
		}
	}
}