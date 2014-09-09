using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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
			ItemHeight = 15;
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			base.OnDrawItem(e);

			if (e.Index < 0)
				return;

			var size = e.Graphics.MeasureString("fg", Font);
			var item = Items[e.Index] as Palette;
			if (item == null)
				return;

			using (var backgroundBrush = new SolidBrush(BackColor))
			{
				e.Graphics.FillRectangle(backgroundBrush, e.Bounds);
				e.DrawFocusRectangle();

				var w = (float)e.Bounds.Width;
				var h = e.Bounds.Height;

				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

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
		}
		protected override void OnMeasureItem(MeasureItemEventArgs e)
		{
			base.OnMeasureItem(e);

			var size = e.Graphics.MeasureString("fg", Font);

			e.ItemWidth = ClientSize.Width;
			e.ItemHeight = (int)size.Height;
		}
	}
}
