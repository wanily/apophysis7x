using System;

namespace Xyrus.Apophysis.Windows.Controls
{
	public class CellValueChangedEventArgs : EventArgs
	{
		public CellValueChangedEventArgs(int column, int row, float value)
		{
			if (column < 0) throw new ArgumentOutOfRangeException("column");
			if (row < 0) throw new ArgumentOutOfRangeException("row");

			Column = column;
			Row = row;
			Value = value;
		}

		public int Column { get; private set; }
		public int Row { get; private set; }

		public float Value { get; set; }
		public bool Cancel { get; set; }
	}
}