using System;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Interfaces.Controllers
{
	public interface IUndoController : IController
	{
		void Reset([NotNull] Flame flame);
		void CommitChange([NotNull] Flame flame);

		bool CanUndo { get; }
		bool CanRedo { get; }

		void Undo();
		void Redo();

		event EventHandler StackChanged;
		event EventHandler CurrentReplaced;
		event EventHandler ChangeCommitted;

		Flame RequestCurrent();
	}
}