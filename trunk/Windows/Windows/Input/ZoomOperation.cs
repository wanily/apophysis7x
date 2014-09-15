using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class ZoomOperation : CameraInputOperation
	{
		public double Factor { get; private set; }
		public double OriginalFactor { get; private set; }
		public bool UseScale { get; private set; }

		public ZoomOperation([NotNull] Flame flame, double factor, double originalFactor, bool useScale) : base(flame)
		{
			Factor = factor;
			OriginalFactor = originalFactor;
		}

		public override string ToString()
		{
			return string.Format(UseScale ? "Scale: {0}" : "Zoom: {0}", Factor.ToString("0.000", InputController.Culture));
		}
	}
}