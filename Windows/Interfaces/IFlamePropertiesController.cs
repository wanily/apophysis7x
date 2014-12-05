using System;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;

namespace Xyrus.Apophysis.Windows.Interfaces
{
	public interface IFlamePropertiesController : IViewController
	{
		void UpdateToolbar();
		void UpdateWindowTitle();
		void UpdatePreview();
		void UpdateCamera();
		void UpdatePalette();

		FlamePropertiesPreviewController PreviewController { get; }
		IUndoController UndoController { get; }

		Flame Flame { get; set; }
		event EventHandler FlameChanged;
		void RaiseFlameChanged();

		void ApplyCanvas(bool withResize);
		void CommitValue();
	}
}