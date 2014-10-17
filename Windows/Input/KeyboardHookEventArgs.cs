using System;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Input
{
	sealed class KeyboardHookEventArgs : EventArgs
	{
		public KeyboardHookEventArgs(Int32 vKeyFlags)
		{
			KeyFlags = (Keys)vKeyFlags;
		}
		public Keys KeyFlags
		{
			get;
			private set;
		}

		public Boolean AltKey
		{
			get { return ((KeyFlags & Keys.LMenu) != 0) || ((KeyFlags & Keys.RMenu) != 0); }
		}
		public Boolean ShiftKey
		{
			get { return ((KeyFlags & Keys.LShiftKey) != 0) || ((KeyFlags & Keys.RShiftKey) != 0); }
		}
		public Boolean ControlKey
		{
			get { return ((KeyFlags & Keys.LControlKey) != 0) || ((KeyFlags & Keys.RControlKey) != 0); }
		}
	}
}