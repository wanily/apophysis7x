using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Interfaces
{
	public interface IBatchListController : IViewController
	{
		void BuildFlameList();
		void UpdateSelectedPreview();

		void SelectFlame([NotNull] Flame flame);
		Flame GetSelectedFlame();

		bool IsIconViewEnabled { get; set; }

		int ListPreviewSize { get; set; }
		float ListPreviewDensity { get; set; }
	}
}