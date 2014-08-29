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
	}
}
