using System;
using Xyrus.Apophysis.Calculation;

namespace Xyrus.Apophysis.Interfaces.Calculation
{
	[PublicAPI]
	public interface IProgressive
	{
		event BitmapReadyEventHandler BitmapReady;
		TimeSpan? TimeUntilNextBitmap { get; }

		void StartIterate([NotNull] Histogram histogram);
		void Iterate([NotNull] Histogram histogram);
	}
}