using System;
using System.Drawing;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Input;

namespace Xyrus.Apophysis.Windows.Interfaces.Views
{
	public interface IMainView : IWindow
	{
		bool CameraEditUseScale { get; set; }
		bool ShowGuidelines { get; set; }
		bool ShowTransparency { get; set; }
		bool FitFrame { get; set; }

		Flame PreviewedFlame { get; set; }
		Bitmap PreviewImage { get; set; }

		CameraEditMode CameraEditMode { get; set; }

		event EventHandler CameraBeginEdit;
		event CameraEndEditEventHandler CameraEndEdit;
		event CameraChangedEventHandler CameraChanged;

		IFlameListView FlameListView { get; }
	}
}