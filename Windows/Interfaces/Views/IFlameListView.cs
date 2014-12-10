using System.Collections.Generic;
using System.ComponentModel;
using Xyrus.Apophysis.Interfaces.Threading;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;

namespace Xyrus.Apophysis.Windows.Interfaces.Views
{
	public interface IFlameListView : IView
	{
		void LoadSettings();
		void SaveSettings();

		bool IsIconViewEnabled { get; set; }

		int PreviewSize { get; set; }
		float PreviewDensity { get; set; }

		IThreadController ThreadController { get; set; }
		IWaitImageController WaitImageController { get; set; }

		IEnumerable<Flame> Flames { get; set; }
		Flame SelectedFlame { get; set; }

		event FlameRenameEventHandler FlameRenamed;
		event FlameSelectEventHandler FlameSelected;

		void ReplaceCurrent(Flame newFlame);
	}
}