using System;

namespace Xyrus.Apophysis.Windows.Interfaces.Views
{
	public interface IFullscreenView : IWindow
	{
		int Progress { get; set; }
		string TimeElapsed { get; set; }
		string TimeRemaining { get; set; }
		bool IsInProgress { get; set; }
		event EventHandler Hidden;
	}
}