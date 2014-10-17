using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Input;

namespace Xyrus.Apophysis.Windows.Controls
{
	[PublicAPI]
	public partial class DragGrid : DataGridView
	{
		private readonly InputController mInputHandler;
		private readonly Lock mInternalEdit;

		private double mStart, mLastValue;
		private Point mCursorStart;
		private bool mIsDragging;
		private bool mIsMouseOver;
		private int mDragColumn;
		private int mDragRow;

		public DragGrid()
		{
			InitializeComponent();

			mInputHandler = new InputController();
			mInternalEdit = new Lock();

			AllowUserToAddRows = false;
			AllowUserToDeleteRows = false;
			AllowUserToOrderColumns = true;
			AllowUserToResizeRows = false;

			EnableHeadersVisualStyles = false;
			MultiSelect = false;
			RowHeadersVisible = false;

			ShowCellErrors = false;
			ShowCellToolTips = false;
			ShowEditingIcon = false;
			ShowRowErrors = false;

			ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
			ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			EditMode = DataGridViewEditMode.EditOnEnter;

			Columns.AddRange(new DataGridViewColumn[]
			{
				new DataGridViewTextBoxColumn { HeaderText = ControlResources.DragGridNameColumnHeader, ReadOnly = true }, 
				new DataGridViewTextBoxColumn { HeaderText = ControlResources.DragGridValueColumnHeader, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill }
			});

			ScrollBars = ScrollBars.Vertical;
			SelectionMode = DataGridViewSelectionMode.FullRowSelect;
		}

		public new event CellValueChangedEventHandler CellValueChanged;
		public event CellValueResetEventHandler CellValueReset;

		public new event EventHandler CellBeginEdit;
		public new event EventHandler CellEndEdit;

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new DataGridViewColumnCollection Columns
		{
			get { return base.Columns; }
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new DataGridViewRowCollection Rows
		{
			get { return base.Rows; }
		}

		public double this[int row]
		{
			get
			{
				if (row < 0)
					return 0;

				double value;
				if (!double.TryParse(Rows[row].Cells[1].Value as string ?? string.Empty, NumberStyles.Float, InputController.Culture, out value))
					value = 0.0;

				return value;
			}
			set
			{
				if (row < 0)
					return;

				Rows[row].Cells[1].Value = value.ToString(InputController.PreciseFormat, InputController.Culture);
			}
		}
		public DragGridResetMode ResetMode
		{
			get; 
			set;
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			base.OnKeyPress(e);
			mInputHandler.HandleKeyPressForNumericTextBox(e);
		}

		protected override void OnCellMouseEnter(DataGridViewCellEventArgs e)
		{
			base.OnCellMouseEnter(e);
			if (e.ColumnIndex != 0 || e.RowIndex < 0)
				return;

			mIsMouseOver = true;
			Cursor.Current = Cursors.Hand;
		}
		protected override void OnCellMouseLeave(DataGridViewCellEventArgs e)
		{
			base.OnCellMouseLeave(e);
			if (e.ColumnIndex != 0 || e.RowIndex < 0)
				return;

			mIsMouseOver = false;
			if (!mIsDragging)
			{
				Cursor.Current = null;
			}
		}

		protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
		{
			base.OnCellMouseDown(e);
			if (e.ColumnIndex != 0 || e.RowIndex < 0)
				return;

			Cursor.Current = Cursors.VSplit;
			MouseInputManager.MouseMove += OnCellGlobalMouseMove;
			MouseInputManager.MouseUp += OnCellGlobalMouseUp;

			mStart = mLastValue = this[e.RowIndex];
			mCursorStart = MouseInputManager.GetPosition();
			mIsDragging = true;
			mDragColumn = e.ColumnIndex;
			mDragRow = e.RowIndex;

			if (CellBeginEdit != null)
				CellBeginEdit(this, new EventArgs());
		}
		protected override void OnCellMouseDoubleClick(DataGridViewCellMouseEventArgs e)
		{
			base.OnCellMouseDoubleClick(e);
			if (e.ColumnIndex != 0 || e.RowIndex < 0)
				return;

			var value = mLastValue = this[e.RowIndex];
			var args = new CellValueResetEventArgs(e.ColumnIndex, e.RowIndex, 1);

			if (CellValueReset != null)
			{
				CellValueReset(this, args);
			}

			if (ResetMode == DragGridResetMode.Toggle && System.Math.Abs(args.Value - value) < double.Epsilon)
			{
				value = 0;
			}
			else
			{
				value = args.Value;
			}
			

			if (CellBeginEdit != null)
			{
				CellBeginEdit(this, new EventArgs());
			}

			this[e.RowIndex] = value;

			if (CellEndEdit != null)
			{
				CellEndEdit(this, new EventArgs());
			}
		}

		protected override void OnCellEnter(DataGridViewCellEventArgs e)
		{
			base.OnCellEnter(e);
			if (e.ColumnIndex != 1 || e.RowIndex < 0)
				return;

			mLastValue = this[e.RowIndex];

			if (CellBeginEdit != null)
				CellBeginEdit(this, new EventArgs());
		}
		protected override void OnCellLeave(DataGridViewCellEventArgs e)
		{
			base.OnCellLeave(e);
			if (e.ColumnIndex != 1 || e.RowIndex < 0)
				return;

			if (CellEndEdit != null)
				CellEndEdit(this, new EventArgs());
		}

		protected override void OnCellValueChanged(DataGridViewCellEventArgs e)
		{
			base.OnCellValueChanged(e);
			if (e.ColumnIndex != 1 || mInternalEdit.IsBusy || e.RowIndex < 0)
				return;

			var value = this[e.RowIndex];
			var args = new CellValueChangedEventArgs(e.ColumnIndex, e.RowIndex, value);

			if (CellValueChanged != null)
			{
				CellValueChanged(this, args);
			}

			if (args.Cancel)
			{
				using (mInternalEdit.Enter())
				{
					this[e.RowIndex] = mLastValue;
				}
				return;
			}

			if (!Equals(args.Value, value))
			{
				using (mInternalEdit.Enter())
				{
					this[e.RowIndex] = args.Value;
				}
			}
		}

		protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
		{
			base.OnCellEndEdit(e);
			if (e.ColumnIndex != 1 || e.RowIndex < 0)
				return;

			//looks funny...just to reformat the cell content
			using (mInternalEdit.Enter())
			{
				this[e.RowIndex] = this[e.RowIndex];
			}
		}

		private void OnCellGlobalMouseMove(MouseHookEventArgs e)
		{
			if (!mIsDragging || mDragColumn != 0)
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

			this[mDragRow] = System.Math.Round(mStart + delta / multiplier, 3);
		}
		private void OnCellGlobalMouseUp(MouseHookEventArgs e)
		{
			MouseInputManager.MouseMove -= OnCellGlobalMouseMove;
			MouseInputManager.MouseUp -= OnCellGlobalMouseUp;

			Cursor.Current = mIsMouseOver ? Cursors.Hand : null;
			mIsDragging = false;
			mStart = 0.0;
			mDragColumn = 0;
			mDragRow = 0;

			if (CellEndEdit != null)
				CellEndEdit(this, new EventArgs());
		}
	}
}
