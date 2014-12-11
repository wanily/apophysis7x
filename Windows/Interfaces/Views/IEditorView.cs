using System;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Interfaces.Views
{
	public interface IEditorView : IWindow
	{
		IIteratorCanvasView Canvas { get; }
		void SetHeader(Flame flame);
		event EventHandler UndoEvent;
	}
}