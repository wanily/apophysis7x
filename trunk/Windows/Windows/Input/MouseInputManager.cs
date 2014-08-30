using System.Drawing;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Input
{
	static class MouseInputManager
	{
		static MouseInputManager()
		{
			MouseMove += (e) => { };
			MouseClick += (e) => { };
			MouseDown += (e) => { };
			MouseUp += (e) => { };
			MouseWheel += (e) => { };
			MouseDoubleClick += (e) => { };

			NativeHookManager.MouseMove += (o, e) => MouseMove(new MouseHookEventArgs(e.X, e.Y, (e.Button), e.Delta));
			NativeHookManager.MouseClick += (o, e) => MouseClick(new MouseHookEventArgs(e.X, e.Y, (e.Button), e.Delta));
			NativeHookManager.MouseDown += (o, e) => MouseDown(new MouseHookEventArgs(e.X, e.Y, (e.Button), e.Delta));
			NativeHookManager.MouseUp += (o, e) => MouseUp(new MouseHookEventArgs(e.X, e.Y, (e.Button), e.Delta));
			NativeHookManager.MouseWheel += (o, e) => MouseWheel(new MouseHookEventArgs(e.X, e.Y, (e.Button), e.Delta));
			NativeHookManager.MouseDoubleClick += (o, e) => MouseDoubleClick(new MouseHookEventArgs(e.X, e.Y, (e.Button), e.Delta));
		}

		public static event MouseHookEventHandler MouseMove;
		public static event MouseHookEventHandler MouseClick;
		public static event MouseHookEventHandler MouseDown;
		public static event MouseHookEventHandler MouseUp;
		public static event MouseHookEventHandler MouseWheel;
		public static event MouseHookEventHandler MouseDoubleClick;

		public static Point GetPosition()
		{
			return new Point(Cursor.Position.X, Cursor.Position.Y);
		}
		public static void SetPosition(Point p)
		{
			Cursor.Position = p;
		}
	}
}