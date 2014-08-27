using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	[PublicAPI]
	public class TransformScaleOperation : TransformInputOperation
	{
		public TransformScaleOperation([NotNull] Transform transform, double scaleFactor)
			: base(transform)
		{
			ScaleFactor = scaleFactor;
		}

		public double ScaleFactor 
		{ 
			get; 
			private set; 
		}

		protected override string GetInfoString()
		{
			return string.Format("Scale: {0:0}%", ScaleFactor * 100);
		}
	}
}