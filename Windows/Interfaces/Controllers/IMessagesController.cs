using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IMessagesController : IViewController
	{
		void Push(string message);
		void Summarize([NotNull] Flame flame);
	}
}