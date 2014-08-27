using System;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Input;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Editor : Form
	{
		public Editor()
		{
			InitializeComponent();

			mStatusXPositionLabel.Text = string.Format("X: {0:0.000}", 0);
			mStatusYPositionLabel.Text = string.Format("Y: {0:0.000}", 0);
		}

		public TransformCollection Transforms
		{
			get { return mCanvas.Transforms; }
			set { mCanvas.Transforms = value; }
		}

		private void OnTransformUpdatedFromCanvas(object sender, TransformUpdatedEventArgs args)
		{
			if (!mStatusStringLabel.Visible)
			{
				mStatusStringLabel.Visible = true;
			}

			mStatusStringLabel.Text = args.Operation.ToString();
		}

		private void OnCanvasBeginEdit(object sender, EventArgs e)
		{
		}
		private void OnCanvasEndEdit(object sender, EventArgs e)
		{
			mStatusStringLabel.Visible = false;
			mStatusStringLabel.Text = string.Empty;
		}

		private void OnCanvasMouseMove(object sender, MouseEventArgs e)
		{
			var cursor = mCanvas.CursorPosition;

			mStatusXPositionLabel.Text = string.Format("X: {0:0.000}", cursor.X);
			mStatusYPositionLabel.Text = string.Format("Y: {0:0.000}", cursor.Y);

			mStatusbar.Refresh();
		}
	}
}
