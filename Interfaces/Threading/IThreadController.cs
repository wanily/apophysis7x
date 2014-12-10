using System;
using System.Threading;
using Xyrus.Apophysis.Windows.Interfaces.Controllers;

namespace Xyrus.Apophysis.Interfaces.Threading
{
	public interface IThreadController : IController
	{
		ThreadPriority Priority { get; set; }

		bool IsRunning { get; }
		bool IsSuspended { get; }
		bool IsCancelling { get; }

		InvokeCallbackMode InvokeCallbackMode { get; set; }

		void StartThread(Action threadAction, Action callback = null);
		void StartThread(Action<IThreadStateToken> threadAction, Action callback = null);
		void StartThread<T>(Func<T> threadAction, Action<T> callback = null);
		void StartThread<T>(Func<IThreadStateToken, T> threadAction, Action<T> callback = null, Action completionCallback = null, Action cancelledCallback = null);

		void Suspend();
		void Resume();
		void Wait();
		void Cancel();
	}
}