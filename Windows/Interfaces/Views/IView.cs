using System.ComponentModel;

namespace Xyrus.Apophysis.Windows.Interfaces.Views
{
	public interface IView : IComponent
	{
		void Show();
		void Hide();
		void Close();
	}
}