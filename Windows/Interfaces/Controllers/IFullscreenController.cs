namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IFullscreenController : IViewController
	{
		void EnterFullscreen();
		void UpdateThreadCount();
		void ReloadSettings();
	}
}