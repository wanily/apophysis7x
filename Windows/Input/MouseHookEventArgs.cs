using System;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Input
{
	sealed class MouseHookEventArgs : EventArgs
    {
		public MouseHookEventArgs(int x, int y, MouseButtons button, float delta)
        {
            Button = button;
            X = x;
            Y = y;
            Delta = delta;
        }

		public int X
		{
            get;
            private set;
        }
		public int Y
		{
            get;
            private set;
        }
		public float Delta
        {
            get;
            private set;
        }
        public MouseButtons Button {
            get;
            private set;
        }
    }
}
