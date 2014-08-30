using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Input
{
	static class KeyboardInputManager
	{
		static KeyboardInputManager()
		{
			KeyUp += (e) => { };
			KeyDown += (e) => { };

			NativeHookManager.KeyDown += (o, e) => KeyDown(new KeyboardHookEventArgs((int)e.KeyData));
			NativeHookManager.KeyUp += (o, e) => KeyUp(new KeyboardHookEventArgs((int)e.KeyData));
		}

		public static event KeyboardHookEventHandler KeyUp;
		public static event KeyboardHookEventHandler KeyDown;

		public static bool GetKeyState(Keys vk)
		{
			return NativeHookManager.GetKeyState((int)vk) < 0;
		}
	}
}