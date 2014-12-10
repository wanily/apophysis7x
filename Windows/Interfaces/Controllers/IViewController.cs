namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IViewController : IController
	{
		void Initialize();
		object View { get; }
	}
}