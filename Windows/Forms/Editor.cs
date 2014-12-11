using System;
using System.ComponentModel;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;
using Xyrus.Apophysis.Windows.Controllers;
using Xyrus.Apophysis.Windows.Interfaces.Views;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Editor : Form, IEditorView
	{
		private readonly InputController mInputHandler;

		public Editor()
		{
			InitializeComponent();

			mInputHandler = new InputController();

			IteratorCanvas.EndEdit += OnRequestCommit;
			IteratorColorDragPanel.EndEdit += OnRequestCommit;
			IteratorColorScrollBar.Scroll += OnScrollbarCommit;
			IteratorColorSpeedDragPanel.EndEdit += OnRequestCommit;
			IteratorDirectColorDragPanel.EndEdit += OnRequestCommit;
			IteratorWeightDragPanel.EndEdit += OnRequestCommit;
			IteratorWeightTextBox.LostFocus += OnRequestCommit;
			IteratorNameTextBox.LostFocus += OnRequestCommit;
			IteratorOpacityDragPanel.EndEdit += OnRequestCommit;
			IteratorPointOxTextBox.LostFocus += OnRequestCommit;
			IteratorPointOyTextBox.LostFocus += OnRequestCommit;
			IteratorPointXxTextBox.LostFocus += OnRequestCommit;
			IteratorPointXyTextBox.LostFocus += OnRequestCommit;
			IteratorPointYxTextBox.LostFocus += OnRequestCommit;
			IteratorPointYyTextBox.LostFocus += OnRequestCommit;
			IteratorPreAffineOxTextBox.LostFocus += OnRequestCommit;
			IteratorPreAffineOyTextBox.LostFocus += OnRequestCommit;
			IteratorPreAffineXxTextBox.LostFocus += OnRequestCommit;
			IteratorPreAffineXyTextBox.LostFocus += OnRequestCommit;
			IteratorPreAffineYxTextBox.LostFocus += OnRequestCommit;
			IteratorPreAffineYyTextBox.LostFocus += OnRequestCommit;
			IteratorPostAffineOxTextBox.LostFocus += OnRequestCommit;
			IteratorPostAffineOyTextBox.LostFocus += OnRequestCommit;
			IteratorPostAffineXxTextBox.LostFocus += OnRequestCommit;
			IteratorPostAffineXyTextBox.LostFocus += OnRequestCommit;
			IteratorPostAffineYxTextBox.LostFocus += OnRequestCommit;
			IteratorPostAffineYyTextBox.LostFocus += OnRequestCommit;
			VariablesGrid.CellEndEdit += OnRequestCommit;
			VariationsGrid.CellEndEdit += OnRequestCommit;
			ClearVariationsButton.Click += OnRequestCommit;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
					components = null;
				}
			}
			base.Dispose(disposing);
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			InputController.HandleKeyboardInput(this, keyData);
			return base.ProcessCmdKey(ref msg, keyData);
		}
		protected override void OnClosing(CancelEventArgs e)
		{
			base.OnClosing(e);

			e.Cancel = true;
			Hide();
		}

		private void OnNumericTextBoxKeyPress(object sender, KeyPressEventArgs e)
		{
			mInputHandler.HandleKeyPressForNumericTextBox(e);
		}
		private void OnRequestCommit(object sender, EventArgs e)
		{
			if (UndoEvent != null)
				UndoEvent(sender, e);
		}
		private void OnScrollbarCommit(object sender, ScrollEventArgs e)
		{
			if (e.Type == ScrollEventType.EndScroll)
				OnRequestCommit(sender, e);
		}

		public IIteratorCanvasView Canvas
		{
			get { return IteratorCanvas; }
		}

		public void SetHeader(Flame flame)
		{
			Text = flame == null ? "Editor" : string.Format("Editor - {0}", flame.CalculatedName);
		}

		public event EventHandler UndoEvent;
	}
}
