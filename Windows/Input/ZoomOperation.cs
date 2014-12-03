using System.Drawing;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class ZoomOperation : CameraInputOperation
	{
		public float NewFactor { get; private set; }
		public float OldFactor { get; private set; }

		public Rectangle InnerRect { get; private set; }
		public Rectangle OuterRect { get; private set; }

		public bool UseScale { get; private set; }

		public ZoomOperation(float newFactor, float oldFactor, Rectangle innerRect, Rectangle outerRect, bool useScale)
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