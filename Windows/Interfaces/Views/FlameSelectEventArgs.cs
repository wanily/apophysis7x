using System;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Interfaces.Views
{
	public class FlameSelectEventArgs : EventArgs
	{
		private readonly Flame mFlame;

		public FlameSelectEventArgs(Flame flame)
		{
			mFlame = flame;
		}

		public Flame Flame
		{
			get { return mFlame; }
		}
	}
}