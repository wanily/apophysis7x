using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Models;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Editor : Form
	{
		public Editor()
		{
			InitializeComponent();
		}

		public TransformCollection Transforms
		{
			get { return mCanvas.Transforms; }
			set { mCanvas.Transforms = value; }
		}

		private void OnTransformUpdatedFromCanvas(object sender, Input.TransformUpdatedEventArgs args)
		{

		}

		private void OnTransformHitOnCanvas(object sender, Input.TransformHitEventArgs args)
		{

		}

		private void OnTransformHitClearedOnCanvas(object sender, System.EventArgs e)
		{

		}

		private void OnCanvasBeginEdit(object sender, System.EventArgs e)
		{

		}

		private void OnCanvasEndEdit(object sender, System.EventArgs e)
		{

		}

		private void OnCanvasMouseMove(object sender, MouseEventArgs e)
		{

		}
	}
}
