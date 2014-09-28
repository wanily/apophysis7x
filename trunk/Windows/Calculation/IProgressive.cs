using System;

namespace Xyrus.Apophysis.Calculation
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