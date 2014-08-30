using System.Globalization;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Input
{
	class ScaleOperation : InputOperation
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
			return string.Format("Scale:\t {0}%", (ScaleFactor * 100).ToString("0", CultureInfo.CurrentCulture).PadLeft(5));
		}
	}
}