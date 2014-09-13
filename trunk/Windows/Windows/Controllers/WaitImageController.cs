using System.Drawing;

namespace Xyrus.Apophysis.Windows.Controllers
{
	static class WaitImageController
	{
		private const char mWaitGlyph = '6';

		public static Bitmap DrawWaitImage(Size size, Color backgroundColor, float glyphFontSize = 50.0f)
		{
			var bitmap = new Bitmap(size.Width, size.Height);

			using (var graphics = Graphics.FromImage(bitmap))
			using (var background = new SolidBrush(backgroundColor))
			using (var foreground = new SolidBrush(Color.FromArgb(255 - backgroundColor.R, 255 - backgroundColor.G, 255 - backgroundColor.B)))
			using (var frame = new Pen(foreground))
			{
				graphics.FillRectangle(background, new Rectangle(new Point(), bitmap.Size));
				graphics.DrawRectangle(frame, new Rectangle(new Point(), bitmap.Size));

				using (var glyphFont = new Font("Wingdings", glyphFontSize, FontStyle.Bold))
				{
					var glyph = new string(mWaitGlyph, 1);
					var glyphSize = graphics.MeasureString(glyph, glyphFont);

					graphics.DrawString(glyph, glyphFont, foreground, size.Width / 2f - glyphSize.Width / 2f, size.Height / 2f - glyphSize.Height / 2f);
				}
			}

			return bitmap;
		}
	}
}