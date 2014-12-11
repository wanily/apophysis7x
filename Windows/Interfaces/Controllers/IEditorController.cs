
namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IEditorController : IViewController
	{
		void UpdateWindowTitle();

		IEditorPreviewController Preview { get; }
		IEditorToolbarController Toolbar { get; }
		IIteratorController IteratorControls { get; }
	}
}