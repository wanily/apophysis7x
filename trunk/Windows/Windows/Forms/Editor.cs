using System.Windows.Forms;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Editor : Form
	{
		public Editor()
		{
			InitializeComponent();
		}

		public IteratorCollection Iterators
		{
			get { return mCanvas.Iterators; }
			set { mCanvas.Iterators = value; }
		}
	}
}
