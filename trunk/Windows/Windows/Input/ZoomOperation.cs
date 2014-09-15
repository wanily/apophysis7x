using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class ZoomOperation : CameraInputOperation
	{
		public double NewFactor { get; private set; }
		public double OldFactor { get; private set; }

		public bool UseScale { get; private set; }

		public ZoomOperation([NotNull] Flame flame, double newFactor, double oldFactor, bool useScale) : base(flame)
		{
			NewFactor = newFactor;
			OldFactor = oldFactor;
		}

		public override string ToString()
		{
			return string.Format(UseScale ? "Scale: {0}" : "Zoom: {0}", NewFactor.ToString("0.000", InputController.Culture));
		}
	}
}