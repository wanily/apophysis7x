namespace Xyrus.Apophysis.Windows.Interfaces
{
	public interface IFullscreenController : IViewController
	{
		void EnterFullscreen();
		void UpdateThreadCount();
		void ReloadSettings();
	}
}