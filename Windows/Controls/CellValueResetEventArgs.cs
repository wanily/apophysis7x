using System;

namespace Xyrus.Apophysis.Windows.Controls
{
	public class CellValueResetEventArgs : EventArgs
	{
		public CellValueResetEventArgs(int column, int row, double value)
		{
			if (column < 0) throw new ArgumentOutOfRangeException("column");
			if (row < 0) throw new ArgumentOutOfRangeException("row");

			Column = column;
			Row = row;
			Value = value;
		}

		public int Column { get; private set; }
		public int Row { get; private set; }

		public double Value { get; set; }
	}
}