using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IAutosaveController : IController
	{
		void ForceCommit(Flame flame, string name);
	}
}