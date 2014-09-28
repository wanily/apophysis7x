namespace Xyrus.Apophysis.Calculation
{
	[PublicAPI]
	public interface IProgressive
	{
		event BitmapReadyEventHandler BitmapReady;
		void StartIterate([NotNull] Histogram histogram);
		void Iterate([NotNull] Histogram histogram);
	}
}