namespace Xyrus.Apophysis.Windows.Interfaces
{
	public interface IViewController : IController
	{
		void Initialize();
		object View { get; }
	}
}