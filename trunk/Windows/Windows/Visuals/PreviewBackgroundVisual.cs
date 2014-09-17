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

		protected override void RegisterEvents(PictureBox control)
		{
		}
		protected override void UnregisterEvents(PictureBox control)
		{
		}

		protected override void OnControlPaint(Graphics graphics)
		{
			if (ShowTransparency)
			{
				var tilesX = (AttachedControl.ClientSize.Width + 10) / 10;
				var tilesY = (AttachedControl.ClientSize.Height + 10) / 10;

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

							graphics.FillRectangle(brush, new Rectangle(new Point(ii, jj), new Size(10, 10)));

							jj += 10;
						}
						ii += 10;
					}

				}
			}
			else
			{
				using (var brush = new SolidBrush(AttachedControl.BackColor))
				{
					graphics.FillRectangle(brush, new Rectangle(new Point(), AttachedControl.ClientSize));
				}
			}
		}
	}
}