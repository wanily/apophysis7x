
namespace Xyrus.Apophysis.Windows.Interfaces.Views
{
	public interface IBannerView : IWindow
	{
		string BannerText { get; set; }
		string VersionText { get; }
	}
}