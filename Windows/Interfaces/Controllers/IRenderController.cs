using System.Drawing;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IRenderController : IDataInputController
	{
		Size? Preset1 { get; set; }
		Size? Preset2 { get; set; }
		Size? Preset3 { get; set; }

		Flame CurrentlyRenderingFlame { get; }

		bool BatchMode { get; set; }
		bool IsRendering { get; }
		bool IsPaused { get; }

		void UpdateSelection();
	}
}