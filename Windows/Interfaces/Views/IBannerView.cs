
namespace Xyrus.Apophysis.Windows.Interfaces.Views
{
	public interface IBannerView : IView
	{
		string BannerText { get; set; }
		string VersionText { get; }
	}
}