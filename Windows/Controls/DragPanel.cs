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

		private TextBox mTextBox;
		private bool mIsMouseOver;

		private float mStart;
		private Point mCursorStart;
		private bool mIsDragging;
		private float mDefault;
		private float mMinimum;
		private float mMaximum;
		private float mDragStepping;

		public DragPanel()
		{
			InitializeComponent();

			mDefault = 0;
			mMinimum = float.MinValue;
			mMaximum = float.MaxValue;
			mDragStepping = 0.1f;

			Cursor = Cursors.Hand;

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
				float oldValue;

				if (mTextBox != null)
				{
					mTextBox.KeyPress -= OnTextBoxKeyPress;
					mTextBox.TextChanged -= OnTextBoxChanged;
					mTextBox.GotFocus -= OnTextBoxGotFocus;
					mTextBox.LostFocus -= OnTextBoxLostFocus;
					mTextBox.KeyUp -= OnTextBoxKeyUp;

					if (!float.TryParse(mTextBox.Text, NumberStyles.Float, InputController.Culture, out oldValue))
						oldValue = 0.0f;
				}
				else oldValue = 0.0f;
				
				mTextBox = value;

				if (mTextBox != null)
				{
					mTextBox.KeyPress += OnTextBoxKeyPress;
					mTextBox.TextChanged += OnTextBoxChanged;
					mTextBox.GotFocus += OnTextBoxGotFocus;
					mTextBox.LostFocus += OnTextBoxLostFocus;
					mTextBox.KeyUp += OnTextBoxKeyUp;
					Value = oldValue;
				}
			}
		}

		public float Value
		{
			get
			{
				if (TextBox == null)
					return 0.0f;

				float value;
				if (!float.TryParse(TextBox.Text, NumberStyles.Float, InputController.Culture, out value))
					return Default;

				return value;
			}
			set
			{
				if (TextBox == null)
					return;

				TextBox.Text = value.ToString(InputController.DefaultFormat, InputController.Culture);
				TextBox.Refresh();
			}
		}
		public float Default
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
		public float Minimum
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
		public float Maximum
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
		public float DragStepping
		{
			get { return mDragStepping; }
			set { mDragStepping = value; }
		}

		private void ConstrainValue(float min, float max, bool withEvents = true)
		{
			var value = Value;
			if (value < min || value > max)
			{
				if (withEvents && BeginEdit != null) BeginEdit(this, new EventArgs());

				Value = Float.Range(value, min, max);

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
		private void OnTextBoxKeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Return || e.KeyData == Keys.Enter)
				((TextBox)sender).Parent.SelectNextControl((TextBox)sender, true, true, true, true);
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
				var delta = pos.X - (float)mCursorStart.X;
				var multiplier = 1000.0f;

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

				var newValue = Float.Round(mStart + delta / multiplier, 3);

				//var screen = Screen.FromPoint(pos);
				//var newPos = new Point(System.Math.Max(screen.Bounds.Left, System.Math.Min(pos.X + delta < 0 ? -1 : 1, screen.Bounds.Right)),pos.Y);

				//MouseInputManager.SetPosition(newPos);

				Value = newValue;
			}

			base.OnMouseMove(e);
		}

		protected override void OnMouseDoubleClick(MouseEventArgs e)
		{
			if (BeginEdit != null)
				BeginEdit(this, new EventArgs());

			Value = Default;

			if (EndEdit != null)
				EndEdit(this, new EventArgs());
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
