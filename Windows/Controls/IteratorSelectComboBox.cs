using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Controls
{
	public partial class IteratorSelectComboBox : ComboBox
	{
		private Flame mFlame;

		public IteratorSelectComboBox()
		{
			InitializeComponent();

			DrawMode = DrawMode.OwnerDrawVariable;
			DropDownStyle = ComboBoxStyle.DropDownList;
			FormattingEnabled = true;
			ItemHeight = 15;
		}

		public Flame Flame
		{
			get { return mFlame; }
			set
			{
				mFlame = value;

				Items.Clear();

				if (value == null)
					return;

				Items.AddRange(value
					.Iterators
					.Select(x => x.GetDisplayName())
					.OfType<object>()
					.ToArray());
			}
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);

			if (e.Index < 0 || Flame == null)
				return;

			var item = Items[e.Index] as string;
			var size = e.Graphics.MeasureString(item ?? string.Empty, Font);

			var triangle = new[]
				{
					new Point(e.Bounds.Left + 2, e.Bounds.Top + e.Bounds.Height - 2),
					new Point(e.Bounds.Left + e.Bounds.Height - 2, e.Bounds.Top + e.Bounds.Height - 2),
					new Point(e.Bounds.Left + e.Bounds.Height - 2, e.Bounds.Top + 2)
				};

			var color = e.Index < 0 || e.Index >= Flame.Iterators.Count ? Color.White : Flame.Iterators[e.Index].GetColor();

			using (var backgroundBrush = new SolidBrush(BackColor))
			using (var foregroundBrush = new SolidBrush(ForeColor))
			using (var iteratorColorBrush = new SolidBrush(color))
			using (var dimIteratorColorBrush = new SolidBrush(Color.FromArgb(0x80, iteratorColorBrush.Color)))
			using (var iteratorColorPen = new Pen(iteratorColorBrush))
			{
				e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
				e.DrawFocusRectangle();

				e.Graphics.DrawPolygon(iteratorColorPen, triangle);
				e.Graphics.FillPolygon(dimIteratorColorBrush, triangle);

				e.Graphics.DrawString(item ?? string.Empty, Font, foregroundBrush,
					e.Bounds.Left + e.Bounds.Height + 2,
					e.Bounds.Top + e.Bounds.Height / 2.0f - size.Height / 2.0f);
			}
		}
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			base.OnMeasureItem(e);

			var item = Items[e.Index] as string;
			var size = e.Graphics.MeasureString(item ?? string.Empty, Font);

			e.ItemWidth = (int)size.Width;
			e.ItemHeight = (int)size.Height;
		}
	}
}
