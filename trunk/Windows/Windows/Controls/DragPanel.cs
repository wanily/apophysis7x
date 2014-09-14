using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Input;

namespace Xyrus.Apophysis.Windows.Controls
{
	public sealed partial class DragPanel : Label
	{
		private readonly InputController mInputHandler;

		private CultureInfo mDisplayCulture;
		private TextBox mTextBox;
		private bool mIsMouseOver;

		private double mStart;
		private Point mCursorStart;
		private bool mIsDragging;
		private double mDefault;
		private double mMinimum;
		private double mMaximum;
		private double mDragStepping;

		public DragPanel()
		{
			InitializeComponent();

			mDefault = 0;
			mMinimum = double.MinValue;
			mMaximum = double.MaxValue;
			mDragStepping = 0.1;

			Cursor = Cursors.Hand;
			mDisplayCulture = InputController.Culture;

			mInputHandler = new InputController();
			EnabledChanged += OnEnabledChanged;
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

				TextBox.Text = value.ToString(InputController.DefaultFormat, DisplayCulture);
				TextBox.Refresh();
			}
		}

		public Double Default
		{
			get { return mDefault; }
			set
			{
				if (value > mMaximum || value < mMinimum)
					throw new ArgumentOutOfRangeException("value");

				mDefault = value;
				Value = value;
			}
		}
		public Double Minimum
		{
			get { return mMinimum; }
			set
			{
				if (value > mMaximum)
					throw new ArgumentOutOfRangeException("value");

				mMinimum = value;
				ConstrainValue(value, mMaximum);
			}
		}
		public Double Maximum
		{
			get { return mMaximum; }
			set
			{
				if (value < mMinimum)
					throw new ArgumentOutOfRangeException("value");

				mMaximum = value;
				ConstrainValue(mMinimum, value);
			}
		}
		public Double DragStepping
		{
			get { return mDragStepping; }
			set { mDragStepping = value; }
		}

		private void ConstrainValue(double min, double max, bool withEvents = true)
		{
			var value = Value;
			if (value < min || value > max)
			{
				if (withEvents && BeginEdit != null) BeginEdit(this, new EventArgs());

				Value = System.Math.Max(min, System.Math.Min(value, max));

				if (withEvents && ValueChanged != null) ValueChanged(this, new EventArgs());
				if (withEvents && EndEdit != null) EndEdit(this, new EventArgs());
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
			ConstrainValue(mMinimum, mMaximum, false);
			if (ValueChanged != null)
				ValueChanged(this, new EventArgs());
		}
		private void OnTextBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			mInputHandler.HandleKeyPressForNumericTextBox(e);
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

				multiplier /= (10 * mDragStepping);

				var newValue = System.Math.Round(mStart + delta / multiplier, 3);

				//var screen = Screen.FromPoint(pos);
				//var newPos = new Point(System.Math.Max(screen.Bounds.Left, System.Math.Min(pos.X + delta < 0 ? -1 : 1, screen.Bounds.Right)),pos.Y);

				//MouseInputManager.SetPosition(newPos);

				Value = newValue;
			}

			base.OnMouseMove(e);
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			Value = Default;
			base.OnMouseDoubleClick(e);
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
		private void OnEnabledChanged(object sender, EventArgs e)
		{
			if (TextBox != null)
			{
				TextBox.Enabled = Enabled;
			}
		}
	}
}
