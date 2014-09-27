using System.Drawing;
using System.Windows.Forms;

namespace Xyrus.Apophysis.Windows.Visuals
{
	class PreviewBackgroundVisual : ControlVisual<PictureBox>
	{
		private bool mShowTransparency;

		public PreviewBackgroundVisual([NotNull] PictureBox control)
			: base(control)
		{
		}

		public bool ShowTransparency
		{
			get { return mShowTransparency; }
			set
			{
				mShowTransparency = value;
				InvalidateControl();
			}
		}

		public Size BackgroundSize { get; set; }
		public Color BackgroundColor { get; set; }

		protected override void RegisterEvents(PictureBox control)
		{
		}
		protected override void UnregisterEvents(PictureBox control)
		{
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			var size = (BackgroundSize.Width <= 0 || BackgroundSize.Height <= 0) ? AttachedControl.ClientSize : BackgroundSize.FitToFrame(AttachedControl.ClientSize);
			var offset = new Point(AttachedControl.ClientSize.Width / 2 - size.Width / 2, AttachedControl.ClientSize.Height / 2 - size.Height / 2);

			using (var brush = new SolidBrush(AttachedControl.BackColor))
			{
				graphics.FillRectangle(brush, new Rectangle(new Point(), AttachedControl.ClientSize));
			}

			if (ShowTransparency)
			{
				var tilesX = (size.Width + 10) / 10;
				var tilesY = (size.Height + 10) / 10;

				if (tilesX % 2 != 0) tilesX++;
				if (tilesY % 2 != 0) tilesY++;

				using (var brushA = new SolidBrush(Color.White))
				using (var brushB = new SolidBrush(Color.LightGray))
				{
					int ii = 0;
					for (int i = 0; i < tilesX; i++)
					{
						int jj = 0;
						for (int j = 0; j <= tilesY; j++)
						{
							var brush = ((i + j) % 2 != 0) ? brushA : brushB;
							graphics.FillRectangle(brush, new Rectangle(new Point(ii + offset.X, jj + offset.Y), new Size(10, 10)));

							jj += 10;
						}
						ii += 10;
					}

					using (var brushC = new SolidBrush(AttachedControl.BackColor))
					{
						graphics.FillRectangle(brushC, new Rectangle(new Point(offset.X + size.Width, 0), new Size(AttachedControl.ClientSize.Width - (offset.X + size.Width), AttachedControl.ClientSize.Height)));
						graphics.FillRectangle(brushC, new Rectangle(new Point(0, offset.Y + size.Height), new Size(AttachedControl.ClientSize.Width, AttachedControl.ClientSize.Height - (offset.Y + size.Height))));
					}
				}
			}
			else
			{
				using (var brush = new SolidBrush(BackgroundColor))
				{
					graphics.FillRectangle(brush, new Rectangle(offset, size));
				}
			}
		}
	}
}