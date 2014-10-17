using System.Drawing;
using System.Windows.Forms;
using Xyrus.Apophysis.Models;

namespace Xyrus.Apophysis.Windows.Controls
{
	public partial class PaletteSelectComboBox : ComboBox
	{
		public PaletteSelectComboBox()
		{
			InitializeComponent();

			DrawMode = DrawMode.OwnerDrawVariable;
			DropDownStyle = ComboBoxStyle.DropDownList;
			FormattingEnabled = true;
			ItemHeight = 25;
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);

			if (e.Index < 0)
				return;

			var item = Items[e.Index] as Palette;
			if (item == null)
				return;

			var w = (float)e.Bounds.Width;
			var h = e.Bounds.Height;

			for (float pos = 0; pos < w; pos++)
			{
				var i = (int)System.Math.Round((item.Length - 1) * pos / w);
				var p = pos / w * (w - 4) + 2;

				using (var brush = new SolidBrush(item[i]))
				{
					e.Graphics.FillRectangle(brush, p + e.Bounds.Left, 2.0f + e.Bounds.Top, w - p, h - 2.0f);
				}
			}
		}
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			base.OnMeasureItem(e);

			e.ItemWidth = ClientSize.Width;
			e.ItemHeight = ItemHeight;
		}
	}
}
