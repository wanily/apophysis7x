using System;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Input;

namespace Xyrus.Apophysis.Windows.Controls
{
	public sealed partial class DragPanel : Label
	{
		private CultureInfo mDisplayCulture;
		private TextBox mTextBox;
		private bool mIsMouseOver;

		private double mStart;
		private Point mCursorStart;
		private bool mIsDragging;

		public DragPanel()
		{
			InitializeComponent();

			Cursor = Cursors.Hand;
			mDisplayCulture = CultureInfo.InvariantCulture;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}

				if (mTextBox != null)
				{
					mTextBox.KeyPress -= OnTextBoxKeyPress;
					mTextBox.TextChanged -= OnTextBoxChanged;
					mTextBox.GotFocus -= OnTextBoxGotFocus;
					mTextBox.LostFocus -= OnTextBoxLostFocus;
					mTextBox = null;
				}

				//MouseInputManager.MouseUp -= OnMouseUp;
			}
			base.Dispose(disposing);
		}

		[CanBeNull]
		public TextBox TextBox
		{
			get { return mTextBox; }
			set
			{
				double oldValue;

				if (mTextBox != null)
				{
					mTextBox.KeyPress -= OnTextBoxKeyPress;
					mTextBox.TextChanged -= OnTextBoxChanged;
					mTextBox.GotFocus -= OnTextBoxGotFocus;
					mTextBox.LostFocus -= OnTextBoxLostFocus;

					if (!double.TryParse(mTextBox.Text, NumberStyles.Float, DisplayCulture, out oldValue))
						oldValue = 0.0;
				}
				else oldValue = 0.0;
				
				mTextBox = value;

				if (mTextBox != null)
				{
					mTextBox.KeyPress += OnTextBoxKeyPress;
					mTextBox.TextChanged += OnTextBoxChanged;
					mTextBox.GotFocus += OnTextBoxGotFocus;
					mTextBox.LostFocus += OnTextBoxLostFocus;
					Value = oldValue;
				}
			}
		}

		[NotNull]
		public CultureInfo DisplayCulture
		{
			get { return mDisplayCulture; }
			set
			{
				if (value == null) throw new ArgumentNullException("value");

				if (TextBox == null)
				{
					mDisplayCulture = value;
				}
				else
				{
					double oldValue;
					if (!double.TryParse(TextBox.Text, NumberStyles.Float, mDisplayCulture, out oldValue))
						oldValue = 0.0;

					mDisplayCulture = value;

					Value = oldValue;
				}
			}
		}
		public Double Value
		{
			get
			{
				if (TextBox == null)
					return 0.0;

				double value;
				if (!double.TryParse(TextBox.Text, NumberStyles.Float, DisplayCulture, out value))
					return 0.0;

				return value;
			}
			set
			{
				if (TextBox == null)
					return;

				TextBox.Text = value.ToString("#,###,##0.000", DisplayCulture);
				TextBox.Refresh();
			}
		}

		public event EventHandler ValueChanged;
		public event EventHandler BeginEdit;
		public event EventHandler EndEdit;

		private void OnTextBoxLostFocus(object sender, EventArgs e)
		{
			if (EndEdit != null)
				EndEdit(this, new EventArgs());
		}
		private void OnTextBoxGotFocus(object sender, EventArgs e)
		{
			if (BeginEdit != null)
				BeginEdit(this, new EventArgs());
		}
		private void OnTextBoxChanged(object sender, EventArgs e)
		{
			if (ValueChanged != null)
				ValueChanged(this, new EventArgs());
		}
		private void OnTextBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			char[] chars =
			{
				'.', ',', '-', 'e', 'E', '+',
				'0', '1', '2', '3', '4', '5',
				'6', '7', '8', '9'
			};

			if (!chars.Contains(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		protected override void OnMouseEnter(EventArgs e)
		{
			mIsMouseOver = true;
			base.OnMouseEnter(e);
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			mIsMouseOver = false;
			base.OnMouseLeave(e);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			Cursor.Current = Cursors.VSplit;

			MouseInputManager.MouseUp += OnMouseUp;

			mStart = Value;
			mCursorStart = MouseInputManager.GetPosition();
			mIsDragging = true;

			if (BeginEdit != null)
				BeginEdit(this, new EventArgs());

			base.OnMouseDown(e);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (mIsDragging)
			{
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

				//var screen = Screen.FromPoint(pos);
				//var newPos = new Point(System.Math.Max(screen.Bounds.Left, System.Math.Min(pos.X + delta < 0 ? -1 : 1, screen.Bounds.Right)),pos.Y);

				//MouseInputManager.SetPosition(newPos);

				Value = newValue;
			}

			base.OnMouseMove(e);
		}

		private void OnMouseUp(MouseHookEventArgs args)
		{
			MouseInputManager.MouseUp -= OnMouseUp;

			Cursor.Current = mIsMouseOver ? Cursors.Hand : null;
			mIsDragging = false;
			mStart = Value;

			if (EndEdit != null)
				EndEdit(this, new EventArgs());
		}
	}
}
