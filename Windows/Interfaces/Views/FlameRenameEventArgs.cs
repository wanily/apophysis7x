using System;

namespace Xyrus.Apophysis.Windows.Interfaces.Views
{
	public class FlameRenameEventArgs : EventArgs
	{
		private readonly string mLabel;

		public FlameRenameEventArgs(string label)
		{
			mLabel = label;
		}

		public string Label
		{
			get { return mLabel; }
		}

		public bool CancelEdit { get; set; }
	}
}