using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Input
{
	static class NativeHookManager
	{
		// ReSharper disable InconsistentNaming
		private const int WH_MOUSE_LL = 14;
		private const int WH_KEYBOARD_LL = 13;
		private const int WM_LBUTTONDOWN = 0x201;
		private const int WM_RBUTTONDOWN = 0x204;
		private const int WM_LBUTTONUP = 0x202;
		private const int WM_RBUTTONUP = 0x205;
		private const int WM_LBUTTONDBLCLK = 0x203;
		private const int WM_RBUTTONDBLCLK = 0x206;
		private const int WM_MOUSEWHEEL = 0x020A;
		private const int WM_KEYDOWN = 0x100;
		private const int WM_KEYUP = 0x101;
		private const int WM_SYSKEYDOWN = 0x104;
		private const int WM_SYSKEYUP = 0x105;

		private const byte VK_SHIFT = 0x10;
		private const byte VK_CAPITAL = 0x14;
		// ReSharper restore InconsistentNaming
		
		private static MouseEventHandler mMouseMove;
		private static MouseEventHandler mMouseClick;
		private static MouseEventHandler mMouseDown;
		private static MouseEventHandler mMouseUp;
		private static MouseEventHandler mMouseWheel;
		private static MouseEventHandler mMouseDoubleClick;

		private static MouseButtons mPrevClickedButton;
		private static Timer mDoubleClickTimer;

		private static KeyPressEventHandler mKeyPress;
		private static KeyEventHandler mKeyUp;
		private static KeyEventHandler mKeyDown;

		private static HookProc mMouseDelegate;
		private static int mMouseHookHandle;

		private static int mOldX;
		private static int mOldY;
		private static HookProc mKeyboardDelegate;
		private static int mKeyboardHookHandle;
		private static bool mFinishHandling;

		public static event MouseEventHandler MouseMove
		{
			add
			{
				EnsureSubscribedToGlobalMouseEvents();
				mMouseMove += value;
			}

			remove
			{
				mMouseMove -= value;
				TryUnsubscribeFromGlobalMouseEvents();
			}
		}
		public static event MouseEventHandler MouseClick
		{
			add
			{
				EnsureSubscribedToGlobalMouseEvents();
				mMouseClick += value;
			}
			remove
			{
				mMouseClick -= value;
				TryUnsubscribeFromGlobalMouseEvents();
			}
		}
		public static event MouseEventHandler MouseDown
		{
			add
			{
				EnsureSubscribedToGlobalMouseEvents();
				mMouseDown += value;
			}
			remove
			{
				mMouseDown -= value;
				TryUnsubscribeFromGlobalMouseEvents();
			}
		}
		public static event MouseEventHandler MouseUp
		{
			add
			{
				EnsureSubscribedToGlobalMouseEvents();
				mMouseUp += value;
			}
			remove
			{
				mMouseUp -= value;
				TryUnsubscribeFromGlobalMouseEvents();
			}
		}
		public static event MouseEventHandler MouseWheel
		{
			add
			{
				EnsureSubscribedToGlobalMouseEvents();
				mMouseWheel += value;
			}
			remove
			{
				mMouseWheel -= value;
				TryUnsubscribeFromGlobalMouseEvents();
			}
		}
		public static event MouseEventHandler MouseDoubleClick
		{
			add
			{
				EnsureSubscribedToGlobalMouseEvents();
				if (mMouseDoubleClick == null)
				{
					//We create a timer to monitor interval between two clicks
					mDoubleClickTimer = new Timer
					{
						//This interval will be set to the value we retrive from windows. This is a windows setting from contro planel.
						Interval = GetDoubleClickTime(),
						//We do not start timer yet. It will be start when the click occures.
						Enabled = false
					};
					//We define the callback function for the timer
					mDoubleClickTimer.Tick += DoubleClickTimeElapsed;
					//We start to monitor mouse up event.
					MouseUp += OnMouseUp;
				}
				mMouseDoubleClick += value;
			}
			remove
			{
				if (mMouseDoubleClick != null)
				{
					mMouseDoubleClick -= value;
					if (mMouseDoubleClick == null)
					{
						//Stop monitoring mouse up
						MouseUp -= OnMouseUp;
						//Dispose the timer
						mDoubleClickTimer.Tick -= DoubleClickTimeElapsed;
						mDoubleClickTimer = null;
					}
				}
				TryUnsubscribeFromGlobalMouseEvents();
			}
		}

		private static void DoubleClickTimeElapsed(object sender, EventArgs e)
		{
			//Timer is alapsed and no second click ocuured
			mDoubleClickTimer.Enabled = false;
			mPrevClickedButton = MouseButtons.None;
		}
		private static void OnMouseUp(object sender, MouseEventArgs e)
		{
			//This should not heppen
			if (e.Clicks < 1)
			{
				return;
			}
			//If the second click happened on the same button
			if (e.Button.Equals(mPrevClickedButton))
			{
				if (mMouseDoubleClick != null)
				{
					//Fire double click
					mMouseDoubleClick.Invoke(null, e);
				}
				//Stop timer
				mDoubleClickTimer.Enabled = false;
				mPrevClickedButton = MouseButtons.None;
			}
			else
			{
				//If it was the firts click start the timer
				mDoubleClickTimer.Enabled = true;
				mPrevClickedButton = e.Button;
			}
		}

		public static event KeyPressEventHandler KeyPress
		{
			add
			{
				EnsureSubscribedToGlobalKeyboardEvents();
				mKeyPress += value;
			}
			remove
			{
				mKeyPress -= value;
				TryUnsubscribeFromGlobalKeyboardEvents();
			}
		}
		public static event KeyEventHandler KeyUp
		{
			add
			{
				EnsureSubscribedToGlobalKeyboardEvents();
				mKeyUp += value;
			}
			remove
			{
				mKeyUp -= value;
				TryUnsubscribeFromGlobalKeyboardEvents();
			}
		}
		public static event KeyEventHandler KeyDown
		{
			add
			{
				EnsureSubscribedToGlobalKeyboardEvents();
				mKeyDown += value;
			}
			remove
			{
				mKeyDown -= value;
				TryUnsubscribeFromGlobalKeyboardEvents();
			}
		}

		private static int MouseHookProc(int nCode, int wParam, IntPtr lParam)
		{
			if (nCode >= 0)
			{
				//Marshall the data from callback.
				var mouseHookStruct = (MouseLLHookStruct) Marshal.PtrToStructure(lParam, typeof (MouseLLHookStruct));

				//detect button clicked
				var button = MouseButtons.None;
				var mouseDelta = (short) 0;
				int clickCount = 0;
				bool mouseDown = false;
				bool mouseUp = false;

				switch (wParam)
				{
					case WM_LBUTTONDOWN:
						mouseDown = true;
						button = MouseButtons.Left;
						clickCount = 1;
						break;
					case WM_LBUTTONUP:
						mouseUp = true;
						button = MouseButtons.Left;
						clickCount = 1;
						break;
					case WM_LBUTTONDBLCLK:
						button = MouseButtons.Left;
						clickCount = 2;
						break;
					case WM_RBUTTONDOWN:
						mouseDown = true;
						button = MouseButtons.Right;
						clickCount = 1;
						break;
					case WM_RBUTTONUP:
						mouseUp = true;
						button = MouseButtons.Right;
						clickCount = 1;
						break;
					case WM_RBUTTONDBLCLK:
						button = MouseButtons.Right;
						clickCount = 2;
						break;
					case WM_MOUSEWHEEL:
						//If the message is WM_MOUSEWHEEL, the high-order word of MouseData member is the wheel delta. 
						//One wheel click is defined as WHEEL_DELTA, which is 120. 
						//(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
						mouseDelta = (short) ((mouseHookStruct.MouseData >> 16) & 0xffff);
						break;
				}

				//generate event 
				var e = new MouseEventArgs(
					button,
					clickCount,
					mouseHookStruct.Point.X,
					mouseHookStruct.Point.Y,
					mouseDelta);

				//Mouse up
				if (mMouseUp != null && mouseUp)
				{
					mMouseUp.Invoke(null, e);
				}

				//Mouse down
				if (mMouseDown != null && mouseDown)
				{
					mMouseDown.Invoke(null, e);
				}

				//If someone listens to click and a click is heppened
				if (mMouseClick != null && clickCount > 0)
				{
					mMouseClick.Invoke(null, e);
				}

				//If someone listens to double click and a click is heppened
				if (mMouseDoubleClick != null && clickCount == 2)
				{
					mMouseDoubleClick.Invoke(null, e);
				}

				if ((mMouseMove != null) && (mOldX != mouseHookStruct.Point.X || mOldY != mouseHookStruct.Point.Y))
				{
					mOldX = mouseHookStruct.Point.X;
					mOldY = mouseHookStruct.Point.Y;
					if (mMouseMove != null)
					{
						mMouseMove.Invoke(null, e);
					}
				}

				//Wheel was moved
				if (mMouseWheel != null && mouseDelta != 0)
				{
					mMouseWheel.Invoke(null, e);
				}

				if (mFinishHandling)
				{
					mFinishHandling = false;
					return -1;
				}
			}

			//call next hook
			return CallNextHookEx(mMouseHookHandle, nCode, wParam, lParam);
		}

		private static void EnsureSubscribedToGlobalMouseEvents()
		{
			// install Mouse hook only if it is not installed and must be installed
			if (mMouseHookHandle == 0)
			{
				//See comment of this field. To avoid GC to clean it up.
				mMouseDelegate = MouseHookProc;
				//install hook
				mMouseHookHandle = SetWindowsHookEx(
					WH_MOUSE_LL,
					mMouseDelegate,
					IntPtr.Zero, /*Marshal.GetHINSTANCE(
                        Assembly.GetAssembly(typeof(GlobalHookManager)).GetModules()[0]),*/
					0);
				//If SetWindowsHookEx fails.
				if (mMouseHookHandle == 0)
				{
					//Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
					int errorCode = Marshal.GetLastWin32Error();
					//do cleanup

					//Initializes and throws a new instance of the Win32Exception class with the specified error. 
					throw new Win32Exception("SetWindowsHookEx failed: " + (new Win32Exception(errorCode)).Message);
				}
			}
		}
		private static void TryUnsubscribeFromGlobalMouseEvents()
		{
			//if no subsribers are registered unsubsribe from hook
			if (mMouseClick == null &&
			    mMouseDown == null &&
			    mMouseMove == null &&
			    mMouseUp == null &&
			    mMouseWheel == null)
			{
				ForceUnsunscribeFromGlobalMouseEvents();
			}
		}
		private static void ForceUnsunscribeFromGlobalMouseEvents()
		{
			if (mMouseHookHandle != 0)
			{
				//uninstall hook
				int result = UnhookWindowsHookEx(mMouseHookHandle);
				//reset invalid handle
				mMouseHookHandle = 0;
				//Free up for GC
				mMouseDelegate = null;
				//if failed and exception must be thrown
				if (result == 0)
				{
					//Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
					int errorCode = Marshal.GetLastWin32Error();
					//Initializes and throws a new instance of the Win32Exception class with the specified error. 
					throw new Win32Exception(errorCode);
				}
			}
		}

		private static int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
		{
			//indicates if any of underlaing events set e.Handled flag
			bool handled = false;

			if (nCode >= 0)
			{
				//read structure KeyboardHookStruct at lParam
				var myKeyboardHookStruct = (KeyboardHookStruct) Marshal.PtrToStructure(lParam, typeof (KeyboardHookStruct));
				//raise KeyDown
				if (mKeyDown != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
				{
					var keyData = (Keys) myKeyboardHookStruct.VirtualKeyCode;
					var e = new KeyEventArgs(keyData);
					mKeyDown.Invoke(null, e);
					handled = e.Handled;
				}

				// raise KeyPress
				if (mKeyPress != null && wParam == WM_KEYDOWN)
				{
					bool isDownShift = ((GetKeyState(VK_SHIFT) & 0x80) == 0x80);
					bool isDownCapslock = (GetKeyState(VK_CAPITAL) != 0);

					var keyState = new byte[256];
					GetKeyboardState(keyState);
					var inBuffer = new byte[2];
					if (ToAscii(myKeyboardHookStruct.VirtualKeyCode,
						myKeyboardHookStruct.ScanCode,
						keyState,
						inBuffer,
						myKeyboardHookStruct.Flags) == 1)
					{
						var key = (char) inBuffer[0];
						if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
						var e = new KeyPressEventArgs(key);
						mKeyPress.Invoke(null, e);
						handled = handled || e.Handled;
					}
				}

				// raise KeyUp
				if (mKeyUp != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
				{
					var keyData = (Keys) myKeyboardHookStruct.VirtualKeyCode;
					var e = new KeyEventArgs(keyData);
					mKeyUp.Invoke(null, e);
					handled = handled || e.Handled;
				}
			}

			//if event handled in application do not handoff to other listeners
			if (handled)
				return -1;

			//forward to other application
			return CallNextHookEx(mKeyboardHookHandle, nCode, wParam, lParam);
		}

		private static void EnsureSubscribedToGlobalKeyboardEvents()
		{
			// install Keyboard hook only if it is not installed and must be installed
			if (mKeyboardHookHandle == 0)
			{
				//See comment of this field. To avoid GC to clean it up.
				mKeyboardDelegate = KeyboardHookProc;
				//install hook
				mKeyboardHookHandle = SetWindowsHookEx(
					WH_KEYBOARD_LL,
					mKeyboardDelegate,
					IntPtr.Zero /*Marshal.GetHINSTANCE(
                        Assembly.GetExecutingAssembly().GetModules()[0])*/,
					0);
				//If SetWindowsHookEx fails.
				if (mKeyboardHookHandle == 0)
				{
					//Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
					int errorCode = Marshal.GetLastWin32Error();
					//do cleanup

					//Initializes and throws a new instance of the Win32Exception class with the specified error. 
					throw new Win32Exception(errorCode);
				}
			}
		}
		private static void TryUnsubscribeFromGlobalKeyboardEvents()
		{
			//if no subsribers are registered unsubsribe from hook
			if (mKeyDown == null &&
			    mKeyUp == null &&
			    mKeyPress == null)
			{
				ForceUnsunscribeFromGlobalKeyboardEvents();
			}
		}
		private static void ForceUnsunscribeFromGlobalKeyboardEvents()
		{
			if (mKeyboardHookHandle != 0)
			{
				//uninstall hook
				int result = UnhookWindowsHookEx(mKeyboardHookHandle);
				//reset invalid handle
				mKeyboardHookHandle = 0;
				//Free up for GC
				mKeyboardDelegate = null;
				//if failed and exception must be thrown
				if (result == 0)
				{
					//Returns the error code returned by the last unmanaged function called using platform invoke that has the DllImportAttribute.SetLastError flag set. 
					int errorCode = Marshal.GetLastWin32Error();
					//Initializes and throws a new instance of the Win32Exception class with the specified error. 
					throw new Win32Exception(errorCode);
				}
			}
		}

		[DllImport("user32", CharSet = CharSet.Auto,
			CallingConvention = CallingConvention.StdCall)]
		private static extern int CallNextHookEx(
			int idHook,
			int nCode,
			int wParam,
			IntPtr lParam);

		[DllImport("user32", CharSet = CharSet.Auto,
			CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		private static extern int SetWindowsHookEx(
			int idHook,
			HookProc lpfn,
			IntPtr hMod,
			int dwThreadId);

		[DllImport("user32", CharSet = CharSet.Auto,
			CallingConvention = CallingConvention.StdCall, SetLastError = true)]
		private static extern int UnhookWindowsHookEx(int idHook);

		[DllImport("user32")]
		private static extern int GetDoubleClickTime();

		[DllImport("user32")]
		private static extern int ToAscii(
			int uVirtKey,
			int uScanCode,
			byte[] lpbKeyState,
			byte[] lpwTransKey,
			int fuState);

		[DllImport("user32")]
		private static extern int GetKeyboardState(byte[] pbKeyState);

		[DllImport("user32.dll", CharSet = CharSet.Auto, 
			CallingConvention = CallingConvention.StdCall)]
		public static extern short GetKeyState(int vKey);

		[DllImport("user32.dll")]
		public static extern bool SetCursorPos(int x, int y);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetCursorPos(out Point lpPoint);

		private delegate int HookProc(int nCode, int wParam, IntPtr lParam);

		[StructLayout(LayoutKind.Sequential)]
		struct IntPoint
		{
			public readonly int X;
			public readonly int Y;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct KeyboardHookStruct
		{
			public readonly int VirtualKeyCode;
			public readonly int ScanCode;
			public readonly int Flags;

			private readonly int Time;
			private readonly int ExtraInfo;
		}

		[StructLayout(LayoutKind.Sequential)]
		struct MouseLLHookStruct
		{
			public IntPoint Point;
			public readonly int MouseData;

			private readonly int Flags;
			private readonly int Time;
			private readonly int ExtraInfo;
		}
	}
}