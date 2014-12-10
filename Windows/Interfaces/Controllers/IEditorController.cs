using System;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IEditorController : IViewController
	{
		void UpdateCoordinates();
		void UpdateActiveMatrix();
		void UpdateToolbar();
		void UpdateVariations();
		void UpdateVariables();
		void UpdateColor();
		void UpdateWindowTitle();
		void UpdatePreview();

		IUndoController UndoController { get; }

		Flame Flame { get; set; }
		Iterator Iterator { get; set; }

		event EventHandler FlameChanged;
		void RaiseFlameChanged();
	}
}