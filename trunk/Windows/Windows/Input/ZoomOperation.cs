using System.Drawing;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class ZoomOperation : CameraInputOperation
	{
		public double NewFactor { get; private set; }
		public double OldFactor { get; private set; }

		public Rectangle InnerRect { get; private set; }
		public Rectangle OuterRect { get; private set; }

		public bool UseScale { get; private set; }

		public ZoomOperation(double newFactor, double oldFactor, Rectangle innerRect, Rectangle outerRect, bool useScale)
		{
			NewFactor = newFactor;
			OldFactor = oldFactor;
			InnerRect = innerRect;
			OuterRect = outerRect;
			UseScale = useScale;
		}

		public override string ToString()
		{
			return string.Format(UseScale ? "Scale: {0}" : "Zoom: {0}", NewFactor.ToString("0.000", InputController.Culture));
		}
	}
}