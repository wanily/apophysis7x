using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Input
{
	class ScaleOperation : IteratorInputOperation
	{
		public ScaleOperation([NotNull] Iterator iterator, double scaleFactor)
			: base(iterator)
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
			return string.Format("Scale:\t {0}%", (ScaleFactor * 100).ToString("0", InputController.Culture).PadLeft(5));
		}
	}
}