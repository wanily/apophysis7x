using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Xyrus.Apophysis.Calculation;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Input;

namespace Xyrus.Apophysis.Windows.Controls
{
	public partial class DragGrid : DataGridView
	{
		private readonly InputController mInputHandler;
		private DataGridViewTextBoxColumn mNameColumn;
		private DataGridViewTextBoxColumn mValueColumn;
		private double mStart;
		private Point mCursorStart;
		private bool mIsDragging;
		private bool mIsMouseOver;

		public DragGrid()
		{
			InitializeComponent();

			mInputHandler = new InputController();

			KeyPress += OnKeyPress;
			CellMouseDown += OnCellMouseDown;
			CellMouseMove += OnCellMouseMove;
			CellMouseDoubleClick += OnCellDoubleClick;
			CellEndEdit += OnCellLeft;
			CellBeginEdit += OnCellEntered;

			AllowUserToAddRows = false;
			AllowUserToDeleteRows = false;
			AllowUserToOrderColumns = true;
			AllowUserToResizeRows = false;
			BackgroundColor = SystemColors.Window;
			BorderStyle = BorderStyle.None;
			ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			Columns.AddRange(new DataGridViewColumn[]
			{
				mNameColumn = new DataGridViewTextBoxColumn { HeaderText = "Name", ReadOnly = true }, 
				mValueColumn = new DataGridViewTextBoxColumn { HeaderText = "Value", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill }
			});
			EditMode = DataGridViewEditMode.EditOnEnter;
			EnableHeadersVisualStyles = false;
			GridColor = SystemColors.ControlLight;
			MultiSelect = false;
			RowHeadersVisible = false;
			ScrollBars = ScrollBars.Vertical;
			SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			ShowCellErrors = false;
			ShowCellToolTips = false;
			ShowEditingIcon = false;
			ShowRowErrors = false;
		}

		public event EventHandler ValueChanged;
		public event EventHandler BeginDragEdit;
		public event EventHandler EndDragEdit;

		private void OnKeyPress(object sender, KeyPressEventArgs e)
		{
			mInputHandler.HandleKeyPressForNumericTextBox(e);
		}

		private void OnCellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex != 0)
				return;

			Cursor.Current = Cursors.VSplit;
			MouseInputManager.MouseUp += OnCellGlobalMouseUp;

			double value;
			if (!double.TryParse(Rows[e.RowIndex].Cells[1].Value as string ?? string.Empty, NumberStyles.Float, InputController.Culture, out value))
				value = 0.0;

			mStart = value;
			mCursorStart = MouseInputManager.GetPosition();
			mIsDragging = true;

			if (BeginDragEdit != null)
				BeginDragEdit(this, new EventArgs());
		}
		private void OnCellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (!mIsDragging || e.ColumnIndex != 0)
				return;

			var pos = MouseInputManager.GetPosition();
			var delta = pos.X - (double)mCursorStart.X;
			var multiplier = 1000.0;

			if (KeyboardInputManager.GetKeyState(Keys.Menu))
				multiplier = 100000;

			else if (
				KeyboardInputManager.GetKeyState(Keys.LControlKey) ||
				KeyboardInputManager.GetKeyState(Keys.RControlKey)
			) multiplier = 10000;

			else if (
				KeyboardInputManager.GetKeyState(Keys.LShiftKey) ||
				KeyboardInputManager.GetKeyState(Keys.RShiftKey)
			) multiplier = 100;

			var newValue = System.Math.Round(mStart + delta / multiplier, 3);
			var row = e.RowIndex;

			//todo
			Rows[row].Cells[1].Value = newValue.ToString(InputController.PreciseFormat, InputController.Culture);
		}
		private void OnCellDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex != 0)
				return;

			double value;
			if (!double.TryParse(Rows[e.RowIndex].Cells[1].Value as string ?? string.Empty, NumberStyles.Float, InputController.Culture, out value))
				value = 0.0;

			if (System.Math.Abs(value) < double.Epsilon)
				value = 1.0;
			else
				value = 0.0;

			Rows[e.RowIndex].Cells[1].Value = value.ToString(InputController.PreciseFormat, InputController.Culture);

			if (ValueChanged != null)
			{
				ValueChanged(this, new EventArgs());
			}
		}
		private void OnCellLeft(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex != 1)
				return;

			if (EndDragEdit != null)
				EndDragEdit(this, new EventArgs());
		}
		private void OnCellEntered(object sender, DataGridViewCellCancelEventArgs e)
		{
			if (e.ColumnIndex != 1)
				return;

			if (BeginDragEdit != null)
				BeginDragEdit(this, new EventArgs());
		}

		private void OnCellGlobalMouseUp(MouseHookEventArgs args)
		{
			MouseInputManager.MouseUp -= OnCellGlobalMouseUp;

			Cursor.Current = mIsMouseOver ? Cursors.Hand : null;
			mIsDragging = false;
			mStart = 0.0;

			if (EndDragEdit != null)
				EndDragEdit(this, new EventArgs());
		}
	}
}
