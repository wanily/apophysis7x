using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Windows.Visuals;

namespace Xyrus.Apophysis.Windows.Forms
{
	public partial class Editor : Form
	{
		public Editor()
		{
			InitializeComponent();

			IteratorSelectionComboBox.DrawItem += OnIteratorComboBoxDrawItem;
			IteratorSelectionComboBox.MeasureItem += OnIteratorComboBoxMeasureItem;
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

		private void OnIteratorComboBoxDrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index < 0)
				return;

			var item = IteratorSelectionComboBox.Items[e.Index] as string;
			var size = e.Graphics.MeasureString(item ?? string.Empty, IteratorSelectionComboBox.Font);

			var triangle = new[]
				{
					new Point(e.Bounds.Left + 2, e.Bounds.Top + e.Bounds.Height - 2),
					new Point(e.Bounds.Left + e.Bounds.Height - 2, e.Bounds.Top + e.Bounds.Height - 2),
					new Point(e.Bounds.Left + e.Bounds.Height - 2, e.Bounds.Top + 2)
				};

			using (var backgroundBrush = new SolidBrush(IteratorSelectionComboBox.BackColor))
			using (var foregroundBrush = new SolidBrush(IteratorSelectionComboBox.ForeColor))
			using (var iteratorColorBrush = new SolidBrush(IteratorVisual.GetColor(IteratorCanvas.Iterators[e.Index])))
			using (var dimIteratorColorBrush = new SolidBrush(Color.FromArgb(0x80, iteratorColorBrush.Color)))
			using (var iteratorColorPen = new Pen(iteratorColorBrush))
			{
				e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
				e.DrawFocusRectangle();

				e.Graphics.DrawPolygon(iteratorColorPen, triangle);
				e.Graphics.FillPolygon(dimIteratorColorBrush, triangle);

				e.Graphics.DrawString(item ?? string.Empty, IteratorSelectionComboBox.Font, foregroundBrush,
					e.Bounds.Left + e.Bounds.Height + 2,
					e.Bounds.Top + e.Bounds.Height / 2.0f - size.Height / 2.0f);
			}
		}
		private void OnIteratorComboBoxMeasureItem(object sender, MeasureItemEventArgs e)
		{
			var item = IteratorSelectionComboBox.Items[e.Index] as string;
			var size = e.Graphics.MeasureString(item ?? string.Empty, IteratorSelectionComboBox.Font);

			e.ItemWidth = (int)size.Width;
			e.ItemHeight = (int)size.Height;
		}
	}
}
