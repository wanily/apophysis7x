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
			public Func<double> Getter;
			public Action<double> Setter;
		}

		private readonly InputController mInputHandler = new InputController();
		private readonly Lock mInitializer = new Lock();

		private readonly Dictionary<DragPanel, InputInfo> mDragPanels = new Dictionary<DragPanel, InputInfo>();
		private readonly Dictionary<ScrollBar, InputInfo> mScrollBars = new Dictionary<ScrollBar, InputInfo>();
		private readonly Dictionary<TextBox, InputInfo> mTextBoxes = new Dictionary<TextBox, InputInfo>();
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

		protected void Register([NotNull] DragPanel dragPanel, [NotNull] Action<double> setter, [NotNull] Func<double> getter)
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
		protected void Register([NotNull] ScrollBar scrollBar, [NotNull] Action<double> setter, [NotNull] Func<double> getter)
		{
			if (scrollBar == null) throw new ArgumentNullException("scrollBar");
			if (setter == null) throw new ArgumentNullException("setter");
			if (getter == null) throw new ArgumentNullException("getter");

			mScrollBars.Add(scrollBar, new InputInfo { Setter = setter, Getter = getter });
			scrollBar.ValueChanged += OnValueChanged;
			scrollBar.Scroll += OnScrollBarScroll;
		}
		protected void Register([NotNull] TextBox textBox, [NotNull] Action<double> setter, [NotNull] Func<double> getter)
		{
			if (textBox == null) throw new ArgumentNullException("setter");
			if (setter == null) throw new ArgumentNullException("setter");
			if (getter == null) throw new ArgumentNullException("getter");

			mTextBoxes.Add(textBox, new InputInfo { Setter = setter, Getter = getter });
			textBox.TextChanged += OnValueChanged;
			textBox.KeyPress += OnTextBoxKeyPress;
			textBox.KeyUp += OnTextBoxKeyUp;
			textBox.LostFocus += OnTextBoxLostFocus;
		}

		private double ConstrainScrollValue(ScrollBar scrollBar, double value)
		{
			return System.Math.Max(scrollBar.Minimum, System.Math.Min(value, scrollBar.Maximum));
		}
		private void WriteBackControlValues(bool dragPanels = true, bool scrollBars = true, bool textBoxes = true)
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
			}
		}

		private void OnValueChanged(object sender, EventArgs e)
		{
			if (sender == null || mInitializer.IsBusy)
				return;

			var isDragPanel = false;
			var isScrollBar = false;
			var isTextBox = false;

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
				double value;
				if (!double.TryParse(textBox.Text, NumberStyles.Float, InputController.Culture, out value))
					return;

				mTextBoxes[textBox].Setter(value);
				isTextBox = true;
			}

			WriteBackControlValues(!isDragPanel, !isScrollBar, !isTextBox);
			OnValueChangedOverride(sender);
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
			double value;

			if (!double.TryParse(textBox.Text, NumberStyles.Float, InputController.Culture, out value))
			{
				using (mInitializer.Enter())
				{
					Func<double> getter = null;

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
	}
}