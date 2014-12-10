namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IMainPreviewController : IViewController
	{
		int PreviewDensity { get; set; }
		bool FitImage { get; set; }

		void UpdatePreview();
		void UpdateThreadCount();

		void ReloadSettings();
	}
}