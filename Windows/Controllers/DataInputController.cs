using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Controls;

namespace Xyrus.Apophysis.Windows.Controllers
{
	public abstract class DataInputController<TView> : Controller<TView> where TView : Component, new()
	{
		class InputInfo
		{
			public Func<float> Getter;
			public Action<float> Setter;
		}

		private readonly InputController mInputHandler = new InputController();
		private readonly Lock mInitializer = new Lock();

		private readonly Dictionary<DragPanel, InputInfo> mDragPanels = new Dictionary<DragPanel, InputInfo>();
		private readonly Dictionary<ScrollBar, InputInfo> mScrollBars = new Dictionary<ScrollBar, InputInfo>();
		private readonly Dictionary<TextBox, InputInfo> mTextBoxes = new Dictionary<TextBox, InputInfo>();
		private readonly Dictionary<ComboBox, InputInfo> mComboBoxes = new Dictionary<ComboBox, InputInfo>();
		private readonly Dictionary<NumericUpDown, InputInfo> mUpDownControls = new Dictionary<NumericUpDown, InputInfo>();
		private readonly Dictionary<TextBox, InputInfo> mDragPanelTextBoxes = new Dictionary<TextBox, InputInfo>();

		protected DataInputController() { }
		protected DataInputController([NotNull] TView view) : base(view) { }
		protected DataInputController([NotNull] TView view, [NotNull] Lock initializeLock)
			: base(view)
		{
			if (initializeLock == null) throw new ArgumentNullException("initializeLock");
			mInitializer = initializeLock;
		}

		public virtual void Update()
		{
			WriteBackControlValues();
		}

		protected void Cleanup()
		{
			foreach (var dragPanel in mDragPanels.Keys)
			{
				dragPanel.ValueChanged -= OnValueChanged;
				dragPanel.EndEdit -= OnDragPanelEndEdit;
			}
			mDragPanels.Clear();

			foreach (var scrollBar in mScrollBars.Keys)
			{
				scrollBar.ValueChanged -= OnValueChanged;
				scrollBar.Scroll -= OnScrollBarScroll;
			}
			mScrollBars.Clear();

			foreach (var textBox in mTextBoxes.Keys)
			{
				textBox.TextChanged -= OnValueChanged;
				textBox.KeyPress -= OnTextBoxKeyPress;
				textBox.KeyUp -= OnTextBoxKeyUp;
				textBox.LostFocus -= OnTextBoxLostFocus;
			}
			mTextBoxes.Clear();

			foreach (var comboBox in mComboBoxes.Keys)
			{
				comboBox.TextChanged -= OnValueChanged;
				comboBox.KeyPress -= OnComboBoxKeyPress;
				comboBox.KeyUp -= OnComboBoxKeyUp;
				comboBox.LostFocus -= OnComboBoxLostFocus;
			}
			mComboBoxes.Clear();

			foreach (var upDown in mUpDownControls.Keys)
			{
				upDown.TextChanged -= OnValueChanged;
				upDown.KeyPress -= OnUpDownKeyPress;
				upDown.KeyUp -= OnUpDownKeyUp;
				upDown.LostFocus -= OnUpDownLostFocus;
				upDown.Scroll -= OnScrollBarScroll;
			}
			mUpDownControls.Clear();

			foreach (var textBox in mDragPanelTextBoxes.Keys)
			{
				textBox.LostFocus -= OnTextBoxLostFocus;
			}
			mDragPanelTextBoxes.Clear();
		}

		protected virtual void OnValueCommittedOverride([NotNull] object control)
		{
		}
		protected virtual void OnValueChangedOverride([NotNull] object control)
		{
		}

		protected void Register([NotNull] DragPanel dragPanel, [NotNull] Action<float> setter, [NotNull] Func<float> getter)
		{
			if (dragPanel == null) throw new ArgumentNullException("dragPanel");
			if (setter == null) throw new ArgumentNullException("setter");
			if (getter == null) throw new ArgumentNullException("getter");

			mDragPanels.Add(dragPanel, new InputInfo { Setter = setter, Getter = getter });
			dragPanel.ValueChanged += OnValueChanged;
			dragPanel.EndEdit += OnDragPanelEndEdit;

			if (dragPanel.TextBox != null)
			{
				mDragPanelTextBoxes.Add(dragPanel.TextBox, new InputInfo { Setter = setter, Getter = getter });
				dragPanel.TextBox.LostFocus += OnTextBoxLostFocus;
			}
		}
		protected void Register([NotNull] ScrollBar scrollBar, [NotNull] Action<float> setter, [NotNull] Func<float> getter)
		{
			if (scrollBar == null) throw new ArgumentNullException("scrollBar");
			if (setter == null) throw new ArgumentNullException("setter");
			if (getter == null) throw new ArgumentNullException("getter");

			mScrollBars.Add(scrollBar, new InputInfo { Setter = setter, Getter = getter });
			scrollBar.ValueChanged += OnValueChanged;
			scrollBar.Scroll += OnScrollBarScroll;
		}
		protected void Register([NotNull] TextBox textBox, [NotNull] Action<float> setter, [NotNull] Func<float> getter)
		{
			if (textBox == null) throw new ArgumentNullException("textBox");
			if (setter == null) throw new ArgumentNullException("setter");
			if (getter == null) throw new ArgumentNullException("getter");

			mTextBoxes.Add(textBox, new InputInfo { Setter = setter, Getter = getter });
			textBox.TextChanged += OnValueChanged;
			textBox.KeyPress += OnTextBoxKeyPress;
			textBox.KeyUp += OnTextBoxKeyUp;
			textBox.LostFocus += OnTextBoxLostFocus;
		}
		protected void Register([NotNull] ComboBox comboBox, [NotNull] Action<float> setter, [NotNull] Func<float> getter)
		{
			if (comboBox == null) throw new ArgumentNullException("comboBox");
			if (setter == null) throw new ArgumentNullException("setter");
			if (getter == null) throw new ArgumentNullException("getter");

			mComboBoxes.Add(comboBox, new InputInfo { Setter = setter, Getter = getter });
			comboBox.TextChanged += OnValueChanged;
			comboBox.KeyPress += OnComboBoxKeyPress;
			comboBox.KeyUp += OnComboBoxKeyUp;
			comboBox.LostFocus += OnComboBoxLostFocus;
		}
		protected void Register([NotNull] NumericUpDown upDown, [NotNull] Action<float> setter, [NotNull] Func<float> getter)
		{
			if (upDown == null) throw new ArgumentNullException("upDown");
			if (setter == null) throw new ArgumentNullException("setter");
			if (getter == null) throw new ArgumentNullException("getter");

			mUpDownControls.Add(upDown, new InputInfo { Setter = setter, Getter = getter });
			upDown.KeyPress += OnUpDownKeyPress;
			upDown.KeyUp += OnUpDownKeyUp;
			upDown.TextChanged += OnValueChanged;
			upDown.LostFocus += OnUpDownLostFocus;
			upDown.Scroll += OnScrollBarScroll;
		}

		private float ConstrainScrollValue(ScrollBar scrollBar, float value)
		{
			return Float.Range(value, scrollBar.Minimum, scrollBar.Maximum);
		}
		private void WriteBackControlValues(bool dragPanels = true, bool scrollBars = true, bool textBoxes = true, bool comboBoxes = true, bool upDownControls = true)
		{
			using (mInitializer.Enter())
			{
				if (dragPanels)
					foreach (var dragPanel in mDragPanels.Keys)
						dragPanel.Value = mDragPanels[dragPanel].Getter();

				if (scrollBars)
					foreach (var scrollBar in mScrollBars.Keys)
						scrollBar.Value = (int)ConstrainScrollValue(scrollBar, mScrollBars[scrollBar].Getter());

				if (textBoxes)
					foreach (var textBox in mTextBoxes.Keys)
						textBox.Text = mTextBoxes[textBox].Getter().ToString(InputController.DefaultFormat, InputController.Culture);

				if (comboBoxes)
					foreach (var comboBox in mComboBoxes.Keys)
						comboBox.Text = ((int)mComboBoxes[comboBox].Getter()).ToString(InputController.Culture);

				if (upDownControls)
					foreach (var upDown in mUpDownControls.Keys)
						upDown.Value = (int)mUpDownControls[upDown].Getter();
			}
		}

		private void OnValueChanged(object sender, EventArgs e)
		{
			if (sender == null || mInitializer.IsBusy)
				return;

			var isDragPanel = false;
			var isScrollBar = false;
			var isTextBox = false;
			var isComboBox = false;
			var isUpDown = false;

			var dragPanel = sender as DragPanel;
			if (dragPanel != null && mDragPanels.ContainsKey(dragPanel))
			{
				mDragPanels[dragPanel].Setter(dragPanel.Value);
				isDragPanel = true;
			}

			var scrollBar = sender as ScrollBar;
			if (scrollBar != null && mScrollBars.ContainsKey(scrollBar))
			{
				mScrollBars[scrollBar].Setter(scrollBar.Value);
				isScrollBar = true;
			}

			var textBox = sender as TextBox;
			if (textBox != null && mTextBoxes.ContainsKey(textBox))
			{
				float value;
				if (!float.TryParse(textBox.Text, NumberStyles.Float, InputController.Culture, out value))
					return;

				mTextBoxes[textBox].Setter(value);
				isTextBox = true;
			}

			var comboBox = sender as ComboBox;
			if (comboBox != null && mComboBoxes.ContainsKey(comboBox))
			{
				float value;
				if (!float.TryParse(comboBox.Text, NumberStyles.Float, InputController.Culture, out value))
					return;

				mComboBoxes[comboBox].Setter(value);
				isComboBox = true;
			}

			var upDown = sender as NumericUpDown;
			if (upDown != null && mUpDownControls.ContainsKey(upDown))
			{
				float value;
				if (!float.TryParse(upDown.Text, NumberStyles.Float, InputController.Culture, out value))
					return;

				mUpDownControls[upDown].Setter(value);
				isUpDown = true;
			}

			WriteBackControlValues(!isDragPanel, !isScrollBar, !isTextBox, !isComboBox, !isUpDown);
			OnValueChangedOverride(sender);
		}
		private void OnUpDownKeyPress(object sender, KeyPressEventArgs e)
		{
			mInputHandler.HandleKeyPressForNumericTextBox(e);
		}
		private void OnComboBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			mInputHandler.HandleKeyPressForNumericTextBox(e);
		}
		private void OnTextBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			mInputHandler.HandleKeyPressForNumericTextBox(e);
		}
		private void OnUpDownKeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Return || e.KeyData == Keys.Enter)
				((NumericUpDown)sender).Parent.SelectNextControl((NumericUpDown)sender, true, true, true, true);
		}
		private void OnComboBoxKeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Return || e.KeyData == Keys.Enter)
				((ComboBox)sender).Parent.SelectNextControl((ComboBox)sender, true, true, true, true);
		}
		private void OnTextBoxKeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Return || e.KeyData == Keys.Enter)
				((TextBox)sender).Parent.SelectNextControl((TextBox)sender, true, true, true, true);
		}
		private void OnDragPanelEndEdit(object sender, EventArgs e)
		{
			if (mInitializer.IsBusy)
				return;

			OnValueCommittedOverride(sender);
		}
		private void OnScrollBarScroll(object sender, ScrollEventArgs e)
		{
			if (mInitializer.IsBusy)
				return;

			if (e.Type == ScrollEventType.EndScroll)
				OnValueCommittedOverride(sender);
		}
		private void OnTextBoxLostFocus(object sender, EventArgs e)
		{
			if (mInitializer.IsBusy)
				return;

			var textBox = (TextBox) sender;
			float value;

			if (!float.TryParse(textBox.Text, NumberStyles.Float, InputController.Culture, out value))
			{
				using (mInitializer.Enter())
				{
					Func<float> getter = null;

					if (mTextBoxes.ContainsKey(textBox))
						getter = mTextBoxes[textBox].Getter;
					else if (mDragPanelTextBoxes.ContainsKey(textBox))
						getter = mDragPanelTextBoxes[textBox].Getter;

					if (getter != null)
					{
						textBox.Text = getter().ToString(InputController.DefaultFormat, InputController.Culture);
					}

					return;
				}
			}

			OnValueCommittedOverride(textBox);
		}
		private void OnComboBoxLostFocus(object sender, EventArgs e)
		{
			if (mInitializer.IsBusy)
				return;

			var comboBox = (ComboBox)sender;
			float value;

			if (!float.TryParse(comboBox.Text, NumberStyles.Float, InputController.Culture, out value))
			{
				using (mInitializer.Enter())
				{
					Func<float> getter = null;

					if (mComboBoxes.ContainsKey(comboBox))
						getter = mComboBoxes[comboBox].Getter;

					if (getter != null)
					{
						comboBox.Text = getter().ToString(InputController.DefaultFormat, InputController.Culture);
					}

					return;
				}
			}

			OnValueCommittedOverride(comboBox);
		}
		private void OnUpDownLostFocus(object sender, EventArgs e)
		{
			if (mInitializer.IsBusy)
				return;

			var upDown = (NumericUpDown)sender;
			float value;

			if (!float.TryParse(upDown.Text, NumberStyles.Float, InputController.Culture, out value))
			{
				using (mInitializer.Enter())
				{
					Func<float> getter = null;

					if (mUpDownControls.ContainsKey(upDown))
						getter = mUpDownControls[upDown].Getter;

					if (getter != null)
					{
						upDown.Text = getter().ToString(InputController.DefaultFormat, InputController.Culture);
					}

					return;
				}
			}

			OnValueCommittedOverride(upDown);
		}
	}
}