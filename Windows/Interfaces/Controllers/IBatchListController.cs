using System.Collections.Generic;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IBatchListController : IViewController
	{
		int PreviewSize { get; set; }
		float PreviewDensity { get; set; }
		bool IsIconViewEnabled { get; set; }

		[NotNull]
		Flame SelectedFlame { get; set; }
		IEnumerable<Flame> Flames { get; set; }
	}
}